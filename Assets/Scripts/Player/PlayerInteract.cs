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

    void Update()
    {
        if (PauseMenu.isPaused) return;
        
        if (PressKey(InputType.SOUTHBUTTON))
        {
            if (exclamationMark.activeSelf && !isViewingTip)
            {
                OpenTip();
            }
            else if (isViewingTip)
            {
                CloseTip();
            }
        }
    }

    public void InteractTip(GameObject tip, TipPosition tip_pos)
    {
        tipObject = tip;
        tipPos = tip_pos;
        SetExclamation(true);
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
        viewingTipObject = tipObject;
        viewingTipPos = tipPos;

        Time.timeScale = 0;
        isViewingTip = true;
        viewingTipObject.SetActive(true);
        SetExclamation(false);
    }

    private void CloseTip()
    {
        Time.timeScale = 1;
        isViewingTip = false;
        viewingTipObject.SetActive(false);

        viewingTipPos.DisableTrigger();
    }

    private bool PressKey(string input_tag)
    {
        return playerMovement.PlayerConfig.Input.actions[input_tag].triggered;
    }
}
