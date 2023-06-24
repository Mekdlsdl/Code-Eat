using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TipSpawner : MonoBehaviour
{
    public static TipSpawner instance { get; private set; }
    public static int foundTipCount = 0;
    [SerializeField] private TextMeshProUGUI countText;

    public EnemySpawner enemySpawner;
    [SerializeField] private GameObject tips;
    private int tipsCount;
    

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;

        tipsCount = tips.transform.childCount;
        countText.text = $"{foundTipCount}/{tipsCount}";
        Spawn(GetRandomPos());
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown("space"))
    //         Debug_OpenAllTips();
    // }

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

    public void UpdateFoundTipCount()
    {
        foundTipCount++;
        countText.text = $"{foundTipCount}/{tipsCount}";
    }

    public void Debug_OpenAllTips()
    {
        foreach (Transform tip in transform) {
            tip.gameObject.SetActive(false);
        }
        enemySpawner.SpawnEnemy();
    }
}
