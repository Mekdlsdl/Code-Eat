using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEngine.SceneManagement;

public class ProblemManager : MonoBehaviour
{
    public static ProblemManager instance { get; private set; }
    public static int totalProblemCount = 0;

    [SerializeField] private GameObject battlePlayerPrefab;
    [SerializeField] private Transform battlePlayerTransform, problemUI;
    public List<Transform> optionTransforms;
    private GameObject currentProblem;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    
    public void Init()
    {   
        SetPlayers();
        SetProblem();
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
    private void SetProblem()
    {
        currentProblem = BattleManager.instance.curEnemy.enemy_type.problem;
    }
    private void SpawnProblem()
    {
        AnswerManager.instance.ResetPlayerAnswers();
        GameObject problem = Instantiate(currentProblem, problemUI);
        problem.GetComponent<StackProblem>().pm = this;

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