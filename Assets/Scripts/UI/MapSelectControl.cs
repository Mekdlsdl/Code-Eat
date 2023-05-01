using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapSelectControl : MonoBehaviour
{
    public static MapSelectControl instance { get; private set; }

    [SerializeField] private GameObject mapUI, mapSelectPlayer;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private List<PageAndMap> pageMapList;

    private int maxPageIndex;
    private int pageIndex, mapIndex;
    
    private bool enableMapSelect = false;

    /*
     MAP INDEX
        0 1
        2 3
    */

    void OnEnable()
    {
        EnableMapSelection();
    }
    private void Init()
    {
        if (instance != null)
            return;
        instance = this;

        maxPageIndex = pageMapList.Count - 1;
        pageIndex = mapIndex = 0;

        foreach (PlayerConfiguration player in PlayerConfigManager.instance.PlayerConfigs) {
            GameObject p = Instantiate(mapSelectPlayer, playerSpawn);
            p.GetComponent<MapSelection>().Init(player);
        }
    }

    private void EnableMapSelection()
    {
        Init();
        HighlightPageMap();

        mapUI.SetActive(true);
        enableMapSelect = true;
    }

    public void MapSelect(PlayerConfiguration playerConfig)
    {
        if (!enableMapSelect)
            return;

        int previousPageIndex = pageIndex;

        if (PressKey(playerConfig, InputType.UP))
        {
            mapIndex = Mathf.Clamp(mapIndex - 2, 0, 3);
        }
        else if (PressKey(playerConfig, InputType.DOWN))
        {
            mapIndex = Mathf.Clamp(mapIndex + 2, 0, 3);
        }
        else if (PressKey(playerConfig, InputType.LEFT))
        {
            if (mapIndex % 2 != 0)
                mapIndex -= 1;
            else
                pageIndex = Mathf.Clamp(pageIndex - 1, 0, maxPageIndex);
        }
        else if (PressKey(playerConfig, InputType.RIGHT))
        {
            if (mapIndex % 2 == 0)
                mapIndex += 1;
            else
                pageIndex = Mathf.Clamp(pageIndex + 1, 0, maxPageIndex);
        }
        else if (PressKey(playerConfig, InputType.SOUTHBUTTON))
        {
            enableMapSelect = false;
            StartCoroutine(ChooseMap());
            return;
        }
        // 하연 : MapSelect 화면에서 바로 공격 모드로 진입하여 테스트할 수 있도록 추가
        else if (PressKey(playerConfig, InputType.EASTBUTTON))
        {
            GameManager.instance.LoadMapForTest("Battle", "BattleMode");
        }
        //
        
        if (previousPageIndex != pageIndex)
            mapIndex = (previousPageIndex < pageIndex) ? (mapIndex - 1) : (mapIndex + 1);
        HighlightPageMap();
    }

    private void HighlightPageMap()
    {
        for (int i = 0; i <= maxPageIndex; i++) {
            pageMapList[i].page.SetActive(false);

            for (int j = 0; j < 4; j++)
                pageMapList[i].maps[j].SetActive(true);
        }
        pageMapList[pageIndex].page.SetActive(true);
        pageMapList[pageIndex].maps[mapIndex].SetActive(false);
    }

    private IEnumerator ChooseMap()
    {
        DOTween.Rewind("EnterMapFade");
        DOTween.Play("EnterMapFade");
        yield return new WaitForSeconds(1f);

        PlayerConfigManager.instance.ResetAllPlayerConfigs();
        GameManager.instance.ResetEncounteredEnemyList();

        string map_name = pageMapList[pageIndex].maps[mapIndex].name;
        GameManager.instance.SetCurrentMapName(map_name);
        GameManager.instance.LoadMap(map_name);
    }

    private bool PressKey(PlayerConfiguration playerConfig, string input_tag)
    {
        return playerConfig.Input.actions[input_tag].triggered;
    }
}

[System.Serializable]
public class PageAndMap
{
    public GameObject page;
    public List<GameObject> maps;
}
