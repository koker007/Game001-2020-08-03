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

        public void SetValues(TypeItem item, int count)
        {
            _typeItem = item;
            _count = count;
        }

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


    public List<Sprite> itemSprites = new List<Sprite>();
    public List<Day> days = new List<Day>();

    private DateTime _epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc); //начало отсчета времени
    private int _lastDay;
    private string _lastDayKey = "LastDay";
    public int DaysInGameCounter;
    private string _daysInGameCounterKey = "DaysInGameCounterKey";
    private int _dayOfPurchase;
    private string _dayOfPurchaseKey = "DayOfPurchase";
    public int DaySubEnded;
    public int DayCombo;
    private string _dayComboKey = "DayCombo";

    private void Awake()
    {
        main = this;
    }

    public void BuySubscription()
    {
        try
        {
            _dayOfPurchase = (int)((TimeWorld.GetTimeWorld() - _epochStart).TotalDays);
        }
        catch
        {
            _dayOfPurchase = (int)((DateTime.UtcNow - _epochStart).TotalDays);
        }
        _lastDay = _dayOfPurchase;
            DayCombo = 1;
            DaySubEnded = 0;

            GenerateNewGifts();
            GlobalMessage.Calendar();

            PlayerPrefs.SetInt(_dayOfPurchaseKey, _dayOfPurchase);
            PlayerPrefs.SetInt(_lastDayKey, _lastDay);
            PlayerPrefs.SetInt(_dayComboKey, DayCombo);
    }

    private void GenerateNewGifts()
    {
        int maxGold = 100;

        for(int i = 0; i < days.Count; i++)
        {
            if(i == 14)
            {
                days[i].SetValues(TypeItem.ShopColor5, 2);
            }
            else
            {
                if (i % 2 == 0)
                {
                    days[i].SetValues(TypeItem.Gold, Gold());
                }
                else
                {
                    int type = UnityEngine.Random.Range(3, 8);

                    switch(type)
                    {
                        case 3:
                            days[i].SetValues((TypeItem)type, UnityEngine.Random.Range(1, 5));
                            break;
                        case 4:
                            days[i].SetValues((TypeItem)type, UnityEngine.Random.Range(1, 3));
                            break;
                        case 5:
                            days[i].SetValues((TypeItem)type, UnityEngine.Random.Range(1, 3));
                            break;
                        case 6:
                            days[i].SetValues((TypeItem)type, UnityEngine.Random.Range(1, 2));
                            break;
                        case 7:
                            days[i].SetValues((TypeItem)type, UnityEngine.Random.Range(1, 3));
                            break;
                    }
                }
            }
        }

        days[26].SetValues(TypeItem.Gold, days[26].RetCount() + maxGold / 2);
        days[28].SetValues(TypeItem.Gold, days[28].RetCount() + maxGold / 2);
        days[days.Count - 1].SetValues(TypeItem.Gold, 20);

        PlayerProfile.main.SaveCalendar();

        int Gold()
        {
            int gold = UnityEngine.Random.Range(4, 10);
            if(maxGold >= gold)
            {
                maxGold -= gold;
            }
            else
            {
                gold = maxGold;
                maxGold = 0;
            }
            return gold;
        }
    }

    public void CheckNewDay()
    {
        try
        {
            _lastDay = PlayerPrefs.GetInt(_lastDayKey, 0);
            DayCombo = PlayerPrefs.GetInt(_dayComboKey, 0);
            DaysInGameCounter = PlayerPrefs.GetInt(_daysInGameCounterKey, 0);

            if (_lastDay < (int)(TimeWorld.GetTimeWorld() - _epochStart).TotalDays)
            {
                _lastDay = (int)((TimeWorld.GetTimeWorld() - _epochStart).TotalDays);
                PlayerPrefs.SetInt(_lastDayKey, _lastDay);

                DaysInGameCounter++;
                PlayerPrefs.SetInt(_daysInGameCounterKey, DaysInGameCounter);

                SubscribeNewDay();
                DailyGifts.main.CheckHaveGift();
            }
        }
        catch {}
    }


    public void SubscribeNewDay()
    {
        _dayOfPurchase = PlayerPrefs.GetInt(_dayOfPurchaseKey, 0);

        if (_dayOfPurchase + days.Count <= (int)((TimeWorld.GetTimeWorld() - _epochStart).TotalDays))
            return;

        DaySubEnded = (int)((TimeWorld.GetTimeWorld() - _epochStart).TotalDays) - _dayOfPurchase;
        DayCombo++;
        PlayerProfile.main.LoadCalendar();
        GlobalMessage.Calendar();
        PlayerPrefs.SetInt(_dayComboKey, DayCombo);
    }

    public void GetGift()
    {
        days[DayCombo - 1].TakeGift();
    }
}
