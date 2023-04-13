using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float hp { get; set; } = 50;
    public bool isDead { get; private set; } = false;
    private Rigidbody2D rb;
    private Vector2 pos;
    private float maxX = 7.0f;
    [field: SerializeField] public float speed { get; set; } = 5.0f;
    [field: SerializeField] public float minSpeed { get; private set; } = 5.0f;
    [SerializeField] private float maxSpeed = 10.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pos = transform.position;
    }

    private void Update()
    {
        if (!BattleManager.instance.isBattleMode || isDead)
            return;
        Move();

    }

    private void Move()
    {
        Vector2 v = pos;
        v.x += maxX * Mathf.Sin(Time.time * speed);
        transform.position = v;
        speed += Time.deltaTime;
        if (speed > maxSpeed)
            speed = maxSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (!collision.CompareTag("Bullet"))
            return;

        StartCoroutine(Stop());

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

    IEnumerator Stop() 
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
    }
}
