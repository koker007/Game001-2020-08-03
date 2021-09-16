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
    public int boxCount;
    public int moldCount;
    public int panelCount;

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

    }

    public void StartGameplay(ref Image[] stars)
    {
        //Если уровень выбран
        score = 0;
        boxCount = LevelsScript.main.ReturnLevel().NeedBox;
        movingCount = 0;
        movingMoldCount = 0;
        moldCount = LevelsScript.main.ReturnLevel().NeedMold;
        movingCan = LevelsScript.main.ReturnLevel().Move;
        CountStars(ref stars);
    }
    //вычитает ход и делает проверку на 0 ходов
    public void MinusMoving(GameFieldCTRL.Combination combination)
    {
        movingCount++;
        movingCan--;
        MenuGameplay.main.updateMoving();

        if (!combination.foundMold) movingMoldCount++;

        
    }

    public void ScoreUpdate(int PlusScore)
    {
        score += PlusScore;
        MenuGameplay.main.updateScore();
    }

    public void MoldUpdate()
    {
        moldCount--;
    }

    public void BoxUpdate()
    {
        boxCount--;
    }

    public void PanelUpdate()
    {
        panelCount--;
    }

    public void CheckEndGame()
    {
        //Если есть ходы, идет игра, и игра не закончена
        if (movingCan <= 0 && isGameplay && GameplayEnd == false)
        {
            //Если
            LevelsScript.Level level = LevelsScript.main.ReturnLevel();
            if (level.PassedWitScore && score >= level.NeedScore || !level.PassedWitScore)
            {
                if (level.PassedWitMold && moldCount >= level.NeedMold || !level.PassedWitMold)
                {
                    if (level.PassedWithBox && boxCount >= level.NeedBox || !level.PassedWithBox)
                    {
                        if (level.PassedWitPanel && panelCount >= level.NeedPanel || !level.PassedWitScore)
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

            PlayerProfile.main.LevelPassed(levelSelect);
            GlobalMessage.Results();
            GlobalMessage.Lose();
            LevelsScript.main.ReturnLevel().MaxScore = score;
            GameplayEnd = true;
        }

    }

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
    public void CountStars(ref Image[] stars)
    {
        CountStars(score,ref stars);
    }
}
