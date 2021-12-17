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
    Text LevelText;
    [SerializeField]
    Text LevelTargetInfo;
    [SerializeField]
    Text LevelInfoText;
    [SerializeField]
    Image[] LevelStars = new Image[3];

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
    


    void Start()
    {
        Inicialize();
    }

    void Inicialize() {

        string messageTitleLVL = TranslateManager.main.GetText(TranslateManager.keyMessageLVLTitle) + " " + Gameplay.main.levelSelect.ToString();
        messageCTRL.title.text = messageTitleLVL;

        LevelGenerator.main.GenerateNewLevel(Gameplay.main.levelSelect);
        string textInfo = TranslateManager.main.GetText("LevelRequrements") + 
            " " + System.Convert.ToString(LevelsScript.main.ReturnLevel().NeedScore) + 
            "\n" + TranslateManager.main.GetText("Record") + 
            "\n" + System.Convert.ToString(LevelsScript.main.ReturnLevel().MaxScore);

        LevelInfoText.text = textInfo;

        Gameplay.main.CountStars(LevelsScript.main.ReturnLevel().MaxScore, ref LevelStars);

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
}
