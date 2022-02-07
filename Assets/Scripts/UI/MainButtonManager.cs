using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// ��������� ��������
/// </summary>
public class MainButtonManager : MonoBehaviour
{
    /// <summary>
    /// �������� ��������� ������
    /// </summary>
    public void PressButtonSound()
    {
        SoundCTRL.main.PlaySound(SoundCTRL.main.clipPressButton);
    }


    public void ButtonClickBuyGold()
    {
        GlobalMessage.ShopBuyGold();
    }

    public void ButtonClickBuyInternal()
    {
        GlobalMessage.ShopBuyInternal();
    }

    public void ButtonClickBuyRocket()
    {
        GlobalMessage.ShopBuyRocket();
    }

    public void ButtonClickBuyBomb()
    {
        GlobalMessage.ShopBuyBomb();
    }

    public void ButtonClickBuyColor5()
    {
        GlobalMessage.ShopBuyColor5();
    }
    public void ButtonClickBuyMixed() {
        GlobalMessage.ShopBuyMixed();
    }
    public void ButtonClickBuyMoneybox()
    {
        GlobalMessage.ShopBuyMoneybox();
    }

    public void ButtonClickPiggyBank() {
        GlobalMessage.ShopPiggyBank();
    }


    //������� ����������� ��������� � ������� �������� �� �����
    public void ButtonClickSubscription()
    {
        GlobalMessage.ShopBuySubscriptionMonth();
    }
}
