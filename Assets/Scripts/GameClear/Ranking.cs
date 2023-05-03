using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Ranking : MonoBehaviour
{
    private TextMesh rankingTextUI;
    private TextMesh rankingText;

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
                    InsertRank(score);
                    rankingString += $"{rank}. P{playerConfig.PlayerIndex + 1}: {score}\n";
                    rank++;
                    break;
                }
            }
        }

        rankingTextUI.text = rankingString;
    }

    public void InsertRank(int Score)
    {
        for (int i = 0; i < 10; i++)
        {
            if (Score > PlayerPrefs.GetInt(i.ToString()))
            {
                for (int j = 9 - i; j > 0; j--)
                {
                    PlayerPrefs.SetInt(j.ToString(), PlayerPrefs.GetInt((j - 1).ToString()));
                }
                PlayerPrefs.SetInt(i.ToString(), Score);
                break;
            }
        }

        rankingText.text = "Ranking\n\n" +
            "1. " + PlayerPrefs.GetInt("0") + "\n\n" +
            "2. " + PlayerPrefs.GetInt("1") + "\n\n" +
            "3. " + PlayerPrefs.GetInt("2") + "\n\n" +
            "4. " + PlayerPrefs.GetInt("3") + "\n\n" +
            "5. " + PlayerPrefs.GetInt("4") + "\n\n" +
            "6. " + PlayerPrefs.GetInt("5") + "\n\n" +
            "7. " + PlayerPrefs.GetInt("6") + "\n\n" +
            "8. " + PlayerPrefs.GetInt("7") + "\n\n" +
            "9. " + PlayerPrefs.GetInt("8") + "\n\n" +
            "10. " + PlayerPrefs.GetInt("9");
    }

    void Start()
    {
        rankingTextUI = FindObjectOfType<TextMesh>();
        rankingText = GameObject.Find("RankingText").GetComponent<TextMesh>();

        UpdateRankings();

    }
}
