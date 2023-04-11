using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float hp { get; private set; } = 100;
    public bool isDead { get; private set; } = false;
    private Rigidbody2D rb;
    private Vector2 pos;
    private float maxX = 6.0f;
    [SerializeField] private float speed = 5.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pos = transform.position;
    }

    private void Update()
    {
        Vector2 v = pos;
        v.x += maxX * Mathf.Sin(Time.time * speed);
        transform.position = v;
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
