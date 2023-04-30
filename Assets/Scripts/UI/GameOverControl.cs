using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverControl : MonoBehaviour
{
    public static GameOverControl instance { get; private set; }
    [SerializeField] private GameObject player;
    private bool enableModeSelect = false;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void Init()
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs) {
            GameObject p = Instantiate(player, transform);
            p.GetComponent<GameOverSelection>().Init(this, playerConfig);
        }
        enableModeSelect = true;
    }

    public void ModeSelect(PlayerConfiguration playerConfig)
    {
        if (!enableModeSelect)
            return;

        if (PressKey(playerConfig, InputType.SOUTHBUTTON))
            GameManager.instance.RetryMapMode();
            
        else if (PressKey(playerConfig, InputType.EASTBUTTON))
            GameManager.instance.ReturnToMapSelectMode();
    }

    private bool PressKey(PlayerConfiguration playerConfig, string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}
