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
        if (exclamationMark.activeSelf && PressKey(InputType.SOUTHBUTTON))
        {
            if (!isViewingTip)
            {
                viewingTipObject = tipObject;
                viewingTipPos = tipPos;

                Time.timeScale = 0;
                isViewingTip = true;
                viewingTipObject.SetActive(true);
            }
            else
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
        exclamationMark.SetActive(true);
    }

    public void DisableTip()
    {
        exclamationMark.SetActive(false);
    }

    private bool PressKey(string input_tag)
    {
        return playerMovement.PlayerConfig.Input.actions[input_tag].triggered;
    }
}
