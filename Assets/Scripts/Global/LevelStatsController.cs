using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatsController : MonoBehaviour
{
    public static LevelStatsController main;

    //Очки, которые игрок получил на уровне
    public int playerScore;
    //Ходы, которые совершил игрок
    public int playerTurns;
    //Смотрел ли игрок рекламу
    public bool adWatched = false;

    private void Start()
    {
        main = this;
    }    

    //Отправляем статистику на сервер (в конце уровня)
    public void SendPlayerStats()
    {
        Debug.Log("Stats have been sent: " + playerScore + " | " + playerTurns);
    }
}
