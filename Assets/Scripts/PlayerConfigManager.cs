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
    
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    /// 플레이어 별 맞은 문제 개수 확인
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ShowPlayerProblemState();
        }
    }
    ///

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

    public void ResetAllPlayerConfigs() // 맵을 시작하기 전에 각 플레이어의 이전 기록을 초기화한다.
    {
        ProblemManager.totalProblemCount = 0;

        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs) {
            playerConfig.PlayerScore = 0;
            playerConfig.CorrectProblemCount = 0;
            playerConfig.PlayerHp = 100;
        }
    }

    private void ShowPlayerProblemState()
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs) {
            Debug.Log($"P{playerConfig.PlayerIndex + 1} : {playerConfig.CorrectProblemCount} / {ProblemManager.totalProblemCount}");
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
    public int PlayerIndex { get; set; }
    public int PlayerHp { get; set; } = 100;
    public bool IsReady { get; set; }
    public string CharacterType { get; set; }
}
