using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Семен
/// <summary>
/// Отвечает за вывод глобальных сообщений на экран
/// </summary>
public class GlobalMessage : MonoBehaviour
{
    static public GlobalMessage main;

    /// <summary>
    /// Конструктор
    /// </summary>
    public GlobalMessage() {
        main = this;
    }

    [SerializeField]
    Image Fon;
    [SerializeField]
    RectTransform SelectMessanger;

    [Header("Prefabs")]
    [SerializeField]
    GameObject PrefabMessanger;
    [SerializeField]
    GameObject PrefabSettings;
    [SerializeField]
    GameObject PrefabHealth;
    [SerializeField]
    GameObject PrefabLVLInfo;

    [SerializeField]
    GameObject PrefabShop;
    [SerializeField]
    GameObject PrefabShopBuyBoom;
    [SerializeField]
    GameObject PrefabShopBuyRocket;
    [SerializeField]
    GameObject PrefabShopBuyBomb;
    [SerializeField]
    GameObject PrefabShopBuyColor5;

    [SerializeField]
    GameObject PrefabEvents;
    [SerializeField]
    GameObject PrefabExitLevel;
    [SerializeField]
    GameObject PrefabLose;
    [SerializeField]
    GameObject PrefabResults;
    [SerializeField]
    GameObject PrefabTickets;
    [SerializeField]
    GameObject PrefabComingSoon;
    [SerializeField]
    GameObject PrefabExitGame;

    //Нужно ли закрыть окно
    [SerializeField]
    bool needClose = true;
    [SerializeField]
    bool openMessage = false;

    // Start is called before the first frame update
    void Start()
    {
        Fon.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOpenClose();
    }

    /// <summary>
    /// Закрыть всплывающее окно
    /// </summary>
    static public void Close()
    {
        //Проверка на безопасность
        if (!main.SelectMessanger)
            return;

        MessageCTRL message = main.SelectMessanger.GetComponent<MessageCTRL>();
        if (message)
        {

            main.openMessage = false;
            //Говорим сообщению выпилиться
            message.NeedClose = true;
            //Говорим фону осветлиться
            main.needClose = true;

            //Забываем текущее сообщение
            main.SelectMessanger = null;
        }
    }

    /// <summary>
    /// Отправить сообщение в сплывающем окне
    /// </summary>
    /// <param name="text"></param>
    static public void Message(string title, string message, string button)
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabMessanger, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();
        messageCTRL.setMessage(title, message, button);

        main.needClose = false;
    }
    static public void Message(string title, string message) {
        Message(title, message, "Ok");
    }

    static public void Settings()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabSettings, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// Всплывающее окно здоровье
    /// </summary>
    static public void Health()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabHealth, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }
    /// <summary>
    /// Всплывающее окно билеты
    /// </summary>
    static public void Tickets()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabTickets, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }
    /// <summary>
    /// Всплывающее окно магазин
    /// </summary>
    static public void Shop()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabShop, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// Всплывающее окно купить первую абилку
    /// </summary>
    static public void ShopBuyBoom()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabShopBuyBoom, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }
    /// <summary>
    /// Всплывающее окно купить вторую абилку
    /// </summary>
    static public void ShopBuyRocket()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabShopBuyRocket, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// Всплывающее окно купить бомбу
    /// </summary>
    static public void ShopBuyBomb() {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabShopBuyBomb, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// Всплывающее окно купить цвет5
    /// </summary>
    static public void ShopBuyColor5()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabShopBuyColor5, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }


    /// <summary>
    /// Всплывающее окно события
    /// </summary>
    static public void Events()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabEvents, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }


    /// <summary>
    /// Всплывающее окно выхода из игры
    /// </summary>
    static public void ExitLevel()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabExitLevel, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }
    /// <summary>
    /// прогигрышь
    /// </summary>
    static public void Lose()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabLose, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }
    /// <summary>
    /// результаты при победе
    /// </summary>
    static public void Results()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabResults, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// скоро будет обновлено (не готовая часть игры)
    /// </summary>
    static public void ComingSoon()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        GameObject messageObj = Instantiate(main.PrefabComingSoon, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// Выбрать уровень и показать информацию о нем
    /// </summary>
    /// <param name="SelectLevel"></param>
    static public void LevelInfo(int levelSelect)
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;
        Gameplay.main.levelSelect = levelSelect;

        GameObject messageObj = Instantiate(main.PrefabLVLInfo, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// выход из игры
    /// </summary>
    static public void ExitGame()
    {
        if (main.openMessage)
        {
            return;
        }
        main.openMessage = true;

        GameObject messageObj = Instantiate(main.PrefabExitGame, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    //Открытие или закрытие информационного меню
    void UpdateOpenClose()
    {

        testFon();
        testClose();

        //Изменение альфы
        void testFon() {
            float alphaMax = 0.5f;
            if (needClose && Fon.color.a > 0)
            {
                float alpha = Fon.color.a;
                alpha -= Time.unscaledDeltaTime;
                if (alpha < 0)
                {
                    Fon.raycastTarget = false;
                    alpha = 0;
                }

                Fon.color = new Color(Fon.color.r, Fon.color.g, Fon.color.b, alpha);
            }
            else if (!needClose && Fon.color.a < alphaMax) {
                Fon.raycastTarget = true;

                float alpha = Fon.color.a;
                alpha += Time.unscaledDeltaTime;
                if (alpha < 0)
                {
                    alpha = alphaMax;
                }
                Fon.color = new Color(Fon.color.r, Fon.color.g, Fon.color.b, alpha);
            }
        }
        //Выключить фон если нет сообщения
        void testClose() {
            //Если сообщения нет и окно настроено на открытие, закрываем
            if (!SelectMessanger && !needClose) {
                needClose = true;
            }
        }
    }

}
