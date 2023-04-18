using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    public static AnswerManager instance { get; private set; }
    
    [SerializeField] private List<PlayerBattleMode> playerBattleList = new List<PlayerBattleMode>();
    public List<PlayerBattleMode> PlayerBattleList => playerBattleList;

    private int answerIndex, answerRank;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void AddToBattlePlayerList(PlayerBattleMode player)
    {
        playerBattleList.Add(player);
    }

    public void SetProblemAnswer(int index)
    {
        answerIndex = index;
    }

    public int ReturnAnswerIndex()
    {
        return answerIndex;
    }

    // 플레이어가 답을 눌렀을 때 호출
    public void SetAnswerRank(PlayerBattleMode player)
    {
        player.answerRank = answerRank;
        answerRank ++;

        TryMarkPlayerAnswer();
    }

    
    private void TryMarkPlayerAnswer()
    {
        if (playerBattleList.All(p => p.inputAnswer != -1))
        {
            MarkPlayerAnswer();
        }
    }

    private void MarkPlayerAnswer()
    {
        List<PlayerBattleMode> correctPlayers = new List<PlayerBattleMode>();

        foreach (PlayerBattleMode player in playerBattleList) {
            if (player.inputAnswer == answerIndex)
                correctPlayers.Add(player);
        }

        correctPlayers = correctPlayers.OrderBy(x => x.answerRank).ToList();
        for (int i = correctPlayers.Count - 1; i >= 0; i--) {
            correctPlayers[i].ObtainBullets(i + 1);
        }
    }

    // 한 문제 풀이가 끝났을 경우 호출
    public void ResetPlayerAnswers()
    {
        foreach (PlayerBattleMode player in playerBattleList) {
            player.inputAnswer = -1;
            player.answerRank = -1;
        }
        answerIndex = -1;
        answerRank = 0;
    }
}

public enum AnswerButton { SouthButton, EastButton, NorthButton, WestButton };
