using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;


public class Winner : MonoBehaviour
{
    [SerializeField] private GameObject leaderBoard, exitText;
    [SerializeField] private TextMeshProUGUI winnerText;

    [System.NonSerialized] public bool canExitResults = false;
    
    public IEnumerator ShowWinner(float delay)
    {
        
        yield return new WaitForSeconds(delay);

        winnerText.text = "최종 우승자는...";
        DOTween.Rewind("ShowWinnerText");
        DOTween.Play("ShowWinnerText");

        yield return new WaitForSeconds(1.5f);

        UpdateWinner();
        SoundManager.instance.PlaySFX("Winner");
        DOTween.Rewind("ShowWinnerText");
        DOTween.Play("ShowWinnerText");

        yield return new WaitForSeconds(1.5f);

        SpawnPlayerResult.instance.ShowAllPlayerScores();
        SoundManager.instance.PlaySFX("Cursor");

        yield return new WaitForSeconds(1.5f);

        SpawnPlayerResult.instance.ShowAllPlayerStats();
        SoundManager.instance.PlaySFX("Cursor");

        yield return new WaitForSeconds(4f);

        SpawnPlayerResult.instance.ShowAllPlayerHitShot();
        SoundManager.instance.PlaySFX("Cursor");

        yield return new WaitForSeconds(4f);

        SpawnPlayerResult.instance.ShowAllPlayerCriticalShot();
        SoundManager.instance.PlaySFX("Cursor");

        yield return new WaitForSeconds(4f);

        leaderBoard.SetActive(true);
        SoundManager.instance.PlaySFX("Change");
        
        yield return new WaitForSeconds(0.5f);

        exitText.SetActive(true);
        canExitResults = true;
    }

    private void UpdateWinner()
    {
        // Get the winner
        List<PlayerConfiguration> winners = GetWinner();

        string output_text = "우승자는 ";

        for (int i = 0; i < winners.Count; i++) {
            output_text += $"<color={GameManager.instance.ReturnColorHex(winners[i].PlayerIndex)}>P{winners[i].PlayerIndex + 1}</color>";
            if (i < winners.Count - 1) output_text += ", ";    
        }
        output_text += "입니다!";
        
        winnerText.text = output_text;
        SpawnPlayerResult.instance.PlayerResponseAnimation(winners);
    }

    private List<PlayerConfiguration> GetWinner()
    {
        List<PlayerConfiguration> players = PlayerConfigManager.instance.PlayerConfigs;
        List<PlayerConfiguration> winners = new List<PlayerConfiguration>();

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
                winners.Clear();
                winners.Add(playerConfig);
            }
            else if (score == highestScore) // 동점인 경우, 정답 개수에 따라 우승자 결정
            {
                // 정답 개수 비교
                if (correctAnswers > highestCorrectAnswers)
                {
                    // 동점자들 중 우승자 선정
                    highestCorrectAnswers = correctAnswers;
                    winners.Clear();
                    winners.Add(playerConfig);
                }
                else if (correctAnswers == highestCorrectAnswers) // 정답 개수까지 동일할 경우
                {
                    // 우승자 2명 이상 선정
                    winners.Add(playerConfig);
                }
            }
        }
        return winners;
    }
}
