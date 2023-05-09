using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;
    public static int totalEnemyHealth = 0;

    public void ReturnTotalEnemyHealth()
    {
        totalEnemyHealth = 0;

        foreach (EnemyAndPos enemy_pos in enemySpawner.spawnList) {
            totalEnemyHealth += enemy_pos.enemy.GetComponent<EnemyType>().enemyHP;
        }
    }
}
