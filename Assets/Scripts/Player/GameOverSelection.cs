using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSelection : MonoBehaviour
{
    private GameOverControl gameOverControl;
    private PlayerConfiguration playerConfig;

    void Update()
    {
        if (playerConfig != null)
            gameOverControl.ModeSelect(playerConfig);
    }
    
    public void Init(GameOverControl goc, PlayerConfiguration player_config)
    {
        gameOverControl = goc;
        playerConfig = player_config;
    }
}
