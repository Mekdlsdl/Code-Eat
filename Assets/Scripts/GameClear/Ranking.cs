using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Ranking : MonoBehaviour
{
    void OnEnable()
    {
        // ClearRank(); // 정확한 디버깅을 위해 임시로 추가
        UpdateRankings();
        DisplayRank();
    }

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

    private void InsertRank(int score)
    {
        for (int i = 0; i < 10; i++)
        {
            if (score > PlayerPrefs.GetInt("score" + i, 0))
            {
                for (int j = 9 - i; j > 0; j--)
                {
                    PlayerPrefs.SetInt("score" + j, PlayerPrefs.GetInt("score" + (j - 1), 0));
                }
                PlayerPrefs.SetInt("score" + i, score);
                PlayerPrefs.Save(); //갱신된 랭킹 저장
                break;
            }
        }
    }

    private void DisplayRank()
    {
        TextMeshProUGUI[] ranks = GetComponentsInChildren<TextMeshProUGUI>();

        int[] scoreArray = new int[10];
        for (int i = 0; i < 10; i++)
            scoreArray[i] = PlayerPrefs.GetInt("score" + i, 0);
        
        int[] uniqueScoreArray = new SortedSet<int>(scoreArray).ToArray();
        Array.Reverse(uniqueScoreArray);

        for (int i = 0; i < uniqueScoreArray.Length; i++)
        {
            int score = uniqueScoreArray[i];
            string textColor = (score == 0) ? "#BAB5B5" : "#A17171";   
            ranks[i].text = $"{i + 1}. <color={textColor}>{score}pt</color>";
        }
    }

    private void ClearRank()
    {
        for (int i = 0; i < 10; i++)
            PlayerPrefs.DeleteKey("score" + i);
    }
}
