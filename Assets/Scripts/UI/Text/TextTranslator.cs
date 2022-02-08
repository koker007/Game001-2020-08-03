using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTranslator : MonoBehaviour
{
    public string key;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        string textNew = TranslateManager.main.GetText(key, text.text);
        if (textNew != null && textNew != "Error") {
            text.text = textNew;
        }
    }
    
}
