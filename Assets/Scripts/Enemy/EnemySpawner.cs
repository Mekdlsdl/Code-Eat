using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyAndPos> spawnList;

    private List<GameObject> existingEnemyList = new List<GameObject>();
    public List<GameObject> ExistingEnemyList => existingEnemyList;

    public void SpawnEnemy()
    {
        for (int i = 0; i < spawnList.Count; i++) {
            string enemy_name = spawnList[i].enemy.GetComponent<EnemyEncounter>().enemy_type.enemyName;

            if (!GameManager.encounteredEnemyset.Contains(enemy_name)) {
                var enemy = Instantiate(spawnList[i].enemy, spawnList[i].spawnPos, gameObject.transform.rotation, gameObject.transform);
                existingEnemyList.Add(enemy);
            }
        }
    }
}

[System.Serializable]
public class EnemyAndPos
{
    public GameObject enemy;
    public Vector3 spawnPos;
}
