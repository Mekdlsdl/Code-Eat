using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroMenu : MonoBehaviour
{
    [SerializeField] private GameObject Title, Credits;
    [SerializeField] private float displayTitleDelay;
    [SerializeField] private Animator anim;

    private PlayerInput playerInput;
    private bool enablePlayerInput = false;
    private bool isShowingCredits = false;
    private bool isGameStarted = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        StartCoroutine(ShowTitleScreen());
    }

    void Update()
    {
        if (!enablePlayerInput || isGameStarted) return;

        if (PressKey(InputType.SOUTHBUTTON) && !isShowingCredits)
            StartCoroutine(StartGame());
        
        else if (PressKey(InputType.WESTBUTTON)) {
            if (!Credits.activeSelf)
                ShowCredits();
            else
                HideCredits();
        }
        
        else if (PressKey(InputType.EASTBUTTON))
            HideCredits(); 
    }

    IEnumerator ShowTitleScreen()
    {
        yield return new WaitForSeconds(displayTitleDelay);

        Title.SetActive(true);
        SoundManager.instance.PlaySFX("Select");

        yield return new WaitForSeconds(8f);
        if (!enablePlayerInput) EnableInput(true);
    }
    
    IEnumerator StartGame()
    {
        isGameStarted = true;
        SoundManager.instance.PlaySFX("Complete");
        anim.Play("GameStartBlink", -1, 0f);
        yield return new WaitForSeconds(1f);

        DOTween.Rewind("MoveFork");
        DOTween.Play("MoveFork");

        yield return new WaitForSeconds(1f);
        var process = SceneManager.LoadSceneAsync("StartingMenu");
        process.completed += (AsyncOperation operation) =>
        {
            SoundManager.instance.PlayBGM("StartingMenu");
        };
    }

    private void ShowCredits()
    {
        if (Credits.activeSelf || isShowingCredits) return;
        SoundManager.instance.PlaySFX("Confirm");
        Credits.SetActive(true);
        isShowingCredits = true;

        DOTween.Rewind("CreditAppear");
        DOTween.Play("CreditAppear");
    }

    private void HideCredits()
    {
        if (!Credits.activeSelf || !isShowingCredits) return;
        SoundManager.instance.PlaySFX("Cancel");
        isShowingCredits = false;

        DOTween.Rewind("CreditHide");
        DOTween.Play("CreditHide");
    }

    public void EnableInput(bool state)
    {
        enablePlayerInput = state;
    }

    private bool PressKey(string input_tag)
    {
        return playerInput.actions[input_tag].triggered;
    }
}
