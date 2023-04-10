using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float hp { get; private set; } = 100;
    public bool isDead { get; private set; } = false;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (!collision.CompareTag("Bullet"))
            return;

        Bullet bullet = collision.GetComponent<Bullet>();

        if (hp - bullet.damage <= 0)
        {
            hp = 0;
            isDead = true;
        }
        else
        {
            hp -= bullet.damage;
        }
    }
}
