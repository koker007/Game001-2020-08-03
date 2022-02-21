using Firebase.Database;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase main;

    //������������� ���������
    static DatabaseReference reference;

    //����� ����������� ������, ������ ��������� �� firebase

    public struct TypeTime{
        public const string Year = "Year"; //���
        public const string Month = "Month"; //�����
    }

    //������ �������
    public class TypeLevel {
        const string LevelsStr = "Levels"; //��������� ������
        const string PlayersVictoryStr = "Players_Passed"; //���������� ������������� ������� ������ ������ �������
        const string StartStr = "Start"; //���������� ��� ����� ������� ��� �������
        const string CountMoveToWinAllTimeStr = "Count_Move_To_Win_AllTime"; //����� ���������� ����� ������� ������� � ������
        const string CountMoveToWinAverageStr = "Count_Move_To_Win_Average"; //������� ���������� ����� ������� ������� ������ � ������
        const string ScoreAverageStr = "Score_Average"; //���������� ���������� ����� � �������
        const string ScoreAllTimeStr = "Score_AllTime"; //���������� ����� �� ��� �����

        int levelNumS = -1;
        int PlayersVictoryL = -1;
        int StartL = -1;
        int CountMoveToWinAllTimeL = -1;
        int ScoreAllTimeL = -1;

        int PlayersVictoryS = -1;
        int StartS = -1;
        int CountMoveToWinS = -1;

        int ScoreAllTimeS = -1;

        bool LevelNeed = false;
        bool PlayersVictoryNeed = true;
        bool StartNeed = true;
        bool CountMoveToWinAllTimeNeed = true;
        //static bool CountMoveToWinAverageNeed = true;
        //static bool ScoreAverageNeed = true;
        bool ScoreAllTimeNeed = true;

        //������� ����� �� ������
        const string Store = "Store"; //������� - ���������
        const string CoronaUseStr = "Corona_Use"; //������
        const string CoronaBuyStr = "Corona_Buy";
        const string pillUseStr = "Pill_Use"; //������
        const string pillBuyStr = "Pill_Buy";
        const string bombUseStr = "Bomb_Use"; //�����
        const string bombBuyStr = "Bomb_Buy";
        const string MixedUseStr = "Mixed_Use"; //�������������
        const string MixedBuyStr = "Mixed_Buy";
        const string HummerUseStr = "Hummer_Use"; //�������
        const string HummerBuyStr = "Hummer_Buy";

        //����������� ��������
        int CoronaUseL = -1;
        int CoronaBuyL = -1;
        int PillUseL = -1;
        int PillBuyL = -1;
        int BombUseL = -1;
        int BombBuyL = -1;
        int MixedUseL = -1;
        int MixedBuyL = -1;
        int HummerUseL = -1;
        int HummerBuyL = -1;

        //�������� ������� ���� ���������
        int CoronaUseS = -1;
        int CoronaBuyS = -1;
        int PillUseS = -1;
        int PillBuyS = -1;
        int BombUseS = -1;
        int BombBuyS = -1;
        int MixedUseS = -1;
        int MixedBuyS = -1;
        int HummerUseS = -1;
        int HummerBuyS = -1;

        bool CoronaUseNeed = true;
        bool CoronaBuyNeed = true;
        bool PillUseNeed = true;
        bool PillBuyNeed = true;
        bool BombUseNeed = true;
        bool BombBuyNeed = true;
        bool MixedUseNeed = true;
        bool MixedBuyNeed = true;
        bool HummerUseNeed = true;
        bool HummerBuyNeed = true;

        //������� ����� 
        public const string MoveBuyStr = "Move_Buy"; //������� ����� �������
        public const string MoveAdStr = "Move_Ad"; //������� ����� ���������� �������
        public const string MoveBuyAverageStr = "Move_Buy_Average";
        public const string MoveAdAverageStr = "Move_Ad_Average";

        int MoveBuyL = -1;
        int MoveAdL = -1;

        int MoveBuyS = -1;
        int MoveAdS = -1;

        bool MoveBuyNeed = true;
        bool MoveAdNeed = true;

        //������� ������
        public const string Gold___50str = "Gold___50"; //���������� ������� ������
        public const string Gold__100str = "Gold__100";
        public const string Gold__250str = "Gold__250";
        public const string Gold__500str = "Gold__500";
        public const string Gold_1000str = "Gold_1000";

        int Gold___50L = -1;
        int Gold__100L = -1;
        int Gold__250L = -1;
        int Gold__500L = -1;
        int Gold_1000L = -1;

        int Gold___50S = -1;
        int Gold__100S = -1;
        int Gold__250S = -1;
        int Gold__500S = -1;
        int Gold_1000S = -1;

        bool Gold___50Need = true;
        bool Gold__100Need = true;
        bool Gold__250Need = true;
        bool Gold__500Need = true;
        bool Gold_1000Need = true;

        //��������� �� ������ ������ //���� ������ ���������� �� ������ �� ����, ������� 0
        public void SetLevelData(int levelNumF, bool PlayersVictoryF, int CountMoveToWinF, int ScoreF, 
            int coronaUseF, int coronaBuyF, 
            int pillUseF, int pillBuyF,
            int bombUseF, int bombBuyF,
            int mixedUseF, int mixedBuyF,
            int hummerUseF, int hummerBuyF,
            int moveBuyF, int moveAdF,
            int gold_50F, int gold_100F,
            int gold_250F, int gold_500F,
            int gold_1000F
            ) {


            if (levelNumF < 0) return;

            levelNumS = levelNumF; //��� ������ ������ �������������� ������
            LevelNeed = true;

            if (PlayersVictoryF) {
                PlayersVictoryNeed = true; //������ ���������� ���������
                PlayersVictoryL = -1; //���������� � ����������� ������ ������
                PlayersVictoryS = 1; //���� ���������� ���������� �� ��� �����
            }


            if (CountMoveToWinF > 0) {
                CountMoveToWinAllTimeNeed = true; //������ ���������� ���������
                CountMoveToWinAllTimeL = -1; //���������� � ����������� ������ ������
                CountMoveToWinS = CountMoveToWinF; //���� ���������� ���������� �� ��� �����

            }
            if (ScoreF > 0) {
                ScoreAllTimeNeed = true;
                ScoreAllTimeL = -1;
                ScoreAllTimeS = ScoreF;
            }

            //�������
            if (coronaBuyF > 0) {
                CoronaBuyNeed = true;
                CoronaBuyL = -1;
                CoronaBuyS = coronaBuyF;
            }
            if (coronaUseF > 0) {
                CoronaUseNeed = true;
                CoronaUseL = -1;
                CoronaUseS = coronaUseF;
            }
            if (pillBuyF > 0) {
                PillBuyNeed = true;
                PillBuyL = -1;
                PillBuyS = pillBuyF;
            }
            if (pillUseF > 0) {
                PillUseNeed = true;
                PillUseL = -1;
                PillUseS = pillUseF;
            }
            if (bombBuyF > 0) {
                BombBuyNeed = true;
                BombBuyL = -1;
                BombBuyS = bombBuyF;
            }
            if (bombUseF > 0) {
                BombUseNeed = true;
                BombUseL = -1;
                BombUseS = bombUseF;
            }
            if (mixedBuyF > 0) {
                MixedBuyNeed = true;
                MixedBuyL = -1;
                MixedBuyS = mixedBuyF;
            }
            if (mixedUseF > 0) {
                MixedUseNeed = true;
                MixedUseL = -1;
                MixedUseS = mixedUseF;
            }
            if (hummerBuyF > 0) {
                HummerBuyNeed = true;
                HummerBuyL = -1;
                HummerBuyS = hummerBuyF;
            }
            if (hummerUseF > 0) {
                HummerUseNeed = true;
                HummerUseL = -1;
                HummerUseS = hummerUseF;
            }

            if (moveBuyF > 0) {
                MoveBuyNeed = true;
                MoveBuyL = -1;
                MoveBuyS = moveBuyF;
            }
            if (moveAdF > 0) {
                MoveAdNeed = true;
                MoveAdL = -1;
                MoveAdS = moveAdF;
            }

            if (gold_50F > 0) {
                Gold___50Need = true;
                Gold___50L = -1;
                Gold___50S = gold_50F;
            }
            if (gold_100F > 0)
            {
                Gold__100Need = true;
                Gold__100L = -1;
                Gold__100S = gold_100F;
            }
            if (gold_250F > 0)
            {
                Gold__250Need = true;
                Gold__250L = -1;
                Gold__250S = gold_250F;
            }
            if (gold_500F > 0)
            {
                Gold__500Need = true;
                Gold__500L = -1;
                Gold__500S = gold_500F;
            }
            if (gold_1000F > 0)
            {
                Gold_1000Need = true;
                Gold_1000L = -1;
                Gold_1000S = gold_1000F;
            }


        }

        //���� ����� ������ ��������������� ������� �������� ��������� ������ �� ����, � ������ ����� ��������� ����� ����� �������� ��� ������ ���� ���������
        public void TestLoadAndSave() {
            //������� ��������� ����� �� ������ ��������� ����� ���� ������ ������
            if (!LevelNeed) return;

            //����� � ��������� ������� �������� �������
            DatabaseReference yearChild = reference.Child(TimeWorld.GetTimeWorld().Year.ToString());
            DatabaseReference monthChild = yearChild.Child(TimeWorld.GetTimeWorld().Month.ToString());
            DatabaseReference levels = monthChild.Child(LevelsStr);
            DatabaseReference levelData = levels.Child(levelNumS.ToString());

            ///////////////////////////////////////////////////////////////////////////////
            //������ ���������

            //������� ������

            //���� ���������� ������� ������� �������� �� �������� -1
            if (PlayersVictoryNeed && PlayersVictoryL == -1) {
                //������ �� ����
                levelData.Child(PlayersVictoryStr).GetValueAsync().ContinueWith(task => {
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) PlayersVictoryL = geted;
                        else PlayersVictoryL = 0;
                    }
                });

                //������ ���������
                PlayersVictoryNeed = false;
            }
            if (StartNeed && StartL == -1)
            {
                //������ �� ����
                levelData.Child(StartStr).GetValueAsync().ContinueWith(task => {
                    int geted = 0;

                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        geted = getInt(task.Result);
                        if (geted == -1) {
                            geted = 0;
                        }
                    }

                    StartL = geted;
                });

                //������ ���������
                StartNeed = false;
            }

            //���� �������� ���������� �������� ����� �� ��������, ������
            if (ScoreAllTimeNeed && ScoreAllTimeL == -1) {
                //������ �� ����
                levelData.Child(ScoreAllTimeStr).GetValueAsync().ContinueWith(task => {
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                    else
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) ScoreAllTimeL = geted;
                        else ScoreAllTimeL = 0;
                    }
                });

                //������ ���������
                ScoreAllTimeNeed = false;
            }

            if (CountMoveToWinAllTimeNeed && CountMoveToWinAllTimeL == -1) {
                //������ �� ����
                levelData.Child(CountMoveToWinAllTimeStr).GetValueAsync().ContinueWith(task => {
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) CountMoveToWinAllTimeL = geted;
                        else CountMoveToWinAllTimeL = 0;
                    }
                });

                //������ ���������
                CountMoveToWinAllTimeNeed = false;
            }

            //�������� ��������
            if (CoronaBuyNeed && CoronaBuyL == -1)
            {
                //������ �� ����
                levelData.Child(CoronaBuyStr).GetValueAsync().ContinueWith(task => {
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                    else
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) CoronaBuyL = geted;
                        else CoronaBuyL = 0;
                    }
                });

                //������ ���������
                CoronaBuyNeed = false;
            }
            if (CoronaUseNeed && CoronaUseL == -1)
            {
                //������ �� ����
                levelData.Child(CoronaUseStr).GetValueAsync().ContinueWith(task => {
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                    else
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) CoronaUseL = geted;
                        else CoronaUseL = 0;
                    }
                });

                //������ ���������
                CoronaUseNeed = false;
            }

            if (PillBuyNeed && PillBuyL == -1)
            {
                //������ �� ����
                levelData.Child(pillBuyStr).GetValueAsync().ContinueWith(task => {
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                    else
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) PillBuyL = geted;
                        else PillBuyL = 0;
                    }
                });

                //������ ���������
                PillBuyNeed = false;
            }
            if (PillUseNeed && PillUseL == -1)
            {
                //������ �� ����
                levelData.Child(pillUseStr).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) PillUseL = result;
                    else PillUseL = 0;
                });

                //������ ���������
                PillUseNeed = false;
            }

            if (BombBuyNeed && BombBuyL == -1)
            {
                //������ �� ����
                levelData.Child(bombBuyStr).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) BombBuyL = result;
                    else BombBuyL = 0;
                });

                //������ ���������
                BombBuyNeed = false;
            }
            if (BombUseNeed && BombUseL == -1)
            {
                //������ �� ����
                levelData.Child(bombUseStr).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) BombUseL = result;
                    else BombUseL = 0;
                });

                //������ ���������
                BombUseNeed = false;
            }

            if (MixedBuyNeed && MixedBuyL == -1)
            {
                //������ �� ����
                levelData.Child(MixedBuyStr).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) MixedBuyL = result;
                    else MixedBuyL = 0;
                });

                //������ ���������
                MixedBuyNeed = false;
            }
            if (MixedUseNeed && MixedUseL == -1)
            {
                //������ �� ����
                levelData.Child(MixedUseStr).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) MixedUseL = result;
                    else MixedUseL = 0;
                });

                //������ ���������
                MixedUseNeed = false;
            }

            if (HummerBuyNeed && HummerBuyL == -1)
            {
                //������ �� ����
                levelData.Child(HummerBuyStr).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) HummerBuyL = result;
                    else HummerBuyL = 0;
                });

                //������ ���������
                HummerBuyNeed = false;
            }
            if (HummerUseNeed && HummerUseL == -1)
            {
                //������ �� ����
                levelData.Child(HummerUseStr).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) HummerUseL = result;
                    else HummerUseL = 0;
                });

                //������ ���������
                HummerUseNeed = false;
            }

            if (MoveBuyNeed && MoveBuyL == -1)
            {
                //������ �� ����
                levelData.Child(MoveBuyStr).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) MoveBuyL = result;
                    else MoveBuyL = 0;
                });

                //������ ���������
                MoveBuyNeed = false;
            }
            if (MoveAdNeed && MoveAdL == -1)
            {
                //������ �� ����
                levelData.Child(MoveAdStr).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) MoveAdL = result;
                    else MoveAdL = 0;
                });

                //������ ���������
                MoveAdNeed = false;
            }

            if (Gold___50Need && Gold___50L == -1)
            {
                //������ �� ����
                levelData.Child(Gold___50str).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) Gold___50L = result;
                    else Gold___50L = 0;
                });

                //������ ���������
                Gold___50Need = false;
            }
            if (Gold__100Need && Gold__100L == -1)
            {
                //������ �� ����
                levelData.Child(Gold__100str).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) Gold__100L = result;
                    else Gold__100L = 0;
                });

                //������ ���������
                Gold__100Need = false;
            }
            if (Gold__250Need && Gold__250L == -1)
            {
                //������ �� ����
                levelData.Child(Gold__250str).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) Gold__250L = result;
                    else Gold__250L = 0;
                });

                //������ ���������
                Gold__250Need = false;
            }
            if (Gold__500Need && Gold__500L == -1)
            {
                //������ �� ����
                levelData.Child(Gold__500str).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) Gold__500L = result;
                    else Gold__500L = 0;
                });

                //������ ���������
                Gold__500Need = false;
            }
            if (Gold_1000Need && Gold_1000L == -1)
            {
                //������ �� ����
                levelData.Child(Gold_1000str).GetValueAsync().ContinueWith(task => {
                    int result = -1;
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }
                    else
                    {
                        result = getInt(task.Result);
                    }

                    if (result != -1) Gold_1000L = result;
                    else Gold_1000L = 0;
                });

                //������ ���������
                Gold_1000Need = false;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////
            //������ ��������

            //���� ����� �� �� ������ �� ���� ���������� ���������� ����� ������
            if (PlayersVictoryL == -1 ||
                StartL == -1 ||
                ScoreAllTimeL == -1 ||
                CountMoveToWinAllTimeL == -1 ||

                CoronaBuyL == -1 || CoronaUseL == -1 ||
                PillBuyL == -1 || PillUseL == -1 ||
                BombBuyL == -1 || BombUseL == -1 ||
                MixedBuyL == -1 || MixedUseL == -1 ||
                HummerBuyL == -1 || HummerUseL == -1 ||

                MoveBuyL == -1 || MoveAdL == -1 ||

                Gold___50L == -1 ||
                Gold__100L == -1 ||
                Gold__250L == -1 ||
                Gold__500L == -1 ||
                Gold_1000L == -1

                ) return;

            //////////////////////////////////////////////////////////////////////////////////////////////
            //������ ������� ����� ������ � ����������

            //������, ����� ��� ������ ����������� � ����������������

            //��������� ������ �� ��� ������� ���������

            if (PlayersVictoryS > 0)
            {
                float PlayersVictoryF = PlayersVictoryS + PlayersVictoryL;
                levelData.Child(PlayersVictoryStr).SetValueAsync(PlayersVictoryF);
            }
            if (StartS > 0) {
                float StartF = StartS + StartL;
                levelData.Child(StartStr).SetValueAsync(StartS);
            }
            if (ScoreAllTimeS > 0) {
                float ScoreAllTimeF = ScoreAllTimeS + ScoreAllTimeL;
                levelData.Child(ScoreAllTimeStr).SetValueAsync(ScoreAllTimeF);
            }
            if (CountMoveToWinS > 0) {
                float CountMoveToWinF = CountMoveToWinS + CountMoveToWinAllTimeL;
                levelData.Child(CountMoveToWinAllTimeStr).SetValueAsync(CountMoveToWinF);
            }
            if (CoronaBuyS > 0) {
                float CoronaBuyF = CoronaBuyS + CoronaBuyL;
                levelData.Child(CoronaBuyStr).SetValueAsync(CoronaBuyF);
            }
            if (CoronaUseS > 0) {
                float CoronaUseF = CoronaUseS + CoronaUseL;
                levelData.Child(CoronaUseStr).SetValueAsync(CoronaUseF);
            }
            if (PillBuyS > 0) {
                float PillBuyF = PillBuyS + PillBuyL;
                levelData.Child(pillBuyStr).SetValueAsync(PillBuyF);
            }
            if (PillUseS > 0) {
                float PillUseF = PillUseS + PillUseL;
                levelData.Child(pillUseStr).SetValueAsync(PillUseF);
            }
            if (BombBuyS > 0) {
                float BombBuyF = BombBuyS + BombBuyL;
                levelData.Child(bombBuyStr).SetValueAsync(BombBuyF);
            }
            if (BombUseS > 0) {
                float BombUseF = BombUseS + BombUseL;
                levelData.Child(bombUseStr).SetValueAsync(BombUseF);
            }
            if (MixedBuyS > 0) {
                float MixedBuyF = MixedBuyS + MixedBuyL;
                levelData.Child(MixedBuyStr).SetValueAsync(MixedBuyF);
            }
            if (MixedUseS > 0) {
                float MixedUseF = MixedUseS + MixedUseL;
                levelData.Child(MixedUseStr).SetValueAsync(MixedUseF);
            }
            if (HummerBuyS > 0) {
                float HummerBuyF = HummerBuyS + HummerBuyL;
                levelData.Child(HummerBuyStr).SetValueAsync(HummerBuyF);
            }
            if (HummerUseS > 0) {
                float HummerUseF = HummerUseS + HummerUseL;
                levelData.Child(HummerUseStr).SetValueAsync(HummerUseF);
            }

            if (MoveBuyS > 0) {
                float MoveBuyF = MoveBuyS + MoveBuyL;
                levelData.Child(MoveBuyStr).SetValueAsync(MoveBuyF);
            }
            if (MoveAdS > 0) {
                float MoveAdF = MoveAdS + MoveAdL;
                levelData.Child(MoveAdStr).SetValueAsync(MoveAdF);
            }

            if (Gold___50S > 0) {
                float Gold___50F = Gold___50S + Gold___50L;
                levelData.Child(Gold___50str).SetValueAsync(Gold___50F);
            }
            if (Gold__100S > 0) {
                float Gold__100F = Gold__100S + Gold__100L;
                levelData.Child(Gold__100str).SetValueAsync(Gold__100F);
            }
            if (Gold__250S > 0) {
                float Gold__250F = Gold__250S + Gold__250L;
                levelData.Child(Gold__250str).SetValueAsync(Gold__250F);
            }
            if (Gold__500S > 0) {
                float Gold__500F = Gold__500S + Gold__500L;
                levelData.Child(Gold__500str).SetValueAsync(Gold__500F);
            }
            if (Gold_1000S > 0) {
                float Gold_1000F = Gold_1000S + Gold_1000L;
                levelData.Child(Gold_1000str).SetValueAsync(Gold_1000F);
            }

            //���������� ������� �������������� �������� ���� ������ ���� �� ���������� ���������� 
            if (CountMoveToWinS > 0 || PlayersVictoryS > 0) {

                // ������� ��������
                float CountMoveToWinF = CountMoveToWinAllTimeL;
                if (CountMoveToWinS > 0) CountMoveToWinF += CountMoveToWinS;

                float PlayersVictoryF = PlayersVictoryL;
                if (PlayersVictoryS > 0) PlayersVictoryF += PlayersVictoryS;

                float CountMoveToWinAverage = CountMoveToWinF / PlayersVictoryF; //������� � ������� ������ ������ ���� �� ������ ����� ��������
                levelData.Child(CountMoveToWinAverageStr).SetValueAsync(CountMoveToWinAverage);
            }

            if (ScoreAllTimeS > 0 || PlayersVictoryS > 0) {
                float ScoreAllTimeF = ScoreAllTimeL;
                if (ScoreAllTimeS > 0) ScoreAllTimeF += ScoreAllTimeS;

                float PlayersVictoryF = PlayersVictoryL;
                if (PlayersVictoryS > 0) PlayersVictoryF += PlayersVictoryS;

                float ScoreAverage = ScoreAllTimeF / PlayersVictoryF; //������� � ������� ������ �������� ����� ��� ������
                //�� ������������
            }

            if (MoveBuyS > 0 || StartS > 0) {
                float MoveBuyF = MoveBuyL;
                if (MoveBuyS > 0) MoveBuyF += MoveBuyS;

                float StartF = StartL;
                if (StartS > 0) StartF += StartS;

                float MoveBuyAverage = MoveBuyF / (float)StartF;
                levelData.Child(MoveBuyAverageStr).SetValueAsync(MoveBuyAverage);
            }
            if (MoveAdS > 0 || StartS > 0) {
                float MoveAdF = MoveAdL;
                if (MoveAdS > 0) MoveAdF += MoveAdS;

                float StartF = StartL;
                if (StartS > 0) StartF += StartS;

                float MoveAdSAverage = MoveAdF / (float)StartF;
                levelData.Child(MoveAdAverageStr).SetValueAsync(MoveAdSAverage);
            }



            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///������ ��������� ������, ����� �������� ��� ��������� ���� ������� � ������ �� ���������
            PlayersVictoryS = 0;
            StartS = 0;
            ScoreAllTimeS = 0;
            CountMoveToWinS = 0;

            CoronaBuyS = 0;
            CoronaUseS = 0;
            PillBuyS = 0;
            PillUseS = 0;
            BombBuyS = 0;
            BombUseS = 0;
            MixedBuyS = 0;
            MixedUseS = 0;
            HummerBuyS = 0;
            HummerUseS = 0;

            MoveBuyS = 0;
            MoveAdS = 0;

            Gold___50S = 0;
            Gold__100S = 0;
            Gold__250S = 0;
            Gold__500S = 0;
            Gold_1000S = 0;

            //������ ������ ���� ���������
            LevelNeed = false;
        }
    }
    public TypeLevel typeLevel = new TypeLevel();

    //������ �������
    public struct TypeProfile {
        public const string Profiles = "Profiles";
        public const string Level_Max = "Level_opened_maximum";
        public const string Level_Open_Averade = "Level_opened_average";
        public const string Level_Open_Averade_Count = "Level_opened_average_count";
    }

    static int getInt(DataSnapshot dataSnapshot) {
        int result = -1;

        //�������� ������
        if (dataSnapshot.Exists) {
            string value = dataSnapshot.GetRawJsonValue();

            result = System.Convert.ToInt32(value);
        }

        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        reference = FirebaseDatabase.DefaultInstance.RootReference;



        //typeLevel.SetLevelData(0, true, 2, 4,5,6,7,8,9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21);
    }

    // Update is called once per frame
    void Update()
    {
        //������� ��������� �� ���� ��� ��������� � ����
        typeLevel.TestLoadAndSave();
    }

}
