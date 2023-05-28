using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance { get; private set; }
    public bool isBattleMode { get; set; } = false;
    public float battleTime { get; private set; }
    public float maxBattleTime { get; private set; } = 15f; // 7초
    private int deadCount = 0;
    private static int totalBulletCount = 0;
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
            BattleModeOffTimeOut();
    }

    public void BattleModeOn(int bulletCount)
    {
        isBattleMode = true;
        battleTime = 0f;
        totalBulletCount = bulletCount;
        curEnemy.transform.position = curEnemy.pos;
        hud.SetActive(true);
    }
    public void BattleModeOff()
    {
        isBattleMode = false;
        battleTime = 0f;
        totalBulletCount = 0;
        Debug.Log("배틀 모드 종료");
        curEnemy.transform.position = curEnemy.pos;
        hud.SetActive(false);
    }

    private void BattleModeOffTimeOut()
    {
        BattleModeOff();
        StartCoroutine(ProblemManager.instance.NextProblem());
    }


    public IEnumerator CheckDead()
    {
        ++deadCount;
        if (deadCount == PlayerConfigManager.instance.GetPlayerConfigs().Count)
        {
            GameManager.isGameOver = true;
            BattleModeOff();
            
            Debug.Log("모든 플레이어가 전투 불능 상태입니다. 모든 플레이어가 죽었음을 충분히 보여주고 2초 뒤 게임 오버로 이동합니다.");
            yield return new WaitForSecondsRealtime(2f);

            StartCoroutine(GameManager.instance.StartGameOver());
        }
    }

    public IEnumerator UpdateTotalBulletCount()
    {
        --totalBulletCount;
        if (totalBulletCount <= 0 && curEnemy.hp > 0) {
            totalBulletCount = 0;

            yield return new WaitForSecondsRealtime(1.5f);

            BattleModeOffTimeOut();
        }
        yield return null;

    }
    
    public void SetEnemy(EnemyType enemy_type, bool is_boss)
    {
        enemy_type.enemyHP *= PlayerConfigManager.instance.GetPlayerConfigs().Count;
        curEnemy.Init(enemy_type, is_boss);
        Instantiate(enemy_type.bodyCollider, curEnemy.transform);

        if (is_boss)
            SoundManager.instance.PlayBGM("Boss");
        else
            SoundManager.instance.PlayBGM("ProblemMode");
    }

}
