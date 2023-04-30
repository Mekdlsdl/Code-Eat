using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEngine.SceneManagement;

public class ProblemManager : MonoBehaviour
{
    public static ProblemManager instance { get; private set; }
    public static int totalProblemCount = 0;

    private EnemyType enemyType;
    [SerializeField] private Enemy enemy;

    [SerializeField] private GameObject battlePlayerPrefab;
    [SerializeField] private Transform battlePlayerTransform, problemUI;
    public List<Transform> optionTransforms;

    [SerializeField] private List<MapAndProblems> mapProblemList;
    private List<GameObject> problems = new List<GameObject>();
    private GameObject currentProblem;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    
    public void Init(EnemyType enemy_type)
    {
        // 적 정보 초기화 부분은 BattleManager 로 옮길 수도 있음
        SetEnemy(enemyType);
        ///
        
        SetPlayers();
        SetProblemGroup(GameManager.instance.currentMapName);
        SpawnProblem();
    }

    private void SetPlayers()
    {
        var playerConfigs = PlayerConfigManager.instance.GetPlayerConfigs();

        for (int i = 0; i < playerConfigs.Count; i++) {
            var player = Instantiate(battlePlayerPrefab, battlePlayerTransform);
            
            // 플레이어별 공격 시스템 초기화
            PlayerBattleMode pbm = player.GetComponent<PlayerBattleMode>();
            pbm.Init(playerConfigs[i]);

            // 플레이어별 답안지를 답안매니저에 추가
            PlayerAnswer player_answer = player.GetComponent<PlayerAnswer>();
            player_answer.Init(playerConfigs[i], pbm);
            AnswerManager.instance.AddToPlayerAnswerList(player_answer);
        }
    }

    private void SetEnemy(EnemyType enemy_type) // EnemyType ScriptableObject 에 포함된 정보를 Enemy.cs 에 초기화
    {
        enemyType = enemy_type;
        // enemy.maxhp = enemy.hp = enemyType.enemyHP;
    }
    
    private void SetProblemGroup(string map_name)
    {
        for (int i = 0; i < mapProblemList.Count; i++) {
            if (mapProblemList[i].map_name == map_name) {
                problems = mapProblemList[i].problems;
                return;
            }
        }
        Debug.LogError($"{map_name} 의 이름을 가진 맵은 존재하지 않습니다.");
    }
    private void SpawnProblem()
    {
        AnswerManager.instance.ResetPlayerAnswers();

        int selectedIndex = Random.Range(0, problems.Count);
        currentProblem = Instantiate(problems[selectedIndex], problemUI);
        currentProblem.GetComponent<StackProblem>().pm = this;

        totalProblemCount++;
    }

    private void NextProblem() // 다음 문제를 불러오고자 할 때 호출
    {
        Destroy(currentProblem);
        SpawnProblem();
    }

    public void DisplayProblem() // 문제 UI를 활성화할 때 호출
    {
        DOTween.Rewind("DisplayProblem");
        DOTween.Play("DisplayProblem");
    }

    public void HideProblem() // 문제 UI를 숨길 때 호출
    {
        DOTween.Rewind("HideProblem");
        DOTween.Play("HideProblem");
    }
}

[System.Serializable]
public class MapAndProblems
{
    public string map_name;
    public List<GameObject> problems;
}