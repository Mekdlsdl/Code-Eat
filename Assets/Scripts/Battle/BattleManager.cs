using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance { get; private set; }
    public bool isBattleMode { get; set; } = false;
    public float battleTime { get; private set; }
    public float maxBattleTime { get; private set; } = 7f; // 7초
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
            BattleModeOff();
    }

    public void BattleModeOn()
    {
        isBattleMode = true;
        battleTime = 0f;
        curEnemy.transform.position = curEnemy.pos;
        hud.SetActive(true);
    }
    private void BattleModeOff()
    {
        isBattleMode = false;
        battleTime = 0f;
        Debug.Log($"{maxBattleTime} 초 초과. 배틀 모드 종료");
        curEnemy.transform.position = curEnemy.pos;
        hud.gameObject.SetActive(false);

        StartCoroutine(ProblemManager.instance.NextProblem(0.6f));
    }

    public void CheckDead()
    {
        ++deadCount;
        if (deadCount == PlayerConfigManager.instance.GetPlayerConfigs().Count)
        {
            GameManager.instance.StartGameOver();
        }
    }
    
    public void SetEnemy(EnemyType enemy_type)
    {
        curEnemy.Init(enemy_type);
    }
}
