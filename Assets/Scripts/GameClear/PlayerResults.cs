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

    [SerializeField] private TextMeshProUGUI score_text, player_text, correct_text, hit_shot_text, critical_shot_text;
    private List<TextMeshProUGUI> result_text_list;

    public void Init(PlayerConfiguration player_config, Winner winner_control)
    {
        winnerControl = winner_control;
        playerConfig = player_config;
        PlayerAnim.runtimeAnimatorController = GameManager.instance.GetResultAnimControl(playerConfig.CharacterTypeIndex);

        int playerIndex = player_config.PlayerIndex;
        int score = player_config.PlayerScore;

        int total = player_config.TotalProblemCount;
        int correct = player_config.CorrectProblemCount;
        double incorrect = (total != 0) ? (((total - correct) / (double)total) * 100) : 0;

        int totalShotCount = player_config.TotalShotCount;
        int hitShotCount = player_config.HitShotCount;
        int criticalShotCount = player_config.CriticalShotCount;

        double hitShotRate = (totalShotCount != 0) ? (hitShotCount / (double)totalShotCount * 100) : 0;
        double criticalShotRate = (totalShotCount != 0) ? (criticalShotCount / (double)totalShotCount * 100) : 0;

        score_text.text = $"{score}pt";
        player_text.text = $"P{playerIndex + 1}";
        correct_text.text = $"{correct} / {total}\n 오답률 : {incorrect:N0}%";
        hit_shot_text.text = $"{hitShotCount} / {totalShotCount}\n 공격 성공률\n : {hitShotRate:N0}%";
        critical_shot_text.text = $"{criticalShotCount} / {totalShotCount}\n 치명타 : {criticalShotRate:N0}%";

        result_text_list = new List<TextMeshProUGUI>() {score_text, correct_text,  hit_shot_text, critical_shot_text};

        player_text.color = GameManager.instance.PlayerColors[player_config.PlayerIndex];
    }

    void Update()
    {
        if (!winnerControl.canExitResults || PauseMenu.isPaused) return;
        
        if (PressKey(InputType.EASTBUTTON) || PressKey(InputType.SOUTHBUTTON) || PressKey(InputType.WESTBUTTON) || PressKey(InputType.NORTHBUTTON)) {
            GameManager.instance.ReturnToMapSelectMode();
        }
    }

    public void ShowScore()
    {
        HideResultText();
        score_text.gameObject.SetActive(true);
    }

    public void ShowStat()
    {
        HideResultText();
        correct_text.gameObject.SetActive(true);
    }

    public void ShowHitShot()
    {
        HideResultText();
        hit_shot_text.gameObject.SetActive(true);
    }

    public void ShowCriticalShot()
    {
        HideResultText();
        critical_shot_text.gameObject.SetActive(true);
    }

    private void HideResultText()
    {
        foreach (var result in result_text_list)
        {
            result.gameObject.SetActive(false);
        }
    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}
