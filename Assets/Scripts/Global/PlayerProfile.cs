using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// ������� ������, �������, ��� ��������, ���������� ������� ������
/// </summary>
public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile main;

    /// <summary>
    /// ������� ������
    /// </summary>
    public int ProfileLevel;
    /// <summary>
    /// ���������� �����
    /// </summary>
    public int ProfileScore;
    /// <summary>
    /// ��������� �������� ��������� �������
    /// </summary>
    public int ProfilelevelOpen;
    /// <summary>
    /// ���������� ����� �� ���������� ������
    /// </summary>
    /// 
    [HideInInspector]
    public int[] nextLevelPoint = new int[] { 1000 , 5000 , 10000 };

    /// <summary>
    /// ���������� ������� ������
    /// </summary>
    public int GoldAmount;

    /// <summary>
    /// ���������� �������� 
    /// </summary>
    public struct Item
    {
        public int Cost;
        public int Amount;

        public Item(int cost)
        {
            Cost = cost;
            Amount = 0;
        }
    }
    //���������� ��������
    public Item Health = new Item(1);
    public Item Ticket = new Item(1);

    public Item ShopBoom = new Item(1);
    public Item ShopRocket = new Item(1);
    public Item ShopColor5 = new Item(1);


    private void Awake()
    {
        main = this;
        LoadProfie();
    }

    private void Start()
    {
    }
    //�������� ������
    private void LoadProfie()
    {
        ProfileLevel = PlayerPrefs.GetInt("ProfileLevel", 1);
        ProfileScore = PlayerPrefs.GetInt("ProfileScore", 0);
        ProfilelevelOpen = PlayerPrefs.GetInt("ProfielevelOpen", 1);
        //ProfilelevelOpen = 40;

        GoldAmount = PlayerPrefs.GetInt("GoldAmount", 10);
        GoldAmount = 40;

        Health.Amount = PlayerPrefs.GetInt("HealthAmount", 5);
        Ticket.Amount = PlayerPrefs.GetInt("TicketAmount", 5);
        ShopBoom.Amount = PlayerPrefs.GetInt("ShopBoom", 3);
        ShopRocket.Amount = PlayerPrefs.GetInt("ShopRocket", 3);
        ShopColor5.Amount = PlayerPrefs.GetInt("ShopColor5", 3);
        
    }
    public void Save() {
        PlayerPrefs.SetInt("ProfileLevel", ProfileLevel);
        PlayerPrefs.SetInt("ProfileScore", ProfileScore);
        PlayerPrefs.SetInt("ProfielevelOpen", ProfilelevelOpen);

        SaveItemAmount();
    }

    /// <summary>
    /// ���������� ����� ������ ������
    /// </summary>
    /// <param name="plus"></param>
    public void ScorePlus(int plus)
    {
        ProfileScore += plus;
        if(ProfileScore > nextLevelPoint[ProfileLevel - 1])
        {
            ProfileScore %= nextLevelPoint[ProfileLevel - 1];
            ProfileLevel++;
        }
        PlayerPrefs.SetInt("ProfileLevel", ProfileLevel);
        PlayerPrefs.SetInt("ProfileScore", ProfileScore);
    }

    /// <summary>
    /// ������� �������
    /// </summary>
    /// <param name="item"></param>
    public bool isPurchaseItem(ref Item item)
    {
        if(GoldAmount < item.Cost)
        {
            return false;
        }
        else
        {
            GoldAmount -= item.Cost;
            item.Amount++;

            MenuWorld.main.SetText();
            //SaveItemAmount();

            return true;
        }
    }

    /// <summary>
    /// ���������� ���������� ���������
    /// </summary>
    private void SaveItemAmount()
    {
        PlayerPrefs.SetInt("GoldAmount", GoldAmount);
        PlayerPrefs.SetInt("HealthAmount", Health.Amount);
        PlayerPrefs.SetInt("TicketAmount", Ticket.Amount);

        PlayerPrefs.SetInt("ShopBoom", ShopBoom.Amount);
        PlayerPrefs.SetInt("ShopRocket", ShopRocket.Amount);
        PlayerPrefs.SetInt("ShopColor5", ShopColor5.Amount);
    }

    public void LevelPassed(int Level)
    {
        if(ProfilelevelOpen < Level + 1)
        {
            ProfilelevelOpen = Level + 1;
        }
        PlayerPrefs.SetInt("ProfilelevelOpen", ProfilelevelOpen);
    }
}
