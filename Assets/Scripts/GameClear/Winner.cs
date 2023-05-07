using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Winner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;

    void UpdateWinner()
    {
        // Get the winner
        PlayerConfiguration winner = GetWinner();
        if (winner != null)
        {
            // 우승자 출력
            winnerText.text = $"P {winner.PlayerIndex + 1} 이(가) 우승자입니다!";
        }
        else
        {
            // 우승자가 없는 경우
            winnerText.text = "우승자가 없습니다.";
        }
    }

    private PlayerConfiguration GetWinner()
    {
        List<PlayerConfiguration> players = PlayerConfigManager.instance.PlayerConfigs;

        PlayerConfiguration winner = null;
        int highestScore = int.MinValue;
        int highestCorrectAnswers = int.MinValue;

        foreach (PlayerConfiguration playerConfig in players)
        {
            int score = playerConfig.PlayerScore;
            int correctAnswers = playerConfig.CorrectProblemCount;

            if (score > highestScore)
            {
                //우승자 선정
                highestScore = score;
                highestCorrectAnswers = correctAnswers;
                winner = playerConfig;
            }
            else if (score == highestScore) // 동점인 경우, 정답 개수에 따라 우승자 결정
            {
                // 정답 개수 비교
                if (correctAnswers > highestCorrectAnswers)
                {
                    // 동점자들 중 우승자 선정
                    highestCorrectAnswers = correctAnswers;
                    winner = playerConfig;
                }
            }
        }

        return winner;
    }

    void Start()
    {
        UpdateWinner();
    }
}
