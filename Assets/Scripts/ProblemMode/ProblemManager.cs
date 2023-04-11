using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemManager : MonoBehaviour
{
    public ProblemType problemType;

    //[System.NonSerialized]
    public EnemyType enemyType;

    void OnEnable()
    {
        
    }
    
}

public enum ProblemType
{
    DataStructure
}