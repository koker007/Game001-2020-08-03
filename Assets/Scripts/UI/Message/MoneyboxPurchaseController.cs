using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyboxPurchaseController : MonoBehaviour
{
    [SerializeField] Text moneyboxCapacityText;

    private void Start()
    {
        SetMoneyboxCapacityText();
    }

    //������� ����������� �������
    public void OpenMoneybox()
    {
        PlayerProfile.main.OpenMoneybox();        
        SetMoneyboxCapacityText();
    }

    //������� ������ ��������� �������
    public void UpgradeMoneybox()
    {
        PlayerProfile.main.UpgradeMoneybox();        
        SetMoneyboxCapacityText();
    }

    //������������� ����� ����������� ������� (���������� ������� / ����� �����������)
    private void SetMoneyboxCapacityText()
    {
        moneyboxCapacityText.text = PlayerProfile.main.moneyboxContent + " / " + PlayerProfile.main.moneyboxCapacity;
    }
}
