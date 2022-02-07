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
    string KeyPanelName; //��� ������
    [SerializeField]
    string KeyDateNotSigned; //�� ����������
    [SerializeField]
    string KeyDateEnd; //���� ���������

    [Header("Text default")]
    [SerializeField]
    string textPanelName = "Subscription month";
    [SerializeField]
    string textPanelNotSigned = "Not working now";
    [SerializeField]
    string textDateEnd = "Works until";


    System.DateTime TimeEnd = new System.DateTime(1,2,3); //����� �������� ���������� � ������

    void inicialize() {
        textPanelName = TranslateManager.main.GetText(KeyPanelName, textPanelName);
        textPanelNotSigned = TranslateManager.main.GetText(KeyDateNotSigned, textPanelNotSigned);
        textDateEnd = TranslateManager.main.GetText(KeyDateEnd, textDateEnd);

        
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

    //�������� ������������� ���������� ������
    void UpdateText() {
        //��������� ����� �������� � ������ � �����������
        //���� ����� ��������� ������ �� ������
        if (TimeEnd == PlayerProfile.main.subscriptionDateEnd) {
            return;
        }

        //����������� ����� ����� ��������
        TimeEnd = PlayerProfile.main.subscriptionDateEnd;

        NamePanelText.text = textPanelName;
        //���� ����� �������� ������ �������� ������� �� �������� ��������
        if (TimeEnd > System.DateTime.Now) {

            string strMonth = "";
            string strDay = "";

            //���� ����� �� ����������� ���������� ����
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

}
