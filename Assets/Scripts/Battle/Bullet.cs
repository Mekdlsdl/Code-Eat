using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [field: SerializeField] public float damage { get; } = 10;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BorderLine"))
            bulletPool.Release(this);
    }


}
