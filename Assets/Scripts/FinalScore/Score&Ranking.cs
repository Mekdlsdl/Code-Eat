using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreResultsWindow : MonoBehaviour
{
    public Text[] playerScoreTexts;
    public Text percentageIncorrectText;
    public Text rankingTextUI;
    public Text playerProbNumTexts;

    private int[] playerNumQuestions;
    private int[] playerNumIncorrect;
    private List<PlayerScore> playerScores = new List<PlayerScore>(); //플레이어 점수 참조한 PlayerScore 리스트

    private class PlayerScore
    {
        public string playerName;
        public int score;
    }

    public void UpdatePlayerScore(int playerNumber, int numQuestions, int numIncorrect)
    {
        // 플레이어 점수 리스트 업데이트
        string playerName = "플레이어 " + playerNumber;

        //각 플레이어가 획득한 총 점수 변수 참조 (?)
        PlayerBattleMode player = transform.parent.GetComponent<PlayerBattleMode>();
        int score = player.playerConfig.PlayerScore; 
        
        PlayerScore playerScore = new PlayerScore { playerName = playerName, score = score };
        playerScores.Add(playerScore);

        // 각 플레이어 별 푼 문제 개수 & 맞힌 문제 개수
        int totalQuestions = 0;
        int totalIncorrect = 0;
        foreach (var ps in playerScores)
        {
            if (ps.playerName == playerName)
            {
                totalQuestions += numQuestions;
                totalIncorrect += numIncorrect;
            }
        }

        // Update UI
        playerNumQuestions[playerNumber - 1] = totalQuestions;
        playerNumIncorrect[playerNumber - 1] = totalIncorrect;
        playerScoreTexts[playerNumber - 1].text = "플레이어 " + playerNumber + " 점수: " + score.ToString();
        playerProbNumTexts.text = "오답 개수 " + totalIncorrect + " / " + totalQuestions;


        // 랭킹 업데이트
        UpdateRankings();
    }

    private void UpdateRankings()
    {
        // 점수에 따른 플레이어 점수 내람차순 정렬
        playerScores.Sort((a, b) => b.score.CompareTo(a.score));

        // 랭킹 업데이트 (top 10)
        string rankingText = "랭킹:\n";
        for (int i = 0; i < Mathf.Min(playerScores.Count, 10); i++)
        {
            rankingText += (i + 1) + ". " + playerScores[i].playerName + " - " + playerScores[i].score + "\n";
        }
        rankingText.TrimEnd('\n');
        rankingTextUI.text = rankingText; // UI text component에 랭킹 텍스트 표시
    }

    void Start()
    {
        playerNumQuestions = new int[playerScoreTexts.Length];
        playerNumIncorrect = new int[playerScoreTexts.Length];
    }
}
