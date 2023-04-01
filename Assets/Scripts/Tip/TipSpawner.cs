using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tips;

    private void Awake()
    {
        Spawn(GetRandomPos());
    }

    private HashSet<int> GetRandomPos()
    {
        var spawnPos = new HashSet<int>();

        int tipsCount = tips.transform.childCount;
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
        int tipsCount = tips.transform.childCount;

        for (int i = 0; i < tipsCount; i++)
        {
            var tipPos = transform.GetChild(spawnPos.ElementAt(i));
            tipPos.gameObject.SetActive(true);
            tipPos.GetComponent<TipPosition>().tip = tips.transform.GetChild(i).GetComponent<Image>();
        }
    }


}
