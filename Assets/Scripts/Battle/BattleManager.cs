using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance { get; private set; }
    public bool isBattleMode { get; private set; } = true;
    public float battleTime { get; private set; }
    public float maxBattleTime { get; private set; } = 7f; // 7초
    [field: SerializeField] public Enemy curEnemy { get; private set; }

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
        {
            isBattleMode = false;
            battleTime = 0f;
            Debug.Log($"{maxBattleTime} 초 초과. 배틀 모드 종료");
            curEnemy.speed = curEnemy.minSpeed;
            curEnemy.transform.position = curEnemy.pos;
        }
    }
    
}
