using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMenu : MonoBehaviour
{
    void OnEnable()
    {
        PauseMenu.menuState = MenuState.Sound;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.X)))
            Exit();
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
