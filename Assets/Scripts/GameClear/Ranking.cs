using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Ranking : MonoBehaviour
{
    public TextMeshProUGUI rankingText;

    public void UpdateRankings()
    {
        // 플레이어 점수 리스트 생성
        List<int> playerScores = new List<int>();
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
        {
            playerScores.Add(playerConfig.PlayerScore);
        }

        // 각 플레이어의 점수를 InsertRank 메소드로 전달
        foreach (int score in playerScores)
        {
            InsertRank(score);
        }
    }

    public void InsertRank(int Score)
    {
        for (int i = 0; i < 10; i++)
        {
            if (Score > PlayerPrefs.GetInt("score" + i, 0))
            {
                for (int j = 9 - i; j > 0; j--)
                {
                    PlayerPrefs.SetInt("score" + j, PlayerPrefs.GetInt("score" + (j - 1), 0));
                }
                PlayerPrefs.SetInt("score" + i, Score);
                PlayerPrefs.Save(); //갱신된 랭킹 저장
                break;
            }
        }
    }

    void Start()
    {
        string rankingString = "";
        for (int i = 0; i < 10; i++)
        {
            int score = PlayerPrefs.GetInt("score" + i, 0);
            if (score > 0)
            {
                rankingString += (i + 1) + ". " + score + "\n";
            }
            else
            {
                break;
            }
        }
        rankingText.text = rankingString;
    }
}
