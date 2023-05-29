using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private List<PauseButton> buttonList;
    [SerializeField] private List<GameObject> settingsList;
    public static bool isPaused = false;
    public static MenuState menuState = MenuState.Pause; 
    private int menuIndex = 0;
    private bool isStickPushed = false;

    void Update()
    {
        if (menuState != MenuState.Pause) return;

        if ((Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Escape)))
        {
            if (!menu.activeSelf && !isPaused)
                OpenMenu();
            
            else if (menu.activeSelf && isPaused)
                CloseMenu();
            return;
        }
        else if ((menu.activeSelf && isPaused && (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.X))))
            CloseMenu();

        Navigate();

        if (menu.activeSelf && (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Z)))
            SelectMenu();
    }

    private void OpenMenu()
    {
        ResetMenu();
        Time.timeScale = 0;
        isPaused = true;

        menu.SetActive(true);
        DOTween.Rewind("OpenPauseMenu");
        DOTween.Play("OpenPauseMenu");

        SoundManager.instance.PauseBGM();
        SoundManager.instance.PlaySFX("Confirm");
    }
    public void CloseMenu()
    {
        DOTween.Rewind("ClosePauseMenu");
        DOTween.Play("ClosePauseMenu");

        Time.timeScale = 1;
        isPaused = false;

        SoundManager.instance.PlayBGM();
        SoundManager.instance.PlaySFX("Cancel");
    }
    private void SelectMenu()
    {
        settingsList[menuIndex].SetActive(true);
        SoundManager.instance.PlaySFX("OK");
    }
    private void Navigate()
    {
        buttonList[menuIndex].NormalizeButton();
        float verticalInput = Input.GetAxis("Vertical");

        if ((!isStickPushed && verticalInput == 1f) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            isStickPushed = true;
            menuIndex--;
            SoundManager.instance.PlaySFX("Cursor");
        }
        else if ((!isStickPushed && verticalInput == -1f) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isStickPushed = true;
            menuIndex++;
            SoundManager.instance.PlaySFX("Cursor");
        }
        else if (verticalInput == 0f)
            isStickPushed = false;

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
}

public enum MenuState { Pause, Resolution, Sound, Exit };
