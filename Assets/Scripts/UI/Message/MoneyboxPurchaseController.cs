using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyboxPurchaseController : MonoBehaviour
{
    public static MoneyboxPurchaseController main;
    private int moneyboxCapacity = 10;
    private int newMoneyboxCapacity = 10;
    private int moneyboxContent = 0;

    private void Start()
    {
        main = this;
    }

    public void OpenMoneybox()
    {
        PlayerProfile.main.OpenMoneybox(moneyboxContent);
        moneyboxContent = 0;
    }

    public void UpgradeMoneybox()
    {
        newMoneyboxCapacity = 15; //test
        SetMoneyboxCapacity();
    }
    private void SetMoneyboxCapacity()
    {
        moneyboxCapacity = newMoneyboxCapacity;
    }

    public void FillMoneyBox(int goldAmount)
    {
        moneyboxContent += goldAmount;
        if (moneyboxContent > moneyboxCapacity)
        {
            moneyboxContent = moneyboxCapacity;
        }
    }
}
