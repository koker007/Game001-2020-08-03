using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
//������
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

    public int ProfileTermsOfUse = 0;

    [HideInInspector]
    public int[] nextLevelPoint = new int[] { 1000 , 5000 , 10000 };

    //���������������� ����������
    const string strProfileTermsOfUse = "ProfileTermsOfUse";

    const string strProfileLevel = "ProfileLevel";
    const string strProfileScore = "ProfileScore";
    const string strProfileLevelOpen = "ProfileLevelOpen";

    const string strGoldAmount = "GoldAmound";
    const string strHealth = "HealtAmount";
    const string strTicket = "TicketAmount";
    const string strShopInternal = "ShopInternal";
    const string strShopRocket = "ShopRocket";
    const string strShopBomb = "ShopBomb";
    const string strShopColor5 = "ShopColor5";
    const string strShopMixed = "ShopMixed";

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

    public Item ShopInternal = new Item(1);
    public Item ShopRocket = new Item(1);
    public Item ShopBomb = new Item(1);
    public Item ShopColor5 = new Item(1);
    public Item ShopMixed = new Item(1);


    private void Awake()
    {
        main = this;
        LoadProfie();
    }

    private void Start()
    {
        main = this;
    }
    //�������� ������
    private void LoadProfie()
    {

        ProfileTermsOfUse = PlayerPrefs.GetInt(strProfileTermsOfUse, 0);
        ProfileTermsOfUse = 0;

        ProfileLevel = PlayerPrefs.GetInt(strProfileLevel, 1);
        ProfileScore = PlayerPrefs.GetInt(strProfileScore, 0);
        ProfilelevelOpen = PlayerPrefs.GetInt(strProfileLevelOpen, 1);
        ProfilelevelOpen = 100;

        GoldAmount = PlayerPrefs.GetInt(strGoldAmount, 10);
        GoldAmount = 40;

        Health.Amount = PlayerPrefs.GetInt(strHealth, 5);
        Ticket.Amount = PlayerPrefs.GetInt(strTicket, 5);
        ShopInternal.Amount = PlayerPrefs.GetInt(strShopInternal, 3);
        ShopRocket.Amount = PlayerPrefs.GetInt(strShopRocket, 3);
        ShopBomb.Amount = PlayerPrefs.GetInt(strShopBomb, 3);
        ShopColor5.Amount = PlayerPrefs.GetInt(strShopColor5, 3);

        ShopMixed.Amount = PlayerPrefs.GetInt(strShopMixed, 3);
        
    }
    public void Save() {
        PlayerPrefs.SetInt(strProfileLevel, ProfileLevel);
        PlayerPrefs.SetInt(strProfileScore, ProfileScore);
        PlayerPrefs.SetInt(strProfileLevelOpen, ProfilelevelOpen);

        PlayerPrefs.SetInt(strProfileTermsOfUse, ProfileTermsOfUse);

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
        PlayerPrefs.SetInt(strProfileLevel, ProfileLevel);
        PlayerPrefs.SetInt(strProfileScore, ProfileScore);
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
        PlayerPrefs.SetInt(strGoldAmount, GoldAmount);
        PlayerPrefs.SetInt(strHealth, Health.Amount);
        PlayerPrefs.SetInt(strTicket, Ticket.Amount);

        PlayerPrefs.SetInt(strShopInternal, ShopInternal.Amount);
        PlayerPrefs.SetInt(strShopRocket, ShopRocket.Amount);
        PlayerPrefs.SetInt(strShopBomb, ShopBomb.Amount);
        PlayerPrefs.SetInt(strShopColor5, ShopColor5.Amount);
        PlayerPrefs.SetInt(strShopMixed, ShopMixed.Amount);
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
