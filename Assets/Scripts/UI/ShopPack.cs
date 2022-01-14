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
    [SerializeField] private Text _NameText;
    [SerializeField] private Text _CostText;
    [SerializeField] private Text _MoneyText;
    [SerializeField] private Text _InternalText;
    [SerializeField] private Text _RocketText;
    [SerializeField] private Text _BombText;
    [SerializeField] private Text _Color5Text;
    [SerializeField] private Text _MixedText;

    public string _Name;
    public int _Cost;
    public int _BuyMoneyNum;
    public int _BuyInternalNum;
    public int _BuyRocketNum;
    public int _BuyBombNum;
    public int _BuyColor5Num;
    public int _BuyMixedNum;

    private void Awake()
    {
        SetText();
    }

    public ShopPack(int money)
    {
        _BuyMoneyNum = money;
    }

    public void BuyPack()
    {
        PlayerProfile.main.PayReal(this);
    }

    private void SetText()
    {
        _NameText.text = TranslateManager.main.GetText(_Name);
        _CostText.text = $"{_BuyMoneyNum} {TranslateManager.main.GetText("$")}";
        _MoneyText.text = $"x {_BuyMoneyNum}";
        _InternalText.text = $"x {_BuyInternalNum}";
        _RocketText.text = $"x {_BuyRocketNum}";
        _BombText.text = $"x {_BuyBombNum}";
        _Color5Text.text = $"x {_BuyColor5Num}";
        _MixedText.text = $"x {_BuyMixedNum}";
    }
}
