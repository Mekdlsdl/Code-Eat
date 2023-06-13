using TMPro;
using UnityEngine;
using DG.Tweening;

public class ResolutionMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resolutionText;
    [SerializeField] private GameObject checkMark;
    private int resolutionNumber;
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
        if (PressKey(playerConfig, InputType.LEFT))
        {
            resolutionNumber--;
            SetResolution();
            
            DOTween.Rewind("LeftResolutionArrow");
            DOTween.Play("LeftResolutionArrow");

            SoundManager.instance.PlaySFX("Cursor");
        }
        else if (PressKey(playerConfig, InputType.RIGHT))
        {
            resolutionNumber++;
            SetResolution();

            DOTween.Rewind("RightResolutionArrow");
            DOTween.Play("RightResolutionArrow");

            SoundManager.instance.PlaySFX("Cursor");
        }
    }

    private void SetResolution()
    {
        resolutionNumber = Mathf.Clamp(resolutionNumber, 4, 8);
        selectedResolution = (240 * resolutionNumber, 135 * resolutionNumber);
        resolutionText.text = $"{selectedResolution.Item1} x {selectedResolution.Item2}";
        Screen.SetResolution(selectedResolution.Item1, selectedResolution.Item2, Screen.fullScreen);
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
