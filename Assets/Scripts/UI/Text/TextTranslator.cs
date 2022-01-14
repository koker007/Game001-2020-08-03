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
        UpdtaeText();
    }

    public void UpdtaeText()
    {
        text.text = TranslateManager.main.GetText(key);
    }
    
}
