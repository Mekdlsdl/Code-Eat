using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Ranking : MonoBehaviour
{
    [SerializeField] private bool isDebug = false;
    private List<int> mergedScores;
    void OnEnable()
    {
        if (isDebug) ClearRank();
        
        UpdateRankings();
    }

    private void UpdateRankings()
    {
        List<int> existingScores = new List<int>();
        for (int i = 0; i < 10; i++)
            existingScores.Add(PlayerPrefs.GetInt("score" + i, 0));
        
        List<int> playerScores = new List<int>();
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
            playerScores.Add(playerConfig.PlayerScore);
        
        // 이전 리더보드의 점수와 현재 플레이어들의 점수를 통합
        List<int> mergedScores = existingScores.Concat(playerScores).ToList();
        
        // 중복되는 플레이어 점수 제거
        int[] uniqueScoreArray = new SortedSet<int>(mergedScores).ToArray();
        Array.Reverse(uniqueScoreArray);

        // 표시할 점수 개수를 10개로 한정
        Array.Resize(ref uniqueScoreArray, 10);

        // 새롭게 갱신된 리더보드 상태 저장
        for (int i = 0; i < 10; i++)
            PlayerPrefs.SetInt("score" + i, uniqueScoreArray[i]);
        PlayerPrefs.Save();

        // 리더보드 표시
        DisplayRank(uniqueScoreArray);
    }

    // private void UpdateRankings()
    // {
    //     // 플레이어 점수 리스트 생성
    //     List<int> playerScores = new List<int>();
    //     foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
    //     {
    //         playerScores.Add(playerConfig.PlayerScore);
    //     }

    //     // 각 플레이어의 점수를 InsertRank 메소드로 전달
    //     foreach (int score in playerScores)
    //     {
    //         InsertRank(score);
    //     }
    // }

    // private void InsertRank(int score)
    // {
    //     for (int i = 0; i < 10; i++)
    //     {
    //         if (score > PlayerPrefs.GetInt("score" + i, 0))
    //         {
    //             for (int j = 9 - i; j > 0; j--)
    //             {
    //                 PlayerPrefs.SetInt("score" + j, PlayerPrefs.GetInt("score" + (j - 1), 0));
    //             }
    //             PlayerPrefs.SetInt("score" + i, score);
    //             PlayerPrefs.Save(); //갱신된 랭킹 저장
    //             break;
    //         }
    //     }
    // }

    private void DisplayRank(int[] scores)
    {
        TextMeshProUGUI[] ranks = GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < 10; i++)
        {
            string textColor = (scores[i] == 0) ? "#BAB5B5" : "#A17171";   
            ranks[i].text = $"{i + 1}. <color={textColor}>{scores[i]}pt</color>";
        }
    }

    private void ClearRank()
    {
        Debug.Log("Clearing Ranks");
        for (int i = 0; i < 10; i++)
            PlayerPrefs.DeleteKey("score" + i);
    }
}
