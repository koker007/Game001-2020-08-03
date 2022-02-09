using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopPanelBuySubscriptionMonth : MonoBehaviour
{

    [SerializeField]
    Text NamePanelText;
    [SerializeField]
    Text DateText;

    [Header("Keys")]
    [SerializeField]
    string KeyPanelName; //Имя панели
    [SerializeField]
    string KeyDateNotSigned; //Не подписанно
    [SerializeField]
    string KeyDateEnd; //Дата окончания

    [Header("Text default")]
    [SerializeField]
    string textPanelName = "Subscription month";
    [SerializeField]
    string textPanelNotSigned = "Not working now";
    [SerializeField]
    string textDateEnd = "Works until";


    System.DateTime TimeEnd = new System.DateTime(1,2,3); //Время подписки записанное в памяти

    void inicialize() {
        textPanelName = TranslateManager.main.GetText(KeyPanelName);
        textPanelNotSigned = TranslateManager.main.GetText(KeyDateNotSigned);
        textDateEnd = TranslateManager.main.GetText(KeyDateEnd);

        
    }

    // Start is called before the first frame update
    void Start()
    {
        inicialize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    //Проверка необходимости обновления текста
    void UpdateText() {
        //Проверяем время подписки в памяти с фактическим
        //Если время совпадает ничего не меняем
        if (TimeEnd == PlayerProfile.main.subscriptionDateEnd) {
            return;
        }

        //Присваиваем новое время подписки
        TimeEnd = PlayerProfile.main.subscriptionDateEnd;

        NamePanelText.text = textPanelName;
        //Если время подписки больше текущего времени то подписка работает
        if (TimeEnd > System.DateTime.Now) {

            string strMonth = "";
            string strDay = "";

            //Если числа не двухзначные прибавляем ноль
            if (TimeEnd.Month < 10) strMonth += "0";
            if (TimeEnd.Day < 10) strDay += "0";

            strMonth += TimeEnd.Month;
            strDay += TimeEnd.Day;

            DateText.text = textDateEnd + " " + TimeEnd.Year + "." + strMonth + "." + strDay;
        }
        else {
            DateText.text = textPanelNotSigned;
        }
    }

    //Открыть всплывающее сообщение о покупки подписки на месяц
    public void ClickButtonOpenInfo()
    {
        GlobalMessage.ShopBuySubscriptionMonth();
    }
}
