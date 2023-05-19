using TMPro;
using UnityEngine;
using DG.Tweening;

public class ResolutionMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resolutionText;
    [SerializeField] private GameObject checkMark;
    private int resolutionNumber;
    private (int, int) selectedResolution;

    private bool isStickPushed = false;
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
    }

    void OnEnable()
    {
        PauseMenu.menuState = MenuState.Resolution;
        isFullScreen = Screen.fullScreen;
        checkMark.SetActive(isFullScreen);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Z)))
            SetFullScreen(!isFullScreen);
        
        else if ((Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.X)))
            ExitMenu();

        Navigate();
    }

    private void Navigate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (!isStickPushed && (horizontalInput == -1f || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            isStickPushed = true;
            resolutionNumber--;
            SetResolution();
            
            DOTween.Rewind("LeftResolutionArrow");
            DOTween.Play("LeftResolutionArrow");
        }
        else if (!isStickPushed && (horizontalInput == 1f || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            isStickPushed = true;
            resolutionNumber++;
            SetResolution();

            DOTween.Rewind("RightResolutionArrow");
            DOTween.Play("RightResolutionArrow");
        }
        else if (horizontalInput == 0f)
            isStickPushed = false;
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
    }

    private void ExitMenu()
    {
        PauseMenu.menuState = MenuState.Pause;
        gameObject.SetActive(false);
    }
}
