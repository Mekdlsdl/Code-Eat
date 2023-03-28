using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectControl : MonoBehaviour
{
    public static MapSelectControl instance { get; private set; }

    [SerializeField] private GameObject mapUI;
    [SerializeField] private List<PageAndMap> pageMapList;

    private int maxPageIndex;
    private int pageIndex, mapIndex;
    
    private bool enableMapSelect = false;

    /*
     MAP INDEX
        0 1
        2 3
    */

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;

        maxPageIndex = pageMapList.Count - 1;
    }

    public void EnableMapSelection()
    {
        pageIndex = mapIndex = 0;
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
            ChooseMap();
            return;
        }
        
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

    private void ChooseMap()
    {
        GameManager.instance.LoadMap(pageMapList[pageIndex].maps[mapIndex].name);
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
