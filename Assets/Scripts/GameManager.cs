using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public static bool isProblemMode = false;
    public static HashSet<string> encounteredEnemyset = new HashSet<string>();

    [SerializeField] List<CharacterType> unlockedCharacters = new List<CharacterType>();
    public List<CharacterType> UnlockedCharacters => unlockedCharacters;
    [SerializeField] List<string> lockedCharacters = new List<string>();

    public List<Color32> PlayerColors = new List<Color32>();

    public string currentMapName { get; private set; } = "FirstMap";

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    ///
    void Update()
    {
        if (Input.GetKeyDown("g")) // 게임오버 화면 테스트
        {
            var process = SceneManager.LoadSceneAsync("GameOver");
            process.completed += (AsyncOperation operation) =>
            {
                GameOverControl.instance.Init();
                return;
            };
        }
        else if (Input.GetKeyDown("b"))
        {
            ReturnToMapMode(); // 풀이 모드에서 빠져나오기 테스트
        }
    }
    ///

    public IEnumerator TryMapSelect()
    {
        yield return new WaitForEndOfFrame();
        if (PlayerConfigManager.instance.PlayerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("MapSelect");
        }
    }

    public void LoadMap(string sceneName, bool isReturningToMap = false)
    {
        var process = SceneManager.LoadSceneAsync($"{sceneName}");
        process.completed += (AsyncOperation operation) =>
        {
            if (isReturningToMap) {
                PositionControl.instance.TurnOffTips();

                PositionControl.instance.RecoverPos();
                PositionControl.instance.enemy_spawner.SpawnEnemy();                
            }
            PlayerSpawn.instance.SpawnPlayers();
            return;
        };
    }

    // 하연 : MapSelect 화면에서 바로 공격 모드로 진입하여 테스트할 수 있도록 추가
    public void LoadMapForTest(string sceneName, string actionMapName)
    {
        var process = SceneManager.LoadSceneAsync($"{sceneName}");
        process.completed += (AsyncOperation operation) =>
        {
            PlayerSpawnForTest.instance.SpawnPlayersForTest(actionMapName);
            return;
        };
        
    }
    //

    public RuntimeAnimatorController GetCharAnimControl(string target_name)
    {
        for (int i = 0; i < unlockedCharacters.Count; i++) {
            if (unlockedCharacters[i].characterName == target_name)
                return unlockedCharacters[i].charAnimControl;
        }
        Debug.LogError($"Character Name '{target_name}' not found.");
        return null;
    }

    public void ChangeActionMaps(string map_name)
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs) {
            playerConfig.Input.SwitchCurrentActionMap(map_name);
        }
    }

    public IEnumerator StartProblemMode(EnemyType enemyType, Vector3 playerPosition)
    {
        GameManager.isProblemMode = true;
        ChangeActionMaps("BattleMode");
        PositionControl.instance.BackupPos();

        PlayerSpawn.instance.SetCirclePosition(playerPosition);
        PlayerSpawn.instance.SetCircleTransition(true);
        yield return new WaitForSeconds(1.5f);

        var process = SceneManager.LoadSceneAsync("ProblemMode");
        process.completed += (AsyncOperation operation) =>
        {
            ProblemManager.instance.Init(enemyType);
        };
    }

    public void ReturnToMapMode() // 문제를 풀고 맵으로 돌아갈 경우
    {
        isProblemMode = false;
        ChangeActionMaps("MapControl");

        LoadMap(currentMapName, true);
    }

    public void ReturnToMapSelectMode() // 맵 선택 모드를 선택해서 이동할 경우
    {
        isProblemMode = false;
        ChangeActionMaps("StartingMenu");
        SceneManager.LoadScene("MapSelect");
    }

    public void RetryMapMode() // 다시 시작을 선택해서 해당 맵을 다시 플레이할 경우
    {
        isProblemMode = false;
        ChangeActionMaps("MapControl");
        LoadMap(currentMapName);
    }

    public void SetCurrentMapName(string map_name) // 문제모드로 돌입하기 전 어떤 맵에서 플레이 중인지 알려준다
    {
        currentMapName = map_name;
    }

    public void ResetEncounteredEnemyList()
    {
        encounteredEnemyset = new HashSet<string>();
    }
}

public class InputType
{
    public const string UP = "UP";
    public const string DOWN = "DOWN";
    public const string LEFT = "LEFT";
    public const string RIGHT = "RIGHT";
    public const string SOUTHBUTTON = "SOUTHBUTTON";
    public const string EASTBUTTON = "EASTBUTTON";
    public const string NORTHBUTTON = "NORTHBUTTON";
    public const string WESTBUTTON = "WESTBUTTON";
}

[System.Serializable]
public class CharacterType
{
    public string characterName;
    public RuntimeAnimatorController charAnimControl;
}
