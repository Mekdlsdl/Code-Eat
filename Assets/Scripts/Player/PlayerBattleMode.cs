using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class PlayerBattleMode : MonoBehaviour
{
    public PlayerConfiguration playerConfig { get; private set; }
    private SpriteRenderer spriter;
    private Animator animator;

    public GameObject cursor;
    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private GameObject bulletUI;
    [SerializeField] private GameObject deadUI;
    [SerializeField] private GameObject incorrectUI;
    [SerializeField] private TextMeshProUGUI playerIndexText;
    [SerializeField] private int bulletCount = 0;
    
    public void Init(PlayerConfiguration player_config)
    {
        playerConfig = player_config;
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = GameManager.instance.GetBattleAnimControl(playerConfig.CharacterTypeIndex);
        playerIndexText.text = "P" + (player_config.PlayerIndex + 1);
    }


    private void Update()
    {
        if (BattleManager.instance.isBattleMode && playerConfig.PlayerHp > 0)
            Fire();
    }

    private void Fire()
    {
        if (PressKey("Fire") && bulletCount > 0)
        {
            var bullet = Instantiate(bulletprefab, transform);
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
        animator.Play($"Battle_{playerConfig.CharacterName}_Shoot", -1, 0f);        
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
        incorrectUI.transform.gameObject.SetActive(false);
    }

    private void ShowBullets()
    {
        for (var i = 0; i < 5; i++)
        {
            bulletUI.transform.GetChild(i).gameObject.SetActive(i < bulletCount);
        }
    }

    public void Damage(int damage)
    {
        if (playerConfig.PlayerHp - damage <= 0) {
            playerConfig.PlayerHp = 0;
            StartCoroutine(BattleManager.instance.CheckDead()); // 죽은 플레이어 수 증가, 게임오버 로직
            deadUI.transform.gameObject.SetActive(true);
            spriter.color = new Color32(255, 255, 255, 90);
        }
        else {
            playerConfig.PlayerHp -= damage;
            StartCoroutine(DamageAnimation());
            incorrectUI.transform.gameObject.SetActive(true);
        }
    }

    IEnumerator DamageAnimation()
    {
        int countTime = 0;

        while (countTime < 10)
        {
            if (countTime % 2 == 0)
                spriter.color = new Color32(255, 255, 255, 90);
            else   
                spriter.color = new Color32(255, 255, 255, 180);
            
            yield return new WaitForSecondsRealtime(0.2f);

            countTime++;
        }

        spriter.color = new Color32(255, 255, 255, 255);

        yield return null;
    }


    public void HoldGun()
    {
        animator.Play($"Battle_{playerConfig.CharacterName}_HoldGun", -1, 0f);
    }
    public void PutAwayGun()
    {
        cursor.SetActive(false);
        animator.Play($"Battle_{playerConfig.CharacterName}_Idle", -1, 0f);
    }
}
