using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
//������
/// <summary>
/// ���������� ������� ���������
/// </summary>
public class Gameplay : MonoBehaviour
{

    static public Gameplay main;
    public bool playerTurn = true;
    /// <summary>
    /// ������� ��������� �������
    /// </summary>
    public int levelSelect = 0;
    /// <summary>
    /// ������
    /// </summary>
    public int tickets = 0;

    public bool isGameplay = false;
    public bool GameplayEnd = false;
    public bool adWatched = false;
    public bool gameStarted = false; //���������, ������ �� ����� ������ ���
    [Header("Level parameters")]
    public int score = 0;
    [SerializeField] Text enemyScoreText;
    public int scoreMax = 0;
    public float timeScale = 1;
    public int enemyScore = 0;

    /// <summary>
    /// ���� ��������� ����
    /// </summary>
    public int[] colorsCount = new int[10];
    private int starsCount = 0;

    /// <summary>
    /// ���������� ���������� �����
    /// </summary>
    public int movingCan = 0;
    /// <summary>
    /// ����� ���������� ���������� �����
    /// </summary>
    public int movingCount = 0;
    /// <summary>
    /// ����� ���������� ��������� ����� �������
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

    //������ ���������� ������� ���������� ��������� ��������� ������ � ���� ���� ��������
    //List<GameFieldCTRL.Combination> combWaiting = new List<GameFieldCTRL.Combination>();
    
    /*
    //��������� ���������� �� �� ��� �� ����� �����
    void combWaitingCalc() {

        //������� ����� ������ ����������
        List<GameFieldCTRL.Combination> combWaitingNew = new List<GameFieldCTRL.Combination>();

        //��������� ���������� ��� �� ����� �� �����
        for (int num = 0; num < combWaiting.Count; num++) {
            //���� ����� ���������� �� �����. ������������� �����
            if (Time.unscaledTime - combWaiting[num].timeLastAction < 1)
            {
                combWaitingNew.Add(combWaiting[num]);
                continue;
            }

            //��������� ��������
            if (!combWaiting[num].foundBenefit) movingMoldCount++;
        }

        //��������
        combWaiting = combWaitingNew;
    }
    */

    //�������� ��� � ������ �������� �� 0 �����
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


            //��������� ���������� ��� ���� ��������
            //combWaiting.Add(combination);
        }
        else {
            EnemyController.MoveCount++;
        }

        //��������� �� ��������
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
            //���� �������� ����� ������ ����� � ������ ���� 
            if (EnemyController.MoveCount <= EnemyController.MoveCountForPlayer)
            {
                enemyScore += PlusScore;
            }
            //���������� ��������� �����
            else {
                enemyScore += PlusScore;
            }
        }
        MenuGameplay.main.updateScore();
    }

    struct Buffer {
        //��������� �� ������
        public bool missionComplite;
        public bool missionDefeat;
        public float missionTestTime;
        
        
    }
    Buffer buffer;

    //��������� �������� ��������� ������� ������
    void TestMission() {
        //���� � ���� ����� �� ���������
        if (buffer.missionTestTime != Time.unscaledTime)
        {
            //������ ����� ��������� ��������
            buffer.missionTestTime = Time.unscaledTime;

            //������ ��������� �� ���������
            buffer.missionComplite = false;
            buffer.missionDefeat = false;

            //���� ���� ����, ���� ����, � ���� �� ���������
            if (isGameplay && GameplayEnd == false)
            {
                //����
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
                                            //���� ����� � ������ � � ��� ������ ��� ����� � ����� � ��� ������ ��� � �����
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
                //���� ����� ��� ��������
                if (movingCan <= 0 && (!level.PassedWithEnemy || (level.PassedWithEnemy && score <= enemyScore)))
                {
                    buffer.missionDefeat = true;
                    LevelStatsController.main.playerScore = score;                    
                    LevelStatsController.main.SendPlayerStats();
                }
            }
        }
    }

    //��������� �� ���� �������
    public bool isMissionComplite() {

        TestMission();

        return buffer.missionComplite;
    }

    //�� ���������� �� �������
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
    /// ������� ������� ����� ����� ������� �� ������
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
    /// ������� ������� ����� ����� ������� �� ������
    /// </summary>
    public void CountStars(ref Image[] stars)
    {
        CountStars(score,ref stars);
    }
}
