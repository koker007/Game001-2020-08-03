using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatsController : MonoBehaviour
{
    public static LevelStatsController main;

    //����, ������� ����� ������� �� ������
    public int playerScore;
    //����, ������� �������� �����
    public int playerTurns;
    //������� �� ����� �������
    public bool adWatched = false;

    private void Start()
    {
        main = this;
    }    

    //���������� ���������� �� ������ (� ����� ������)
    public void SendPlayerStats()
    {
        Debug.Log("Stats have been sent: " + playerScore + " | " + playerTurns);
    }
}
