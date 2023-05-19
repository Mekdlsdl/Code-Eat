using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite highlightedSprite, normalSprite;

    public void HighlightButton()
    {
        buttonImage.sprite = highlightedSprite;
    }

    public void NormalizeButton()
    {
        buttonImage.sprite = normalSprite;
    }
}
