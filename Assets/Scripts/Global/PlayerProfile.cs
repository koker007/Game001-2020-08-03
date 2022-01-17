using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//alexandr
//Андрей
/// <summary>
/// профиль игрока, урвоень, его предметы, количество игровой валюты
/// </summary>
public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile main;

    /// <summary>
    /// уровень игрока
    /// </summary>
    public int ProfileLevel;
    /// <summary>
    /// количество очков
    /// </summary>
    public int ProfileScore;
    /// <summary>
    /// Последний открытый доступный уровень
    /// </summary>
    public int ProfilelevelOpen;
    /// <summary>
    /// количество очков до следующего уровня
    /// </summary>
    /// 

    public int ProfileTermsOfUse = 0;

    [HideInInspector]
    public int[] nextLevelPoint = new int[] { 1000 , 5000 , 10000 };

    private char spliterDAD = ';'; //Разделитель множества данных DAD = DataAndData
    private char spliterKAD = '='; //Разделитель ключа и собственно самих данных KAD = KeyAndData


    private char keyOfLVLnum = 'L';
    private char keyOfLVLstars = 'S';
    private char keyOfLVLgold = 'G';


    //Пользовательское соглашение
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
    /// количество игровой валюты
    /// </summary>
    public int GoldAmount;
    public int moneyboxCapacity; //Свинья копилка общий объем
    public int moneyboxContent; //свинья копилка содержимое на данный момент

    /// Сколько очков получил игрок на этом уровне
    /// </summary>
    public int[] LVLStar = new int[1];
    /// <summary>
    /// какие уровни пройдены на золото игроком
    /// </summary>
    public int[] LVLGold = new int[1];
    

    [System.Serializable]
    /// <summary>
    /// покупаемые предметы 
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
    //покупаемые предметы
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
    private DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc); //начало отсчета времени
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

    //при запуске игры проверяет сколько хп надо восстановить
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

    //проверка нужно ли регенерировать хп и отсчет времени
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

    //загрузка данных
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

    //Сохранить данные в гугл плей в виде строки
    public void SaveToGoogle() {
        //if (!GooglePlay.main.isAutorized)
            //return;

        //Создаем список параметров
        string dataStr = "";

        //Глобальные данные
        dataStr += strProfileLevel + spliterKAD + ProfileLevel + spliterDAD;
        dataStr += strProfileScore + spliterKAD + ProfileScore + spliterDAD;
        dataStr += strProfileLevelOpen + spliterKAD + ProfilelevelOpen + spliterDAD;
        dataStr += strProfileTermsOfUse + spliterKAD + ProfileTermsOfUse + spliterDAD;

        //Данные игры
        dataStr += strGoldAmount + spliterKAD + GoldAmount + spliterDAD;
        dataStr += strHealth + spliterKAD + Health.Amount + spliterDAD;
        dataStr += strTicket + spliterKAD + Ticket.Amount + spliterDAD;
        dataStr += strMoneyboxCapacity + spliterKAD + moneyboxCapacity + spliterDAD;
        dataStr += strMoneyboxContent + spliterKAD + moneyboxContent + spliterDAD;

        //Данные магазина
        dataStr += strShopInternal + spliterKAD + ShopInternal.Amount + spliterDAD;
        dataStr += strShopRocket + spliterKAD + ShopRocket.Amount + spliterDAD;
        dataStr += strShopBomb + spliterKAD + ShopBomb.Amount + spliterDAD;
        dataStr += strShopColor5 + spliterKAD + ShopColor5.Amount + spliterDAD;
        dataStr += strShopMixed + spliterKAD + ShopMixed.Amount + spliterDAD;

        //Отправить данные за сохрание в гугл
        GooglePlay.main.AddBufferWaitingFile(GooglePlay.KeyFileProfile, dataStr);

        //Сохранить звезды уровней
        SaveForGoogleLVL();
    }

    //начать процесс загрузки данных из гугла
    public void LoadFromGoogle() {
        
    }

    //Если передается строка значит данные уже были загруженны и требуют расшифровки
    public void LoadFromGoogle(string dataStr) {
        
        //разделяем строку на множество данных
        string[] dataDADs = dataStr.Split(spliterDAD);

        foreach (string dataDAD in dataDADs) {
            string[] dataKAD = dataDAD.Split(spliterKAD);
            //Если данных не 2 то это ошибка
            if (dataKAD.Length != 2) {
                continue;
            }

            //Переписать данные профиля игрока данными из гугла
            WriteDataFromKey(dataKAD[0], dataKAD[1]);
        }

    }

    /// <summary>
    /// переписать данные по ключу
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

    //Проверить размер массива и расширить если не хватает места
    void TestArray(int length, ref int[] array)
    {
        //Если расширение не требуется
        if (length < array.Length) return;

        //расширяем до последнего значения
        int[] arrayNew = new int[length + 1];

        //Заполняем данными
        for (int x = 0; x < array.Length; x++)
        {
            arrayNew[x] = array[x];
        }

        array = arrayNew;
    }

    public void LoadFromGoogleLVL(string dataStr) {
        //разделяем строку на множество данных
        string[] dataDADs = dataStr.Split(spliterDAD);

        foreach (string dataDAD in dataDADs)
        {
            string[] dataKAD = dataDAD.Split(spliterKAD);
            //Если данных меньше 2 то это ошибка
            if (dataKAD.Length < 2)
            {
                continue;
            }

            //Переписать данные профиля игрока данными из гугла
            WriteDataLVL(dataKAD);
        }

        //Прочитать данные профиля игрока данными из гугла
        void WriteDataLVL(string[] dataKAD) {

            string LVLStr = "";
            string starsStr = "";
            string goldStr = "";

            //Разделяем данные
            foreach(string data in dataKAD) {
                string dataOnly = "";
                for (int num = 1; num < data.Length; num++) {
                    dataOnly += data[num];
                }

                //Выходим если информации нет
                if (dataOnly == "") continue;

                //Выбираем чему присвоить информацию
                //Номер уровня
                if (data[0] == keyOfLVLnum)
                    LVLStr = dataOnly;
                //Количество звезд
                else if (data[0] == keyOfLVLstars)
                    starsStr = dataOnly;
                //Пройдено ли на золото
                else if (data[0] == keyOfLVLgold)
                    goldStr = dataOnly;
                
            }

            //Выходим если уровень неопределен
            if (LVLStr == "") return;

            //проверяем какой уровень и преобразуем данные
            int lvl = System.Convert.ToInt32(LVLStr);
            int stars = System.Convert.ToInt32(starsStr);
            int gold = System.Convert.ToInt32(goldStr);

            //если загружаемый уровень выше чем размерность массива, то рассширяем массив
            TestArray(lvl, ref LVLStar);
            TestArray(lvl, ref LVLGold);

            /*
            if (lvl >= LVLStar.Length) {
                //расширяем до последнего значения
                int[] LVLStarNew = new int[lvl+1];

                //Заполняем данными
                for (int x = 0; x < LVLStar.Length; x++) {
                    LVLStarNew[x] = LVLStar[x];
                }

                LVLStar = LVLStarNew;
            }
            */

            //Внедряем данные по значению
            LVLStar[lvl] = stars;
            LVLGold[lvl] = gold;

        }
    }
    void SaveForGoogleLVL() {
        //Создаем список параметров
        string dataStr = "";

        TestArray(ProfilelevelOpen, ref LVLStar);
        TestArray(ProfilelevelOpen, ref LVLGold);

        //Начинаем с конца чтобы последний лвл записался первым и потом первым считался
        for (int x = ProfilelevelOpen; x > 0; x--) {

            //Если данных нет то пропускаем этот лвл
            if (LVLStar[x] == 0 &&
                 LVLGold[x] == 0
                ) {
                continue;
            }

            //Данные есть заполняем
            dataStr += "" + keyOfLVLnum + x + spliterKAD; //Номер уровня
            dataStr += "" + keyOfLVLstars + LVLStar[x] + spliterKAD; //количество звезд

            dataStr += "" + keyOfLVLgold + LVLGold[x] + spliterDAD; //золотое прохождение //Должен быть последним, в конце символ окончания данных
        }
        
        //для теста
        //LoadFromGoogleLVL(dataStr);

        //Старое 
        /*
        for (int x = LVLStar.Length - 1; x > 0; x--) {
            //Пропускаем если звезд нет
            if (LVLStar[x] == 0)
                continue;

            //Записываем в строку |LVL=NUM|
            dataStr += "" + x + spliterKAD + LVLStar[x] + spliterDAD;
        }
        */

        //if (dataStr == "") return;

        //Отправить данные на сохрание в гугл
        GooglePlay.main.AddBufferWaitingFile(GooglePlay.KeyFileLVLs, dataStr);
    }
    
    //Сохранить звезды
    public void SetLVLStar(int LVLnum, int starsCount) {
        

        //Расширяемся если не хватает места
        if (LVLnum >= LVLStar.Length) {
            int[] LVLStarNew = new int[LVLnum + 1];

            //Переносим данные
            for (int x = 0; x < LVLStar.Length; x++) {
                LVLStarNew[x] = LVLStar[x];
            }

            LVLStar = LVLStarNew;
        }

        LVLStar[LVLnum] = starsCount;

        SaveForGoogleLVL();
    }
    
    //Сохранить золото
    public void SetLVLGold(int LVLnum, int goldNum) {
        if (LVLnum >= LVLGold.Length)
        {
            int[] LVLGoldNew = new int[LVLnum + 1];

            //Переносим данные
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
    /// увеличение очков уровня игрока
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
    /// покупка редмета за игровую валюту
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
    /// Покупка предмета за реал
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
    /// сохранение количества предметов
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

    //Опустошаем копилку
    public void OpenMoneybox()
    {
        GoldAmount += moneyboxContent;
        moneyboxContent = 0;
        SaveItemAmount();
    }

    //Улучшаем копилку
    public void UpgradeMoneybox()
    {
        moneyboxCapacity += 5;
        SaveItemAmount();
    }

    //Заполняем копилку
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
