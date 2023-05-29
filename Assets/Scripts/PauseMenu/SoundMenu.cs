using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundMenu : MonoBehaviour
{

    [SerializeField] private List<TextMeshProUGUI> texts;
    [SerializeField] private List<Slider> sliders;
    [SerializeField] private Color highlightColor, defaultColor;
    [SerializeField] private List<float> previousVolume;
    private float step = 0.1f;

    private int menuIndex = 0;
    private bool isStickPushed = false;


    void OnEnable()
    {
        PauseMenu.menuState = MenuState.Sound;
    }

    void Update()
    {
        Navigate();
        SetUpVolume();

        if ((Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.X)))
            Exit();
    }

    private void Navigate()
    {
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

    private void SetUpVolume()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if ((!isStickPushed && horizontalInput == -1f) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isStickPushed = true;
            sliders[menuIndex].value = previousVolume[menuIndex] - step;

            SoundManager.instance.PlaySFX("Cursor");
        }
        else if ((!isStickPushed && horizontalInput == 1f) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            isStickPushed = true;
            sliders[menuIndex].value = previousVolume[menuIndex] + step;

            SoundManager.instance.PlaySFX("Cursor");
        }

        previousVolume[menuIndex] = sliders[menuIndex].value;

    }

    private void HighlightButton()
    {
        texts[menuIndex].color = highlightColor;
        sliders[menuIndex].interactable = true;

        texts[1-menuIndex].color = defaultColor;
        sliders[1-menuIndex].interactable = false;
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
