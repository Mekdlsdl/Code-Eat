using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance { get; private set; }
    public static bool isPaused = false;
    public static MenuState menuState = MenuState.Pause; 

    public ResolutionMenu resolutionMenu;
    public SoundMenu soundMenu;
    public ExitMenu exitMenu;

    [SerializeField] private GameObject menu;
    [SerializeField] private List<PauseButton> buttonList;
    [SerializeField] private List<GameObject> settingsList;

    public int menuPlayerIndex = 0;
    private int menuIndex = 0;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void PauseMenuNavigate(PlayerConfiguration playerConfig)
    {
        if (menuState != MenuState.Pause) return;

        if (PressKey(playerConfig, InputType.PAUSE))
        {
            if (!menu.activeSelf && !isPaused) {
                // Debug.Log($"{playerConfig.PlayerIndex}이 누름");
                menuPlayerIndex = playerConfig.PlayerIndex;

                if (playerConfig.PlayerIndex == menuPlayerIndex) {
                    OpenMenu();
                }
            }
                
            else if (menu.activeSelf && isPaused) {
                if (playerConfig.PlayerIndex == menuPlayerIndex) {
                    CloseMenu();
                }
            }
            return;
        }
        else if (menu.activeSelf && isPaused && PressKey(playerConfig, InputType.EASTBUTTON)) {
            if (playerConfig.PlayerIndex == menuPlayerIndex) {
                CloseMenu();
            }
        }

        if (playerConfig.PlayerIndex == menuPlayerIndex) {
            Navigate(playerConfig);
        }

        if (menu.activeSelf && PressKey(playerConfig, InputType.SOUTHBUTTON))
            if (playerConfig.PlayerIndex == menuPlayerIndex) {
                SelectMenu();
            }
    }

    private void OpenMenu()
    {
        ResetMenu();
        Time.timeScale = 0;
        isPaused = true;

        menu.SetActive(true);
        DOTween.Rewind("OpenPauseMenu");
        DOTween.Play("OpenPauseMenu");

        SoundManager.instance.PlaySFX("Confirm");
    }
    public void CloseMenu()
    {
        DOTween.Rewind("ClosePauseMenu");
        DOTween.Play("ClosePauseMenu");

        Time.timeScale = 1;
        isPaused = false;

        SoundManager.instance.PlaySFX("Cancel");
    }
    private void SelectMenu()
    {
        settingsList[menuIndex].SetActive(true);
        SoundManager.instance.PlaySFX("OK");
    }
    private void Navigate(PlayerConfiguration playerConfig)
    {
        if (!isPaused) return;
        
        buttonList[menuIndex].NormalizeButton();

        if (PressKey(playerConfig, InputType.UP))
        {
            menuIndex--;
            SoundManager.instance.PlaySFX("Cursor");
        }
        else if (PressKey(playerConfig, InputType.DOWN))
        {
            menuIndex++;
            SoundManager.instance.PlaySFX("Cursor");
        }

        menuIndex = Mathf.Clamp(menuIndex, 0, buttonList.Count - 1);
        buttonList[menuIndex].HighlightButton();
    }
    private void ResetMenu()
    {
        for (int i = 0; i < buttonList.Count; i++) {
            buttonList[i].NormalizeButton();
            settingsList[i].SetActive(false);
        }
        buttonList[0].HighlightButton();

        menuIndex = 0;
    }

    private bool PressKey(PlayerConfiguration playerConfig, string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}

public enum MenuState { Pause, Resolution, Sound, Exit };
