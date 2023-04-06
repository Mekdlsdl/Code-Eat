using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleMode : MonoBehaviour
{
    private PlayerConfiguration playerConfig;
    [SerializeField] private Sprite playerBattleSprite;
    private SpriteRenderer spriter;
    
    public void Init(PlayerConfiguration player_config)
    {
        playerConfig = player_config;
        spriter = GetComponent<SpriteRenderer>();
        spriter.sprite = playerBattleSprite;
    }

    private void Update()
    {
        if (PressKey("Fire"))
            Debug.Log("Fire");
        
    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }

}
