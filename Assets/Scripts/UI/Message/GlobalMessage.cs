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
    GameObject PrefabShopBuyGold;
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

        if (MessageCTRL.selected)
        {
            //Говорим сообщению выпилиться
            MessageCTRL.selected.AddInBuffer();
            MessageCTRL.selected.ClickButtonClose();
        }
    }

    /// <summary>
    /// Отправить сообщение в сплывающем окне
    /// </summary>
    /// <param name="text"></param>
    static public void Message(string title, string message, string button)
    {

        GameObject messageObj = Instantiate(main.PrefabMessanger, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        messageCTRL.setMessage(title, message, button);

        MessageCTRL.NewMessage(messageCTRL);
    }
    static public void Message(string title, string message) {
        Message(title, message, "Ok");
    }

    static public void Settings()
    {
        GameObject messageObj = Instantiate(main.PrefabSettings, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }

    /// <summary>
    /// Всплывающее окно здоровье
    /// </summary>
    static public void Health()
    {
        GameObject messageObj = Instantiate(main.PrefabHealth, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }
    /// <summary>
    /// Всплывающее окно билеты
    /// </summary>
    static public void Tickets()
    {
        GameObject messageObj = Instantiate(main.PrefabTickets, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }
    /// <summary>
    /// Всплывающее окно магазин
    /// </summary>
    static public void Shop()
    {
        GameObject messageObj = Instantiate(main.PrefabShop, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }

    /// <summary>
    /// Купить Золото
    /// </summary>
    static public void ShopBuyGold()
    {
        GameObject messageObj = Instantiate(main.PrefabShopBuyGold, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();


        MessageCTRL.NewMessage(messageCTRL);
    }
    /// <summary>
    /// Всплывающее окно купить первую абилку
    /// </summary>
    static public void ShopBuyInternal()
    {
        GameObject messageObj = Instantiate(main.PrefabShopBuyBoom, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }
    /// <summary>
    /// Всплывающее окно купить вторую абилку
    /// </summary>
    static public void ShopBuyRocket()
    {
        GameObject messageObj = Instantiate(main.PrefabShopBuyRocket, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();


        MessageCTRL.NewMessage(messageCTRL);
    }

    /// <summary>
    /// Всплывающее окно купить бомбу
    /// </summary>
    static public void ShopBuyBomb() {

        GameObject messageObj = Instantiate(main.PrefabShopBuyBomb, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }

    /// <summary>
    /// Всплывающее окно купить цвет5
    /// </summary>
    static public void ShopBuyColor5()
    {
        GameObject messageObj = Instantiate(main.PrefabShopBuyColor5, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }


    /// <summary>
    /// Всплывающее окно события
    /// </summary>
    static public void Events()
    {
        GameObject messageObj = Instantiate(main.PrefabEvents, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }


    /// <summary>
    /// Всплывающее окно выхода из игры
    /// </summary>
    static public void ExitLevel()
    {
        GameObject messageObj = Instantiate(main.PrefabExitLevel, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }
    /// <summary>
    /// прогигрышь
    /// </summary>
    static public void Lose()
    {
        GameObject messageObj = Instantiate(main.PrefabLose, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }
    /// <summary>
    /// результаты при победе
    /// </summary>
    static public void Results()
    {
        GameObject messageObj = Instantiate(main.PrefabResults, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }

    /// <summary>
    /// скоро будет обновлено (не готовая часть игры)
    /// </summary>
    static public void ComingSoon()
    {
        GameObject messageObj = Instantiate(main.PrefabComingSoon, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }

    /// <summary>
    /// Выбрать уровень и показать информацию о нем
    /// </summary>
    /// <param name="SelectLevel"></param>
    static public void LevelInfo(int levelSelect)
    {
        Gameplay.main.levelSelect = levelSelect;

        GameObject messageObj = Instantiate(main.PrefabLVLInfo, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }

    /// <summary>
    /// выход из игры
    /// </summary>
    static public void ExitGame()
    {

        GameObject messageObj = Instantiate(main.PrefabExitGame, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }

    //Открытие или закрытие информационного меню
    void UpdateOpenClose()
    {

        testFon();

        //Изменение альфы
        void testFon() {
            float alphaMax = 0.5f;
            if (!MessageCTRL.selected && Fon.color.a > 0)
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
            else if (MessageCTRL.selected && Fon.color.a < alphaMax) {
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
    }

}
