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
    public int[] nextLevelPoint = new int[] { 1000, 5000, 10000 };

    private char spliterDAD = ';'; //����������� ��������� ������ DAD = DataAndData
    private char spliterKAD = '='; //����������� ����� � ���������� ����� ������ KAD = KeyAndData


    private char keyOfLVLnum = 'L';
    private char keyOfLVLstars = 'S';
    private char keyOfLVLgold = 'G';


    //����� ������� ��������� ����� ��������� �������������
    const string strProfileTimeStart = "ProfileTimeStart";
    public string profileTimeStart = "";

    //���������������� ����������
    const string strProfileTermsOfUse = "ProfileTermsOfUse";
    const float startProfileTermsOfUse = 0.00f;

    const string strProfileLevel = "ProfileLevel";
    const int startProfileLevel = 1;
    const string strProfileLevelGetGift = "ProfileLevelGetGift";
    const int startProfileLevelGetGift = 0;

    const string strProfileScore = "ProfileScore";
    const int stastProfileScore = 0;
    const string strProfileLevelOpen = "ProfileLevelOpen";
    const int startProfileLevelOpen = 1;

    const string strPiggyBankNow = "PiggyBankNow";
    const int startPiggyBankNow = 0;
    const string strPiggyBankMax = "PiggyBankMax";
    const int startPiggyBankMax = 30;

    const string strSubscriptionDataYear = "SubscriptionYear";
    const int startSubscriptionDataYear = 1;
    const string strSubscriptionDataMonth = "SubscriptionMonth";
    const int startSubscriptionDataMonth = 1;
    const string strSubscriptionDataDay = "SubscriptionDay";
    const int startSubscriptionDataDay = 1;




    const string strGoldAmount = "GoldAmound";
    const int startGoldAmount = 10;
    const string strMoneyboxCapacity = "MoneyboxCapacity";
    const int startMoneyboxCapacity = 10;
    const string strMoneyboxContent = "MoneyboxContent";
    const int startMoneyboxContent = 0;
    const string strHealth = "HealtAmount";
    const int startHealth = 5;
    const string strTicket = "TicketAmount";
    const int startTicket = 5;
    const string strShopInternal = "ShopInternal";
    const int startShopInternal = 3;
    const string strShopRocket = "ShopRocket";
    const int startShopRocket = 3;
    const string strShopBomb = "ShopBomb";
    const int startShopBomb = 3;
    const string strShopColor5 = "ShopColor5";
    const int startShopColors5 = 3;
    const string strShopMixed = "ShopMixed";
    const int startShopMixed = 3;

    const string strDayVipCalendarTypeGift = "DayVipCalendarTypeGift";
    const string strDayVipCalendarCountGift = "DayVipCalendarCountGift";

    /// <summary>
    /// ���������� ������� ������
    /// </summary>
    public int GoldAmount;
    public int moneyboxCapacity; //������ ������� ����� �����
    public int moneyboxContent; //������ ������� ���������� �� ������ ������

    //������ ������� � ������ � ��� � ������ ������ � ������������ ���������
    public int PiggyBankNow = 0;
    public int PiggyBankMax = 30;

    //��������
    int subscriptionYear = 1;
    int subscriptionMonth = 1;
    int subscriptionDay = 1;
    public System.DateTime subscriptionDateEnd = new System.DateTime(1);


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
    public Item ShopRocket = new Item(20);
    public Item ShopBomb = new Item(20);
    public Item ShopColor5 = new Item(45);
    public Item ShopMixed = new Item(20);

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
        
        profileTimeStart = PlayerPrefs.GetString(strProfileTimeStart, " ");
        

        ProfileTermsOfUse = PlayerPrefs.GetFloat(strProfileTermsOfUse, startProfileTermsOfUse);

        ProfileLevel = PlayerPrefs.GetInt(strProfileLevel, startProfileLevel);
        ProfileScore = PlayerPrefs.GetInt(strProfileScore, 0);
        ProfilelevelOpen = PlayerPrefs.GetInt(strProfileLevelOpen, startProfileLevelOpen);

        GoldAmount = PlayerPrefs.GetInt(strGoldAmount, startGoldAmount);
        moneyboxCapacity = PlayerPrefs.GetInt(strMoneyboxCapacity, startMoneyboxCapacity);
        moneyboxContent = PlayerPrefs.GetInt(strMoneyboxContent, startMoneyboxContent);

        PiggyBankNow = PlayerPrefs.GetInt(strPiggyBankNow, startPiggyBankNow);
        PiggyBankMax = PlayerPrefs.GetInt(strPiggyBankMax, startPiggyBankMax);

        //���� ��������
        subscriptionYear = PlayerPrefs.GetInt(strSubscriptionDataYear, startSubscriptionDataYear);
        subscriptionMonth = PlayerPrefs.GetInt(strSubscriptionDataMonth, startSubscriptionDataMonth);
        subscriptionDay = PlayerPrefs.GetInt(strSubscriptionDataDay, startSubscriptionDataDay);

        //����������� ������ � ����
        subscriptionDateEnd = new System.DateTime(subscriptionYear, subscriptionMonth, subscriptionDay);


        int loadHealth = PlayerPrefs.GetInt(strHealth, startHealth);
        if(Health.Amount < loadHealth)  
            Health.Amount = PlayerPrefs.GetInt(strHealth, startHealth);


        Ticket.Amount = PlayerPrefs.GetInt(strTicket, startTicket);
        ShopInternal.Amount = PlayerPrefs.GetInt(strShopInternal, startShopInternal);
        ShopRocket.Amount = PlayerPrefs.GetInt(strShopRocket, startShopRocket);
        ShopBomb.Amount = PlayerPrefs.GetInt(strShopBomb, startShopBomb);
        ShopColor5.Amount = PlayerPrefs.GetInt(strShopColor5, startShopColors5);

        ShopMixed.Amount = PlayerPrefs.GetInt(strShopMixed, startShopMixed);

        if (Settings.main.DeveloperTesting) {
            GoldAmount = 100;
            ProfilelevelOpen = 250;
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

        //��������� ����� ������ �����������. 
        //���� ����� � ������ ���� �� ��������
        if (profileTimeStart.Length < 2 && TimeWorld.GetTimeWorld().Year > 1000)
            profileTimeStart = TimeWorld.GetTimeWorld().ToString() + " " + Random.Range(1, 9999);

        PlayerPrefs.SetString(strProfileTimeStart, profileTimeStart);

        PlayerPrefs.SetInt(strProfileLevel, ProfileLevel);
        PlayerPrefs.SetInt(strProfileScore, ProfileScore);
        PlayerPrefs.SetInt(strProfileLevelOpen, ProfilelevelOpen);

        PlayerPrefs.SetFloat(strProfileTermsOfUse, ProfileTermsOfUse);

        PlayerPrefs.SetInt(strProfileLevelGetGift, ProfileLevelGetGift);

        PlayerPrefs.SetInt(strPiggyBankNow, PiggyBankNow);
        PlayerPrefs.SetInt(strPiggyBankMax, PiggyBankMax);

        //����������� ���� �� ����
        subscriptionYear = subscriptionDateEnd.Year;
        subscriptionMonth = subscriptionDateEnd.Month;
        subscriptionDay = subscriptionDateEnd.Day;
        PlayerPrefs.SetInt(strSubscriptionDataYear, subscriptionYear);
        PlayerPrefs.SetInt(strSubscriptionDataMonth, subscriptionMonth);
        PlayerPrefs.SetInt(strSubscriptionDataDay, subscriptionDay);

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

        dataStr += strPiggyBankNow + spliterKAD + PiggyBankNow + spliterDAD;
        dataStr += strPiggyBankMax + spliterKAD + PiggyBankMax + spliterDAD;

        dataStr += strSubscriptionDataYear + spliterKAD + subscriptionDateEnd.Year + spliterDAD;
        dataStr += strSubscriptionDataMonth + spliterKAD + subscriptionDateEnd.Month + spliterDAD;
        dataStr += strSubscriptionDataDay + spliterKAD + subscriptionDateEnd.Day + spliterDAD;



        //������ ����
        dataStr += strGoldAmount + spliterKAD + GoldAmount + spliterDAD;
        //dataStr += strHealth + spliterKAD + Health.Amount + spliterDAD;
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
                GooglePlay.main.LastMessage = "Profile: Lenght != 2";
                continue;
            }

            //���������� ������ ������� ������ ������� �� �����
            WriteDataFromKey(dataKAD[0], dataKAD[1]);
        }

        //��� ������ ���� ������������� ������ ������� ���� ����

        //���������� ���� ��������� ��������
        subscriptionDateEnd = new System.DateTime(subscriptionYear, subscriptionMonth, subscriptionDay);

    }

    /// <summary>
    /// ���������� ������ �� �����
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public void WriteDataFromKey(string key, string data) {

        if (key == strProfileLevel)
        {
            GooglePlay.main.LastMessage = "Profile: " + strProfileLevel;
            ProfileLevel = System.Convert.ToInt32(data);
        }
        else if (key == strProfileLevelGetGift)
        {
            GooglePlay.main.LastMessage = "Profile: " + strProfileLevel;
            ProfileLevelGetGift = System.Convert.ToInt32(data);
        }
        else if (key == strProfileScore)
        {
            GooglePlay.main.LastMessage = "Profile: " + strProfileScore;
            ProfileScore = System.Convert.ToInt32(data);
        }
        else if (key == strProfileLevelOpen)
        {
            GooglePlay.main.LastMessage = "Profile: " + strProfileLevelOpen;
            ProfilelevelOpen = System.Convert.ToInt32(data);
        }
        else if (key == strProfileTermsOfUse)
        {
            GooglePlay.main.LastMessage = "Profile: " + strProfileTermsOfUse;
            ProfileTermsOfUse = (float)System.Convert.ToDouble(data);
        }

        //������ �������
        else if (key == strPiggyBankNow)
        {
            PiggyBankNow = System.Convert.ToInt32(data);
        }
        else if (key == strPiggyBankMax) {
            PiggyBankMax = System.Convert.ToInt32(data);
        }

        //�������� (���� ���������)
        else if (key == strSubscriptionDataYear) {
            subscriptionYear = System.Convert.ToInt32(data);
        }
        else if (key == strSubscriptionDataMonth) {
            subscriptionMonth = System.Convert.ToInt32(data);
        }
        else if (key == strSubscriptionDataDay) {
            subscriptionDay = System.Convert.ToInt32(data);
        }

        else if (key == strGoldAmount)
        {
            GooglePlay.main.LastMessage = "Profile: " + strGoldAmount;
            GoldAmount = System.Convert.ToInt32(data);
        }
        //else if (key == strHealth)
        //{
        //    GooglePlay.main.LastMessage = "Profile: " + strHealth;
        //    Health.Amount = System.Convert.ToInt32(data);
        //}
        else if (key == strTicket)
        {
            GooglePlay.main.LastMessage = "Profile: " + strTicket;
            Ticket.Amount = System.Convert.ToInt32(data);
        }
        else if (key == strMoneyboxCapacity)
        {
            GooglePlay.main.LastMessage = "Profile: " + strMoneyboxCapacity;
            moneyboxCapacity = System.Convert.ToInt32(data);
        }
        else if (key == strMoneyboxContent)
        {
            GooglePlay.main.LastMessage = "Profile: " + strMoneyboxContent;
            moneyboxContent = System.Convert.ToInt32(data);
        }

        else if (key == strShopInternal)
        {
            GooglePlay.main.LastMessage = "Profile: " + strShopInternal;
            ShopInternal.Amount = System.Convert.ToInt32(data);
        }
        else if (key == strShopRocket)
        {
            GooglePlay.main.LastMessage = "Profile: " + strShopRocket;
            ShopRocket.Amount = System.Convert.ToInt32(data);
        }
        else if (key == strShopBomb)
        {
            GooglePlay.main.LastMessage = "Profile: " + strShopBomb;
            ShopBomb.Amount = System.Convert.ToInt32(data);
        }
        else if (key == strShopColor5)
        {
            GooglePlay.main.LastMessage = "Profile: " + strShopColor5;
            ShopColor5.Amount = System.Convert.ToInt32(data);
        }
        else if (key == strShopMixed)
        {
            GooglePlay.main.LastMessage = "Profile: " + strShopMixed;
            ShopMixed.Amount = System.Convert.ToInt32(data);
        }


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
            foreach (string data in dataKAD) {
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

    /// <summary>
    /// �������� ���������� ���� �� ������� ������
    /// </summary>
    /// <param name="LVLnum"></param>
    /// <returns></returns>
    public int GetLVLStar(int LVLnum) {

        //���� ����� ������� ���, �� � ����� ��� ���
        if (LVLnum >= LVLStar.Length || LVLnum < 0)
        {
            return 0;
        }
        else {
            return LVLStar[LVLnum];
        }

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
        if (ProfileLevelGetGift >= ProfileLevel ||
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
        if (ProfileScore > nextLevelPoint[ProfileLevel - 1])
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
        if (GoldAmount < item.Cost)
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
    //��������� ��������� ���������
    public void SaveCalendar()
    {
        for(int i = 0; i < GiftCalendar.main.days.Count; i++)
        {
            PlayerPrefs.SetInt($"{strDayVipCalendarCountGift}:{i}", GiftCalendar.main.days[i].RetCount());
            PlayerPrefs.SetInt($"{strDayVipCalendarTypeGift}:{i}", (int)GiftCalendar.main.days[i].RetTypeItem());
        }
    }

    //��������� ��������� ���������
    public void LoadCalendar()
    {
        for (int i = 0; i < GiftCalendar.main.days.Count; i++)
        {
            int count = PlayerPrefs.GetInt($"{strDayVipCalendarCountGift}:{i}");
            int type = PlayerPrefs.GetInt($"{strDayVipCalendarTypeGift}:{i}");
            GiftCalendar.main.days[i].SetValues((GiftCalendar.TypeItem)type, count);
        }
    }

    //������ �������
    public void GiftItem(ref Item item, int num)
    {
        item.Amount += num;
        SaveItemAmount();
    }
    public void GiftGold(int num)
    {
        GoldAmount += num;
        SaveItemAmount();
    }

    //�������� ����� ��������
    public void AddSubscriptionMonth(){
        //����� ��������� ����
        System.DateTime dateEndNew = System.DateTime.Now;

        //���� ������� ���� �������� ������ ��� ������
        if (subscriptionDateEnd > dateEndNew) {
            //����� ���� �������� � �������� ����� �������
            dateEndNew = subscriptionDateEnd;
        }

        //������ � ��������� ���� ���������� �����
        dateEndNew = dateEndNew.AddDays(30);

        //���������� ���� ��������
        subscriptionDateEnd = dateEndNew;

        GiftCalendar.main.BuySubscription();

        //���������� ��������� � ����
        Save();

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

    //������� ������� ������ ������ ��� ��� ������
    public void DeleteProfile() {

        //�������� ����� ����� �������
        profileTimeStart = TimeWorld.GetTimeWorld().ToString() + " " + Random.Range(1, 9999);
        PlayerPrefs.SetString(strProfileTimeStart, profileTimeStart);

        ProfileLevel = startProfileLevel;
        PlayerPrefs.SetInt(strProfileLevel, ProfileLevel);
        ProfileScore = 0;
        PlayerPrefs.SetInt(strProfileScore, ProfileScore);
        ProfilelevelOpen = startProfileLevelOpen;
        PlayerPrefs.SetInt(strProfileLevelOpen, ProfilelevelOpen);

        ProfileTermsOfUse = startProfileTermsOfUse;
        PlayerPrefs.SetFloat(strProfileTermsOfUse, ProfileTermsOfUse);

        ProfileLevelGetGift = startProfileLevelGetGift;
        PlayerPrefs.SetInt(strProfileLevelGetGift, ProfileLevelGetGift);

        //������ �������
        PiggyBankNow = startPiggyBankNow;
        PiggyBankMax = startPiggyBankMax;

        //��������� ����
        subscriptionYear = startSubscriptionDataYear;
        PlayerPrefs.SetInt(strSubscriptionDataYear, subscriptionYear);
        subscriptionMonth = startSubscriptionDataMonth;
        PlayerPrefs.SetInt(strSubscriptionDataMonth, subscriptionMonth);
        subscriptionDay = startSubscriptionDataDay;
        PlayerPrefs.SetInt(strSubscriptionDataDay, subscriptionDay);
        subscriptionDateEnd = new System.DateTime(subscriptionYear, subscriptionMonth, subscriptionDay);

        GoldAmount = startGoldAmount;
        PlayerPrefs.SetInt(strGoldAmount, GoldAmount);
        Health.Amount = startHealth;
        PlayerPrefs.SetInt(strHealth, Health.Amount);
        Ticket.Amount = startTicket;
        PlayerPrefs.SetInt(strTicket, Ticket.Amount);
        moneyboxCapacity = startMoneyboxCapacity;
        PlayerPrefs.SetInt(strMoneyboxCapacity, moneyboxCapacity);
        moneyboxContent = startMoneyboxContent;
        PlayerPrefs.SetInt(strMoneyboxContent, moneyboxContent);

        ShopInternal.Amount = startShopInternal;
        PlayerPrefs.SetInt(strShopInternal, ShopInternal.Amount);
        ShopRocket.Amount = startShopRocket;
        PlayerPrefs.SetInt(strShopRocket, ShopRocket.Amount);
        ShopBomb.Amount = startShopBomb;
        PlayerPrefs.SetInt(strShopBomb, ShopBomb.Amount);
        ShopColor5.Amount = startShopColors5;
        PlayerPrefs.SetInt(strShopColor5, ShopColor5.Amount);
        ShopMixed.Amount = startShopMixed;
        PlayerPrefs.SetInt(strShopMixed, ShopMixed.Amount);

        //��������� ������ ������� �� �������� � ����
        GooglePlay.main.AddBufferWaitingFile(GooglePlay.KeyFileLVLs, "");

        SaveToGoogle();
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


    //�������� ������ � ������ �������
    public void PiggyBankAdd(int count) {
        //���������� ������
        PiggyBankNow += count;
        //���� ������ ������ ��������� �� ����������� �� ���������
        if (PiggyBankNow > PiggyBankMax) {
            PiggyBankNow = PiggyBankMax;
        }
    }
    //������� ������ �������
    public void PiggyBankOpen() {
        //���������� ����������� ������ � ������ ���������� ������
        GoldAmount += PiggyBankNow;
        //� ������ ������ ������ ���
        PiggyBankNow = 0;

        //��������� ������ ��������
        Save();
    }
    //�������� ������ �������
    public void PiggyBankUpgrade() {
        //�������� ������ �������
        int plus = 30;

        PiggyBankMax += (int)(plus);

        //��������� ���������
        Save();
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
