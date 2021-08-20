using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int movingCan = 0;
    public int movingCount = 0;
    public int colors = 3;
    public int combo = 0;

    private const float threeStartFactor = 2f;
    private const float twoStartFactor = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGameplay()
    {
        //Если уровень выбран
        score = 0;
        movingCan = LevelsScript.main.ReturnLevel().Move;
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

    public void CheckEndGame()
    {
        if (score >= LevelsScript.main.ReturnLevel().NeedScore && GameplayEnd == false)
        {
            GlobalMessage.Results();
            GameplayEnd = true;
        }
        else if (movingCan <= 0 && isGameplay && GameplayEnd == false)
        {
            GlobalMessage.Lose();
            GameplayEnd = true;
        }
    }

    public int CountStars(int score)
    {
        if (score >= LevelsScript.main.ReturnLevel().NeedScore * threeStartFactor)
        {
            return 3;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore * twoStartFactor)
        {
            return 2;
        }
        else if (score >= LevelsScript.main.ReturnLevel().NeedScore)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public int CountStars()
    {
        return CountStars(score);
    }
}
