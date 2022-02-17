using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGifts : MonoBehaviour
{
    public static DailyGifts main;

    public GiftCalendar.Day[] SevenDays = new GiftCalendar.Day[7];

    private void Awake()
    {
        main = this;
    }

    public void CheckHaveGift()
    {
        if (GiftCalendar.main.DaysInGameCounter <= SevenDays.Length && GiftCalendar.main.DaysInGameCounter > 0)
            GlobalMessage.DailyGift();
    }
}
