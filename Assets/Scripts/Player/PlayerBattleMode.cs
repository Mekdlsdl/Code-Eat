using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerBattleMode : BulletLauncher
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
        Fire();
    }

    private void Fire()
    {
        if (PressKey("Fire"))
            OnFire();

    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }

}
