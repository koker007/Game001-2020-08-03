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
    /// ������� ������ �� ������� �� ������� �������
    /// </summary>
    public int ProfileLevelGetGift;
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
    public float ProfileCurrentLVLScoreMax = 100;
    public float ProfileCurrentLVLScoreNow = 100;

    public float ProfileTermsOfUse = 0;

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
    const string strProfileLevelGetGift = "ProfileLevelGetGift";

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
    int Star1Count = 0;
    int Star2Count = 0;
    int Star3Count = 0;

    /// <summary>
    /// ����� ������ �������� �� ������ �������
    /// </summary>
    public int[] LVLGold = new int[1];
    public int LVLGoldCount = 0;

    [SerializeField]
    public MenuGameplay.SuperHitType[] ListGiftProfileLVL = new MenuGameplay.SuperHitType[1];

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

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        LoadProfie();
        HealthTimer.main.HealthRegenerateRealTime();
    }

    //�������� ������
    private void LoadProfie()
    {

        ProfileTermsOfUse = PlayerPrefs.GetInt(strProfileTermsOfUse, 0);

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
            ProfilelevelOpen = 200;
            Health.Amount = 100;

            ShopInternal.Amount = 10;
            ShopRocket.Amount = 10;
            ShopBomb.Amount = 10;
            ShopColor5.Amount = 10;
            ShopMixed.Amount = 10;
        }

        ReCalcStarsCount();
        ReCalcGoldCount();
        ReCalcProfileLVL();

    }
    public void Save() {
        PlayerPrefs.SetInt(strProfileLevel, ProfileLevel);
        PlayerPrefs.SetInt(strProfileScore, ProfileScore);
        PlayerPrefs.SetInt(strProfileLevelOpen, ProfilelevelOpen);

        PlayerPrefs.SetFloat(strProfileTermsOfUse, ProfileTermsOfUse);

        PlayerPrefs.SetInt(strProfileLevelGetGift, ProfileLevelGetGift);

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
        dataStr += strProfileLevelGetGift + spliterKAD + ProfileLevelGetGift + spliterDAD;
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
        else if (key == strProfileLevelGetGift) ProfileLevelGetGift = System.Convert.ToInt32(data);
        else if (key == strProfileScore) ProfileScore = System.Convert.ToInt32(data);
        else if (key == strProfileLevelOpen) ProfilelevelOpen = System.Convert.ToInt32(data);
        else if (key == strProfileTermsOfUse) ProfileTermsOfUse = System.Convert.ToInt32(data);

        else if (key == strGoldAmount) GoldAmount = System.Convert.ToInt32(data);
        else if (key == strHealth) Health.Amount = System.Convert.ToInt32(data);
        else if (key == strTicket) Ticket.Amount = System.Convert.ToInt32(data);
        else if (key == strMoneyboxCapacity) moneyboxCapacity = System.Convert.ToInt32(data);
        else if (key == strMoneyboxContent) moneyboxContent = System.Convert.ToInt32(data);

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

            //�������� ������ �� ��������
            LVLStar[lvl] = stars;
            LVLGold[lvl] = gold;

        }

        ReCalcStarsCount();
        ReCalcGoldCount();
        ReCalcProfileLVL();
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

        ReCalcStarsCount();
        ReCalcGoldCount();
        ReCalcProfileLVL();

        SaveForGoogleLVL();
    }

    void ReCalcGoldCount() {
        LVLGoldCount = 0;
        foreach (int x in LVLGold) {
            if (x == 1) LVLGoldCount++;
        }
    }
    void ReCalcStarsCount() {
        Star1Count = 0;
        Star2Count = 0;
        Star3Count = 0;

        foreach (int x in LVLStar) {
            if (x == 1) Star1Count++;
            else if (x == 2) Star2Count++;
            else if (x == 3) Star3Count++;
        }


    }

    //����������� ������� ������ � ��� ����
    void ReCalcProfileLVL() {
        //�������� ���������� ����� ��
        ProfileScore = 0;
        ProfileLevel = 1;

        //�� �������� ������ ����� �������� 1 ����
        ProfileScore += ProfilelevelOpen * 1;

        //�� ������ ����� �������� 2,3,4 ����
        ProfileScore += Star1Count * 2;
        ProfileScore += Star2Count * 3;
        ProfileScore += Star3Count * 4;

        //�� ���������� �� ������ ������ ����� �������� 5 �����
        ProfileScore += LVLGoldCount * 5;

        //������� ������� ������
        //����������� ������ ������� ����� 
        ProfileCurrentLVLScoreMax = 10;
        float CoofNextLVL = 1.18f;

        bool isComplite = false;
        //������� ����� ��� ��������� ���� ������� ��� ��������
        ProfileCurrentLVLScoreNow = ProfileScore;
        while (!isComplite) {
            ProfileCurrentLVLScoreMax *= CoofNextLVL;

            //���� ����� � ������ ������ ��� ���� �� ������� �������
            if (ProfileCurrentLVLScoreNow >= ProfileCurrentLVLScoreMax)
            {
                //��������� �������
                ProfileLevel++;
                //�������� ���� ������ �� ���������� �����
                ProfileCurrentLVLScoreNow -= ProfileCurrentLVLScoreMax;
            }

            //��� ��� �������� �������
            else {
                isComplite = true;
            }
        }

        //
    }

    public bool CanPlusPlayerLVLGift()
    {

        //���� ������� �� ������� ����� ��� ������� ������� ������ ���� ����� ������ ������ ���
        //���� ��� �� ���� ��������� ������ ������� �� ����� �� �������
        if (ProfileLevelGetGift >= ProfileLevel &&
            !GooglePlay.main.FirstGetProfile)
        {
            return false;
        }

        return true;
    
    }
    public bool PlusPlayerLVLGift() {
        bool isOk = false;

        //���� ���������� ������ �������
        if (!CanPlusPlayerLVLGift()) {
            return isOk;
        }

        //��� ��, ���������� �������
        ProfileLevelGetGift++;
        isOk = true;

        //���������� ������ � ����
        Save();

        return isOk;
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

    public void GiftItem(Item item, int num)
    {
        item.Amount += num;
        SaveItemAmount();
    }
    public void GiftGold(int num)
    {
        GoldAmount += num;
        SaveItemAmount();
    }



    //������������ ���������� ������
    public void SetHealth(int value)
    {
        if (value < 0)
            value = 0;

        Health.Amount = value;
        PlayerPrefs.SetInt(strHealth, Health.Amount);
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
