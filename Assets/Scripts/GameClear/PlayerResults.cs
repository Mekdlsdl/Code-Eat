using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerResults : MonoBehaviour
{
    public Animator PlayerAnim;
    public RectTransform PlayerTransform;
    public GameObject CelebrateEffect;

    private Winner winnerControl;
    private PlayerConfiguration playerConfig;
    public PlayerConfiguration PlayerConfig => playerConfig;

    [SerializeField] private TextMeshProUGUI score_text, player_text, correct_text;


    public void Init(PlayerConfiguration player_config, Winner winner_control)
    {
        winnerControl = winner_control;
        playerConfig = player_config;
        PlayerAnim.runtimeAnimatorController = GameManager.instance.GetResultAnimControl(playerConfig.CharacterTypeIndex);

        int playerIndex = player_config.PlayerIndex;
        int score = player_config.PlayerScore;

        int total = ProblemManager.totalProblemCount;
        int correct = player_config.CorrectProblemCount;
        double incorrect = ((total - correct) / (double)total) * 100;

        score_text.text = $"{score}pt";
        player_text.text = $"P{playerIndex + 1}";
        correct_text.text = $"{correct} / {total}\n 오답률 : {incorrect:N0}%";

        player_text.color = GameManager.instance.PlayerColors[player_config.PlayerIndex];
    }

    void Update()
    {
        if (!winnerControl.canExitResults) return;
        
        if (PressKey(InputType.EASTBUTTON) || PressKey(InputType.SOUTHBUTTON) || PressKey(InputType.WESTBUTTON) || PressKey(InputType.NORTHBUTTON)) {
            GameManager.instance.ReturnToMapSelectMode();
        }
    }

    public void ShowScore()
    {
        score_text.gameObject.SetActive(true);
        correct_text.gameObject.SetActive(false);
    }

    public void ShowStat()
    {
        correct_text.gameObject.SetActive(true);
        score_text.gameObject.SetActive(false);
    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}
