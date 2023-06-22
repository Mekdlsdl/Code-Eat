using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class SpriteListStorage : SerializableDictionary.Storage<List<Sprite>>{}

[System.Serializable]
public class StringSpriteListDictionary : SerializableDictionary<string, List<Sprite>, SpriteListStorage>{}

[System.Serializable]
public class VideoListStorage : SerializableDictionary.Storage<List<VideoClip>>{}

[System.Serializable]
public class StringVideoListDictionary : SerializableDictionary<string, List<VideoClip>, VideoListStorage>{}

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance { get; private set; }

    public int currentLanguage;
    
    public Dictionary<string, string[]> translationDict = new Dictionary<string, string[]>();
    public StringSpriteListDictionary spriteDict;
    public StringVideoListDictionary videoDict;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;

        LoadTranslationData();
    }

    private void LoadTranslationData()
    {
        string readFromFilePath = Application.streamingAssetsPath + "/LocalizationData.txt";

        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();

        foreach (string line in fileLines) {
            if (string.IsNullOrEmpty(line)) continue;

            string[] splitTag = line.Split(':');

            string tag_key = splitTag[0].Trim();
            string[] tag_values = splitTag[1].Split('|').Select(x => x.Trim()).ToArray();

            translationDict[tag_key] = tag_values;
        }
    }

    public string ReturnTranslatedText(string tag)
    {
        return translationDict[tag][currentLanguage];
    }

    public Sprite ReturnTranslatedImage(string tag)
    {
        return spriteDict[tag][currentLanguage];
    }

    public VideoClip ReturnTranslatedVideo(string tag)
    {
        return videoDict[tag][currentLanguage];
    }
}
