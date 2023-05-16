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
    [SerializeField] private float maxX = 13.0f;
    [field: SerializeField] public float hp { get; private set; } = 50;
    [field: SerializeField] public float maxHp { get; private set; } = 50;
    [field: SerializeField] public float speed { get; set; } = 5.0f;
    [field: SerializeField] public int damage { get; set; } = 25;
    [SerializeField] private FlashHitEffect flashHitEffect;
    [SerializeField] private GameObject hitEffect;
    
    private bool isBoss = false;

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
        flashHitEffect.Flash();
        animator.Play($"{enemyType.enemyName}_Hurt", -1, 0f);
        
        if (hp - damage <= 0) {
            StartCoroutine(DefeatEnemy());
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

    IEnumerator DefeatEnemy()
    {
        hp = 0;
        isDead = true;
        animator.Play($"{enemyType.enemyName}_Dead", -1, 0f);

        yield return new WaitForSecondsRealtime(1f);

        BattleManager.instance.BattleModeOff();

        if (isBoss)
            StartCoroutine(GameManager.instance.StartResultMode());
        else
            StartCoroutine(GameManager.instance.ExitProblemMode());
    }

    public void Init(EnemyType type, bool is_boss)
    {
        enemyType =  type;
        isBoss = is_boss;
        animator.runtimeAnimatorController = enemyType.animControl;
        maxHp = hp = enemyType.enemyHP;
        damage = enemyType.damage;
    }

    private void ReturnToIdleAnimation()
    {
        animator.Play($"{enemyType.enemyName}_Idle", -1, 0f);
    }
}
