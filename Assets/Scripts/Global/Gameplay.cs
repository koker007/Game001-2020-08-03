using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Глобальные игровые параметры
/// </summary>
public class Gameplay : MonoBehaviour
{

    static public Gameplay main;


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

    [Header("Level parameters")]
    public int score = 0;
    public int scoreMax = 0;

    /// <summary>
    /// Цель окончания игры
    /// </summary>
    public int[] colorsCount = new int[10];


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
    public int combo = 0;

    public float threeStartFactor = 2f;
    public float twoStartFactor = 1.5f;

    public int buttonDestroyInternal = 10;
    public int buttonRosket = 10;
    public int buttonSuperColor = 10;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        combWaitingCalc();
    }

    public void StartGameplay(ref Image[] stars)
    {
        //Если уровень выбран
        score = 0;
        movingCount = 0;
        movingMoldCount = 0;
        movingCan = LevelsScript.main.ReturnLevel().Move;
        CountStars(ref stars);


        for (int x = 0; x < colorsCount.Length; x++) {
            colorsCount[x] = 0;
        }

    }

    //Список комбинаций которые получились благодаря действиям игрока и ждут пост проверок
    List<GameFieldCTRL.Combination> combWaiting = new List<GameFieldCTRL.Combination>();
    
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

    //вычитает ход и делает проверку на 0 ходов
    public void MinusMoving(GameFieldCTRL.Combination combination)
    {
        movingCount++;
        movingCan--;
        MenuGameplay.main.updateMoving();

        //Добавляем комбинацию для пост проверки
        combWaiting.Add(combination);

    }

    public void ScoreUpdate(int PlusScore)
    {
        score += PlusScore;
        MenuGameplay.main.updateScore();
    }

    public void CheckEndGame()
    {
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
                            if (MenuGameplay.main.gameFieldCTRL.CountInteractiveCells <= MenuGameplay.main.gameFieldCTRL.CountPanelSpread || !level.PassedWithPanel)
                            {
                                if (level.NeedCrystal <= main.colorsCount[(int)level.NeedColor] || !level.PassedWithCrystal)
                                {
                                    PlayerProfile.main.LevelPassed(levelSelect);
                                    GlobalMessage.Results();
                                    LevelsScript.main.ReturnLevel().MaxScore = score;
                                    GameplayEnd = true;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            //если ходов нет проигрышь
            if (movingCan <= 0)
            {
                GlobalMessage.Lose();
                LevelsScript.main.ReturnLevel().MaxScore = score;
            }
        }

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
                                if (MenuGameplay.main.gameFieldCTRL.CountInteractiveCells <= MenuGameplay.main.gameFieldCTRL.CountPanelSpread || !level.PassedWithPanel)
                                {
                                    if (level.NeedCrystal <= main.colorsCount[(int)level.NeedColor] || !level.PassedWithCrystal)
                                    {
                                        buffer.missionComplite = true;
                                    }
                                }
                            }
                        }
                    }
                }
                //если ходов нет проигрышь
                if (movingCan <= 0)
                {
                    buffer.missionDefeat = true;
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
        PlayerProfile.main.LevelPassed(levelSelect);
        GlobalMessage.Results();
        LevelsScript.main.ReturnLevel().MaxScore = score;
        GameplayEnd = true;

    }
    public void OpenMessageDefeat() {
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
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore * twoStartFactor)
        {
            stars[0].color = Color.yellow;
            stars[1].color = Color.yellow;
            stars[2].color = new Color32(140, 140, 60, 255);
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore)
        {
            stars[0].color = Color.yellow;
            stars[1].color = new Color32(140, 140, 60, 255);
            stars[2].color = new Color32(140, 140, 60, 255);
        }
        else
        {
            stars[0].color = new Color32(140, 140, 60, 255);
            stars[1].color = new Color32(140, 140, 60, 255);
            stars[2].color = new Color32(140, 140, 60, 255);
        }
    }
    /// <summary>
    /// считает сколько звезд игрок получил на уровне
    /// </summary>
    /// <param name="score"></param>
    /// <param name="stars"></param>
    public void CountStars(ref Image[] stars)
    {
        CountStars(score,ref stars);
    }
}
