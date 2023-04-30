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
            var enemy = Instantiate(spawnList[i].enemy, spawnList[i].spawnPos, gameObject.transform.rotation, gameObject.transform);
            existingEnemyList.Add(enemy);
        }
    }

    // public void DeleteEnemy(Transform input_enemy)
    // {
    //     for(int i = 0; i < spawnList.Count; i++) {
    //         if (existingEnemyList[i].transform == input_enemy) {

    //             return;
    //         }
                
    //     } // Debug.Log($"Found {spawnList[i].enemy.name}");
    // }
}

[System.Serializable]
public class EnemyAndPos
{
    public GameObject enemy;
    public Vector3 spawnPos;
}
