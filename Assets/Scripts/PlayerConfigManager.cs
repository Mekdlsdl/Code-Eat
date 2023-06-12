using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerConfigManager : MonoBehaviour
{
    public static PlayerConfigManager instance { get; private set; }

    private List<PlayerConfiguration> playerConfigs = new List<PlayerConfiguration>();
    public List<PlayerConfiguration> PlayerConfigs => playerConfigs;

    [SerializeField] private GameObject playerUI;

    [System.NonSerialized] public bool enableJoin = false;
    
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void HandlePlayerJoin(PlayerInput player_input)
    {
        if (!enableJoin) return;

        if (!playerConfigs.Any(p => p.PlayerIndex == player_input.playerIndex)) {
            PlayerConfiguration player = new PlayerConfiguration(player_input);
            playerConfigs.Add(player);
            player_input.transform.SetParent(PlayerConfigManager.instance.transform);
            player_input.GetComponent<PauseNavigation>().Init(player);

            GameObject player_ui = Instantiate(playerUI, GameObject.FindWithTag("StartingLayout").transform);
            player_ui.transform.localScale = new Vector3(1f, 1f, 1f);
            player_ui.gameObject.GetComponent<PlayerSetup>().SetPlayer(player);

            Debug.Log($"Player {player_input.playerIndex + 1} Joined.");

            SoundManager.instance.PlaySFX("Select");
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void RespawnPlayers()
    {
        foreach (Transform existingPlayer in GameObject.FindWithTag("StartingLayout").transform) {
            Destroy(existingPlayer.gameObject);
        }
        
        foreach (PlayerConfiguration player in playerConfigs) {
            player.IsReady = false;
            GameObject player_ui = Instantiate(playerUI, GameObject.FindWithTag("StartingLayout").transform);
            player_ui.transform.localScale = new Vector3(1f, 1f, 1f);
            player_ui.gameObject.GetComponent<PlayerSetup>().SetPlayer(player);
        }
    }

    public void ResetAllPlayerConfigs() // 맵을 시작하기 전에 각 플레이어의 이전 기록을 초기화한다.
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs) {
            playerConfig.PlayerScore = 0;
            playerConfig.CorrectProblemCount = 0;
            playerConfig.TotalProblemCount = 0;
            playerConfig.PlayerHp = 100;
            playerConfig.TotalShotCount = 0;
            playerConfig.HitShotCount = 0;
            playerConfig.CriticalShotCount = 0;
        }
    }

    public void ResetAllPlayerHealth()
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs) {
            playerConfig.PlayerHp = 100;
        }
    }

    public void IncreasePlayerProblemCount()
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs) {
            if (playerConfig.PlayerHp > 0)
                playerConfig.TotalProblemCount++;
        }
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
    public int CorrectProblemCount { get; set; }
    public int TotalProblemCount { get; set; }
    public int TotalShotCount { get; set; }
    public int HitShotCount { get; set; }
    public int CriticalShotCount { get; set; }
    public int PlayerIndex { get; set; }
    public int PlayerHp { get; set; } = 100;
    public int CharacterTypeIndex { get; set; }
    public string CharacterName { get; set; }
    public bool IsReady { get; set; }
}
