using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//alexandr
/// <summary>
/// �������� ���������� � ������� �������
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

    public enum Type { 
        verySmallPack,
        smallPack,
        normalPack,
        bigPack,
        veryBigPack
    }

    public Type typePack;

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


    public void ClickDataBaceGold50() {

        DataBase.main.typeProfile.setProfileData(1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        DataBase.main.typeProfileMonth.setProfileData(1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        if (GameFieldCTRL.main != null) {
            DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
        }    
    }
    public void ClickDataBaceGold100() {

        DataBase.main.typeProfile.setProfileData(1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        DataBase.main.typeProfileMonth.setProfileData(1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        if (GameFieldCTRL.main != null)
        {
            DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
        }
    }
    public void ClickDataBaceGold250() {

        DataBase.main.typeProfile.setProfileData(1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        DataBase.main.typeProfileMonth.setProfileData(1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        if (GameFieldCTRL.main != null)
        {
            DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
        }
    }
    public void ClickDataBaceGold500() {

        DataBase.main.typeProfile.setProfileData(1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        DataBase.main.typeProfileMonth.setProfileData(1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        if (GameFieldCTRL.main != null)
        {
            DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
        }
    }
    public void ClickDataBaceGold1000() {

        DataBase.main.typeProfile.setProfileData(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        DataBase.main.typeProfileMonth.setProfileData(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        if (GameFieldCTRL.main != null)
        {
            DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        }
    }
}
