using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FinalScore : MonoBehaviour
{
    private GameObject goUI;
    private Text playerScoreText;
    private Text playerNameText;
    private Text playerProblemText;

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
        goUI.SetActive(true);


        // Update UI
        playerNameText.text = GetPlayerNameState();
        playerScoreText.text = GetPlayerScoreState();
        playerProblemText.text = GetPlayerProblemState();
    }

    private void Start()
    {
        goUI = GameObject.Find("GoUI");
        playerNameText = goUI.transform.Find("PlayerNameText").GetComponent<Text>();
        playerScoreText = goUI.transform.Find("PlayerScoreText").GetComponent<Text>();
        playerProblemText = goUI.transform.Find("PlayerProblemText").GetComponent<Text>();

        UpdatePlayerScore();
    }
}
