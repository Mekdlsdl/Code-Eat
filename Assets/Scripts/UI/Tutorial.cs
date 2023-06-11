using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject gameobject;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 1"))
        {
            Destroy(gameobject);
        }
        if (Input.GetKeyDown("z"))
        {
            Destroy(gameobject);
        }
    }
}
