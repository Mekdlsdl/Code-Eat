using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerConfigManager : MonoBehaviour
{
    public static PlayerConfigManager instance { get; private set; }

    private List<PlayerConfiguration> playerConfigs = new List<PlayerConfiguration>();
    public List<PlayerConfiguration> PlayerConfigs => playerConfigs;

    [SerializeField] private GameObject playerUI;
    
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void HandlePlayerJoin(PlayerInput player_input)
    {
        if (!playerConfigs.Any(p => p.PlayerIndex == player_input.playerIndex)) {
            PlayerConfiguration player = new PlayerConfiguration(player_input);
            playerConfigs.Add(player);
            player_input.transform.SetParent(PlayerConfigManager.instance.transform);

            GameObject player_ui = Instantiate(playerUI, GameObject.FindWithTag("StartingLayout").transform);
            player_ui.transform.localScale = new Vector3(1f, 1f, 1f);
            player_ui.gameObject.GetComponent<PlayerSetup>().SetPlayer(player);

            Debug.Log($"Player {player_input.playerIndex + 1} Joined.");
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput player_input) {
        Input = player_input;
        Input.SwitchCurrentActionMap("StartingMenu");
        PlayerIndex = player_input.playerIndex;
    }
    public PlayerInput Input { get; set; }
    public int PlayerScore { get; set; }
    public int PlayerIndex { get; set; }
    public int PlayerHp { get; set; }
    public bool IsReady { get; set; }
    public string CharacterType { get; set; }
}
