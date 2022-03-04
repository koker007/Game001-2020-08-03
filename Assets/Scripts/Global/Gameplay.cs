using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
//Андрей
/// <summary>
/// Глобальные игровые параметры
/// </summary>
public class Gameplay : MonoBehaviour
{

    static public Gameplay main;
    public bool playerTurn = true;
    /// <summary>
    /// Текущий выбранный уровень
    /// </summary>
    public int levelSelect = 0;
    /// <summary>
    /// Билеты
    /// </summary>
    public int tickets = 0;

    public CellInternalObject.Type StartBonus = CellInternalObject.Type.none;
    public bool isGameplay = false;
    public bool GameplayEnd = false;
    public int adWatchedCount = 0; //Количество просмотров рекламмы
    public int buyMoveCount = 0; //Количество покупок ходов
    public bool gameStarted = false; //Проверяем, сделал ли игрок первый ход
    [Header("Level parameters")]
    public int score = 0;
    [SerializeField] Text enemyScoreText;
    public int scoreMax = 0;
    public float timeScale = 1;
    public int enemyScore = 0;

    public bool speedEnd = false;

    /// <summary>
    /// Цель окончания игры
    /// </summary>
    public int[] colorsCount = new int[10];
    private int starsCount = 0;

    /// <summary>
    /// Оставшиеся количество ходов
    /// </summary>
    public int movingCan = 0;
    /// <summary>
    /// общее количество выполненых ходов
    /// </summary>
    public int movingCount = 0;
    /// <summary>
    /// общее количество выполненных комбинаций
    /// </summary>
    public int movingCombCount = 0;
    /// <summary>
    /// Общее количество требуемых ходов плесени
    /// </summary>
    public int movingMoldCount = 0;
    public int colors = 3;
    public int superColorPercent = 0;
    public int typeBlockerPercent = 0;
    public int combo = 0;
    public bool moveCompleted = false;
    public float StarFactor3 = 1f;
    public float StarFactor2 = 0.81f;
    public float StarFactor1 = 0.61f;

    void Start()
    {
        main = this;
    }

    public void StartGameplay(ref Image[] stars)
    {
        LevelStatsController.main.playerTurns = 0;
        gameStarted = false;
        timeScale = 1;
        starsCount = 0;
        score = 0;
        enemyScore = 0;
        EnemyController.MoveCount = 0;
        movingCount = 0;
        movingMoldCount = 0;
        movingCan = LevelsScript.main.ReturnLevel().Move;
        movingCombCount = 0; 
        adWatchedCount = 0;
        buyMoveCount = 0;
        moveCompleted = false;
        playerTurn = true;
        CountStars(ref stars);

        speedEnd = false;

        for (int x = 0; x < colorsCount.Length; x++) {
            colorsCount[x] = 0;
        }

        TutorialController.main.CheckLevelTutorial(levelSelect);
    }

    //Список комбинаций которые получились благодаря действиям игрока и ждут пост проверок
    //List<GameFieldCTRL.Combination> combWaiting = new List<GameFieldCTRL.Combination>();
    
    /*
    //Проверить комбинации на то что их время вышло
    void combWaitingCalc() {

        //Создаем новый список комбинаций
        List<GameFieldCTRL.Combination> combWaitingNew = new List<GameFieldCTRL.Combination>();

        //Проверяем комбинации что их время не вышло
        for (int num = 0; num < combWaiting.Count; num++) {
            //Если время комбинации не вышло. переключаемся далее
            if (Time.unscaledTime - combWaiting[num].timeLastAction < 1)
            {
                combWaitingNew.Add(combWaiting[num]);
                continue;
            }

            //Выполняем действия
            if (!combWaiting[num].foundBenefit) movingMoldCount++;
        }

        //Заменяем
        combWaiting = combWaitingNew;
    }
    */

