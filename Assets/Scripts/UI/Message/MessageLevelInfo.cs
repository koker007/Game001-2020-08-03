using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//alexandr
//Андрей
/// <summary>
/// отвечает за сообщение уровней
/// </summary>
public class MessageLevelInfo : MonoBehaviour
{

    [SerializeField]
    Text LevelText;
    [SerializeField]
    Text LevelInfoText;
    [SerializeField]
    Image[] LevelStars = new Image[3];

    void Start()
    {
        Inicialize();
    }

    void Inicialize() {
        
        LevelText.text = System.Convert.ToString(Gameplay.main.levelSelect);
        LevelGenerator.main.GenerateNewLevel(Gameplay.main.levelSelect);
        string textInfo = TranslateManager.main.GetText("LevelRequrements") + 
            " " + System.Convert.ToString(LevelsScript.main.ReturnLevel().NeedScore) + 
            "\n" + TranslateManager.main.GetText("Record") + 
            "\n" + System.Convert.ToString(LevelsScript.main.ReturnLevel().MaxScore);

        LevelInfoText.text = textInfo;

        Gameplay.main.CountStars(LevelsScript.main.ReturnLevel().MaxScore, ref LevelStars);
    }

    public void ClickButtonStartLevel() {
        if (PlayerProfile.main.Health.Amount > 0)
        {
            UICTRL.main.OpenGameplay();

            MessageCTRL myMessageCTRL = gameObject.GetComponent<MessageCTRL>();
            myMessageCTRL.ClickButtonClose();
        }
        else
        {
            GlobalMessage.ShopBuyHealth();
            MessageCTRL myMessageCTRL = gameObject.GetComponent<MessageCTRL>();
            myMessageCTRL.ClickButtonClose();
        }
    }
}
