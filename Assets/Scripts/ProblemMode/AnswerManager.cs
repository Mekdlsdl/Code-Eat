using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    public static AnswerManager instance { get; private set; }
    
    private List<PlayerAnswer> playerAnswerList = new List<PlayerAnswer>();
    public List<PlayerAnswer> PlayerAnswerList => playerAnswerList;

    List<PlayerAnswer> correctPlayers = new List<PlayerAnswer>();

    private int answerIndex, answerRank;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void AddToPlayerAnswerList(PlayerAnswer player)
    {
        playerAnswerList.Add(player);
    }

    public void SetProblemAnswer(int index) // 문제에 대한 정답 인덱스를 초기화
    {
        answerIndex = index;
        PlayerAnswer.enableAnswerSelect = true; // 정답 초기화 후 모든 플레이어의 답 선택 허용
    }

    public int ReturnAnswerIndex()
    {
        return answerIndex;
    }

    public void LockPlayerAnswer(PlayerAnswer player) // 플레이어가 답을 눌렀을 때 호출
    {
        RankPlayerAnswer(player);
        player.player_battle_mode.HoldGun();
        StartCoroutine(TryMarkPlayerAnswer());
    }

    private void RankPlayerAnswer(PlayerAnswer player) // 각 플레이어가 답안을 선택한 순서대로 랭크를 부여
    {
        player.answerRank = answerRank;
        answerRank ++;
    }
    
    public IEnumerator TryMarkPlayerAnswer()
    {
        if (!PlayerAnswer.enableAnswerSelect)
            yield break;
        
        if (playerAnswerList.All(p => (p.inputAnswer != -1) || (ProblemManager.instance.Timer == 0) || (p.player_battle_mode.playerConfig.PlayerHp == 0))) // 살아있는 모든 플레이어가 답을 선택했을 경우
        {
            PlayerAnswer.enableAnswerSelect = false;
            ProblemManager.instance.HideProblem();

            yield return MarkPlayerAnswer();

            // 맞힌 플레이어가 아무도 없을 경우 전투 모드 스킵
            if (correctPlayers.Count > 0)
                BattleManager.instance.BattleModeOn();
            else
                StartCoroutine(ProblemManager.instance.NextProblem());
        }
    }

    IEnumerator MarkPlayerAnswer()
    {
        correctPlayers = new List<PlayerAnswer>();

        foreach (PlayerAnswer player in playerAnswerList) {
            PlayerBattleMode pbm = player.player_battle_mode;

            if (player.inputAnswer == answerIndex) {
                correctPlayers.Add(player);
                player.AddCorrectCount();
                pbm.cursor.SetActive(true);
            }
            else if (pbm.playerConfig.PlayerHp > 0) {
                Debug.Log($"P{pbm.playerConfig.PlayerIndex + 1} 는 틀렸습니다.");
                pbm.PutAwayGun();
                pbm.Damage(BattleManager.instance.curEnemy.damage); 
            }
        }

        correctPlayers = correctPlayers.OrderBy(x => x.answerRank).ToList();
        for (int i = 0; i < correctPlayers.Count; i++) {
            correctPlayers[i].player_battle_mode.ObtainBullets(correctPlayers.Count - i);
            Debug.Log(
                $"P{correctPlayers[i].player_battle_mode.playerConfig.PlayerIndex + 1} 는 정답입니다!" +
                $"\n총알 {correctPlayers.Count - i}개 를 지급받습니다."
            );
        }
        yield break;
    }

    public void ResetPlayerAnswers() // 플레이어 답안과 랭크 초기화, 모든 플레이어의 답 선택 잠금
    {
        foreach (PlayerAnswer player in playerAnswerList) {
            player.inputAnswer = -1;
            player.answerRank = -1;

            player.player_battle_mode.ClearBullets();
            if (!player.player_battle_mode.IsPlayerDead())
                player.player_battle_mode.PutAwayGun();
        }
        answerIndex = -1;
        answerRank = 0;
    }
}

public enum AnswerButton { SouthButton, EastButton, NorthButton, WestButton };
