using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnswer : MonoBehaviour
{
    [System.NonSerialized] public int inputAnswer = -1;
    [System.NonSerialized] public int answerRank = -1;
    public static bool enableAnswerSelect = false;
    
    private PlayerConfiguration playerConfig;

    private PlayerBattleMode playerBattleMode;
    public PlayerBattleMode player_battle_mode => playerBattleMode;

    public void Init(PlayerConfiguration player_config, PlayerBattleMode pbm)
    {
        playerConfig = player_config;
        playerBattleMode = pbm;
    }

    void Update()
    {
        if (!GameManager.isProblemMode || !enableAnswerSelect || (inputAnswer != -1) || (playerBattleMode.playerConfig.PlayerHp == 0))
            return;
        
        if (PressKey(InputType.SOUTHBUTTON))
            inputAnswer = (int) AnswerButton.SouthButton;
        
        else if (PressKey(InputType.EASTBUTTON))
            inputAnswer = (int) AnswerButton.EastButton;
        
        else if (PressKey(InputType.NORTHBUTTON))
            inputAnswer = (int) AnswerButton.NorthButton;
        
        else if (PressKey(InputType.WESTBUTTON))
            inputAnswer = (int) AnswerButton.WestButton;
        
        if (inputAnswer != -1) {
            Debug.Log($"P{playerConfig.PlayerIndex + 1}이 선택한 답 : {(AnswerButton) inputAnswer}");
            AnswerManager.instance.LockPlayerAnswer(this);
        }
    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }

    public void AddCorrectCount()
    {
        playerConfig.CorrectProblemCount++;
    }
}
