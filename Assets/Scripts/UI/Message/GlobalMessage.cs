using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//�����
//������
/// <summary>
/// �������� �� ����� ���������� ��������� �� �����
/// </summary>
public class GlobalMessage : MonoBehaviour
{
    static public GlobalMessage main;

    /// <summary>
    /// �����������
    /// </summary>
    public GlobalMessage() {
        main = this;
    }

    [SerializeField]
    Image Fon;

    [Header("Prefabs")]
    [SerializeField]
    GameObject PrefabTermsOfUse;
    [SerializeField]
    GameObject PrefabGetGiftProfileLVL;

    [SerializeField]
    GameObject PrefabMessanger;
    [SerializeField]
    GameObject PrefabSettings;
    [SerializeField]
    GameObject PrefabHealth;
    [SerializeField]
    GameObject PrefabLVLInfo;

    [SerializeField]
    GameObject PrefabLVLTutorial;

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
    GameObject PrefabShopBuyMixed;
    [SerializeField]
    GameObject PrefabShopBuyMoneybox;
    [SerializeField]
    GameObject PrefabShopBuyHealth;

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

    [SerializeField]
    GameObject LevelRedactor;


    void Start()
    {
        Fon.gameObject.SetActive(true);
        InvokeRepeating("InvokeTermsOfUse", 0.1f, 5f);
    }

    void Update()
    {
        UpdateFon();
    }

    public void InvokeTermsOfUse() {
        if (PlayerProfile.main.ProfileTermsOfUse +0.0001f >= System.Convert.ToDouble(Application.version) ||         //���� ���������� ��� �������
            MessageCTRL.selected //��� ������ ������������ �����-�� ���������
            ) {
            return;
        }

        if (GooglePlay.main.isAutorized && //����������� ������ �������
            GooglePlay.main.FirstGetProfile //���������� � ������� ���� ���������
            )
        {
            TermsOfUse();
        }
        else if (Time.unscaledTime > 10) {

                TermsOfUse();
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////
    ///���� ������� ������ ���������

    /// <summary>
    /// ������� ����������� ����
    /// </summary>
    static public void Close()
    {

        if (MessageCTRL.selected)
        {
            //������� ��������� ����������
            MessageCTRL.selected.AddInBuffer();
            MessageCTRL.selected.ClickButtonClose();
        }
    }

    static void SendMessage(GameObject prefabMessage) {
        GameObject messageObj = Instantiate(prefabMessage, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }

    /// <summary>
    /// ��������� ��������� � ���������� ����
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

    static public void TermsOfUse() {
        GameObject messageObj = Instantiate(main.PrefabTermsOfUse, main.transform);
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        MessageCTRL.NewMessage(messageCTRL);
    }

    static public void Settings()
    {
        SendMessage(main.PrefabSettings);
    }

    /// <summary>
    /// �������� ������� �� ����� �������
    /// </summary>
    static public void GetGiftNewProfileLVL()
    {
        //���� ��� ���� ����� ��������� �� �������
        if (MessageGetGiftNewProfileLevel.main != null) {
            return;
        }
        SendMessage(main.PrefabGetGiftProfileLVL);
    }

    /// <summary>
    /// ����������� ���� ��������
    /// </summary>
    static public void Health()
    {
        SendMessage(main.PrefabHealth);
    }
    /// <summary>
    /// ����������� ���� ������
    /// </summary>
    static public void Tickets()
    {
        SendMessage(main.PrefabTickets);
    }
    /// <summary>
    /// ����������� ���� �������
    /// </summary>
    static public void Shop()
    {
        SendMessage(main.PrefabShop);
    }

    /// <summary>
    /// ������ ������
    /// </summary>
    static public void ShopBuyGold()
    {
        SendMessage(main.PrefabShopBuyGold);
    }
    /// <summary>
    /// ����������� ���� ������ ������ ������
    /// </summary>
    static public void ShopBuyInternal()
    {
        SendMessage(main.PrefabShopBuyBoom);
    }
    /// <summary>
    /// ����������� ���� ������ ������ ������
    /// </summary>
    static public void ShopBuyRocket()
    {
        SendMessage(main.PrefabShopBuyRocket);
    }

    /// <summary>
    /// ����������� ���� ������ �����
    /// </summary>
    static public void ShopBuyBomb() {

        SendMessage(main.PrefabShopBuyBomb);
    }

    /// <summary>
    /// ����������� ���� ������ ����5
    /// </summary>
    static public void ShopBuyColor5()
    {
        SendMessage(main.PrefabShopBuyColor5);
    }

    /// <summary>
    /// ����������� ���� ������ �������������
    /// </summary>
    static public void ShopBuyMixed()
    {
        SendMessage(main.PrefabShopBuyMixed);
    }

    /// <summary>
    /// ����������� ���� ������ �������
    /// </summary>
    static public void ShopBuyMoneybox()
    {
        SendMessage(main.PrefabShopBuyMoneybox);
    }

    /// <summary>
    /// ����������� ���� ������ �����
    /// </summary>
    static public void ShopBuyHealth()
    {
        SendMessage(main.PrefabShopBuyHealth);
    }


    /// <summary>
    /// ����������� ���� �������
    /// </summary>
    static public void Events()
    {
        SendMessage(main.PrefabEvents);
    }


    /// <summary>
    /// ����������� ���� ������ �� ����
    /// </summary>
    static public void ExitLevel()
    {
        SendMessage(main.PrefabExitLevel);
    }
    /// <summary>
    /// ���������
    /// </summary>
    static public void Lose()
    {
        SendMessage(main.PrefabLose);
    }
    /// <summary>
    /// ���������� ��� ������
    /// </summary>
    static public void Results()
    {
        SendMessage(main.PrefabResults);
    }

    /// <summary>
    /// ����� ����� ��������� (�� ������� ����� ����)
    /// </summary>
    static public void ComingSoon()
    {
        SendMessage(main.PrefabComingSoon);
    }

    /// <summary>
    /// ������� ������� � �������� ���������� � ���
    /// </summary>
    /// <param name="SelectLevel"></param>
    static public void LevelInfo(int levelSelect)
    {

        Gameplay.main.levelSelect = levelSelect;
        SendMessage(main.PrefabLVLInfo);
    }

    /// <summary>
    /// ������� ������� � �������� ��������
    /// </summary>
    static public void LevelTutorial(float TutoialNum)
    {
        SendMessage(main.PrefabLVLTutorial);
    }

    /// <summary>
    /// ����� �� ����
    /// </summary>
    static public void ExitGame()
    {
        SendMessage(main.PrefabExitGame);
    }

    /// <summary>
    /// �������� �������
    /// </summary>
    static public void OpenLevelRedactor()
    {
        SendMessage(main.LevelRedactor);
    }

    //�������� ��� �������� ��������������� ����
    void UpdateFon()
    {
        testFon();

        //��������� �����
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
                else if (alpha > 0.5f)
                {
                    alpha = 0.5f;
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
                else if (alpha > 0.5f) {
                    alpha = 0.5f;
                }
                Fon.color = new Color(Fon.color.r, Fon.color.g, Fon.color.b, alpha);
            }
        }
    }
}
