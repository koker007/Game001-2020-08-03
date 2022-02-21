using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDailyGift : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private Text _countText;

    private GiftCalendar.Day _day;

    private void Start()
    {
        try
        {
            Inicializate(DailyGifts.main.SevenDays[GiftCalendar.main.DaysInGameCounter - 1]);
        }
        catch { }
    }

    private void Inicializate(GiftCalendar.Day day)
    {
        _day = day;
        _itemImage.sprite = GiftCalendar.main.itemSprites[(int)day.RetTypeItem()];
        _countText.text = $"X{day.RetCount()}";
    }

    public void ButtonTake()
    {
        _day.TakeGift();

        GiftCalendar.main.SubscribeNewDay();
    }
}
