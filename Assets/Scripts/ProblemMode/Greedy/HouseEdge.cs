using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HouseEdge : MonoBehaviour
{
    [SerializeField] private Image edgeImage;
    public TextMeshProUGUI costText;
    public int nodeNumberA, nodeNumberB;
    public int cost;
    private bool endHighlight = false;

    public void UpdateCost(int random_cost)
    {
        cost = random_cost;
        costText.text = $"{cost}";
    }

    public void HighlightEdge()
    {
        StartCoroutine(HighlightCost());
    }

    IEnumerator HighlightCost() {
        for (int i=0; i<2; i++) {
            // costText.DOFontSize(120, 0.3f);
            costText.fontSize = 120;
            yield return new WaitForSeconds(0.2f);
            // costText.DOFontSize(100, 0.3f);
            costText.fontSize = 100;
            yield return new WaitForSeconds(0.2f);
        }

        endHighlight = true;
        if (endHighlight) {
            edgeImage.color = new Color32(210, 143, 137, 255);
            costText.color = new Color32(210, 141, 101, 255);
            costText.fontSize = 80;
        }
    }
}
