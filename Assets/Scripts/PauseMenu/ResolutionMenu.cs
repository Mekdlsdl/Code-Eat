using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class ResolutionMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI languageText, resolutionText;
    [SerializeField] private GameObject checkMark;
    [SerializeField] private Color highlightColor, defaultColor;
    [SerializeField] private List<TextMeshProUGUI> options;

    public delegate void SetLanguageDelegate();
    public SetLanguageDelegate setUiLanguage;

    private int menuIndex = 0;
    private int resolutionNumber;
    private int languageNumber = 0;

    private (int, int) selectedResolution;

    private bool isFullScreen = false;

    void Start()
    {
        for (int i = 4; i <= 8; i++) {
            if (Screen.currentResolution.width == 240 * i) {
                resolutionNumber = i;
                SetResolution();
                break;
            }
        }
        isFullScreen = Screen.fullScreen;
        checkMark.SetActive(isFullScreen);
    }

    void OnEnable()
    {
        NormalizeButton();
        menuIndex = 0;
        HighlightButton();
        PauseMenu.menuState = MenuState.Resolution;
    }
    
    void OnDisable()
    {
        PauseMenu.menuState = MenuState.Pause;
    }

    public void ResolutionMenuNavigate(PlayerConfiguration playerConfig)
    {
        if (!PauseMenu.isPaused || playerConfig.PlayerIndex != PauseMenu.instance.menuPlayerIndex) return;

        if (PressKey(playerConfig, InputType.SOUTHBUTTON)) {
            SetFullScreen(!isFullScreen);
        }
        
        else if (PressKey(playerConfig, InputType.EASTBUTTON)) {
            ExitMenu();
        }
        Navigate(playerConfig);
    }

    private void Navigate(PlayerConfiguration playerConfig)
    {
        if (PressKey(playerConfig, InputType.UP))
        {
            NormalizeButton();
            menuIndex = 0;
            SoundManager.instance.PlaySFX("Cursor");
            HighlightButton();
        }
        else if (PressKey(playerConfig, InputType.DOWN))
        {
            NormalizeButton();
            menuIndex = 1;
            SoundManager.instance.PlaySFX("Cursor");
            HighlightButton();
        }
        else if (PressKey(playerConfig, InputType.LEFT))
        {
            if (menuIndex == 0) {
                languageNumber--;
                SetLanguage();

                DOTween.Rewind("LeftLanguageArrow");
                DOTween.Play("LeftLanguageArrow");
            }
            else if (menuIndex == 1) {
                resolutionNumber--;
                SetResolution();
                
                DOTween.Rewind("LeftResolutionArrow");
                DOTween.Play("LeftResolutionArrow");
            }
            SoundManager.instance.PlaySFX("Cursor");
        }
        else if (PressKey(playerConfig, InputType.RIGHT))
        {
            if (menuIndex == 0) {
                languageNumber++;
                SetLanguage();

                DOTween.Rewind("RightLanguageArrow");
                DOTween.Play("RightLanguageArrow");
            }
            else if (menuIndex == 1) {
                resolutionNumber++;
                SetResolution();

                DOTween.Rewind("RightResolutionArrow");
                DOTween.Play("RightResolutionArrow");
            }
            SoundManager.instance.PlaySFX("Cursor");
        }
    }

    private void HighlightButton()
    {
        options[menuIndex].color = highlightColor;
    }

    private void NormalizeButton()
    {
        options[menuIndex].color = defaultColor;
    }

    private void SetResolution()
    {
        resolutionNumber = Mathf.Clamp(resolutionNumber, 4, 8);
        selectedResolution = (240 * resolutionNumber, 135 * resolutionNumber);
        resolutionText.text = $"{selectedResolution.Item1} x {selectedResolution.Item2}";
        Screen.SetResolution(selectedResolution.Item1, selectedResolution.Item2, Screen.fullScreen);
    }

    private void SetLanguage()
    {
        Resources.UnloadUnusedAssets();
        LocalizationManager.instance.currentLanguage = languageNumber = Mathf.Clamp(languageNumber, 0, 1);
        languageText.text = LocalizationManager.instance.translationDict["Language"][languageNumber];
        setUiLanguage?.Invoke();
    }

    public void SetFullScreen (bool is_full_screen)
    {
        isFullScreen = is_full_screen;
        checkMark.SetActive(is_full_screen);
        Screen.fullScreen = is_full_screen;

        if (is_full_screen)
            SoundManager.instance.PlaySFX("Confirm");
        else
            SoundManager.instance.PlaySFX("Cancel");
    }

    private void ExitMenu()
    {
        gameObject.SetActive(false);
        SoundManager.instance.PlaySFX("Cancel");
    }

    private bool PressKey(PlayerConfiguration playerConfig, string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}
