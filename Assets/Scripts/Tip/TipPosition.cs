using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPosition : MonoBehaviour
{
    [field: SerializeField] public Image tip { get; set; }


    // 아래는 테스트 용
    private void OnTriggerEnter2D(Collider2D other) {
        if (tip) {
            tip.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (tip) {
            tip.gameObject.SetActive(false);
        }
    }
}
