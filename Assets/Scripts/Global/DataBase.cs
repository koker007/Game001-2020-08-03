using UnityEngine;
using Firebase.Database;

public class DataBase : MonoBehaviour
{
    //������������� ���������
    static DatabaseReference reference;

    //����� ����������� ������, ������ ��������� �� firebase

    public struct TypeTime{
        public const string Year = "Year"; //���
        public const string Month = "Month"; //�����
    }

    //������ �������
    public struct TypeLevel {
        public const string Levels = "Levels"; //��������� ������
        public const string MoveToWin = "Move_To_Win";
        public const string PlayersVictory = "Players_Passed";
        public const string PlayersNotPassed = "Players_Not_Passed";
        public const string AverageScore = "Average_Score"; //���������� ���������� ����� � �������

        //������� ����� �� ������
        public const string Store = "Store"; //������� - ���������
        public const string CoronaUse = "Corona_Use"; //������
        public const string CoronaBuy = "Corona_Buy";
        public const string pillUse = "Pill_Use"; //������
        public const string pillBuy = "Pill_Buy";
        public const string bombUse = "Bomb_Use"; //�����
        public const string bombBuy = "Bomb_Buy";
        public const string MixedUse = "Mixed_Use"; //�������������
        public const string MixedBuy = "Mixed_Buy";
        public const string HummerUse = "Hummer_Use"; //�������
        public const string HummedBuy = "Hummer_Buy";

        //������� ����� 
        public const string MoveBuy = "Move_Buy"; //������� ����� �������
        public const string MoveAd = "Move_Ad"; //������� ����� ���������� �������

        //������� ������
        public const string Gold___50 = "Gold___50";
        public const string Gold__100 = "Gold__100";
        public const string Gold__250 = "Gold__250";
        public const string Gold__500 = "Gold__500";
        public const string Gold_1000 = "Gold_1000";
    }

    //������ �������
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

    //��������� ������ � ���� ������


    static public void SaveLevelData(int numlevel, string dataType, string dataValue) {
        reference.Child(TypeLevel.Levels).Child(numlevel.ToString()).Child(dataType).SetValueAsync(dataValue);

    }
}
