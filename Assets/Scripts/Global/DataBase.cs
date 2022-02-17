using UnityEngine;
using Firebase.Database;

public class DataBase : MonoBehaviour
{
    //Инициализация референса
    static DatabaseReference reference;

    //Имена сохраняемых данных, должно совпадать на firebase

    public struct TypeTime{
        public const string Year = "Year"; //год
        public const string Month = "Month"; //месяц
    }

    //Раздел уровней
    public struct TypeLevel {
        public const string Levels = "Levels"; //Подраздел уровни
        public const string MoveToWin = "Move_To_Win";
        public const string PlayersVictory = "Players_Passed";
        public const string PlayersNotPassed = "Players_Not_Passed";
        public const string AverageScore = "Average_Score"; //Количество набираемых очков в среднем

        //Покупки общие на уровне
        public const string Store = "Store"; //Покупки - подраздел
        public const string CoronaUse = "Corona_Use"; //корона
        public const string CoronaBuy = "Corona_Buy";
        public const string pillUse = "Pill_Use"; //Пилюля
        public const string pillBuy = "Pill_Buy";
        public const string bombUse = "Bomb_Use"; //бомба
        public const string bombBuy = "Bomb_Buy";
        public const string MixedUse = "Mixed_Use"; //перемешивания
        public const string MixedBuy = "Mixed_Buy";
        public const string HummerUse = "Hummer_Use"; //молоток
        public const string HummedBuy = "Hummer_Buy";

        //Покупки ходов 
        public const string MoveBuy = "Move_Buy"; //покупка ходов золотом
        public const string MoveAd = "Move_Ad"; //Покупка ходов просмотром рекламы

        //покупки золота
        public const string Gold___50 = "Gold___50";
        public const string Gold__100 = "Gold__100";
        public const string Gold__250 = "Gold__250";
        public const string Gold__500 = "Gold__500";
        public const string Gold_1000 = "Gold_1000";
    }

    //Раздел профиля
    public struct TypeProfile {
        public const string Profiles = "Profiles";
        public const string Level_Max = "Level_opened_maximum";
        public const string Level_Open_Averade = "Level_opened_average";
        public const string Level_Open_Averade_Count = "Level_opened_average_count";
    }

    // Start is called before the first frame update
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        SaveLevelData(0, TypeLevel.HummerUse, "set data test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Сохранить данные в базу данных


    static public void SaveLevelData(int numlevel, string dataType, string dataValue) {
        reference.Child(TypeLevel.Levels).Child(numlevel.ToString()).Child(dataType).SetValueAsync(dataValue);

    }
}
