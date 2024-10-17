using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public string key;
    public TextMeshProUGUI textGameObject;
    // Start is called before the first frame update
    void Start()
    {
      if (LanguageManager.instance == null)
        {
            Debug.LogError("LanguageManager instance is null!");
        }

        if (textGameObject == null)
        {
            Debug.LogError("textGameObject is not assigned in the Inspector!");
        }

        if (LanguageManager.instance != null && textGameObject != null)
        {
            textGameObject.text = LanguageManager.instance.GetTranslation(key);
        }
        
    }

    void Awake(){
        
    }

    void Update(){
        
    }
    public void UpdateText()
    {
        if (LanguageManager.instance != null)
        {
            textGameObject.text = LanguageManager.instance.GetTranslation(key);
        }
    }
}
