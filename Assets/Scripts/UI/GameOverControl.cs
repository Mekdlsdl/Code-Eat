using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameOverControl : MonoBehaviour
{
    public static GameOverControl instance { get; private set; }
    [SerializeField] private GameObject player, screenCover;
    [SerializeField] private List<Image> btnList;
    [SerializeField] private Color32 highlightColor, defaultColor;
    [SerializeField] private int btnIndex = 0;
    private bool enableModeSelect = false;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;

        screenCover.SetActive(true);
    }

    public void Init()
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs) {
            GameObject p = Instantiate(player, transform);
            p.GetComponent<GameOverSelection>().Init(this, playerConfig);
        }

        btnIndex = 0;
        SetButtonIndex(0);
        enableModeSelect = true;
    }

    public void ModeSelect(PlayerConfiguration playerConfig)
    {
        if (!enableModeSelect)
            return;
        
        if (PressKey(playerConfig, InputType.LEFT))
            SetButtonIndex(0);
        
        else if (PressKey(playerConfig, InputType.RIGHT))
            SetButtonIndex(1);
        
        else if (PressKey(playerConfig, InputType.SOUTHBUTTON))
            SelectBtn();
    }

    private void SetButtonIndex(int index)
    {
        NormalizeBtn();
        btnIndex = index;
        HighlightBtn();
    }
    private void NormalizeBtn()
    {
        btnList[btnIndex].color = defaultColor;
    }
    private void HighlightBtn()
    {
        btnList[btnIndex].color = highlightColor;
    }
    private void SelectBtn()
    {
        if (btnIndex == 0)
            GameManager.instance.RetryMapMode();
        else
            GameManager.instance.ReturnToMapSelectMode();
    }
    private bool PressKey(PlayerConfiguration playerConfig, string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}
