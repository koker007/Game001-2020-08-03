using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBuyItem : MonoBehaviour
{
    [SerializeField]
    MessageCTRL messageCTRL;

    [SerializeField]
    int DealPrice = 1;
    [SerializeField]
    int DealResult = 1;

    [SerializeField]
    Image From;
    [SerializeField]
    Text FromCount;
    [SerializeField]
    Text FromPrice;

    [SerializeField]
    Image To;
    [SerializeField]
    bool health;

    [SerializeField]
    Image Target;
    [SerializeField]
    Text TargetCount;
    [SerializeField]
    Text TargetPrice;

    enum TypePay {
        Real,
        Gold
    }

    public enum TypeBuy {
        none,
        internalObj,
        rosket2x,
        bomb,
        Color5,
        mixed,
        healt,
        gold100
    }

    [SerializeField]
    TypeBuy typeBuy;

    private const int healthMax = 5;

    void Update()
    {
        UpdateText();
    }

    void UpdateText() {

        if (!health)
        {
            FromCount.text = System.Convert.ToString(PlayerProfile.main.GoldAmount);
            FromPrice.text = "-"+ System.Convert.ToString(DealPrice);
        }

        if(TargetPrice != null)
            TargetPrice.text = "+" + System.Convert.ToString(DealResult);

        if (typeBuy == TypeBuy.internalObj)
        {
            TargetCount.text = System.Convert.ToString(PlayerProfile.main.ShopInternal.Amount);
        }
        else if (typeBuy == TypeBuy.rosket2x)
        {
            TargetCount.text = System.Convert.ToString(PlayerProfile.main.ShopRocket.Amount);
        }
        else if (typeBuy == TypeBuy.bomb)
        {
            TargetCount.text = System.Convert.ToString(PlayerProfile.main.ShopBomb.Amount);
        }
        else if (typeBuy == TypeBuy.Color5) {
            TargetCount.text = System.Convert.ToString(PlayerProfile.main.ShopColor5.Amount);
        }
        else if (typeBuy == TypeBuy.mixed) {
            TargetCount.text = System.Convert.ToString(PlayerProfile.main.ShopMixed.Amount);
        }
        else if (health)
        {
            TargetCount.text = System.Convert.ToString(PlayerProfile.main.Health.Amount);
        }
    }

    public void ButtonClickBuy() {

        bool NeedBuyGold = false;

        //Если хватает золота
        if (typeBuy == TypeBuy.internalObj) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopInternal)) {
                NeedBuyGold = true;
            }

            else 
            {
                DataBase.main.typeProfile.setProfileData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);

                //Сохраняем в базу данных
                if (GameFieldCTRL.main != null)
                    DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);
            }
        }
        else if (typeBuy == TypeBuy.rosket2x) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopRocket)) {
                NeedBuyGold = true;
            }
            else
            {
                DataBase.main.typeProfile.setProfileData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);

                if (GameFieldCTRL.main != null)
                    DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
        }
        else if (typeBuy == TypeBuy.bomb) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopBomb)) {
                NeedBuyGold = true;
            }
            else
            {
                DataBase.main.typeProfile.setProfileData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);

                if (GameFieldCTRL.main != null)
                    DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
        }
        else if (typeBuy == TypeBuy.Color5) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopColor5)) {
                NeedBuyGold = true;
            }
            else
            {
                DataBase.main.typeProfile.setProfileData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);

                if (GameFieldCTRL.main != null)
                    DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
        }
        else if (typeBuy == TypeBuy.mixed)
        {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopMixed))
            {
                NeedBuyGold = true;
            }
            else if (GameFieldCTRL.main != null)
            {
                DataBase.main.typeProfile.setProfileData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
                if (GameFieldCTRL.main != null)
                    DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
        }
        //Кнопка покупки за реал
        else if (
            typeBuy == TypeBuy.gold100
            ) {
            PlayerProfile.main.PayPack(new ShopPack(100));
        }
        else if (health)
        {

            //если здоровья больше 5 выходим
            if (PlayerProfile.main.Health.Amount >= healthMax) {
                return;
            }

            //Если количества золота больше либо равно стоимости
            if (PlayerProfile.main.GoldAmount >= PlayerProfile.main.Health.Cost)
            {
                PlayerProfile.main.GoldAmount -= PlayerProfile.main.Health.Cost;
                PlayerProfile.main.Health.Amount++;

                MenuWorld.main.SetText();

                //Если здоровья стало максимум
                if (PlayerProfile.main.Health.Amount >= healthMax) {
                    messageCTRL.ClickButtonClose();
                }
            }
            else {
                NeedBuyGold = true;
            }
        }

        //Если не хватило голды на покупку, то открываем окно о покупке голды за реал
        if (NeedBuyGold) {
            messageCTRL.ClickButtonClose();
            GlobalMessage.ShopBuyGold();
        }

        PlayerProfile.main.Save();
    }

    public void AddHealtForAds() {

        if (PlayerProfile.main.Health.Amount >= healthMax - 1)
        {
            messageCTRL.ClickButtonClose();
        }
        //Просто добавляем жизнь
        AdMobController.main.ShowPlusHealthAd();
    }

}
