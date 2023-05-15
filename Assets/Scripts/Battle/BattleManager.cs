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
            BattleModeOffTimeOut();
    }

    public void BattleModeOn()
    {
        isBattleMode = true;
        battleTime = 0f;
        curEnemy.transform.position = curEnemy.pos;
        hud.SetActive(true);
    }
    public void BattleModeOff()
    {
        isBattleMode = false;
        battleTime = 0f;
        Debug.Log("배틀 모드 종료");
        curEnemy.transform.position = curEnemy.pos;
        hud.gameObject.SetActive(false);
    }

    private void BattleModeOffTimeOut()
    {
        BattleModeOff();
        StartCoroutine(ProblemManager.instance.NextProblem(0.6f));
    }


    public IEnumerator CheckDead()
    {
        ++deadCount;
        if (deadCount == PlayerConfigManager.instance.GetPlayerConfigs().Count)
        {
            BattleModeOff();
            
            Debug.Log("모든 플레이어가 전투 불능 상태입니다. 모든 플레이어가 죽었음을 충분히 보여주고 2초 뒤 게임 오버로 이동합니다.");
            yield return new WaitForSecondsRealtime(2f);

            StartCoroutine(GameManager.instance.StartGameOver());
        }
    }
    
    public void SetEnemy(EnemyType enemy_type, bool is_boss)
    {
        curEnemy.Init(enemy_type, is_boss);
        Instantiate(enemy_type.bodyCollider, curEnemy.transform);
    }
}
