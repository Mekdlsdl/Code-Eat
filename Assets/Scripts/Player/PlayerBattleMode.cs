using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerBattleMode : ObjectPooler
{
    public PlayerConfiguration playerConfig { get; private set; }
    private SpriteRenderer spriter;

    [Header("Battle Sprite")]
    [SerializeField] private Sprite playerBattleSprite;
    
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
        {
            var bullet = pool.Get();
            var bulletPos = new Vector2(transform.position.x + 0.8f, transform.position.y);
            bullet.transform.position = bulletPos;
            bullet.GetComponent<Bullet>().Fire();
        }

    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }

}
