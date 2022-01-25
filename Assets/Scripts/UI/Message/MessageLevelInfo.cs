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
    MessageCTRL messageCTRL;

    [SerializeField]
    Image Fon;
    [SerializeField]
    Text LevelText;
    [SerializeField]
    Text LevelTargetInfo;
    [SerializeField]
    Text LevelNeedScore;
    [SerializeField]
    Text LevelInfoText;
    [SerializeField]
    GameObject[] LevelStars;

    [SerializeField]
    Text TargetTitle;
    [SerializeField]
    RawImage TargetMission;
    [Header("Target Images")]
    [SerializeField]
    Texture TargetScore;
    [SerializeField]
    Texture TargetCrystal;
    [SerializeField]
    Texture TargetIce;
    [SerializeField]
    Texture TargetMold;
    [SerializeField]
    Texture TargetBox;
    [SerializeField]
    Texture TargetPanel;
    [SerializeField]
    Texture TargetRock;
    [SerializeField]
    Texture TargetEnemy;

    [Header("Fons")]
    [SerializeField]
    Texture FonGreen;
    [SerializeField]
    Texture FonYellow;
    [SerializeField]
    Texture FonOrange;
    [SerializeField]
    Texture FonRed;
    [SerializeField]
    Texture FonViolet;

    void Start()
    {
        Inicialize();
    }

    void Inicialize() {

        string messageTitleLVL = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTitle) + " " + Gameplay.main.levelSelect.ToString();
        messageCTRL.title.text = messageTitleLVL;

        LevelGenerator.main.GenerateNewLevel(Gameplay.main.levelSelect);

        string textNeedScore = TranslateManager.main.GetText("LevelRequrements") +
                               "\n" + 
                               System.Convert.ToString(LevelsScript.main.ReturnLevel().NeedScore);
        LevelNeedScore.text = textNeedScore;

        string textInfo = //TranslateManager.main.GetText("LevelRequrements") + 
            //" " + System.Convert.ToString(LevelsScript.main.ReturnLevel().NeedScore) + 
            TranslateManager.main.GetText("Record") + 
            "\n" + System.Convert.ToString(LevelsScript.main.ReturnLevel().MaxScore);

        LevelInfoText.text = textInfo;

        //Gameplay.main.CountStars(LevelsScript.main.ReturnLevel().MaxScore, ref LevelStars, false);

        //Рисуем звезды
        if (Gameplay.main.levelSelect < PlayerProfile.main.LVLStar.Length)
        {

            if (PlayerProfile.main.LVLStar[Gameplay.main.levelSelect] == 1)
            {
                LevelStars[0].SetActive(true);
                LevelStars[1].SetActive(false);
                LevelStars[2].SetActive(false);
            }
            else if (PlayerProfile.main.LVLStar[Gameplay.main.levelSelect] == 2)
            {
                LevelStars[0].SetActive(true);
                LevelStars[1].SetActive(true);
                LevelStars[2].SetActive(false);
            }
            else if (PlayerProfile.main.LVLStar[Gameplay.main.levelSelect] == 3)
            {
                LevelStars[0].SetActive(true);
                LevelStars[1].SetActive(true);
                LevelStars[2].SetActive(true);
            }
            else
            {
                LevelStars[0].SetActive(false);
                LevelStars[1].SetActive(false);
                LevelStars[2].SetActive(false);
            }
        }
        //Если нету звезд то выключаем
        else {
            LevelStars[0].SetActive(false);
            LevelStars[1].SetActive(false);
            LevelStars[2].SetActive(false);
        }

        //Получаем данные уровня
        LevelsScript.Level level = LevelsScript.main.ReturnLevel();

        IniTarget();

        void IniTarget() {
            TargetTitle.text = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTarget);

            //В зависимости от цели уровня ставим картинку
            Texture texture = null;
            string text = " ";
            
            if (level.PassedWithEnemy)
            {
                texture = TargetEnemy;
                text = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTargetEnemy);
            }
            else if (level.PassedWithMold)
            {
                texture = TargetMold;
                text = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTargetMold);
            }
            else if (level.PassedWithRock) {
                texture = TargetRock;
                text = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTargetRock);
            }
            else if (level.PassedWithPanel) {
                texture = TargetPanel;
                text = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTargetPanel);
            }
            else if (level.PassedWithIce) {
                texture = TargetIce;
                text = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTargetICE);
            }
            else if (level.PassedWithCrystal) {
                texture = TargetCrystal;
                text = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTargetCrystal);
            }
            else if (level.PassedWithBox) {
                texture = TargetBox;
                text = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTargetBox);
            }
            else if (level.PassedWithScore) {
                texture = TargetScore;
                text = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTargetScore);
            }

            TargetMission.texture = texture;
            LevelTargetInfo.text = text;
        }
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

    public void ClickButtonSeeBonusAd()
    {

        if (PlayerProfile.main.Health.Amount > 0)
        {
            AdMobController.main.ShowPlayBonusAd();

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
