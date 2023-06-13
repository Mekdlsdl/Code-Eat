using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExitMenu : MonoBehaviour
{
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] List<TextMeshProUGUI> buttons;
    [SerializeField] private Color highlightColor, defaultColor, disabledColor;
    private int menuIndex = 0;

    void OnEnable()
    {
        if (GameManager.instance.CheckActiveScene("StartingMenu"))
            buttons[1].color = disabledColor;

        PauseMenu.menuState = MenuState.Exit;
        NormalizeButton();
        menuIndex = 0;
        HighlightButton();
    }

    void OnDisable()
    {
        buttons[1].color = defaultColor;
        PauseMenu.menuState = MenuState.Pause;
    }

    public void ExitMenuNavigate(PlayerConfiguration playerConfig)
    {
        if (!PauseMenu.isPaused  || playerConfig.PlayerIndex != PauseMenu.instance.menuPlayerIndex) return;

        Navigate(playerConfig);

        if (PressKey(playerConfig, InputType.SOUTHBUTTON))
            SelectOption();
        
        else if (PressKey(playerConfig, InputType.EASTBUTTON))
            Exit();
    }
    private void Navigate(PlayerConfiguration playerConfig)
    {
        NormalizeButton();

        if (PressKey(playerConfig, InputType.UP))
        {
            menuIndex--;
            SoundManager.instance.PlaySFX("Cursor");
            
            if (ExcludeMapSelect()) menuIndex--;
        }
        else if (PressKey(playerConfig, InputType.DOWN))
        {
            menuIndex++;
            SoundManager.instance.PlaySFX("Cursor");

            if (ExcludeMapSelect()) menuIndex++;
        }
        menuIndex = Mathf.Clamp(menuIndex, 0, buttons.Count - 1);
        HighlightButton();
    }

    private void SelectOption()
    {
        SoundManager.instance.PlaySFX("OK");

        if (menuIndex == 0) {
            gameObject.SetActive(false);
            pauseMenu.CloseMenu();
            GameManager.instance.ReturnToCharacterSelect();
        }
        
        else if (menuIndex == 1) {
            gameObject.SetActive(false);
            pauseMenu.CloseMenu();
            GameManager.instance.ReturnToMapSelectMode();
        }
        
        else if  (menuIndex == 2)
            QuitGame();
    }

    private bool ExcludeMapSelect()
    {
        return ((menuIndex == 1) && GameManager.instance.CheckActiveScene("StartingMenu"));
    }

    private void HighlightButton()
    {
        buttons[menuIndex].color = highlightColor;
    }

    private void NormalizeButton()
    {
        buttons[menuIndex].color = defaultColor;
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        #else
        Application.Quit();

        #endif
    }

    private void Exit()
    {
        PauseMenu.menuState = MenuState.Pause;
        gameObject.SetActive(false);
        SoundManager.instance.PlaySFX("Cancel");
    }

    private bool PressKey(PlayerConfiguration playerConfig, string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}
