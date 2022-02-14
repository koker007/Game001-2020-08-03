using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBuySubscription : MonoBehaviour
{

    [SerializeField]
    System.DateTime dateEnd = new System.DateTime(1,2,3);

    [Header("Text")]
    [SerializeField]
    Text TextDateEnd;
    [SerializeField]
    Text TextStatus;

    [Header("Keys")]
    [SerializeField]
    string KeyNotWorkingNow;
    [SerializeField]
    string KeyWorkingNow;
    [SerializeField]
    string KeyDateEnd;

    [Header("DefaultText")]
    [SerializeField]
    string strNotWorkingNow;
    [SerializeField]
    string strWorkingNow;
    [SerializeField]
    string strDateEnd;


    //получаем текст перевода по ключам
    void inicizlize() {
        strNotWorkingNow = TranslateManager.main.GetText(KeyNotWorkingNow);
        strWorkingNow = TranslateManager.main.GetText(KeyWorkingNow);
        strDateEnd = TranslateManager.main.GetText(KeyDateEnd);
    }

    // Start is called before the first frame update
    void Start()
    {
        inicizlize();
    }

    void UpdateText() {
        //Выходим если обновление не требуется
        if (dateEnd == PlayerProfile.main.subscriptionDateEnd) {
            return;
        }

        //обновляем дату
        dateEnd = PlayerProfile.main.subscriptionDateEnd;

        //Если дата еще не закончилась
        if (dateEnd > System.DateTime.Now) {

            string strMonth = "";
            string strDay = "";

            //Если числа не двухзначные прибавляем ноль
            if (dateEnd.Month < 10) strMonth += "0";
            if (dateEnd.Day < 10) strDay += "0";
            
            strMonth += dateEnd.Month;
            strDay += dateEnd.Day;

            //Еще работает
            TextStatus.text = strWorkingNow;
            TextDateEnd.text = strDateEnd + " " + dateEnd.Year + "." + strMonth + "." + strDay;
        }
        else {
            TextStatus.text = strNotWorkingNow;
            TextDateEnd.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void ClickBuySubscriptionMonth() {
        PlayerProfile.main.AddSubscriptionMonth();
    }
}
