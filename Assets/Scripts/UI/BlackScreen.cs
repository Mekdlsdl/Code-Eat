using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    public void ResetScreen()
    {
        gameObject.GetComponent<Animator>().Play("BlackScreenDefault", -1, 0f);
        gameObject.SetActive(false);
    }
}
