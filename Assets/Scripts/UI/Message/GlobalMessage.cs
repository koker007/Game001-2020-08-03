using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//�����
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

    //����� �� ������� ����
    [SerializeField]
    bool needClose = true;

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
    /// ������� ����������� ����
    /// </summary>
    static public void Close()
    {
        //�������� �� ������������
        if (!main.SelectMessanger)
            return;

        MessageCTRL message = main.SelectMessanger.GetComponent<MessageCTRL>();
        if (message)
        {
            //������� ��������� ����������
            message.NeedClose = true;
            //������� ���� �����������
            main.needClose = true;

            //�������� ������� ���������
            main.SelectMessanger = null;
        }
    }

    /// <summary>
    /// ��������� ��������� � ���������� ����
    /// </summary>
    /// <param name="text"></param>
    static public void Message(string title, string message, string button){
        GameObject messageObj = Instantiate(main.PrefabMessanger, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();
        messageCTRL.setMessage(title, message, button);

        main.needClose = false;
    }
    static public void Message(string title, string message) {
        Message(title, message, "Ok");
    }

    static public void Settings() {
        GameObject messageObj = Instantiate(main.PrefabSettings, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// ����������� ���� ��������
    /// </summary>
    static public void Health() {
        GameObject messageObj = Instantiate(main.PrefabHealth, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }
    /// <summary>
    /// ����������� ���� ������
    /// </summary>
    static public void Tickets() {
        
    }
    /// <summary>
    /// ����������� ���� �������
    /// </summary>
    static public void Shop()
    {
        GameObject messageObj = Instantiate(main.PrefabShop, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// ������� ������� � �������� ���������� � ���
    /// </summary>
    /// <param name="SelectLevel"></param>
    static public void LevelInfo(int levelSelect) {
        Gameplay.main.levelSelect = levelSelect;

        GameObject messageObj = Instantiate(main.PrefabLVLInfo, main.transform);
        main.SelectMessanger = messageObj.GetComponent<RectTransform>();
        MessageCTRL messageCTRL = messageObj.GetComponent<MessageCTRL>();

        main.needClose = false;
    }

    /// <summary>
    /// ����������� ���� �������
    /// </summary>
    static public void Events() {

    }

    //�������� ��� �������� ��������������� ����
    void UpdateOpenClose()
    {

        testFon();
        testClose();

        //��������� �����
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
        //��������� ��� ���� ��� ���������
        void testClose() {
            //���� ��������� ��� � ���� ��������� �� ��������, ���������
            if (!SelectMessanger && !needClose) {
                needClose = true;
            }
        }
    }

}