    //вычитает ход и делает проверку на 0 ходов
    public void MinusMoving(GameFieldCTRL.Combination combination)
    {

        LevelsScript.Level level = LevelsScript.main.ReturnLevel();

        if (playerTurn)
        {
            //Прибавляем ход
            movingCount++;
            TutorialController.main.CheckNextTutorialField(levelSelect, (int)movingCount/2);

            //Если уровень с врагом
            if (level.PassedWithEnemy)
            {
                //То отнимаем ход при определенных условиях

                //Если враг уже сходил достаточное количество раз на этом уровне
                //Если комбинации раньше не было
                if (EnemyController.MoveCount >= EnemyController.MoveCountForPlayer &&
                    movingCombCount == 0)
                {
                    movingCan--;
                }
            }

            //Если уровень без врага то в любом случае отнимаем ход
            else movingCan--;

            LevelStatsController.main.playerTurns++;
            MenuGameplay.main.updateMoving();


            //Добавляем комбинацию для пост проверки
            //combWaiting.Add(combination);

            //прибавляем ход плесени только если ее не задели в этой комбинации
            if (!combination.foundMould) {
                movingMoldCount++;
            }
        }
        else {
            EnemyController.MoveCount++;
        }

        //Если комбинация то добавляем комбинацию в счетчик комбинаций
        if (combination.cross ||
            combination.line4 ||
            combination.line5 ||
            combination.square)
        {
            movingCombCount++;
        }
        else {
            //ставим овер дохера комбинаций чтобы точно закончить этот ход
            movingCombCount = 0;
        }

        //Завершено ли действие
        moveCompleted = true;
    }

    public void ScoreUpdate(int PlusScore)
    {
        if (playerTurn)
        {
            score += PlusScore;
        }
        else
        {
            //Враг получает вдвое больше очков в начале игры 
            if (EnemyController.MoveCount <= EnemyController.MoveCountForPlayer)
            {
                enemyScore += PlusScore;
            }
            //Нормальное получение очков
            else {
                enemyScore += PlusScore;
            }
        }
        MenuGameplay.main.updateScore();
    }

    struct Buffer {
        //Завершена ли миссия
        public bool missionComplite;
        public bool missionDefeat;
        public float missionTestTime;
        
        
    }
    Buffer buffer;

    //Выполнить проверку состояний текущей миссии
    void TestMission() {
        //если в этом кадре не проверяли
        if (buffer.missionTestTime != Time.unscaledTime)
        {
            //Ставим время последней проверки
            buffer.missionTestTime = Time.unscaledTime;
            
            //Делаем параметры по умолчанию
            buffer.missionComplite = false;
            buffer.missionDefeat = false;

            //Если есть ходы, идет игра, и игра не закончена
            if (isGameplay && GameplayEnd == false)
            {
                //Если
                LevelsScript.Level level = LevelsScript.main.ReturnLevel();
                if (
                    (!level.PassedWithScore || score >= level.NeedScore) &&                                                                                     //Если очков больше чем нужно или этот уровень завешается не через 
                    (!level.PassedWithMold || MenuGameplay.main.gameFieldCTRL.CountMold <= 0) &&                                                                //Если плесени нету
                    (!level.PassedWithBox || MenuGameplay.main.gameFieldCTRL.CountBoxBlocker <= 0) &&                                                           //Если нет коробок
                    (!level.PassedWithRock || MenuGameplay.main.gameFieldCTRL.CountRockBlocker <= 0) &&                                                         //Если нет решеток
                    (!level.PassedWithIce || MenuGameplay.main.gameFieldCTRL.CountIce <= 0) &&                                                                  //Если нет льда
                    (!level.PassedWithPanel || MenuGameplay.main.gameFieldCTRL.CountInteractiveCells <= MenuGameplay.main.gameFieldCTRL.CountPanelSpread) &&    //Если все замазанно мазью
                    (!level.PassedWithCrystal || level.NeedCrystal <= main.colorsCount[(int)level.NeedColor]) &&                                                //Если собранны все кристалы
                    (!level.PassedWithUnderObj || MenuGameplay.main.gameFieldCTRL.listUnderIceObj.Count <= 0) &&                                                //Если все внутренние объекты достались
                    (!level.PassedWithEnemy || score > enemyScore && EnemyController.MoveCount > EnemyController.MoveCountForPlayer && playerTurn)              //Если у нас больше очков чем у врага
                    )
                {

                    buffer.missionComplite = true;
                    LevelStatsController.main.playerScore = score;
                    LevelStatsController.main.SendPlayerStats();
                }
                //если ходов нет проигрыш
                if (movingCan <= 0 && (!level.PassedWithEnemy || (level.PassedWithEnemy && score <= enemyScore)))
                {
                    buffer.missionDefeat = true;
                    LevelStatsController.main.playerScore = score;                    
                    LevelStatsController.main.SendPlayerStats();
                }
            }

            if (Input.GetMouseButtonDown(0) && //Если нажата кнопка то включаем завершающий пропуск 
                (buffer.missionComplite || buffer.missionDefeat)
                ) {
                //Говорим что пропускаем завершаюшие действия
                speedEnd = true;
            }
        }
    }

