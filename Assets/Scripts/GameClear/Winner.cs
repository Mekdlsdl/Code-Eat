using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Winner : MonoBehaviour
{
    private TextMesh winnerTextUI;
    private Text winnerText;

    void UpdateWinner()
    {
       // 우승자 출력
       PlayerConfiguration winner = GetWinner();
       if (winner != null)
       {
           Debug.Log($"P {winner.PlayerIndex + 1} 이(가) 우승자입니다!");
       }
       else
       {
           Debug.Log("우승자가 없습니다.");
       }
   }

   private PlayerConfiguration GetWinner()
   {
       List<PlayerConfiguration> players = PlayerConfigManager.instance.PlayerConfigs;

       PlayerConfiguration winner = null;
       int highestScore = int.MinValue;

       foreach (PlayerConfiguration playerConfig in players)
       {
           int score = playerConfig.PlayerScore;
           if (score > highestScore)
           {
               highestScore = score;
               winner = playerConfig;
           }
       }

       return winner;
   }
    void Start()
    {
        winnerText = winnerTextUI.transform.Find("WinnerText").GetComponent<Text>();

        UpdateWinner();
    }
}
