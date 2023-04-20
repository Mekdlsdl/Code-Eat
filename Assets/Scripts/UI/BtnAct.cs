using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BtnAct : MonoBehaviour
{


    public void Update()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            MapSelect();
        }
        
        if (Input.GetKey(KeyCode.JoystickButton1))
        {
            OnRetry();
        }
    
    }
    public void MapSelect()
    {    
        SceneManager.LoadScene(1);
    }
    public void OnRetry()
    {       
        SceneManager.LoadScene(2);
    }

    
}
