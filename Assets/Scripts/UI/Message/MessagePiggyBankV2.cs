using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePiggyBankV2 : MonoBehaviour
{

    [SerializeField]
    public Text[] CountNow;
    [SerializeField]
    public Text[] CountMax;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void UpdateTextCount() {
        foreach (Text text in CountNow) text.text = PlayerProfile.main.PiggyBankNow.ToString();
        foreach (Text text in CountMax) text.text = PlayerProfile.main.PiggyBankMax.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTextCount();
    }


    //������� ������ ������� � �������� ��� ����������� ������
    public void ClickGetGold() {
        
    }

    //�������� ������ ������� ����� ��������� ������ ������
    public void ClickUpdatePiggyBank() {
        //
    }
}