    //Выполнена ли цель задания
    public bool isMissionComplite() {

        TestMission();

        return buffer.missionComplite;
    }

    //Не проваленно ли задание
    public bool isMissionDefeat() {
        TestMission();

        return buffer.missionDefeat;
    }

    
    public void OpenMessageComplite() {
        timeScale = 1;
        PlayerProfile.main.LevelPassed(levelSelect);
        GlobalMessage.Results();
        LevelsScript.main.ReturnLevel().MaxScore = score;
        PlayerProfile.main.FillMoneyBox(starsCount);
        PlayerProfile.main.Save();
        GameplayEnd = true;
    }
    public void OpenMessageDefeat() {
        if (movingCan > 0) return;

        //Ищем это сообщение в буфере
        foreach (MessageCTRL message in MessageCTRL.BufferMessages) {
            if (message == null) continue;

            MessageYouLose messageYouLose = message.gameObject.GetComponent<MessageYouLose>();
            if (messageYouLose != null) {
                timeScale = 1;
                LevelsScript.main.ReturnLevel().MaxScore = score;

                message.OpenMessageBuffer();
                return;
            }
            
        }

        timeScale = 1;
        GlobalMessage.Lose();
        LevelsScript.main.ReturnLevel().MaxScore = score;
    }


    /// <summary>
    /// считает сколько звезд игрок получил на уровне
    /// </summary>
    /// <param name="score"></param>
    /// <param name="stars"></param>
    public void CountStars(int score, ref Image[] stars, bool saveStars)
    {
        if (score >= LevelsScript.main.ReturnLevel().NeedScore * StarFactor3)
        {
            stars[0].color = Color.yellow;
            stars[1].color = Color.yellow;
            stars[2].color = Color.yellow;
            starsCount = 3;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore * StarFactor2)
        {
            stars[0].color = Color.yellow;
            stars[1].color = Color.yellow;
            stars[2].color = new Color32(140, 140, 60, 255);
            starsCount = 2;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore * StarFactor1)
        {
            stars[0].color = Color.yellow;
            stars[1].color = new Color32(140, 140, 60, 255);
            stars[2].color = new Color32(140, 140, 60, 255);
            starsCount = 1;
        }
        else
        {
            stars[0].color = new Color32(140, 140, 60, 255);
            stars[1].color = new Color32(140, 140, 60, 255);
            stars[2].color = new Color32(140, 140, 60, 255);
            starsCount = 0;
        }

        if (saveStars)
            PlayerProfile.main.SetLVLStar(levelSelect, starsCount);

    }
    public int CountStars(int score) {
        int num = 0;

        if (score >= LevelsScript.main.ReturnLevel().NeedScore * StarFactor3)
        {
            num = 3;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore * StarFactor2)
        {
            num = 2;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore * StarFactor1)
        {
            num = 1;
        }

        return num;
    }

    
    /// <summary>
    /// считает сколько звезд игрок получил на уровне
    /// </summary>
    public void CountStars(ref Image[] stars)
    {

        CountStars(score,ref stars, false);
    }

    public void randomizedPlayBonus() {

        StartBonus = CellInternalObject.Type.rocketHorizontal;

        int randType = Random.Range(0,4);

        if (randType == 0) {
            if(Random.Range(0, 100) < 50) StartBonus = CellInternalObject.Type.color5;
        }
        else if (randType == 1) {
            if(Random.Range(0, 100) < 50) StartBonus = CellInternalObject.Type.bomb;
        }
        else if (randType == 2) {
            if (Random.Range(0, 100) < 50) StartBonus = CellInternalObject.Type.airplane;
        }
    }
}
