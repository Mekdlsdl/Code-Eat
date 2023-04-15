using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelection : MonoBehaviour
{
    private PlayerConfiguration playerConfig;

    void Update()
    {
        if (playerConfig != null)
            MapSelectControl.instance.MapSelect(playerConfig);
    }
    
    public void Init(PlayerConfiguration player_config)
    {
        playerConfig = player_config;
    }
}
