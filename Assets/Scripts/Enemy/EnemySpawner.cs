using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyAndPos> spawnList;
    public void SpawnEnemy()
    {
        for (int i = 0; i < spawnList.Count; i++) {
            Instantiate(spawnList[i].enemy, spawnList[i].spawnPos, gameObject.transform.rotation, gameObject.transform);
        }
    }
}

[System.Serializable]
public class EnemyAndPos
{
    public GameObject enemy;
    public Vector3 spawnPos;
}
