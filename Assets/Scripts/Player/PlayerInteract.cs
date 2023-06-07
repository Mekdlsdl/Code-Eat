using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject exclamationMark;
    private TipPosition tipPos;
    private GameObject tipObject;

    private static TipPosition viewingTipPos;
    private static GameObject viewingTipObject;
    private static bool isViewingTip = false;
    private static int tipCount;

    void Update()
    {
        if (PauseMenu.isPaused) return;

        // if (tipCount == -1) {
        //     tipCount = 1;
        // }
        
        if (PressKey(InputType.SOUTHBUTTON) && exclamationMark.activeSelf && !isViewingTip)
                OpenTip();
        
        else if (PressKey(InputType.EASTBUTTON) && isViewingTip)
                CloseTip();
    }

    public void InteractTip(GameObject tip, TipPosition tip_pos)
    {
        tipObject = tip;
        tipPos = tip_pos;

        tipCount = tipObject.GetComponent<RectTransform>().childCount - 1;
        SetExclamation(true);
        
        SoundManager.instance.PlaySFX("Select");
    }

    public void SetExclamation(bool state)
    {
        exclamationMark.SetActive(state);
    }

    public IEnumerator FlickExclamation()
    {
        SetExclamation(true);
        yield return new WaitForSeconds(0.5f);
        SetExclamation(false);
    }

    private void OpenTip()
    {
        TipSpawner.instance.UpdateFoundTipCount();

        viewingTipObject = tipObject;
        viewingTipPos = tipPos;

        Time.timeScale = 0;
        isViewingTip = true;
        viewingTipObject.SetActive(true);
        SetExclamation(false);

        SoundManager.instance.PlaySFX("Confirm");
    }

    private void CloseTip()
    {
        RectTransform tipTransform = viewingTipObject.GetComponent<RectTransform>();
        
        Time.timeScale = 1;

        if (tipCount == 0) {
            viewingTipObject.SetActive(false);
            isViewingTip = false;
        }
        else {
            tipTransform.GetChild(tipCount).gameObject.SetActive(false);
            isViewingTip = true;
            tipCount --;
        }

        viewingTipPos.DisableTrigger();

        SoundManager.instance.PlaySFX("Cancel");
    }

    private bool PressKey(string input_tag)
    {
        return playerMovement.PlayerConfig.Input.actions[input_tag].triggered;
    }
}
