using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject criticalPopup;
    [field: SerializeField] public int damage { get; } = 20;
    [SerializeField] private float speed = 50.0f;
    private PlayerBattleMode player;
    private int layerMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = transform.parent.GetComponent<PlayerBattleMode>();
        layerMask = (1 << LayerMask.NameToLayer("BorderLine")) + (1 << LayerMask.NameToLayer("Enemy"));
        
    }

    private void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.up, 0, layerMask); // BorderLine or Enemy
        
        if (ray.collider == null)
            return;

        if (ray.collider.gameObject.layer == 7) { // Enemy
            Enemy enemy = ray.collider.GetComponentInParent<Enemy>();

            if (enemy.isDead)
                return;
            
            int curDamage = 0;

            if (ray.collider.CompareTag("EnemySide")) {
                curDamage =  this.damage / 2;
                SoundManager.instance.PlaySFX("Normal Explosion");
                SoundManager.instance.PlaySFX("Enemy Hurt");
            }
            else if (ray.collider.CompareTag("EnemyCenter")) {
                curDamage = this.damage;
                Instantiate(criticalPopup, enemy.transform.position, Quaternion.identity, enemy.transform);
                ++player.playerConfig.CriticalShotCount;
                SoundManager.instance.PlaySFX("Critical Explosion");
                SoundManager.instance.PlaySFX("Critical Hurt");
            }

            enemy.Hit(curDamage);
            player.playerConfig.PlayerScore += curDamage;
            ++player.playerConfig.HitShotCount;
            Debug.Log($"플레이어 {player.playerConfig.PlayerIndex + 1} 점수: " + player.playerConfig.PlayerScore);

        }

        Destroy(gameObject);

    }

    public void Fire()
    {
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
        Explode();
    }

    private void Explode()
    {
        var pos = transform.position;
        pos.x -= 0.8f - 0.35f;
        pos.y += 1.0f;
        Instantiate(explosion, pos, Quaternion.identity, transform.parent);
    }

}
