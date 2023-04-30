using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BtnAct : MonoBehaviour
{
    public void MapSelect()
    {    
        GameManager.instance.ReturnToMapSelectMode();
    }
    public void OnRetry()
    {
        GameManager.instance.RetryMapMode();
    }
}
