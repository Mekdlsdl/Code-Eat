using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExitMenu : MonoBehaviour
{
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] List<TextMeshProUGUI> buttons;
    [SerializeField] private Color highlightColor, defaultColor;
    private int menuIndex = 0;
    private bool isStickPushed = false;

    void OnEnable()
    {
        PauseMenu.menuState = MenuState.Exit;
        NormalizeButton();
        menuIndex = 0;
        HighlightButton();
    }

    void Update()
    {
        if (!PauseMenu.isPaused) return;

        Navigate();

        if ((Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Z)))
            SelectOption();
        
        if ((Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.X)))
            Exit();
    }
    private void Navigate()
    {
        NormalizeButton();
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

        menuIndex = Mathf.Clamp(menuIndex, 0, 1);
        HighlightButton();
    }

    private void SelectOption()
    {
        SoundManager.instance.PlaySFX("OK");

        if (menuIndex == 0)
            QuitGame();
        
        else if  (menuIndex == 1) {
            gameObject.SetActive(false);
            pauseMenu.CloseMenu();
            GameManager.instance.ReturnToCharacterSelect();
        }
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

    void OnDisable()
    {
        PauseMenu.menuState = MenuState.Pause;
    }
}
