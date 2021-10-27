using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// управляет кнопками
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
}
