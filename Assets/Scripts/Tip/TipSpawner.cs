using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TipSpawner : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    [SerializeField] private GameObject tips;
    private int tipsCount;

    private void Awake()
    {
        tipsCount = tips.transform.childCount;
        Spawn(GetRandomPos());
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
            Debug_OpenAllTips();
    }

    private HashSet<int> GetRandomPos()
    {
        var spawnPos = new HashSet<int>();
        int spawnCount = transform.childCount;

        int i = 0;
        while (i < tipsCount)
        {
            int random = Random.Range(0, spawnCount);

            if (spawnPos.Add(random)) {
                ++i;
            } 
        }

        return spawnPos;

    }

    private void Spawn(HashSet<int> spawnPos)
    {
        for (int i = 0; i < tipsCount; i++)
        {
            var tipPos = transform.GetChild(spawnPos.ElementAt(i));
            tipPos.gameObject.SetActive(true);
            tipPos.GetComponent<TipPosition>().tip = tips.transform.GetChild(i).gameObject;
        }
    }

    public void Debug_OpenAllTips()
    {
        foreach (Transform tip in transform) {
            tip.gameObject.SetActive(false);
        }
        enemySpawner.SpawnEnemy();
    }
}
