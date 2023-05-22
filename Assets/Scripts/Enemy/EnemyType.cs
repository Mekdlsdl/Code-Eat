using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Create Enemy Type")]
public class EnemyType : ScriptableObject
{
    public string enemyName;
    public int enemyHP, enemyDamage;
    public float enemySpeed;
    public int C;
    public RuntimeAnimatorController animControl;
    public GameObject bodyCollider, problem;
}
