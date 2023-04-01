using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject exclamationMark;
    private TipPosition tipPos;
    private GameObject tipObject;
    private bool isViewingTip = false;

    void Update()
    {
        if (exclamationMark.activeSelf)
        {
            if (PressKey(InputType.SOUTHBUTTON))
            {
                if (!isViewingTip) {
                    Time.timeScale = 0;
                    isViewingTip = true;
                    tipObject.SetActive(true);
                }
                else {
                    Time.timeScale = 1;
                    isViewingTip = false;
                    tipObject.SetActive(false);
                    tipPos.DisableTrigger();
                }
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
