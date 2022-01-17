using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Item Health = new Item(1);
    public Item Ticket = new Item(1);

    public Item ShopInternal = new Item(15);
    public Item ShopRocket = new Item(45);
    public Item ShopBomb = new Item(45);
    public Item ShopColor5 = new Item(100);
    public Item ShopMixed = new Item(35);

    private void Start()
    {
        main = this;
        LoadProfie();
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
