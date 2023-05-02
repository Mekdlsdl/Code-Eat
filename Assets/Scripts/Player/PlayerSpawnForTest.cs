using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnForTest : MonoBehaviour
{
    public static PlayerSpawnForTest instance { get; private set; }
    
    public List<Vector3> spawnPos;

    [SerializeField] private GameObject playerPrefab;

    private List<Transform> playerTransforms = new List<Transform>();
    public List<Transform> PlayerTransforms => playerTransforms;


    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void SpawnPlayersForTest(string actionMapName)
    {
        GameManager.instance.ChangeActionMaps($"{actionMapName}");

        var playerConfigs = PlayerConfigManager.instance.GetPlayerConfigs();
        for (int i = 0; i < playerConfigs.Count; i++) {
            var player = Instantiate(playerPrefab, spawnPos[i], gameObject.transform.rotation, gameObject.transform);
            playerTransforms.Add(player.transform);
            player.GetComponent<PlayerBattleMode>().Init(playerConfigs[i]);
        }
    }
    
}
