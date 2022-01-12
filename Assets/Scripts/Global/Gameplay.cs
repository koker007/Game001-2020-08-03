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

    public bool isGameplay = false;
    public bool GameplayEnd = false;
    public bool adWatched = false;
    public bool gameStarted = false; //Проверяем, сделал ли игрок первый ход
    [Header("Level parameters")]
    public int score = 0;
    [SerializeField] Text enemyScoreText;
    public int scoreMax = 0;
    public float timeScale = 1;
    public int enemyScore = 0;

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
    /// Общее количество требуемых ходов плесени
    /// </summary>
    public int movingMoldCount = 0;
    public int colors = 3;
    public int superColorPercent = 0;
    public int typeBlockerPercent = 0;
    public int combo = 0;
    public bool moveCompleted = false;
    public float threeStartFactor = 2f;
    public float twoStartFactor = 1.5f;

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
        adWatched = false;
        moveCompleted = false;
        playerTurn = true;
        CountStars(ref stars);

        for (int x = 0; x < colorsCount.Length; x++) {
            colorsCount[x] = 0;
        }
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
            movingCount++;

            if (level.PassedWithEnemy && EnemyController.MoveCount >= EnemyController.MoveCountForPlayer)
                movingCan--;
            else movingCan--;

            LevelStatsController.main.playerTurns++;
            MenuGameplay.main.updateMoving();


            //Добавляем комбинацию для пост проверки
            //combWaiting.Add(combination);
        }
        else {
            EnemyController.MoveCount++;
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
                if (score >= level.NeedScore || !level.PassedWithScore)
                {                   
                    if (MenuGameplay.main.gameFieldCTRL.CountMold <= 0 || !level.PassedWithMold)
                    {
                        if (MenuGameplay.main.gameFieldCTRL.CountBoxBlocker <= 0 || !level.PassedWithBox)
                        {
                            if (MenuGameplay.main.gameFieldCTRL.CountRockBlocker <= 0 || !level.PassedWithRock)
                            {
                                if (MenuGameplay.main.gameFieldCTRL.CountIce <= 0 || !level.PassedWithIce)
                                {
                                    if (MenuGameplay.main.gameFieldCTRL.CountInteractiveCells <= MenuGameplay.main.gameFieldCTRL.CountPanelSpread || !level.PassedWithPanel)
                                    {                                       
                                        if (level.NeedCrystal <= main.colorsCount[(int)level.NeedColor] || !level.PassedWithCrystal)
                                        {
                                            //Если битва с врагом и у нас больше нет ходов и очков у нас больше чем у врага
                                            if (!level.PassedWithEnemy || score > enemyScore && EnemyController.MoveCount > EnemyController.MoveCountForPlayer && playerTurn)
                                            {
                                                buffer.missionComplite = true;
                                                LevelStatsController.main.playerScore = score;
                                                LevelStatsController.main.SendPlayerStats();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //если ходов нет проигрыш
                if (movingCan <= 0 && (!level.PassedWithEnemy || (level.PassedWithEnemy && score <= enemyScore)))
                {
                    buffer.missionDefeat = true;
                    LevelStatsController.main.playerScore = score;                    
                    LevelStatsController.main.SendPlayerStats();
                }
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
        timeScale = 1;
        GlobalMessage.Lose();
        LevelsScript.main.ReturnLevel().MaxScore = score;
    }


    /// <summary>
    /// считает сколько звезд игрок получил на уровне
    /// </summary>
    /// <param name="score"></param>
    /// <param name="stars"></param>
    public void CountStars(int score, ref Image[] stars)
    {
        if (score >= LevelsScript.main.ReturnLevel().NeedScore * threeStartFactor)
        {
            stars[0].color = Color.yellow;
            stars[1].color = Color.yellow;
            stars[2].color = Color.yellow;
            starsCount = 3;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore * twoStartFactor)
        {
            stars[0].color = Color.yellow;
            stars[1].color = Color.yellow;
            stars[2].color = new Color32(140, 140, 60, 255);
            starsCount = 2;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore)
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


        PlayerProfile.main.SetLVLStar(levelSelect, starsCount);

    }
    public int CountStars(int score) {
        int num = 0;

        if (score >= LevelsScript.main.ReturnLevel().NeedScore * threeStartFactor)
        {
            num = 3;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore * twoStartFactor)
        {
            num = 2;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore)
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
        CountStars(score,ref stars);
    }
}
