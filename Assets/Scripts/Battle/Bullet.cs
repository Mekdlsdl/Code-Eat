using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [field: SerializeField] public int damage { get; } = 10;
    [SerializeField] private float speed = 10.0f;
    IObjectPool<Bullet> bulletPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Fire()
    {
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    public void SetPool(IObjectPool<Bullet> pool)
    {
        bulletPool = pool;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderLine") || collision.CompareTag("Enemy"))
        {
            bulletPool.Release(this);
            if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                if (!enemy.isDead)
                {
                PlayerBattleMode player = transform.parent.GetComponent<PlayerBattleMode>();
                player.playerConfig.PlayerScore += this.damage;
                Debug.Log($"플레이어 {player.playerConfig.PlayerIndex} 점수: " + player.playerConfig.PlayerScore);
                }
            }

        }
        
    }


}
