using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//alexandr
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
    public class Day
    {
        [SerializeField] private TypeItem _typeItem;
        [SerializeField] private int _count;

        public TypeItem RetTypeItem()
        {
            return _typeItem;
        }
        public int RetCount()
        {
            return _count;
        }

        public void TakeGift()
        {
            switch (_typeItem)
            {
                case TypeItem.Gold:
                    PlayerProfile.main.GiftGold(_count);
                    break;
                case TypeItem.Health:
                    PlayerProfile.main.GiftItem(ref PlayerProfile.main.Health, _count);
                    break;
                case TypeItem.Ticket:
                    PlayerProfile.main.GiftItem(ref PlayerProfile.main.Ticket, _count);
                    break;
                case TypeItem.ShopInternal:
                    PlayerProfile.main.GiftItem(ref PlayerProfile.main.ShopInternal, _count);
                    break;
                case TypeItem.ShopRocket:
                    PlayerProfile.main.GiftItem(ref PlayerProfile.main.ShopRocket, _count);
                    break;
                case TypeItem.ShopBomb:
                    PlayerProfile.main.GiftItem(ref PlayerProfile.main.ShopBomb, _count);
                    break;
                case TypeItem.ShopColor5:
                    PlayerProfile.main.GiftItem(ref PlayerProfile.main.ShopColor5, _count);
                    break;
                case TypeItem.ShopMixed:
                    PlayerProfile.main.GiftItem(ref PlayerProfile.main.ShopMixed, _count);
                    break;
            }
        }
    }

    public List<Day> days = new List<Day>();

    private DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc); //начало отсчета времени
    private int LastDay;
    private string LastDayKey = "LastDay";
    private int DayOfPurchase;
    private string DayOfPurchaseKey = "DayOfPurchase";
    public int DaySubEnded;
    public int DayCombo = 0;
    private string DayComboKey = "DayCombo";

    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            BuySubscription();
        }
    }

    public void BuySubscription()
    {
        DayOfPurchase = (int)((System.DateTime.UtcNow - epochStart).TotalMinutes);
        LastDay = DayOfPurchase;
        DayCombo = 1;
        DaySubEnded = 0;

        GlobalMessage.Calendar();

        PlayerPrefs.SetInt(DayOfPurchaseKey, DayOfPurchase);
        PlayerPrefs.SetInt(LastDayKey, LastDay);
        PlayerPrefs.SetInt(DayComboKey, DayCombo);
    }

    public void CheckNewDay()
    {
        DayOfPurchase = PlayerPrefs.GetInt(DayOfPurchaseKey, 0);

        if (DayOfPurchase + days.Count < (int)((System.DateTime.UtcNow - epochStart).TotalMinutes))
            return;

        DaySubEnded = (int)((System.DateTime.UtcNow - epochStart).TotalMinutes) - DayOfPurchase;
        LastDay = PlayerPrefs.GetInt(LastDayKey, 0);
        DayCombo = PlayerPrefs.GetInt(DayComboKey, 0);

        if (LastDay < (int)(System.DateTime.UtcNow - epochStart).TotalMinutes)
        {
            LastDay = (int)((System.DateTime.UtcNow - epochStart).TotalMinutes);
            PlayerPrefs.SetInt(LastDayKey, LastDay);

            DayCombo++;
            GlobalMessage.Calendar();

            PlayerPrefs.SetInt(DayComboKey, DayCombo);
        }
    }

    public void GetGift()
    {
        days[DayCombo - 1].TakeGift();
    }
}
