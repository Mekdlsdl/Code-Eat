using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance { get; private set; }
    public bool isBattleMode { get; private set; } = false;
    public float battleTime { get; private set; }
    public float maxBattleTime { get; private set; } = 7f; // 7초
    public float maxScore { get; private set; } = 0;
    private int deadCount = 0;
    [field: SerializeField] public Enemy curEnemy { get; private set; }
    [SerializeField] private GameObject hud; // isBattleMode = true 시 활성화 필요

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    private void Update()
    {
        if (!isBattleMode) 
            return;
        
        BattleTimer();
    }

    private void BattleTimer()
    {
        battleTime += Time.deltaTime;

        if (battleTime > maxBattleTime)
            BattleEnd();
    }

    private void BattleEnd()
    {
        isBattleMode = false;
        battleTime = 0f;
        Debug.Log($"{maxBattleTime} 초 초과. 배틀 모드 종료");
        curEnemy.speed = curEnemy.minSpeed;
        curEnemy.transform.position = curEnemy.pos;
        hud.gameObject.SetActive(false);
    }

    public void UpdateMaxScore(int enemyHp)
    {
        maxScore += enemyHp;
    }

    public void CheckDead()
    {
        ++deadCount;
        if (deadCount == PlayerConfigManager.instance.GetPlayerConfigs().Count)
        {
            //  Game over
        }
    }
    
    public void SetEnemy(EnemyType enemy_type)
    {
        curEnemy.Init(enemy_type);
    }
}
