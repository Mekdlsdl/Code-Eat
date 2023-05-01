using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreResultsWindow : MonoBehaviour
{
    public Text playerNameText;
    public Text playerScoreText;
    public Text playerProblemText;
    public Text rankingTextUI;


    public string GetPlayerNameState()
    {
        string playerNameState = "";

        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
        {
            playerNameState += $"P{playerConfig.PlayerIndex + 1}\n";
        }

        return playerNameState;
    }

    public string GetPlayerProblemState()
    {
        string playerProblemState = "";

        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
        {
            playerProblemState += $"{playerConfig.CorrectProblemCount} / {ProblemManager.totalProblemCount}\n";
        }

        return playerProblemState;
    }


    public string GetPlayerScoreState()
    {
        string playerScoreState = "";

        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
        {
            int score = playerConfig.PlayerScore;
            playerScoreState += $"{score}\n";
        }
        

        return playerScoreState;
    }

    public void UpdatePlayerScore()
    {

        // Update UI
        playerNameText.text = GetPlayerNameState();
        playerScoreText.text = GetPlayerScoreState();
        playerProblemText.text = GetPlayerProblemState();


        // 랭킹 업데이트
        UpdateRankings();
    }

    public void UpdateRankings()
    {
        // 플레이어 점수 리스트 생성
        List<int> playerScores = new List<int>();
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
        {
            playerScores.Add(playerConfig.PlayerScore);
        }

        // 내림차순 정렬
        playerScores.Sort();
        playerScores.Reverse();

        // 랭킹을 보여주는 문자열 생성
        string rankingString = "";
        int rank = 1;
        foreach (int score in playerScores)
        {
            if (rank > 10) break; // top10만 보여주도록 설정
            foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
            {
                if (playerConfig.PlayerScore == score)
                {
                    rankingString += $"{rank}. P{playerConfig.PlayerIndex + 1}: {score}\n";
                    rank++;
                    break;
                }
            }
        }

        rankingTextUI.text = rankingString;
    }


    void Start()
    {
    }
}
