using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionControl : MonoBehaviour
{
    public static PositionControl instance { get; private set; }
    private static Vector3[] backupPlayerPos;
    private static Dictionary<string, Vector3> backupEnemyPos = new Dictionary<string, Vector3>();
    
    public PlayerSpawn playerSpawn;
    [SerializeField] private EnemySpawner enemySpawner;
    public EnemySpawner enemy_spawner => enemySpawner;

    [SerializeField] private GameObject tips, tipPos, goalGuide;
    
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
        
        for (int j = 0; j < enemySpawner.ExistingEnemyList.Count; j++) {
            string enemy_name = enemySpawner.ExistingEnemyList[j].GetComponent<EnemyEncounter>().enemy_type.enemyName;
            backupEnemyPos[enemy_name] = enemySpawner.ExistingEnemyList[j].transform.localPosition;
        }
    }

    public void RecoverPos()
    {
        for (int i = 0; i < backupPlayerPos.Length; i++)
            playerSpawn.spawnPos[i] = backupPlayerPos[i];
        
        for (int j = 0; j < enemySpawner.spawnList.Count; j++) {
            var temp = Instantiate(enemySpawner.spawnList[j].enemy);
            string enemy_name = temp.GetComponent<EnemyEncounter>().enemy_type.enemyName;
            Destroy(temp);
            
            if (backupEnemyPos.ContainsKey(enemy_name))
                enemySpawner.spawnList[j].spawnPos = backupEnemyPos[enemy_name];
        }
    }

    public void TurnOffTips()
    {
        tips.SetActive(false);
        tipPos.SetActive(false);
    }

    public void ShowGuide()
    {
        goalGuide.SetActive(true);
        PlayerInteract.isViewingGuide = true;
    }

    public void TurnOffGuide()
    {
        goalGuide.SetActive(false);
    }
}
