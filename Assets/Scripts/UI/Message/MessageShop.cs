using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageShop : MonoBehaviour
{
    [SerializeField]
    Text CountGold;
    [SerializeField]
    Text CountPiggyNow;
    [SerializeField]
    Text CountPiggyMax;

    [SerializeField]
    Text CountInternal;
    [SerializeField]
    Text CountRocket;
    [SerializeField]
    Text CountBomb;
    [SerializeField]
    Text CountColor5;
    [SerializeField]
    Text CountMixed;

    [Header("Header")]
    [SerializeField]
    RawImage subscriptionImage;
    [SerializeField]
    Texture subscriptionWorking;
    [SerializeField]
    Texture subscriptionNotWorking;

    void Update()
    {
        UpdateCountItem();
        UpdateSubscription();
    }

    void UpdateCountItem() {
        CountGold.text = PlayerProfile.main.GoldAmount.ToString();
        CountPiggyNow.text = PlayerProfile.main.PiggyBankNow.ToString();
        CountPiggyMax.text = PlayerProfile.main.PiggyBankMax.ToString();


        CountInternal.text = PlayerProfile.main.ShopInternal.Amount.ToString();
        CountRocket.text = PlayerProfile.main.ShopRocket.Amount.ToString();
        CountBomb.text = PlayerProfile.main.ShopBomb.Amount.ToString();
        CountColor5.text = PlayerProfile.main.ShopColor5.Amount.ToString();
        CountMixed.text = PlayerProfile.main.ShopMixed.Amount.ToString();
    }


    System.DateTime timeEndSubscription = new System.DateTime(1, 2, 3); //Время подписки записанное в памяти
    void UpdateSubscription() {
        //Проверяем время подписки в памяти с фактическим
        //Если время совпадает ничего не меняем
        if (timeEndSubscription == PlayerProfile.main.subscriptionDateEnd)
        {
            return;
        }

        //Присваиваем новое время подписки
        timeEndSubscription = PlayerProfile.main.subscriptionDateEnd;

        //Если время подписки больше текущего времени то подписка работает
        if (timeEndSubscription > System.DateTime.Now)
        {

            string strMonth = "";
            string strDay = "";

            //Если числа не двухзначные прибавляем ноль
            if (timeEndSubscription.Month < 10) strMonth += "0";
            if (timeEndSubscription.Day < 10) strDay += "0";

            strMonth += timeEndSubscription.Month;
            strDay += timeEndSubscription.Day;

            //DateText.text = textDateEnd + " " + timeEndSubscription.Year + "." + strMonth + "." + strDay;

            //Подписка работает
            subscriptionImage.texture = subscriptionWorking;
        }
        //Иначе подписка не работает
        else
        {
            subscriptionImage.texture = subscriptionNotWorking;
        }
    }


    //Открыть всплывающее сообщение о покупки подписки на месяц
    public void ClickButtonOpenInfoSubscription()
    {
        GlobalMessage.ShopBuySubscriptionMonth();
    }

}
