using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionControl : MonoBehaviour
{
    public static PositionControl instance { get; private set; }
    private static Vector3[] backupPlayerPos, backupEnemyPos;
    
    public PlayerSpawn playerSpawn;
    [SerializeField] private EnemySpawner enemySpawner;
    public EnemySpawner enemy_spawner => enemySpawner;

    [SerializeField] private GameObject tips, tipPos;    
    
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void BackupPos()
    {
        backupPlayerPos = new Vector3[playerSpawn.PlayerTransforms.Count];
        for (int i = 0; i < backupPlayerPos.Length; i++)
            backupPlayerPos[i] = playerSpawn.PlayerTransforms[i].localPosition;
        
        backupEnemyPos = new Vector3[enemySpawner.ExistingEnemyList.Count];
        for (int j = 0; j < backupEnemyPos.Length; j++)
            backupEnemyPos[j] = enemySpawner.ExistingEnemyList[j].transform.localPosition;
    }

    public void RecoverPos()
    {
        for (int i = 0; i < backupPlayerPos.Length; i++)
            playerSpawn.spawnPos[i] = backupPlayerPos[i];
        
        for (int j = 0; j < backupEnemyPos.Length; j++)
            enemySpawner.spawnList[j].spawnPos = backupEnemyPos[j];
    }

    public void TurnOffTips()
    {
        tips.SetActive(false);
        tipPos.SetActive(false);
    }
}
