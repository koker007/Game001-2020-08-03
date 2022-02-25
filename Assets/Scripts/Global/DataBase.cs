using Firebase.Database;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase main;

    //Инициализация референса
    static DatabaseReference reference;

    //Имена сохраняемых данных, должно совпадать на firebase

    public struct TypeTime{
        public const string Year = "Year"; //год
        public const string Month = "Month"; //месяц
    }

    //Раздел уровней
    public class TypeLevel {
        const string LevelsStr = "Levels"; //Подраздел уровни
        const string PlayersVictoryStr = "Players_Passed"; //Количество пользователей которые прошли данный уровень
        const string StartStr = "Start"; //Количество раз когда уровень был запущен
        const string CountMoveToWinAllTimeStr = "Count_Move_To_Win_AllTime"; //Общее количество ходов которые привели к победе
        const string CountMoveToWinAverageStr = "Count_Move_To_Win_Average"; //Среднее количество ходов которые привели игрока к победе
        const string ScoreAverageStr = "Score_Average"; //Количество набираемых очков в среднем
        const string ScoreAllTimeStr = "Score_AllTime"; //Количество очков за все время

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

        //Покупки общие на уровне
        const string Store = "Store"; //Покупки - подраздел
        const string CoronaUseStr = "Corona_Use"; //корона
        const string CoronaBuyStr = "Corona_Buy";
        const string pillUseStr = "Pill_Use"; //Пилюля
        const string pillBuyStr = "Pill_Buy";
        const string bombUseStr = "Bomb_Use"; //бомба
        const string bombBuyStr = "Bomb_Buy";
        const string MixedUseStr = "Mixed_Use"; //перемешивания
        const string MixedBuyStr = "Mixed_Buy";
        const string HummerUseStr = "Hummer_Use"; //молоток
        const string HummerBuyStr = "Hummer_Buy";

        //Загруженное значение
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

        //Значения которые надо сохранить
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

        //Покупки ходов 
        public const string MoveBuyStr = "Move_Buy"; //покупка ходов золотом
        public const string MoveAdStr = "Move_Ad"; //Покупка ходов просмотром рекламы
        public const string MoveBuyAverageStr = "Move_Buy_Average";
        public const string MoveAdAverageStr = "Move_Ad_Average";

        int MoveBuyL = -1;
        int MoveAdL = -1;

        int MoveBuyS = -1;
        int MoveAdS = -1;

        bool MoveBuyNeed = true;
        bool MoveAdNeed = true;

        //покупки золота
        public const string Gold___50str = "Gold___50"; //Количество покупок пакета
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

        //Отправить на запись данные //Если данную отправлять на запись не надо, отправь 0
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

            levelNumS = levelNumF; //Для какого уровня предназначенны данные
            LevelNeed = true;

            if (PlayersVictoryF) {
                PlayersVictoryNeed = true; //Данную необходимо загрузить
                PlayersVictoryL = -1; //Информацию о загруженной данной чистим
                PlayersVictoryS = 1; //Наша переменная изменилась на это число
            }


            if (CountMoveToWinF > 0) {
                CountMoveToWinAllTimeNeed = true; //Данную необходимо загрузить
                CountMoveToWinAllTimeL = -1; //Информацию о загруженной данной чистим
                CountMoveToWinS = CountMoveToWinF; //Наша переменная изменилась на это число

            }
            if (ScoreF > 0) {
                ScoreAllTimeNeed = true;
                ScoreAllTimeL = -1;
                ScoreAllTimeS = ScoreF;
            }

            //Магазин
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

        //Этот метод строго последовательно сначала пытается загрузить данные из сети, и только потом сохраняет когда точно известно что данные были полученны
        public void TestLoadAndSave() {
            //Сначала проверяем нужно ли вообще загружать какие либо данные уровня
            if (!LevelNeed) return;

            //Зашли в подраздел уровней текущего времени
            DatabaseReference yearChild = reference.Child(TimeWorld.GetTimeWorld().Year.ToString());
            DatabaseReference monthChild = yearChild.Child(TimeWorld.GetTimeWorld().Month.ToString());
            DatabaseReference levels = monthChild.Child(LevelsStr);
            DatabaseReference levelData = levels.Child(levelNumS.ToString());

            ///////////////////////////////////////////////////////////////////////////////
            //Стадия подгрузки

            //Загузка уровня

            //если количество игроков которые победили не известно -1
            if (PlayersVictoryNeed && PlayersVictoryL == -1) {
                //Грузим из сети
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

                //Запрос отправлен
                PlayersVictoryNeed = false;
            }
            if (StartNeed && StartL == -1)
            {
                //Грузим из сети
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

                //Запрос отправлен
                StartNeed = false;
            }

            //если сумарное количество победных очков не известно, грузим
            if (ScoreAllTimeNeed && ScoreAllTimeL == -1) {
                //грузим из сети
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

                //Запрос отправлен
                ScoreAllTimeNeed = false;
            }

            if (CountMoveToWinAllTimeNeed && CountMoveToWinAllTimeL == -1) {
                //грузим из сети
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

                //Запрос отправлен
                CountMoveToWinAllTimeNeed = false;
            }

            //Загрузка магазина
            if (CoronaBuyNeed && CoronaBuyL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                CoronaBuyNeed = false;
            }
            if (CoronaUseNeed && CoronaUseL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                CoronaUseNeed = false;
            }

            if (PillBuyNeed && PillBuyL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                PillBuyNeed = false;
            }
            if (PillUseNeed && PillUseL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                PillUseNeed = false;
            }

            if (BombBuyNeed && BombBuyL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                BombBuyNeed = false;
            }
            if (BombUseNeed && BombUseL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                BombUseNeed = false;
            }

            if (MixedBuyNeed && MixedBuyL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                MixedBuyNeed = false;
            }
            if (MixedUseNeed && MixedUseL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                MixedUseNeed = false;
            }

            if (HummerBuyNeed && HummerBuyL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                HummerBuyNeed = false;
            }
            if (HummerUseNeed && HummerUseL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                HummerUseNeed = false;
            }

            if (MoveBuyNeed && MoveBuyL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                MoveBuyNeed = false;
            }
            if (MoveAdNeed && MoveAdL == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                MoveAdNeed = false;
            }

            if (Gold___50Need && Gold___50L == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                Gold___50Need = false;
            }
            if (Gold__100Need && Gold__100L == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                Gold__100Need = false;
            }
            if (Gold__250Need && Gold__250L == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                Gold__250Need = false;
            }
            if (Gold__500Need && Gold__500L == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                Gold__500Need = false;
            }
            if (Gold_1000Need && Gold_1000L == -1)
            {
                //грузим из сети
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

                //Запрос отправлен
                Gold_1000Need = false;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////
            //Стадия ожидания

            //Если какие ли бо данные не были загруженны продолжаем ждать ответа
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
            //Стадия расчета новых данных и сохранения

            //Теперь, Когда все данные подгруженны и синхронизированы

            //Сохраняем только то что требует изменения

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

            //Перерасчет средних статистических значений если хотябы одна из переменных изменилась 
            if (CountMoveToWinS > 0 || PlayersVictoryS > 0) {

                // считаем значение
                float CountMoveToWinF = CountMoveToWinAllTimeL;
                if (CountMoveToWinS > 0) CountMoveToWinF += CountMoveToWinS;

                float PlayersVictoryF = PlayersVictoryL;
                if (PlayersVictoryS > 0) PlayersVictoryF += PlayersVictoryS;

                float CountMoveToWinAverage = CountMoveToWinF / PlayersVictoryF; //Сколько в средним игроки делали ходы на уровне чтобы выиграть
                levelData.Child(CountMoveToWinAverageStr).SetValueAsync(CountMoveToWinAverage);
            }

            if (ScoreAllTimeS > 0 || PlayersVictoryS > 0) {
                float ScoreAllTimeF = ScoreAllTimeL;
                if (ScoreAllTimeS > 0) ScoreAllTimeF += ScoreAllTimeS;

                float PlayersVictoryF = PlayersVictoryL;
                if (PlayersVictoryS > 0) PlayersVictoryF += PlayersVictoryS;

                float ScoreAverage = ScoreAllTimeF / PlayersVictoryF; //Сколько в среднем игроки набирают очков при победе
                //Не используется
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
            ///стадия зачищения данных, чтобы показать что изменения были приняты и заного не сохранять
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

            //Данные уровня были сохранены
            LevelNeed = false;
        }
    }
    public TypeLevel typeLevel = new TypeLevel();

    //Раздел профиля
    public class TypeProfile {
        bool loadNeed = true;

        public const string Profiles = "Profiles";
        public const string Level_Max = "Level_opened_maximum"; //Максимальный открытый уровень
        public const string Level_Open_Averade = "Level_opened_average"; //

        //Имя профиля
        public const string name = "Name";
        public const string level_max = "level_max";

        //магазин пользователя
        public const string shopStr = "Shop";
        public const string buyingStr = "buying";
        public const string gold_50Str = "gold_50";
        public const string gold_100Str = "gold_100";
        public const string gold_250Str = "gold_250";
        public const string gold_500Str = "gold_500";
        public const string gold_1000Str = "gold_1000";

        public const string pack_01Str = "pack01";
        public const string pack_02Str = "pack02";
        public const string pack_03Str = "pack03";
        public const string pack_04Str = "pack04";
        public const string pack_05Str = "pack05";
        public const string vipStr = "vip";

        public const string hummerStr = "hummer";
        public const string bombStr = "bomb";
        public const string pillStr = "pill";
        public const string coronaStr = "corona";
        public const string mixedStr = "mixed";

        int buyingL = -1;
        int gold_50L = -1;
        int gold_100L = -1;
        int gold_250L = -1;
        int gold_500L = -1;
        int gold_1000L = -1;
        int pack_01L = -1;
        int pack_02L = -1;
        int pack_03L = -1;
        int pack_04L = -1;
        int pack_05L = -1;
        int vipL = -1;
        int hummerL = -1;
        int bombL = -1;
        int pillL = -1;
        int coronaL = -1;
        int mixedL = -1;

        int buyingS = -1;
        int gold_50S = -1;
        int gold_100S = -1;
        int gold_250S = -1;
        int gold_500S = -1;
        int gold_1000S = -1;
        int pack_01S = -1;
        int pack_02S = -1;
        int pack_03S = -1;
        int pack_04S = -1;
        int pack_05S = -1;
        int vipS = -1;
        int hummerS = -1;
        int bombS = -1;
        int pillS = -1;
        int coronaS = -1;
        int mixedS = -1;

        bool buyingNeed = true;
        bool gold_50Need = true;
        bool gold_100Need = true;
        bool gold_250Need = true;
        bool gold_500Need = true;
        bool gold_1000Need = true;
        bool pack_01Need = true;
        bool pack_02Need = true;
        bool pack_03Need = true;
        bool pack_04Need = true;
        bool pack_05Need = true;
        bool vipNeed = true;
        bool hummerNeed = true;
        bool bombNeed = true;
        bool pillNeed = true;
        bool coronaNeed = true;
        bool mixedNeed = true;



        public void setProfileData(int buyingF, 
            int gold_50F, int gold_100F, int gold_250F, int gold_500F, int gold_1000F, 
            int pack_01F, int pack_02F, int pack_03F, int pack_04F, int pack_05F, int vipF,
            int hummerF, int pillF, int bombF, int coronaF, int mixedF) {

            if (buyingF > 0)
            {
                buyingNeed = true;
                buyingL = -1;
                buyingS = buyingF;
            }
            if (gold_50F > 0)
            {
                gold_50Need = true;
                gold_50L = -1;
                gold_50S = gold_50F;
            }
            if (gold_100F > 0)
            {
                gold_100Need = true;
                gold_100L = -1;
                gold_100S = gold_100F;
            }
            if (gold_250F > 0)
            {
                gold_250Need = true;
                gold_250L = -1;
                gold_250S = gold_250F;
            }
            if (gold_500F > 0)
            {
                gold_500Need = true;
                gold_500L = -1;
                gold_500S = gold_500F;
            }
            if (gold_1000F > 0)
            {
                gold_1000Need = true;
                gold_1000L = -1;
                gold_1000S = gold_100F;
            }
            if (pack_01F > 0)
            {
                pack_01Need = true;
                pack_01L = -1;
                pack_01S = pack_01F;
            }
            if (pack_02F > 0)
            {
                pack_02Need = true;
                pack_02L = -1;
                pack_02S = pack_02F;
            }
            if (pack_03F > 0)
            {
                pack_03Need = true;
                pack_03L = -1;
                pack_03S = pack_03F;
            }
            if (pack_04F > 0)
            {
                pack_04Need = true;
                pack_04L = -1;
                pack_04S = pack_04F;
            }
            if (pack_05F > 0)
            {
                pack_05Need = true;
                pack_05L = -1;
                pack_05S = pack_05F;
            }
            if (vipF > 0)
            {
                vipNeed = true;
                vipL = -1;
                vipS = vipF;
            }

            if (hummerF > 0) {
                hummerNeed = true;
                hummerL = -1;
                hummerS = hummerF;
            }
            if (pillF > 0) {
                pillNeed = true;
                pillL = -1;
                pillS = pillF;
            }
            if (bombF > 0) {
                bombNeed = true;
                bombL = -1;
                bombS = bombF;
            }
            if (coronaF > 0) {
                coronaNeed = true;
                coronaL = -1;
                coronaS = coronaF;
            }
            if (mixedF > 0) {
                mixedNeed = true;
                mixedL = -1;
                mixedS = mixedF;
            }

            loadNeed = true;
        }

        public void TestLoadAndSave() {

            if (!loadNeed) return;

            //Зашли в подраздел уровней текущего времени
            DatabaseReference yearChild = reference.Child(TimeWorld.GetTimeWorld().Year.ToString());
            DatabaseReference monthChild = yearChild.Child(TimeWorld.GetTimeWorld().Month.ToString());

            DatabaseReference profile = monthChild.Child(Profiles);
            DatabaseReference profileData = profile.Child(PlayerProfile.main.profileTimeStart);

            DatabaseReference profileShop = profileData.Child(shopStr);

            ///////////////////////////////////////////////////////////////////////////////
            //Стадия подгрузки

            //Загузка уровня

            //если количество игроков которые победили не известно -1
            
            //Магазин 
            if (buyingNeed && buyingL == -1)
            {
                //Грузим из сети
                profileShop.Child(buyingStr).GetValueAsync().ContinueWith(task => {

                    buyingL = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) buyingL = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                buyingNeed = false;
            }
            if (gold_50Need && gold_50L == -1)
            {
                //Грузим из сети
                profileShop.Child(gold_50Str).GetValueAsync().ContinueWith(task => {

                    gold_50L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) gold_50L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                gold_50Need = false;
            }
            if (gold_100Need && gold_100L == -1)
            {
                //Грузим из сети
                profileShop.Child(gold_100Str).GetValueAsync().ContinueWith(task => {

                    gold_100L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) gold_100L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                gold_100Need = false;
            }
            if (gold_250Need && gold_250L == -1)
            {
                //Грузим из сети
                profileShop.Child(gold_250Str).GetValueAsync().ContinueWith(task => {

                    gold_250L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) gold_250L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                gold_250Need = false;
            }
            if (gold_500Need && gold_500L == -1)
            {
                //Грузим из сети
                profileShop.Child(gold_500Str).GetValueAsync().ContinueWith(task => {

                    gold_500L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) gold_500L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                gold_500Need = false;
            }
            if (gold_1000Need && gold_1000L == -1)
            {
                //Грузим из сети
                profileShop.Child(gold_1000Str).GetValueAsync().ContinueWith(task => {

                    gold_1000L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) gold_1000L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                gold_1000Need = false;
            }

            if (pack_01Need && pack_01L == -1)
            {
                //Грузим из сети
                profileShop.Child(pack_01Str).GetValueAsync().ContinueWith(task => {

                    pack_01L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) pack_01L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                pack_01Need = false;
            }
            if (pack_02Need && pack_02L == -1)
            {
                //Грузим из сети
                profileShop.Child(pack_02Str).GetValueAsync().ContinueWith(task => {

                    pack_02L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) pack_02L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                pack_02Need = false;
            }
            if (pack_03Need && pack_03L == -1)
            {
                //Грузим из сети
                profileShop.Child(pack_03Str).GetValueAsync().ContinueWith(task => {

                    pack_03L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) pack_03L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                pack_03Need = false;
            }
            if (pack_04Need && pack_04L == -1)
            {
                //Грузим из сети
                profileShop.Child(pack_04Str).GetValueAsync().ContinueWith(task => {

                    pack_04L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) pack_04L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                pack_04Need = false;
            }
            if (pack_05Need && pack_05L == -1)
            {
                //Грузим из сети
                profileShop.Child(pack_05Str).GetValueAsync().ContinueWith(task => {

                    pack_05L = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) pack_05L = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                pack_05Need = false;
            }
            if (vipNeed && vipL == -1)
            {
                //Грузим из сети
                profileShop.Child(vipStr).GetValueAsync().ContinueWith(task => {

                    vipL = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) vipL = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                vipNeed = false;
            }

            if (hummerNeed && hummerL == -1)
            {
                //Грузим из сети
                profileShop.Child(hummerStr).GetValueAsync().ContinueWith(task => {

                    hummerL = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) hummerL = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                hummerNeed = false;
            }
            if (bombNeed && bombL == -1)
            {
                //Грузим из сети
                profileShop.Child(bombStr).GetValueAsync().ContinueWith(task => {

                    bombL = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) bombL = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                bombNeed = false;
            }
            if (pillNeed && pillL == -1)
            {
                //Грузим из сети
                profileShop.Child(pillStr).GetValueAsync().ContinueWith(task => {

                    pillL = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) pillL = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                pillNeed = false;
            }
            if (coronaNeed && coronaL == -1)
            {
                //Грузим из сети
                profileShop.Child(coronaStr).GetValueAsync().ContinueWith(task => {

                    coronaL = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) coronaL = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                coronaNeed = false;
            }
            if (mixedNeed && mixedL == -1)
            {
                //Грузим из сети
                profileShop.Child(mixedStr).GetValueAsync().ContinueWith(task => {

                    mixedL = 0;
                    if (task != null && task.Result != null && task.IsCompleted)
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) mixedL = geted;

                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                });

                //Запрос отправлен
                mixedNeed = false;
            }

            //Данные профиля

            ///////////////////////////////////////////////////////////////////////////////////////////////
            //Стадия ожидания

            if (buyingL == -1 ||
                gold_50L == -1 || gold_100L == -1 || gold_250L == -1 || gold_500L == -1 || gold_1000L == -1 ||
                pack_01L == -1 || pack_02L == -1 || pack_03L == -1 || pack_04L == -1 || pack_05L == -1 ||
                vipL == -1 ||
                hummerL == -1 || bombL == -1 || pillL == -1 || coronaL == -1 || mixedL == -1
                ) return;


            //////////////////////////////////////////////////////////////////////////////////////////////
            //Стадия расчета новых данных и сохранения
            //Теперь, Когда все данные подгруженны и синхронизированы
            //Сохраняем только то что требует изменения

            if (buyingS > 0)
            {
                float buyingF = buyingS + buyingL;
                profileData.Child(buyingStr).SetValueAsync(buyingF);
            }
            if (gold_50S > 0)
            {
                float gold_50F = gold_50S + gold_50L;
                profileData.Child(gold_50Str).SetValueAsync(gold_50F);
            }
            if (gold_100S > 0)
            {
                float gold_100F = gold_100S + gold_100L;
                profileData.Child(gold_100Str).SetValueAsync(gold_100F);
            }
            if (gold_250S > 0)
            {
                float gold_250F = gold_250S + gold_250L;
                profileData.Child(gold_250Str).SetValueAsync(gold_250F);
            }
            if (gold_500S > 0)
            {
                float gold_500F = gold_500S + gold_500L;
                profileData.Child(gold_500Str).SetValueAsync(gold_500F);
            }
            if (gold_1000S > 0)
            {
                float gold_1000F = gold_1000S + gold_1000L;
                profileData.Child(gold_1000Str).SetValueAsync(gold_1000F);
            }
            if (pack_01L > 0)
            {
                float pack_01F = pack_01S + pack_01L;
                profileData.Child(pack_01Str).SetValueAsync(pack_01F);
            }
            if (pack_02L > 0)
            {
                float pack_02F = pack_02S + pack_02L;
                profileData.Child(pack_02Str).SetValueAsync(pack_02F);
            }
            if (pack_03L > 0)
            {
                float pack_03F = pack_03S + pack_03L;
                profileData.Child(pack_03Str).SetValueAsync(pack_03F);
            }
            if (pack_04L > 0)
            {
                float pack_04F = pack_04S + pack_04L;
                profileData.Child(pack_04Str).SetValueAsync(pack_04F);
            }
            if (pack_05L > 0)
            {
                float pack_05F = pack_05S + pack_05L;
                profileData.Child(pack_05Str).SetValueAsync(pack_05F);
            }
            if (vipL > 0)
            {
                float vipF = vipS + vipL;
                profileData.Child(vipStr).SetValueAsync(vipF);
            }
            if (hummerL > 0) {
                float hummerF = hummerS + hummerL;
                profileData.Child(hummerStr).SetValueAsync(hummerF);
            }
            if (pillL > 0)
            {
                float pillF = pillS + pillL;
                profileData.Child(pillStr).SetValueAsync(pillF);
            }
            if (bombL > 0)
            {
                float bombF = bombS + bombL;
                profileData.Child(pillStr).SetValueAsync(bombF);
            }
            if (coronaL > 0)
            {
                float coronaF = coronaS + coronaL;
                profileData.Child(coronaStr).SetValueAsync(coronaF);
            }
            if (mixedL > 0)
            {
                float mixedF = mixedS + mixedL;
                profileData.Child(mixedStr).SetValueAsync(mixedL);
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///стадия зачищения данных, чтобы показать что изменения были приняты и заного не сохранять
            buyingS = 0;
            gold_50S = 0;
            gold_100S = 0;
            gold_250S = 0;
            gold_500S = 0;
            gold_1000S = 0;
            pack_01S = 0;
            pack_02S = 0;
            pack_03S = 0;
            pack_04S = 0;
            pack_05S = 0;
            vipS = 0;
            hummerS = 0;
            pillS = 0;
            bombS = 0;
            coronaS = 0;
            mixedS = 0;

            //Данные уровня были сохранены
            loadNeed = false;

        }
    }
    public TypeProfile typeProfile = new TypeProfile();

    static int getInt(DataSnapshot dataSnapshot) {
        int result = -1;

        //Вынимаем данные
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
        //попытка загрузить из сети или сохранить в сеть
        typeLevel.TestLoadAndSave();
        typeProfile.TestLoadAndSave();
    }

}
