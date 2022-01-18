using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftCalendar : MonoBehaviour
{
    public static GiftCalendar main;

    [System.Serializable]
    public enum TypeItem
    {
        Gold,
        Health,
        Ticket,
        ShopInternal,
        ShopRocket,
        ShopBomb,
        ShopColor5,
        ShopMixed
    }

    [System.Serializable]
    private class Day
    {
        [SerializeField] private TypeItem _typeItem;
        [SerializeField] private int _count;

        public void TakeGift()
        {
            switch (_typeItem)
            {
                case TypeItem.Gold:
                    PlayerProfile.main.GiftGold(_count);
                    break;
                case TypeItem.Health:
                    PlayerProfile.main.GiftItem(PlayerProfile.main.Health, _count);
                    break;
                case TypeItem.Ticket:
                    PlayerProfile.main.GiftItem(PlayerProfile.main.Ticket, _count);
                    break;
                case TypeItem.ShopInternal:
                    PlayerProfile.main.GiftItem(PlayerProfile.main.ShopInternal, _count);
                    break;
                case TypeItem.ShopRocket:
                    PlayerProfile.main.GiftItem(PlayerProfile.main.ShopRocket, _count);
                    break;
                case TypeItem.ShopBomb:
                    PlayerProfile.main.GiftItem(PlayerProfile.main.ShopBomb, _count);
                    break;
                case TypeItem.ShopColor5:
                    PlayerProfile.main.GiftItem(PlayerProfile.main.ShopColor5, _count);
                    break;
                case TypeItem.ShopMixed:
                    PlayerProfile.main.GiftItem(PlayerProfile.main.ShopMixed, _count);
                    break;
            }
        }
    }

    [SerializeField] public List<Sprite> _itemSprites = new List<Sprite>();

    [SerializeField] private List<Day> _days = new List<Day>();

    private DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc); //начало отсчета времени
    private int LastDay;
    private string LastDayKey = "LastDay";
    private int DayCombo = 0;
    private string DayComboKey = "DayCombo";

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        LastDay = PlayerPrefs.GetInt(LastDayKey, 0);
        DayCombo = PlayerPrefs.GetInt(DayComboKey, 0);

        if (LastDay == (int)((System.DateTime.UtcNow - epochStart).TotalDays) - 1)
        {
            SetDay(DayCombo + 1);
        }
        else
        {
            SetDay(0);
        }
    }

    private void SetDay(int value)
    {
        DayCombo = value;
        if (DayCombo >= _days.Count)
        {
            DayCombo = 0;
        }
        PlayerPrefs.SetInt(DayComboKey, DayCombo);
        LastDay = (int)((System.DateTime.UtcNow - epochStart).TotalDays);
        PlayerPrefs.SetInt(LastDayKey, LastDay);
    }

    public void ButtonGetGift()
    {
        _days[DayCombo].TakeGift();
    }
}
