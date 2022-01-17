using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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

    private char spliterDAD = ';'; //����������� ��������� ������ DAD = DataAndData
    private char spliterKAD = '='; //����������� ����� � ���������� ����� ������ KAD = KeyAndData


    private char keyOfLVLnum = 'L';
    private char keyOfLVLstars = 'S';
    private char keyOfLVLgold = 'G';


    //���������������� ����������
    const string strProfileTermsOfUse = "ProfileTermsOfUse";

    const string strProfileLevel = "ProfileLevel";
    const string strProfileScore = "ProfileScore";
    const string strProfileLevelOpen = "ProfileLevelOpen";

    const string strGoldAmount = "GoldAmound";
    const string strMoneyboxCapacity = "MoneyboxCapacity";
    const string strMoneyboxContent = "MoneyboxContent";
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
    public int moneyboxCapacity; //������ ������� ����� �����
    public int moneyboxContent; //������ ������� ���������� �� ������ ������

    /// ������� ����� ������� ����� �� ���� ������
    /// </summary>
    public int[] LVLStar = new int[1];
    /// <summary>
    /// ����� ������ �������� �� ������ �������
    /// </summary>
    public int[] LVLGold = new int[1];
    

    [System.Serializable]
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
    [SerializeField]
    public Item Health = new Item(1);
    public Item Ticket = new Item(1);

    public Item ShopInternal = new Item(15);
    public Item ShopRocket = new Item(45);
    public Item ShopBomb = new Item(45);
    public Item ShopColor5 = new Item(100);
    public Item ShopMixed = new Item(35);

    private bool _healthRegenerateStat;
    private int _TimeStartRegeneration;
    private int _SystemTimeStartRegeneration;
    private const string _SystemTimeStartRegenerationID = "SystemTimeStartRegeneration";
    private const int _TimeForRegenerate = 30; //second
    private DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc); //������ ������� �������
    [SerializeField]private Text _TimeRegenerationText;
    private void Start()
    {
        main = this;
        LoadProfie();
        //HealthRegenerateRealTime();
    }
    private void Update()
    {
        HealthRegenerate();
    }

    //��� ������� ���� ��������� ������� �� ���� ������������
    private void HealthRegenerateRealTime()
    {
        _SystemTimeStartRegeneration = PlayerPrefs.GetInt("SystemTimeStartRegeneration", (int)(System.DateTime.UtcNow - epochStart).TotalSeconds);
        int plusHealth = (int)((System.DateTime.UtcNow - epochStart).TotalSeconds - _SystemTimeStartRegeneration) / _TimeForRegenerate;
        if(plusHealth + Health.Amount > 5)
        {
            Health.Amount = 5;
        }
        else
        {
            Health.Amount = plusHealth;
        }
    }

    //�������� ����� �� �������������� �� � ������ �������
    private void HealthRegenerate()
    {
        if (Health.Amount >= 5 && !_healthRegenerateStat)
            return;

        if (_healthRegenerateStat)
        {
            if (_TimeForRegenerate <= (int)Time.time - _TimeStartRegeneration)
            {
                Health.Amount++;
                _TimeRegenerationText.text = "";
                _healthRegenerateStat = false;
            }
            else
            {
                int timeForRegenerate = _TimeForRegenerate - ((int)Time.time - _TimeStartRegeneration);
                int second = timeForRegenerate % 60;
                int minute = timeForRegenerate / 60;
                _TimeRegenerationText.text = $"{minute}:{second}";
            }
        }
        else
        {
            _healthRegenerateStat = true; 
            _SystemTimeStartRegeneration = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
            PlayerPrefs.SetInt("SystemTimeStartRegeneration", _SystemTimeStartRegeneration);
            _TimeStartRegeneration = (int)Time.time;
        }
    }

    //�������� ������
    private void LoadProfie()
    {

        ProfileTermsOfUse = PlayerPrefs.GetInt(strProfileTermsOfUse, 0);
        ProfileTermsOfUse = 0;

        ProfileLevel = PlayerPrefs.GetInt(strProfileLevel, 1);
        ProfileScore = PlayerPrefs.GetInt(strProfileScore, 0);
        ProfilelevelOpen = PlayerPrefs.GetInt(strProfileLevelOpen, 1);

        GoldAmount = PlayerPrefs.GetInt(strGoldAmount, 10);
        moneyboxCapacity = PlayerPrefs.GetInt(strMoneyboxCapacity, 10);
        moneyboxContent = PlayerPrefs.GetInt(strMoneyboxContent, 0);


        Health.Amount = PlayerPrefs.GetInt(strHealth, 5);
        Ticket.Amount = PlayerPrefs.GetInt(strTicket, 5);
        ShopInternal.Amount = PlayerPrefs.GetInt(strShopInternal, 3);
        ShopRocket.Amount = PlayerPrefs.GetInt(strShopRocket, 3);
        ShopBomb.Amount = PlayerPrefs.GetInt(strShopBomb, 3);
        ShopColor5.Amount = PlayerPrefs.GetInt(strShopColor5, 3);

        ShopMixed.Amount = PlayerPrefs.GetInt(strShopMixed, 3);

        if (Settings.main.DeveloperTesting) {
            GoldAmount = 100;
            ProfilelevelOpen = 100;
            Health.Amount = 100;

            ShopInternal.Amount = 10;
            ShopRocket.Amount = 10;
            ShopBomb.Amount = 10;
            ShopColor5.Amount = 10;
            ShopMixed.Amount = 10;
        }
        
    }
    public void Save() {
        PlayerPrefs.SetInt(strProfileLevel, ProfileLevel);
        PlayerPrefs.SetInt(strProfileScore, ProfileScore);
        PlayerPrefs.SetInt(strProfileLevelOpen, ProfilelevelOpen);

        PlayerPrefs.SetInt(strProfileTermsOfUse, ProfileTermsOfUse);

        SaveItemAmount();

        SaveToGoogle();
    }

    //��������� ������ � ���� ���� � ���� ������
    public void SaveToGoogle() {
        //if (!GooglePlay.main.isAutorized)
            //return;

        //������� ������ ����������
        string dataStr = "";

        //���������� ������
        dataStr += strProfileLevel + spliterKAD + ProfileLevel + spliterDAD;
        dataStr += strProfileScore + spliterKAD + ProfileScore + spliterDAD;
        dataStr += strProfileLevelOpen + spliterKAD + ProfilelevelOpen + spliterDAD;
        dataStr += strProfileTermsOfUse + spliterKAD + ProfileTermsOfUse + spliterDAD;

        //������ ����
        dataStr += strGoldAmount + spliterKAD + GoldAmount + spliterDAD;
        dataStr += strHealth + spliterKAD + Health.Amount + spliterDAD;
        dataStr += strTicket + spliterKAD + Ticket.Amount + spliterDAD;
        dataStr += strMoneyboxCapacity + spliterKAD + moneyboxCapacity + spliterDAD;
        dataStr += strMoneyboxContent + spliterKAD + moneyboxContent + spliterDAD;

        //������ ��������
        dataStr += strShopInternal + spliterKAD + ShopInternal.Amount + spliterDAD;
        dataStr += strShopRocket + spliterKAD + ShopRocket.Amount + spliterDAD;
        dataStr += strShopBomb + spliterKAD + ShopBomb.Amount + spliterDAD;
        dataStr += strShopColor5 + spliterKAD + ShopColor5.Amount + spliterDAD;
        dataStr += strShopMixed + spliterKAD + ShopMixed.Amount + spliterDAD;

        //��������� ������ �� �������� � ����
        GooglePlay.main.AddBufferWaitingFile(GooglePlay.KeyFileProfile, dataStr);

        //��������� ������ �������
        SaveForGoogleLVL();
    }

    //������ ������� �������� ������ �� �����
    public void LoadFromGoogle() {
        
    }

    //���� ���������� ������ ������ ������ ��� ���� ���������� � ������� �����������
    public void LoadFromGoogle(string dataStr) {
        
        //��������� ������ �� ��������� ������
        string[] dataDADs = dataStr.Split(spliterDAD);

        foreach (string dataDAD in dataDADs) {
            string[] dataKAD = dataDAD.Split(spliterKAD);
            //���� ������ �� 2 �� ��� ������
            if (dataKAD.Length != 2) {
                continue;
            }

            //���������� ������ ������� ������ ������� �� �����
            WriteDataFromKey(dataKAD[0], dataKAD[1]);
        }

    }

    /// <summary>
    /// ���������� ������ �� �����
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public void WriteDataFromKey(string key, string data) {

        if (key == strProfileLevel) ProfileLevel = System.Convert.ToInt32(data);
        else if (key == strProfileScore) ProfileScore = System.Convert.ToInt32(data);
        else if (key == strProfileLevelOpen) ProfilelevelOpen = System.Convert.ToInt32(data);
        else if (key == strProfileTermsOfUse) ProfileTermsOfUse = System.Convert.ToInt32(data);

        else if (key == strGoldAmount) ProfileTermsOfUse = System.Convert.ToInt32(GoldAmount);
        else if (key == strHealth) ProfileTermsOfUse = System.Convert.ToInt32(Health.Amount);
        else if (key == strTicket) ProfileTermsOfUse = System.Convert.ToInt32(Ticket.Amount);
        else if (key == strMoneyboxCapacity) ProfileTermsOfUse = System.Convert.ToInt32(moneyboxCapacity);
        else if (key == strMoneyboxContent) ProfileTermsOfUse = System.Convert.ToInt32(moneyboxContent);

        else if (key == strShopInternal) ShopInternal.Amount = System.Convert.ToInt32(data);
        else if (key == strShopRocket) ShopRocket.Amount = System.Convert.ToInt32(data);
        else if (key == strShopBomb) ShopBomb.Amount = System.Convert.ToInt32(data);
        else if (key == strShopColor5) ShopColor5.Amount = System.Convert.ToInt32(data);
        else if (key == strShopMixed) ShopMixed.Amount = System.Convert.ToInt32(data);

        
    }

    //��������� ������ ������� � ��������� ���� �� ������� �����
    void TestArray(int length, ref int[] array)
    {
        //���� ���������� �� ���������
        if (length < array.Length) return;

        //��������� �� ���������� ��������
        int[] arrayNew = new int[length + 1];

        //��������� �������
        for (int x = 0; x < array.Length; x++)
        {
            arrayNew[x] = array[x];
        }

        array = arrayNew;
    }

    public void LoadFromGoogleLVL(string dataStr) {
        //��������� ������ �� ��������� ������
        string[] dataDADs = dataStr.Split(spliterDAD);

        foreach (string dataDAD in dataDADs)
        {
            string[] dataKAD = dataDAD.Split(spliterKAD);
            //���� ������ ������ 2 �� ��� ������
            if (dataKAD.Length < 2)
            {
                continue;
            }

            //���������� ������ ������� ������ ������� �� �����
            WriteDataLVL(dataKAD);
        }

        //��������� ������ ������� ������ ������� �� �����
        void WriteDataLVL(string[] dataKAD) {

            string LVLStr = "";
            string starsStr = "";
            string goldStr = "";

            //��������� ������
            foreach(string data in dataKAD) {
                string dataOnly = "";
                for (int num = 1; num < data.Length; num++) {
                    dataOnly += data[num];
                }

                //������� ���� ���������� ���
                if (dataOnly == "") continue;

                //�������� ���� ��������� ����������
                //����� ������
                if (data[0] == keyOfLVLnum)
                    LVLStr = dataOnly;
                //���������� �����
                else if (data[0] == keyOfLVLstars)
                    starsStr = dataOnly;
                //�������� �� �� ������
                else if (data[0] == keyOfLVLgold)
                    goldStr = dataOnly;
                
            }

            //������� ���� ������� �����������
            if (LVLStr == "") return;

            //��������� ����� ������� � ����������� ������
            int lvl = System.Convert.ToInt32(LVLStr);
            int stars = System.Convert.ToInt32(starsStr);
            int gold = System.Convert.ToInt32(goldStr);

            //���� ����������� ������� ���� ��� ����������� �������, �� ���������� ������
            TestArray(lvl, ref LVLStar);
            TestArray(lvl, ref LVLGold);

            /*
            if (lvl >= LVLStar.Length) {
                //��������� �� ���������� ��������
                int[] LVLStarNew = new int[lvl+1];

                //��������� �������
                for (int x = 0; x < LVLStar.Length; x++) {
                    LVLStarNew[x] = LVLStar[x];
                }

                LVLStar = LVLStarNew;
            }
            */

            //�������� ������ �� ��������
            LVLStar[lvl] = stars;
            LVLGold[lvl] = gold;

        }
    }
    void SaveForGoogleLVL() {
        //������� ������ ����������
        string dataStr = "";

        TestArray(ProfilelevelOpen, ref LVLStar);
        TestArray(ProfilelevelOpen, ref LVLGold);

        //�������� � ����� ����� ��������� ��� ��������� ������ � ����� ������ ��������
        for (int x = ProfilelevelOpen; x > 0; x--) {

            //���� ������ ��� �� ���������� ���� ���
            if (LVLStar[x] == 0 &&
                 LVLGold[x] == 0
                ) {
                continue;
            }

            //������ ���� ���������
            dataStr += "" + keyOfLVLnum + x + spliterKAD; //����� ������
            dataStr += "" + keyOfLVLstars + LVLStar[x] + spliterKAD; //���������� �����

            dataStr += "" + keyOfLVLgold + LVLGold[x] + spliterDAD; //������� ����������� //������ ���� ���������, � ����� ������ ��������� ������
        }
        
        //��� �����
        //LoadFromGoogleLVL(dataStr);

        //������ 
        /*
        for (int x = LVLStar.Length - 1; x > 0; x--) {
            //���������� ���� ����� ���
            if (LVLStar[x] == 0)
                continue;

            //���������� � ������ |LVL=NUM|
            dataStr += "" + x + spliterKAD + LVLStar[x] + spliterDAD;
        }
        */

        //if (dataStr == "") return;

        //��������� ������ �� �������� � ����
        GooglePlay.main.AddBufferWaitingFile(GooglePlay.KeyFileLVLs, dataStr);
    }
    
    //��������� ������
    public void SetLVLStar(int LVLnum, int starsCount) {
        

        //����������� ���� �� ������� �����
        if (LVLnum >= LVLStar.Length) {
            int[] LVLStarNew = new int[LVLnum + 1];

            //��������� ������
            for (int x = 0; x < LVLStar.Length; x++) {
                LVLStarNew[x] = LVLStar[x];
            }

            LVLStar = LVLStarNew;
        }

        LVLStar[LVLnum] = starsCount;

        SaveForGoogleLVL();
    }
    
    //��������� ������
    public void SetLVLGold(int LVLnum, int goldNum) {
        if (LVLnum >= LVLGold.Length)
        {
            int[] LVLGoldNew = new int[LVLnum + 1];

            //��������� ������
            for (int x = 0; x < LVLGold.Length; x++)
            {
                LVLGoldNew[x] = LVLGold[x];
            }

            LVLGold = LVLGoldNew;
        }

        if (LVLGold[LVLnum] < goldNum)
            LVLGold[LVLnum] = goldNum;
        else return;

        SaveForGoogleLVL();
    }

    /// <summary>
    /// ���������� ����� ������ ������
    /// </summary>
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
    /// ������� ������� �� ������� ������
    /// </summary>
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
            SaveItemAmount();

            return true;
        }
    }

    /// <summary>
    /// ������� �������� �� ����
    /// </summary>
    /// <returns></returns>
    public void PayPack(ShopPack Pack) 
    {
        GoldAmount += Pack.BuyMoneyNum;
        ShopInternal.Amount += Pack.BuyInternalNum;
        ShopRocket.Amount += Pack.BuyRocketNum;
        ShopBomb.Amount += Pack.BuyBombNum;
        ShopColor5.Amount += Pack.BuyColor5Num;
        ShopMixed.Amount += Pack.BuyMixedNum;

        SaveItemAmount();
    }
    public bool PayPackForMoney(ShopPack Pack)
    {
        if (GoldAmount < Pack.CostMoney)
        {
            return false;
        }
        else
        {
            GoldAmount -= Pack.CostMoney;
            PayPack(Pack);
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
        PlayerPrefs.SetInt(strMoneyboxCapacity, moneyboxCapacity);
        PlayerPrefs.SetInt(strMoneyboxContent, moneyboxContent);

        PlayerPrefs.SetInt(strShopInternal, ShopInternal.Amount);
        PlayerPrefs.SetInt(strShopRocket, ShopRocket.Amount);
        PlayerPrefs.SetInt(strShopBomb, ShopBomb.Amount);
        PlayerPrefs.SetInt(strShopColor5, ShopColor5.Amount);
        PlayerPrefs.SetInt(strShopMixed, ShopMixed.Amount);

        SaveToGoogle();
    }

    public void LevelPassed(int Level)
    {
        if(ProfilelevelOpen < Level + 1)
        {
            ProfilelevelOpen = Level + 1;
        }
        PlayerPrefs.SetInt("ProfilelevelOpen", ProfilelevelOpen);
    }

    #region moneybox

    //���������� �������
    public void OpenMoneybox()
    {
        GoldAmount += moneyboxContent;
        moneyboxContent = 0;
        SaveItemAmount();
    }

    //�������� �������
    public void UpgradeMoneybox()
    {
        moneyboxCapacity += 5;
        SaveItemAmount();
    }

    //��������� �������
    public void FillMoneyBox(int goldAmount)
    {
        moneyboxContent += goldAmount;
        if (moneyboxContent > moneyboxCapacity)
        {
            moneyboxContent = moneyboxCapacity;
        }
        SaveItemAmount();
    }
    #endregion
}
