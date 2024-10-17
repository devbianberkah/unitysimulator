using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalizedText
{
    public string key;
    public string indonesian;
    public string english;
}

[CreateAssetMenu(fileName = "LocalizationData",menuName = "Localization/LanguageData")]
public class LocalizationData:ScriptableObject
{
     public List<LocalizedText> translations; // List of localized text entries

    // Get translation based on the selected language
    public string GetTranslation(string key, string language)
    {
        foreach (var item in translations)
        {
            if (item.key == key)
            {
                switch (language)
                {
                    case "ind":
                        return item.indonesian;
                    case "eng":
                        return item.english;
                    default:
                        return item.english; // Default to English if language not found
                }
            }
        }
        return string.Empty;
    }
}