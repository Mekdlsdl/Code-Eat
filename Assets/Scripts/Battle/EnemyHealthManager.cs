using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public static EnemyHealthManager instance { get; private set; }
    [SerializeField] EnemySpawner enemySpawner;
    public static int totalEnemyHealth = 0;
    
    
    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void ReturnTotalEnemyHealth()
    {
        totalEnemyHealth = 0;

        foreach (EnemyAndPos enemy_pos in enemySpawner.spawnList) {
            totalEnemyHealth += enemy_pos.enemy.GetComponent<EnemyType>().enemyHP;
        }

    }
}
