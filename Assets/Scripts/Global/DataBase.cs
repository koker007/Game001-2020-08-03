using Firebase.Database;
using UnityEngine;

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
        const string LevelsStr = "Levels"; //Подраздел уровни
        const string PlayersVictoryStr = "Players_Passed"; //Количество пользователей которые прошли данный уровень
        const string StartStr = "Start"; //Количество раз когда уровень был запущен
        const string CountMoveToWinAllTimeStr = "Count_Move_To_Win_AllTime"; //Общее количество ходов которые привели к победе
        const string CountMoveToWinAverageStr = "Count_Move_To_Win_Average"; //Среднее количество ходов которые привели игрока к победе
        const string ScoreAverageStr = "Score_Average"; //Количество набираемых очков в среднем
        const string ScoreAllTimeStr = "Score_AllTime"; //Количество очков за все время

        static int Level = -1;
        static int PlayersVictoryL = -1;
        static int StartL = -1;
        static int CountMoveToWinAllTimeL = -1;
        static int ScoreAllTimeL = -1;

        static int PlayersVictoryS = -1;
        static int StartS = -1;
        static int CountMoveToWinS = -1;

        static int ScoreAllTimeS = -1;

        static bool LevelNeed = false;
        static bool PlayersVictoryNeed = true;
        static bool StartNeed = true;
        static bool CountMoveToWinAllTimeNeed = true;
        //static bool CountMoveToWinAverageNeed = true;
        //static bool ScoreAverageNeed = true;
        static bool ScoreAllTimeNeed = true;

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
        static int CoronaUseL = -1;
        static int CoronaBuyL = -1;
        static int PillUseL = -1;
        static int PillBuyL = -1;
        static int BombUseL = -1;
        static int BombBuyL = -1;
        static int MixedUseL = -1;
        static int MixedBuyL = -1;
        static int HummerUseL = -1;
        static int HummerBuyL = -1;

        //Значения которые надо сохранить
        static int CoronaUseS = -1;
        static int CoronaBuyS = -1;
        static int PillUseS = -1;
        static int PillBuyS = -1;
        static int BombUseS = -1;
        static int BombBuyS = -1;
        static int MixedUseS = -1;
        static int MixedBuyS = -1;
        static int HummerUseS = -1;
        static int HummerBuyS = -1;

        static bool CoronaUseNeed = true;
        static bool CoronaBuyNeed = true;
        static bool PillUseNeed = true;
        static bool PillBuyNeed = true;
        static bool BombUseNeed = true;
        static bool BombBuyNeed = true;
        static bool MixedUseNeed = true;
        static bool MixedBuyNeed = true;
        static bool HummerUseNeed = true;
        static bool HummerBuyNeed = true;

        //Покупки ходов 
        public const string MoveBuyStr = "Move_Buy"; //покупка ходов золотом
        public const string MoveAdStr = "Move_Ad"; //Покупка ходов просмотром рекламы
        public const string MoveBuyAverageStr = "Move_Buy_Average";
        public const string MoveAdAverageStr = "Move_Ad_Average";

        static int MoveBuyL = -1;
        static int MoveAdL = -1;

        static int MoveBuyS = -1;
        static int MoveAdS = -1;

        static bool MoveBuyNeed = true;
        static bool MoveAdNeed = true;

        //покупки золота
        public const string Gold___50str = "Gold___50"; //Количество покупок пакета
        public const string Gold__100str = "Gold__100";
        public const string Gold__250str = "Gold__250";
        public const string Gold__500str = "Gold__500";
        public const string Gold_1000str = "Gold_1000";

        static int Gold___50L = -1;
        static int Gold__100L = -1;
        static int Gold__250L = -1;
        static int Gold__500L = -1;
        static int Gold_1000L = -1;

        static int Gold___50S = -1;
        static int Gold__100S = -1;
        static int Gold__250S = -1;
        static int Gold__500S = -1;
        static int Gold_1000S = -1;

        static bool Gold___50Need = true;
        static bool Gold__100Need = true;
        static bool Gold__250Need = true;
        static bool Gold__500Need = true;
        static bool Gold_1000Need = true;

        //Отправить на запись данные //Если данную отправлять на запись не надо, отправь 0
        public static void SetLevelData(int levelNum, int CountMoveToWinF, int ScoreF, 
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


            if (levelNum < 0) return;

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
        public static void TestLoadAndSave() {
            //Сначала проверяем нужно ли вообще загружать какие либо данные уровня
            if (!LevelNeed) return;

            //Зашли в подраздел уровней текущего времени
            DatabaseReference yearChild = reference.Child(TimeWorld.GetTimeWorld().Year.ToString());
            DatabaseReference monthChild = yearChild.Child(TimeWorld.GetTimeWorld().Month.ToString());
            DatabaseReference levels = monthChild.Child(LevelsStr);
            DatabaseReference levelData = levels.Child(LevelNeed.ToString());

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
                levelData.Child(CoronaBuyStr).GetValueAsync().ContinueWith(task => {
                    if (task.Result == null || !task.IsCompleted)
                    {
                        Debug.LogError("FirebaseStorageService - RetrieveSummary has failed!");
                    }

                    else
                    {
                        int geted = getInt(task.Result);
                        if (geted != -1) CoronaUseL = geted;
                        else CoronaBuyL = 0;
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

                    if (result != -1) MixedUseL = result;
                    else MixedUseL = 0;
                });

                //Запрос отправлен
                MixedUseNeed = false;
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
            //Стадия расчета новых данных

            //Теперь, Когда все данные подгруженны и синхронизированы

            //Считаем новые значения
            PlayersVictoryS += PlayersVictoryL;
            StartS += StartL;
            ScoreAllTimeS += ScoreAllTimeL;
            CountMoveToWinS += CountMoveToWinAllTimeL;

            CoronaBuyS += CoronaBuyL;
            CoronaUseS += CoronaUseL;
            PillBuyS += PillBuyL;
            PillUseS += PillUseL;
            BombBuyS += BombBuyL;
            BombUseS += BombUseL;
            MixedBuyS += MixedBuyL;
            MixedUseS += MixedUseL;
            HummerBuyS += HummerBuyL;
            HummerUseS += HummerUseL;

            MoveBuyS += MoveBuyL;
            MoveAdS += MoveAdL;

            Gold___50S += Gold___50L;
            Gold__100S += Gold__100L;
            Gold__250S += Gold__250L;
            Gold__500S += Gold__500L;
            Gold_1000S += Gold_1000L;

            //Расчет средних статистических значений
            float CountMoveToWinAverage = CountMoveToWinS / PlayersVictoryS; //Сколько в средним игроки делали ходы на уровне чтобы выиграть
            float ScoreAverage = ScoreAllTimeS / PlayersVictoryS; //Сколько в среднем игроки набирают очков при победе

            float MoveBuyAverage = MoveBuyS / (float)StartS;
            float MoveAdSAverage = MoveAdS / (float)StartS;

            //////////////////////////////////////////////////////////////////////////////////////////////
            //стадия сохранения

            levelData.Child(PlayersVictoryStr).SetValueAsync(PlayersVictoryS);
            levelData.Child(StartStr).SetValueAsync(StartS);
            levelData.Child(ScoreAllTimeStr).SetValueAsync(ScoreAllTimeS);
            levelData.Child(CountMoveToWinAllTimeStr).SetValueAsync(CountMoveToWinS);
            
            levelData.Child(CountMoveToWinAverageStr).SetValueAsync(CountMoveToWinAverage);

            levelData.Child(CoronaBuyStr).SetValueAsync(CoronaBuyS);
            levelData.Child(CoronaUseStr).SetValueAsync(CoronaUseS);
            levelData.Child(pillBuyStr).SetValueAsync(PillBuyS);
            levelData.Child(pillUseStr).SetValueAsync(PillUseS);
            levelData.Child(bombBuyStr).SetValueAsync(BombBuyS);
            levelData.Child(bombUseStr).SetValueAsync(BombUseS);
            levelData.Child(MixedBuyStr).SetValueAsync(MixedBuyS);
            levelData.Child(MixedUseStr).SetValueAsync(MixedUseS);
            levelData.Child(HummerBuyStr).SetValueAsync(HummerBuyS);
            levelData.Child(HummerUseStr).SetValueAsync(HummerUseS);

            levelData.Child(MoveBuyStr).SetValueAsync(MoveBuyS);
            levelData.Child(MoveAdStr).SetValueAsync(MoveAdS);
            levelData.Child(MoveBuyAverageStr).SetValueAsync(MoveBuyAverage);
            levelData.Child(MoveAdAverageStr).SetValueAsync(MoveAdSAverage);

            levelData.Child(Gold___50str).SetValueAsync(Gold___50S);
            levelData.Child(Gold__100str).SetValueAsync(Gold__100S);
            levelData.Child(Gold__250str).SetValueAsync(Gold__250S);
            levelData.Child(Gold__500str).SetValueAsync(Gold__500S);
            levelData.Child(Gold_1000str).SetValueAsync(Gold_1000S);


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

    //Раздел профиля
    public struct TypeProfile {
        public const string Profiles = "Profiles";
        public const string Level_Max = "Level_opened_maximum";
        public const string Level_Open_Averade = "Level_opened_average";
        public const string Level_Open_Averade_Count = "Level_opened_average_count";
    }

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
        reference = FirebaseDatabase.DefaultInstance.RootReference;



        TypeLevel.SetVictory(0, 12, 123, 4,5,6,7,8,9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
    }

    // Update is called once per frame
    void Update()
    {
        //попытка загрузить из сети или сохранить в сеть
        TypeLevel.TestLoadAndSave();
    }

}
