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
    public const string keyMessageLVLTitle = "MessageLVLTitle";
    public const string keyMessageLVLTarget = "MessageLVLTarget";

    public const string keyMessageLVLTargetScore = "MessageLVLScore";
    public const string keyMessageLVLTargetCrystal = "MessageLVLTargetCrystal";
    public const string keyMessageLVLTargetICE = "MessageLVLTargetICE";
    public const string keyMessageLVLTargetUnderObj = "MessageLVLTargetUnderObj";
    public const string keyMessageLVLTargetBox = "MessageLVLTargetBox";
    public const string keyMessageLVLTargetMold = "MessageLVLTargetMold";
    public const string keyMessageLVLTargetPanel = "MessageLVLTargetPanel";
    public const string keyMessageLVLTargetRock = "MessageLVLTargetRock";
    public const string keyMessageLVLTargetEnemy = "MessageLVLTargetEnemy";

    private struct Translate
    {
        public string key;
        public string text;
    }

    private const int _MaximumKeyOneSimbol = 256;

    private Translate[] _translate = new Translate[char.MaxValue * _MaximumKeyOneSimbol];
    private Translate[] _englishLaunguage = new Translate[char.MaxValue * _MaximumKeyOneSimbol];

    private const char _stringsKey = '|';

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
            LoadLaunguageFile(file, false);
            LoadLaunguageFile("Launguage/English/text", true);
        }
        catch
        {
            LoadLaunguageFile("Launguage/English/text", false);
            LoadLaunguageFile("Launguage/English/text", true);
            Debug.Log("Error: Load launguage");
        }
    }

    private void LoadLaunguageFile(string filePosition, bool isEnglish)
    {
        var text = Resources.Load<TextAsset>(filePosition).text;
        string[] fileStrings = text.Split('\n');
        foreach (string str in fileStrings)
        {
            SetText(str, isEnglish);
        }
    }

    private void SetText(string str, bool isEnglish)
    {
        string[] KeyOrText = str.Split(_stringsKey);

        Translate[] tempTranslate = isEnglish ? _englishLaunguage : _translate;

        if (KeyOrText.Length != 2)
        {
            return;
        }

        string key = KeyOrText[0];
        string text = KeyOrText[1];

        int StartPositionKey = (int)key[0] * _MaximumKeyOneSimbol;
        for(int num = StartPositionKey; num < StartPositionKey + _MaximumKeyOneSimbol && num < tempTranslate.Length; num++)
        {
            if (tempTranslate[num].key == null || _translate[num].key == "")
            {
                tempTranslate[num].key = key;
                tempTranslate[num].text = text;
                return;
            }
        }
    }

    public string GetText(string key, bool isEnglish)
    {
        Translate[] tempTranslate = isEnglish ? _englishLaunguage : _translate;
        string text = null;
        try
        {
            int StartPositionKey = (int)key[0] * _MaximumKeyOneSimbol;
            for (int num = StartPositionKey; num < StartPositionKey + _MaximumKeyOneSimbol || num >= tempTranslate.Length; num++)
            {
                if (key == tempTranslate[num].key)
                {
                    text = tempTranslate[num].text;
                    break;
                }
            }
        }
        catch { }
        if (text == null)
        {
            Debug.Log($"Error. Not find text {key} key");
            return null;
        }
        return text;
    }
    public string GetText(string key) {
        return GetText(key, false) == null ? GetText(key, true) : GetText(key, false);
    }
}
