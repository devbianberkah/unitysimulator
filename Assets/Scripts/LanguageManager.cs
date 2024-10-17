using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;
    public LocalizationData localizationData;
    public string selectedLanguage = "eng";

    private void Awake(){
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("LanguageManager instance set and persists across scenes.");
        }
        else if (instance != this)
        {
            Debug.Log("Destroying duplicate LanguageManager.");
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
     // Method to switch languages
    public void SetLanguage(string language)
    {
        selectedLanguage = language;
    }

    // Method to get translated text
    public string GetTranslation(string key)
    {
        return localizationData.GetTranslation(key, selectedLanguage);
    }
}
