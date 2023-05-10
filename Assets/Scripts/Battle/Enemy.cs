using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyType enemyType; public EnemyType enemy_type => enemyType;
    private SpriteRenderer sprite;
    private Animator animator;

    public bool isDead { get; private set; } = false;
    public Vector2 pos { get; private set; }
    private float maxX = 7.0f;
    [field: SerializeField] public float hp { get; private set; } = 50;
    [field: SerializeField] public float maxHp { get; private set; } = 50;
    [field: SerializeField] public float speed { get; set; } = 5.0f;
    [SerializeField] private GameObject hitEffect;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        var curPos = maxX * Mathf.Sin(Time.time * speed);
        v.x += curPos;
        sprite.flipX = curPos < 0;
        transform.position = v;
    }

    public void Hit(int damage)
    {
        StartCoroutine(Stop());
        Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
        
        if (hp - damage <= 0) {
            hp = 0;
            isDead = true;
            BattleManager.instance.BattleModeOff();
            GameManager.instance.ReturnToMapSelectMode();
        }
        else {
            hp -= damage;
        }
    }


    IEnumerator Stop() 
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
    }

    public void Init(EnemyType type)
    {
        enemyType =  type;
        animator.runtimeAnimatorController = enemyType.animControl;
        maxHp = hp = enemyType.enemyHP;
    }
}
