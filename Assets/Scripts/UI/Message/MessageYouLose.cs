using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//alexandr
//Семен
//Андрей
/// <summary>
/// отвечает за сообщение проигрыша
/// </summary>
public class MessageYouLose : MonoBehaviour
{
    [SerializeField] Text ScoreText;
    [SerializeField] GameObject showAdButton;
    //[SerializeField] AdMobController adMobController;

    [SerializeField]
    GameObject ButtonBuyMoveOfGold;
    [SerializeField]
    Text PriceMoveOfGoldText;
    int PriceMoveOfGold = 1;

    public void OnEnable()
    {
        //Удаляем сообщение если оказывается жизни еще есть
        if (Gameplay.main.movingCan > 0) {
            Destroy(gameObject);
        }

        ScoreText.text = $"{TranslateManager.main.GetText("Score")}:\n{Gameplay.main.score.ToString()}";
        if (Gameplay.main.adWatchedCount < 3)
        {
            showAdButton.SetActive(true);
        }
        else
        {
            showAdButton.SetActive(false);
        }

        float volume = Settings.main.VolumeMusicFrom0To1;
        if (Settings.main.VolumeSoundFrom0To1 > Settings.main.VolumeMusicFrom0To1) {
            volume = Settings.main.VolumeSoundFrom0To1;
        }
        SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipLVLFailed, volume, 1);

        ReCalcCostMoveOfGold();

    }

    void ReCalcCostMoveOfGold() {
        //Проверяем сколько ходов уже было купленно
        if (Gameplay.main.buyMoveCount < 1)
        {
            PriceMoveOfGold = 9;
        }
        else if (Gameplay.main.buyMoveCount < 2)
        {
            PriceMoveOfGold = 15;
        }
        else if (Gameplay.main.buyMoveCount < 3)
        {
            PriceMoveOfGold = 25;
        }
        else if (Gameplay.main.buyMoveCount < 4)
        {
            PriceMoveOfGold = 40;
        }
        else if (Gameplay.main.buyMoveCount < 5)
        {
            PriceMoveOfGold = 55;
        }
        else if (Gameplay.main.buyMoveCount < 6)
        {
            PriceMoveOfGold = 70;
        }
        else if (Gameplay.main.buyMoveCount < 7)
        {
            PriceMoveOfGold = 85;
        }
        else if (Gameplay.main.buyMoveCount < 8)
        {
            PriceMoveOfGold = 100;
        }
        else if (Gameplay.main.buyMoveCount < 9)
        {
            PriceMoveOfGold = 115;
        }
        else if (Gameplay.main.buyMoveCount < 10)
        {
            PriceMoveOfGold = 130;
        }
        else {
            ButtonBuyMoveOfGold.SetActive(false);
        }

        PriceMoveOfGoldText.text = PriceMoveOfGold.ToString();
    }

    public void Restart()
    {
        if (PlayerProfile.main.Health.Amount > 0)
        {
            Gameplay.main.GameplayEnd = false;
            Destroy(MenuGameplay.GameField);
            UICTRL.main.OpenWorld();
            UICTRL.main.OpenGameplay();
        }
        else
        {
            GlobalMessage.ShopBuyHealth();
        }
    }

    public void ClickButtonSeeRewardedAd() {

        AdMobController.main.ShowRewardedAd();
    }

    public void ExitGameplay()
    {
        Gameplay.main.GameplayEnd = false;
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }

    public void ClickButtonContinueGold() {
        //Если золота не хватает.. открываем окно с покупкой золота
        if (PlayerProfile.main.GoldAmount < PriceMoveOfGold)
        {
            MessageCTRL.selected.ClickButtonClose();

            GlobalMessage.ShopBuyGold();

        }
        //Если золота достаточно покупаем 5 ходов
        else {
            PlayerProfile.main.GoldAmount -= PriceMoveOfGold;
            Gameplay.main.movingCan += 5;

            //Прибавляем количество покупок
            Gameplay.main.buyMoveCount++;

            //Закрываем окно
            MessageCTRL.selected.ClickButtonClose();
        }
    }

    private void Update()
    {
        //Сообщение не уместно так как у игрока есть еще ходы
        if (Gameplay.main.movingCan > 0)
        {
            //Если сообщение видно игроку закрываем медленно
            if (MessageCTRL.selected != null && MessageCTRL.selected.gameObject == gameObject) {
                MessageCTRL.selected.ClickButtonClose();
            }
            //Если сообщение не видно игроку закрываем резко
            else {
                MessageCTRL.selected.Destroy();
            }
        }
    }
}
