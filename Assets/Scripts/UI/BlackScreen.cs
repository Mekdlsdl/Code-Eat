using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    public void ResetScreen()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<Animator>().Play("BlackScreenDefault", -1, 0f);
    }
}
