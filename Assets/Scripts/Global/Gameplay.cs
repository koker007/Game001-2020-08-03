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
    public int colors = 3;
    public int combo = 0;
    public int boxCount;
    public int moldCount;

    public float threeStartFactor = 2f;
    public float twoStartFactor = 1.5f;

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
        moldCount = LevelsScript.main.ReturnLevel().NeedMold;
        movingCan = LevelsScript.main.ReturnLevel().Move;
        CountStars(ref stars);
    }
    //вычитает ход и делает проверку на 0 ходов
    public void MinusMoving()
    {
        //movingCount++;
        movingCan--;
        MenuGameplay.main.updateMoving();
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

    public void CheckEndGame()
    {
        if (movingCan <= 0 && isGameplay && GameplayEnd == false)
        {
            if (LevelsScript.main.ReturnLevel().pasType == LevelsScript.PassedType.score && score >= LevelsScript.main.ReturnLevel().NeedScore)
            {
                PlayerProfile.main.LevelPassed(levelSelect);
                GlobalMessage.Results();
            }
            else if (LevelsScript.main.ReturnLevel().pasType == LevelsScript.PassedType.box && boxCount <= 0)
            {
                PlayerProfile.main.LevelPassed(levelSelect);
                GlobalMessage.Results();
            }
            else if (LevelsScript.main.ReturnLevel().pasType == LevelsScript.PassedType.mold && boxCount <= 0)
            {
                PlayerProfile.main.LevelPassed(levelSelect);
                GlobalMessage.Results();
            }
            else
            {
                GlobalMessage.Lose();
            }
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
