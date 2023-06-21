using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LocalizeUI : MonoBehaviour
{
    [SerializeField] private UIType uiType;
    [SerializeField] private string tagName, location;
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private Image targetImage;
    [SerializeField] private VideoClip targetVideo;
    
    void Start()
    {
        PauseMenu.instance.resolutionMenu.setUiLanguage += OnLanguageChange;
        OnLanguageChange();
    }

    private void OnLanguageChange()
    {
        Debug.Log(",,,");
        switch (uiType) {
            case UIType.Text:
                // TextMeshProUGUI targetText = GetComponent<TextMeshProUGUI>();
                targetText.text = LocalizationManager.instance.translationDict[tagName][LocalizationManager.instance.currentLanguage];
                break;

            case UIType.Image:
                // Image targetImage = GetComponent<Image>();
                targetImage.sprite = Resources.Load($"Tips/{location}/{tagName}") as Sprite;
                break;

            case UIType.Video:
                // VideoClip targetVideo = GetComponent<VideoClip>();
                targetVideo = Resources.Load($"Tips/{location}/{tagName}") as VideoClip;
                break;
            
            default:
                break;
        }
    }

    void OnDestroy()
    {
        PauseMenu.instance.resolutionMenu.setUiLanguage -= OnLanguageChange;
    }
}

public enum UIType { Text, Image, Video, Animation };
