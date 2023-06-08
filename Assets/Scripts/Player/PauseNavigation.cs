using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseNavigation : MonoBehaviour
{
    private PlayerConfiguration playerConfig;

    public void Init(PlayerConfiguration player_config)
    {
        playerConfig = player_config;
    }

    void Update()
    {
        if (playerConfig == null) return;
    
        switch (PauseMenu.menuState) {
            case (MenuState.Pause):
                PauseMenu.instance.PauseMenuNavigate(playerConfig);
                break;
            
            case (MenuState.Resolution):
                PauseMenu.instance.resolutionMenu.ResolutionMenuNavigate(playerConfig);
                break;
            
            case (MenuState.Sound):
                PauseMenu.instance.soundMenu.SoundMenuNavigate(playerConfig);
                break;
            
            case (MenuState.Exit):
                PauseMenu.instance.exitMenu.ExitMenuNavigate(playerConfig);
                break;
            
            default:
                break;
        }
    }
}
