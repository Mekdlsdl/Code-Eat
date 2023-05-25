using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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

        if (!isStickPushed && (verticalInput == 1f || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            isStickPushed = true;
            menuIndex--;
        }
        else if (!isStickPushed && (verticalInput == -1f || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            isStickPushed = true;
            menuIndex++;
        }
        else if (verticalInput == 0f)
            isStickPushed = false;

        menuIndex = Mathf.Clamp(menuIndex, 0, 1);
        HighlightButton();
    }

    private void SelectOption()
    {
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
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    private void Exit()
    {
        PauseMenu.menuState = MenuState.Pause;
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        PauseMenu.menuState = MenuState.Pause;
    }
}
