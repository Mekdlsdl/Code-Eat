using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HouseEdge : MonoBehaviour
{
    [SerializeField] private Image edgeImage;
    public TextMeshProUGUI costText;
    public int nodeNumberA, nodeNumberB;
    public int cost;

    public void UpdateCost(int random_cost)
    {
        cost = random_cost;
        costText.text = $"{cost}";
    }
    public void HighlightEdge()
    {
        edgeImage.color = new Color32(231, 75, 75, 255);
        costText.color = new Color32(255, 165, 100, 255);
        costText.fontSize = 65;
    }
}
