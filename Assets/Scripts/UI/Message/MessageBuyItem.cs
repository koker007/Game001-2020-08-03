using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBuyItem : MonoBehaviour
{
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

        bool NeedBuyAntigen = false;

        //Если хватает антител
        if (typeBuy == TypeBuy.internalObj) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopInternal)) {
                NeedBuyAntigen = true;
            }
        }
        else if (typeBuy == TypeBuy.rosket2x) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopRocket)) {
                NeedBuyAntigen = true;
            }
        }
        else if (typeBuy == TypeBuy.bomb) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopBomb)) {
                NeedBuyAntigen = true;
            }
        }
        else if (typeBuy == TypeBuy.Color5) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopColor5)) {
                NeedBuyAntigen = true;
            }
        }
        else if (typeBuy == TypeBuy.mixed)
        {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopMixed))
            {
                NeedBuyAntigen = true;
            }
        }
        //Кнопка покупки за реал
        else if (
            typeBuy == TypeBuy.gold100
            ) {
            PlayerProfile.main.PayReal(typeBuy);
        }
        else if (health)
        {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.Health))
            {
                NeedBuyAntigen = true;
            }
        }

        //Если не хватило голды на покупку, то открываем окно о покупке голды за реал
        if (NeedBuyAntigen) {

        }
    }

}
