using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance { get; private set; }
    public Dictionary<string, string[]> translationDict = new Dictionary<string, string[]>();

    public int currentLanguage;

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
}
