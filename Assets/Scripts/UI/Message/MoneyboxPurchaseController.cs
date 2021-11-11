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

    //Функция опустошения копилки
    public void OpenMoneybox()
    {
        PlayerProfile.main.OpenMoneybox();        
        SetMoneyboxCapacityText();
    }

    //Функция кнопки улучшения копилки
    public void UpgradeMoneybox()
    {
        PlayerProfile.main.UpgradeMoneybox();        
        SetMoneyboxCapacityText();
    }

    //Устанавливаем текст содержимого копилки (содержимое копилки / лимит содержимого)
    private void SetMoneyboxCapacityText()
    {
        moneyboxCapacityText.text = PlayerProfile.main.moneyboxContent + " / " + PlayerProfile.main.moneyboxCapacity;
    }
}
