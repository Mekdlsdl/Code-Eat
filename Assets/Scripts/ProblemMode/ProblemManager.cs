using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProblemManager : MonoBehaviour
{
    public static ProblemManager instance { get; private set; }
    private EnemyType enemyType;
    [SerializeField] private Enemy enemy;

    [SerializeField] private GameObject battlePlayerPrefab;
    
    public List<Transform> optionTransforms;
    [SerializeField] private Transform battlePlayerTransform, problemUI;
    
    [SerializeField] private List<GameObject> problems = new List<GameObject>();
    private GameObject currentProblem;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void Init(EnemyType enemy_type)
    {
        enemyType = enemy_type;
        SetEnemy();
        SetPlayers();
        SpawnProblem();
    }

    private void SetPlayers()
    {
        var playerConfigs = PlayerConfigManager.instance.GetPlayerConfigs();

        for (int i = 0; i < playerConfigs.Count; i++) {
            var player_battle = Instantiate(battlePlayerPrefab, battlePlayerTransform);
            //PlayerBattleMode pbm = player_battle.GetComponent<PlayerBattleMode>();
            //pbm.Init(playerConfigs[i]);
            //AnswerManager.instance.AddToBattlePlayerList(pbm);
        }
    }
    
    private void SetEnemy()
    {
        // enemy.maxhp = enemy.hp = enemyType.enemyHP;
    }
    private void SpawnProblem()
    {
        int selectedIndex = Random.Range(0, problems.Count);
        currentProblem = Instantiate(problems[selectedIndex], problemUI);
        currentProblem.GetComponent<StackProblem>().pm = this;
    }

    private void NextProblem()
    {
        Destroy(currentProblem);
        SpawnProblem();
    }

    public void HideProblem()
    {
        DOTween.Rewind("HideProblem");
        DOTween.Play("HideProblem");
    }
}