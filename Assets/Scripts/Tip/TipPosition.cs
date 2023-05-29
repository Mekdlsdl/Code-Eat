using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPosition : MonoBehaviour
{
    [field: SerializeField] public GameObject tip { get; set; }


    private void OnTriggerEnter2D(Collider2D other) {
        if (tip)
            other.GetComponent<PlayerInteract>().InteractTip(tip, this);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (tip)
            other.GetComponent<PlayerInteract>().SetExclamation(false);
    }

    public void DisableTrigger()
    {
        gameObject.SetActive(false);

        if (CheckOpenedAllTips()) {
            transform.parent.GetComponent<TipSpawner>().enemySpawner.SpawnEnemy();
            SoundManager.instance.PlaySFX("OK");
        }
    }

    private bool CheckOpenedAllTips()
    {
        foreach (Transform tipPos in transform.parent) {
            if (tipPos.gameObject.activeSelf)
                return false;
        }
        return true;
    }
}
