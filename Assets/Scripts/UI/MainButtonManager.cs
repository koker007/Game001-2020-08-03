using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// управл€ет кнопками
/// </summary>
public class MainButtonManager : MonoBehaviour
{
    /// <summary>
    /// содержит парамтеры кнопок
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


    //ќткрыть всплывающее сообщение о покупки подписки на мес€ц
    public void ButtonClickSubscription()
    {
        GlobalMessage.ShopBuySubscriptionMonth();
    }
}
