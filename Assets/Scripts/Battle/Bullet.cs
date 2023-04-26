using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : Poolable
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject explosion;
    [field: SerializeField] public int damage { get; } = 10;
    [SerializeField] private float speed = 10.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
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
        Instantiate(explosion, pos, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderLine") || collision.gameObject.layer == 7)
        {
            pool.Release(this);
            
            if (collision.gameObject.layer == 7)
            {
                Enemy enemy = collision.GetComponentInParent<Enemy>();

                if (enemy.isDead)
                    return;
                
                PlayerBattleMode player = transform.parent.GetComponent<PlayerBattleMode>();
                int curDamage = 0;

                if (collision.CompareTag("Enemy1")) {
                    curDamage =  this.damage / 2;
                }
                else if (collision.CompareTag("Enemy2")) {
                    curDamage = this.damage;
                }
                else if (collision.CompareTag("Enemy3")) {
                    curDamage = damage * 2;
                }

                enemy.Hit(curDamage);
                player.playerConfig.PlayerScore += curDamage;
                Debug.Log($"플레이어 {player.playerConfig.PlayerIndex} 점수: " + player.playerConfig.PlayerScore);


            }

        }
        
    }




}
