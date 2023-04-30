using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public static PlayerSpawn instance { get; private set; }
    [SerializeField] GameObject screenCover, circleMask;
    
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

    public void SpawnPlayers()
    {
        GameManager.instance.ChangeActionMaps("MapControl");

        var playerConfigs = PlayerConfigManager.instance.GetPlayerConfigs();
        for (int i = 0; i < playerConfigs.Count; i++) {
            var player = Instantiate(playerPrefab, spawnPos[i], gameObject.transform.rotation, gameObject.transform);
            playerTransforms.Add(player.transform);
            player.GetComponent<PlayerMovement>().Init(playerConfigs[i]);
        }
    }

    // 하연 : MapSelect 화면에서 바로 공격 모드로 진입하여 테스트할 수 있도록 추가
    public void SpawnPlayersForTest(string actionMapName)
    {
        GameManager.instance.ChangeActionMaps($"{actionMapName}");

        var playerConfigs = PlayerConfigManager.instance.GetPlayerConfigs();
        for (int i = 0; i < playerConfigs.Count; i++) {
            var player = Instantiate(playerPrefab, spawnPos[i], gameObject.transform.rotation, gameObject.transform);
            player.GetComponent<PlayerBattleMode>().Init(playerConfigs[i]);
        }
    }
    //
    
    public void SetCirclePosition(Vector3 position)
    {
        circleMask.transform.localPosition = position;
    }

    public void SetCircleTransition(bool state)
    {
        screenCover.SetActive(state);
        circleMask.SetActive(state);
    }

    public void FadeOutScreen()
    {
        screenCover.GetComponent<Animator>().Play("BlackScreenFadeOut", -1, 0f);
    }
}
