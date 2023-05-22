using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class ProblemManager : MonoBehaviour
{
    public static ProblemManager instance { get; private set; }
    public static int totalProblemCount = 0;

    [SerializeField] private float maxProblemTime;
    private float timer; public float Timer => timer;
    [SerializeField] private TextMeshProUGUI timerUI;

    [SerializeField] private GameObject battlePlayerPrefab, screenCover, stageCompleteText;
    [SerializeField] private Transform battlePlayerTransform, problemUI;
    public List<Transform> optionTransforms;
    private GameObject currentProblem, tempProblem;

    [System.NonSerialized] public bool isShowingAnswer = false;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;

        ShowScreen();
        StartCoroutine(NextProblem(0.9f));
    }

    void Update()
    {
        if (PlayerAnswer.enableAnswerSelect) {
            CountDown();
        }
    }
    private void CountDown()
    {
        int beforeTime = Mathf.FloorToInt(timer);
        timer -= Time.deltaTime;

        if (timer < 0)
            TimeOut();
        
        int afterTime = Mathf.FloorToInt(timer);
        timerUI.text = Mathf.FloorToInt(timer).ToString();
        timerUI.color = timer < 4 ? Color.red : Color.yellow;

        if (afterTime != beforeTime) {
            DOTween.Rewind("TimerBounce");
            DOTween.Play("TimerBounce");
        }
    }

    private void TimeOut()
    {
        timer = 0;
        StartCoroutine(AnswerManager.instance.TryMarkPlayerAnswer());
    }
    
    public void Init()
    {   
        SetPlayers();
        SetProblem();
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
        tempProblem = Instantiate(currentProblem, problemUI);

        totalProblemCount++;
        Debug.Log($"{totalProblemCount} 번째 문제");
    }

    public IEnumerator NextProblem(float waitTime = 0.6f) // 다음 문제를 불러오고자 할 때 호출
    {
        timer = maxProblemTime;
        timerUI.text = timer.ToString("F0");
        timerUI.color = Color.white;

        yield return new WaitForSeconds(waitTime);
        if (tempProblem)
            Destroy(tempProblem);
        DisplayProblem();
        SpawnProblem();
    }

    private void DisplayProblem() // 문제 UI를 활성화할 때 호출
    {
        gameObject.SetActive(true);
        DOTween.Rewind("DisplayProblem");
        DOTween.Play("DisplayProblem");
    }

    public void HideProblem() // 문제 UI를 숨길 때 호출
    {
        DOTween.Rewind("HideProblem");
        DOTween.Play("HideProblem");
    }

    private void ShowScreen()
    {
        screenCover.SetActive(true);
        DOTween.Rewind("ProblemModeIn");
        DOTween.Play("ProblemModeIn");
    }

    public void HideScreen()
    {
        DOTween.Rewind("ProblemModeOut");
        DOTween.Play("ProblemModeOut");
    }

    public IEnumerator ShowCorrectOption()
    {
        isShowingAnswer = true;
        string correctOption = "";

        for (int i = 0; i < optionTransforms.Count; i++) {
            if (i != AnswerManager.instance.AnswerIndex)
                optionTransforms[i].GetComponent<Image>().color = new Color32(157, 157, 157, 255);
            else {
                correctOption = optionTransforms[i].gameObject.name;
                DOTween.Rewind($"{correctOption}");
                DOTween.Play($"{correctOption}");
            }
        }
        yield return new WaitForSeconds(1.5f);
        
        DOTween.Pause($"{correctOption}");
        DOTween.Rewind($"{correctOption}");
        for (int i = 0; i < optionTransforms.Count; i++)
            optionTransforms[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        
        isShowingAnswer = false;
    }

    public void ShowStageCompleteText()
    {
        stageCompleteText.SetActive(true);
    }
}