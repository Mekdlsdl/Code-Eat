using UnityEngine;
using UnityEngine.InputSystem;

public class PauseNavigation : MonoBehaviour
{
    private PlayerConfiguration playerConfig;

    public void Init(PlayerConfiguration player_config)
    {
        playerConfig = player_config;
        player_config.Input.onDeviceLost += OnDeviceLost;
    }

    private void OnDeviceLost(PlayerInput playerInput)
    {
        if (PauseMenu.instance.gameObject.activeSelf && (playerConfig.PlayerIndex == PauseMenu.instance.menuPlayerIndex))
            PauseMenu.instance.CloseMenu();
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
