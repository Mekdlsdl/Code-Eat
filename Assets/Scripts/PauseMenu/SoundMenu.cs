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


    void OnEnable()
    {
        PauseMenu.menuState = MenuState.Sound;
    }

    void OnDisable()
    {
        PauseMenu.menuState = MenuState.Pause;
    }

    public void SoundMenuNavigate(PlayerConfiguration playerConfig)
    {
        if (!PauseMenu.isPaused) return;
        
        Navigate(playerConfig);
        SetUpVolume(playerConfig);

        if (PressKey(playerConfig, InputType.EASTBUTTON))
            Exit();
    }

    private void Navigate(PlayerConfiguration playerConfig)
    {
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
        menuIndex = Mathf.Clamp(menuIndex, 0, 1);
        HighlightButton();
    }

    private void SetUpVolume(PlayerConfiguration playerConfig)
    {
        if (PressKey(playerConfig, InputType.LEFT))
        {
            sliders[menuIndex].value = previousVolume[menuIndex] - step;
            SoundManager.instance.PlaySFX("Cursor");
        }
        else if (PressKey(playerConfig, InputType.RIGHT))
        {
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

    private bool PressKey(PlayerConfiguration playerConfig, string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}
