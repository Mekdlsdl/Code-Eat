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
    [SerializeField] private VideoPlayer targetVideo;
    
    void Start()
    {
        PauseMenu.instance.resolutionMenu.setUiLanguage += OnLanguageChange;
        OnLanguageChange();
    }

    private void OnLanguageChange()
    {
        switch (uiType) {
            case UIType.Text:
                string[] texts = LocalizationManager.instance.ReturnTranslatedText(tagName).Split('@');
                
                targetText.text = texts[0];
                if (texts.Length > 1) targetText.fontSize = int.Parse(texts[1]);
                break;

            case UIType.Image:
                targetImage.sprite = LocalizationManager.instance.ReturnTranslatedImage(tagName);
                break;

            case UIType.Video:
                targetVideo.clip = LocalizationManager.instance.ReturnTranslatedVideo(tagName);
                break;

            case UIType.Animation:
                GetComponent<Animator>().Play($"{LocalizationManager.instance.ReturnTranslatedText(tagName)}", -1, 0f);
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
