using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerResult : MonoBehaviour
{
    public static SpawnPlayerResult instance { get; private set; }

    [SerializeField] Winner winnerControl;
    [SerializeField] private GameObject player_result, screen_cover;
    
    private List<PlayerResults> playerResultList = new List<PlayerResults>();

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void Init()
    {
        List<PlayerConfiguration> players = PlayerConfigManager.instance.PlayerConfigs;

        for (int i = 0; i < players.Count; i++) {
            GameObject player = Instantiate(player_result, transform);
            PlayerResults result = player.GetComponent<PlayerResults>();
            
            result.Init(players[i]);
            playerResultList.Add(result);
        }

        screen_cover.SetActive(true);
        StartCoroutine(winnerControl.ShowWinner(1.1f));
    }

    public void ShowAllPlayerScores()
    {
        foreach (PlayerResults result in playerResultList)
            result.ShowScore();
    }

    public void ShowAllPlayerStats()
    {
        foreach (PlayerResults result in playerResultList)
            result.ShowStat();
    }

    public void PlayerResponseAnimation(List<PlayerConfiguration> winners)
    {
        List<int> winnerIndexes = new List<int>();
        for (int i = 0; i < winners.Count; i++)
            winnerIndexes.Add(winners[i].PlayerIndex);

        for (int i = 0; i < playerResultList.Count; i++) {
            PlayerConfiguration player = playerResultList[i].PlayerConfig;

            if (winnerIndexes.Contains(i)) {
                playerResultList[i].PlayerAnim.Play($"Result_{player.CharacterName}_Celebrate", -1, 0f);
                playerResultList[i].CelebrateEffect.SetActive(true);
            }

            else {
                playerResultList[i].PlayerAnim.Play($"Result_{player.CharacterName}_Applaud", -1, 0f);
                
                // 우승자가 있는 방향으로 바라보도록 조정
                if (i < winnerIndexes[0])
                    playerResultList[i].PlayerTransform.localScale = new Vector2(-1, 1);
            }
        }
    }
}
