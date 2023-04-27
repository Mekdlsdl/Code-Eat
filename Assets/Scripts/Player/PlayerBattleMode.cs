using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerBattleMode : ObjectPooler
{
    public PlayerConfiguration playerConfig { get; private set; }
    private SpriteRenderer spriter;
    private Animator animator;

    [Header("Player Setting")]
    [SerializeField] private Sprite playerBattleSprite;
    [SerializeField] private GameObject bulletUI;
    [SerializeField] private int bulletCount = 3;
    
    public void Init(PlayerConfiguration player_config)
    {
        playerConfig = player_config;
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spriter.sprite = playerBattleSprite;

        // 테스트용
        ShowBullets();
    }


    private void Update()
    {
        if (BattleManager.instance.isBattleMode)
            Fire();
    }

    private void Fire()
    {
        if (PressKey("Fire") && bulletCount > 0)
        {
            var bullet = pool.Get();
            var bulletPos = new Vector2(transform.position.x + 0.8f, transform.position.y);
            bullet.transform.position = bulletPos;
            bullet.GetComponent<Bullet>().Fire();
            PlayAnimation();

            // 총알 개수, UI 변동
            --bulletCount;
            bulletUI.transform.GetChild(bulletCount).gameObject.SetActive(false);
        }

    }

    private bool PressKey(string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }

    private void PlayAnimation()
    {
        animator.enabled = true;
        animator.SetTrigger("Shoot");
    }

    public void ObtainBullets(int bullet)
    {
        bulletCount = bullet;
        ShowBullets();
    }

    public void ClearBullets()
    {
        bulletCount = 0;
        ShowBullets();
    }

    private void ShowBullets()
    {
        for (var i = 0; i < 5; i++)
        {
            bulletUI.transform.GetChild(i).gameObject.SetActive(i < bulletCount);
        }

    }

}
