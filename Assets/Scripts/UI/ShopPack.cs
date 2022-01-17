using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//alexandr
/// <summary>
/// содержит информацию о платных пакетах
/// </summary>
public class ShopPack : MonoBehaviour
{
    [SerializeField] private Text _CostMoneyText;
    [SerializeField] private Text _NameText;
    [SerializeField] private Text _MoneyText;
    [SerializeField] private Text _InternalText;
    [SerializeField] private Text _RocketText;
    [SerializeField] private Text _BombText;
    [SerializeField] private Text _Color5Text;
    [SerializeField] private Text _MixedText;

    public string Name;
    public int CostMoney;

    public int BuyMoneyNum;
    public int BuyInternalNum;
    public int BuyRocketNum;
    public int BuyBombNum;
    public int BuyColor5Num;
    public int BuyMixedNum;

    private void Awake()
    {
        SetText();
    }

    public ShopPack(int money)
    {
        BuyMoneyNum = money;
    }

    public void BuyPackForReal()
    {
        PlayerProfile.main.PayPack(this);
    }

    public void BuyPackForMoney()
    {
        PlayerProfile.main.PayPackForMoney(this);
    }

    private void SetText()
    {
        try
        {
            _NameText.text = Name;
            _CostMoneyText.text = $"{CostMoney} {TranslateManager.main.GetText("money")}";
        }
        catch { }
        try
        {
            _MoneyText.text = $"x {BuyMoneyNum}";
        }
        catch { }
        try
        {
            _InternalText.text = $"x {BuyInternalNum}";
            _RocketText.text = $"x {BuyRocketNum}";
            _BombText.text = $"x {BuyBombNum}";
            _Color5Text.text = $"x {BuyColor5Num}";
            _MixedText.text = $"x {BuyMixedNum}";
        }
        catch { }
    }
}
