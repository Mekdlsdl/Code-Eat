using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] List<CharacterType> unlockedCharacters = new List<CharacterType>();
    public List<CharacterType> UnlockedCharacters => unlockedCharacters;
    [SerializeField] List<string> lockedCharacters = new List<string>();

    public List<Color32> PlayerColors = new List<Color32>();

    public static bool isProblemMode = false;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public IEnumerator TryMapSelect()
    {
        yield return new WaitForEndOfFrame();
        if (PlayerConfigManager.instance.PlayerConfigs.All(p => p.IsReady == true))
        {
            MapSelectControl.instance.EnableMapSelection();
        }
    }

    public void LoadMap(string sceneName)
    {
        var process = SceneManager.LoadSceneAsync($"{sceneName}");
        process.completed += (AsyncOperation operation) =>
        {
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
            PlayerSpawn.instance.SpawnPlayersForTest(actionMapName);
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
        GameManager.instance.ChangeActionMaps("BattleMode");

        PlayerSpawn.instance.SetCirclePosition(playerPosition);
        PlayerSpawn.instance.SetCircleTransition(true);
        yield return new WaitForSeconds(2f);
        
        ProblemManager problemManager = PlayerSpawn.instance.problemManager;
        problemManager.enemyType = enemyType;
        problemManager.gameObject.SetActive(true);

        PlayerSpawn.instance.FadeOutScreen();

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void ExitProblemMode()
    {
        GameManager.isProblemMode = false;
        GameManager.instance.ChangeActionMaps("MapControl");
        
        ProblemManager problemManager = PlayerSpawn.instance.problemManager;
        problemManager.gameObject.SetActive(false);
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
