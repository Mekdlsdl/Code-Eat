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
        if (PressKey(InputType.SOUTHBUTTON))
        {
            if (exclamationMark.activeSelf && !isViewingTip)
            {
                viewingTipObject = tipObject;
                viewingTipPos = tipPos;

                Time.timeScale = 0;
                isViewingTip = true;
                viewingTipObject.SetActive(true);
                SetExclamation(false);
            }
            else if (isViewingTip)
            {
                Time.timeScale = 1;
                isViewingTip = false;
                viewingTipObject.SetActive(false);

                viewingTipPos.DisableTrigger();
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

    private bool PressKey(string input_tag)
    {
        return playerMovement.PlayerConfig.Input.actions[input_tag].triggered;
    }
}
