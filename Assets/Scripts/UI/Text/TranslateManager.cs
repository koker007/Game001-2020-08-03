using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Меняет язык
/// Хранит установленный перевод
/// </summary>
public class TranslateManager : MonoBehaviour
{
    private struct Translate
    {
        public string key;
        public string text;
    }

    private const int MaximumKeyOneSimbol = 256;

    private Translate[] translate = new Translate[char.MaxValue * MaximumKeyOneSimbol];

    char StringsKey = '|';

    public static TranslateManager main;

    private void Awake()
    {
        main = this;
    }

    public void LoadFile(string launguage)
    {
        string file = "Launguage/" + launguage + "/text";
        try
        {
            var text = Resources.Load<TextAsset>(file).text;
            string[] fileStrings = text.Split('\n');
            foreach (string str in fileStrings)
            {
                SetText(str);
            }
        }
        catch{
            Debug.Log("Error: Load launguage");
        }
    }

    private void SetText(string str)
    {
        string[] KeyOrText = str.Split(StringsKey);

        if(KeyOrText.Length != 2)
        {
            return;
        }

        string key = KeyOrText[0];
        string text = KeyOrText[1];

        int StartPositionKey = (int)key[0] * MaximumKeyOneSimbol;
        for(int num = StartPositionKey; num < StartPositionKey + MaximumKeyOneSimbol && num < translate.Length; num++)
        {
            if (translate[num].key == null || translate[num].key == "")
            {
                translate[num].key = key;
                translate[num].text = text;
                return;
            }
        }
    }

    public string GetText(string key)
    {
        string text = null;
        int StartPositionKey = (int)key[0] * MaximumKeyOneSimbol;
        for (int num = StartPositionKey; num < StartPositionKey + MaximumKeyOneSimbol && num < translate.Length; num++)
        {
            if (key == translate[num].key)
            {
                text = translate[num].text;
                break;
            }
        }
        if(text == null)
        {
            return "Error";
        }
        return text;
    }
}
