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

    public CellInternalObject.Type StartBonus = CellInternalObject.Type.none;
    public bool isGameplay = false;
    public bool GameplayEnd = false;
    public int adWatchedCount = 0; //���������� ���������� ��������
    public int buyMoveCount = 0; //���������� ������� �����
    public bool gameStarted = false; //���������, ������ �� ����� ������ ���
    [Header("Level parameters")]
    public int score = 0;
    [SerializeField] Text enemyScoreText;
    public int scoreMax = 0;
    public float timeScale = 1;
    public int enemyScore = 0;

    public bool speedEnd = false;

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
    /// ����� ���������� ����������� ����������
    /// </summary>
    public int movingCombCount = 0;
    /// <summary>
    /// ����� ���������� ��������� ����� �������
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
            //���������� ���
            movingCount++;
            TutorialController.main.CheckNextTutorialField(levelSelect, (int)movingCount/2);

            //���� ������� � ������
            if (level.PassedWithEnemy)
            {
                //�� �������� ��� ��� ������������ ��������

                //���� ���� ��� ������ ����������� ���������� ��� �� ���� ������
                //���� ���������� ������ �� ����
                if (EnemyController.MoveCount >= EnemyController.MoveCountForPlayer &&
                    movingCombCount == 0)
                {
                    movingCan--;
                }
            }

            //���� ������� ��� ����� �� � ����� ������ �������� ���
            else movingCan--;

            LevelStatsController.main.playerTurns++;
            MenuGameplay.main.updateMoving();


            //��������� ���������� ��� ���� ��������
            //combWaiting.Add(combination);

            //���������� ��� ������� ������ ���� �� �� ������ � ���� ����������
            if (!combination.foundMould) {
                movingMoldCount++;
            }
        }
        else {
            EnemyController.MoveCount++;
        }

        //���� ���������� �� ��������� ���������� � ������� ����������
        if (combination.cross ||
            combination.line4 ||
            combination.line5 ||
            combination.square)
        {
            movingCombCount++;
        }
        else {
            //������ ���� ������ ���������� ����� ����� ��������� ���� ���
            movingCombCount = 0;
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
                if (
                    (!level.PassedWithScore || score >= level.NeedScore) &&                                                                                     //���� ����� ������ ��� ����� ��� ���� ������� ���������� �� ����� 
                    (!level.PassedWithMold || MenuGameplay.main.gameFieldCTRL.CountMold <= 0) &&                                                                //���� ������� ����
                    (!level.PassedWithBox || MenuGameplay.main.gameFieldCTRL.CountBoxBlocker <= 0) &&                                                           //���� ��� �������
                    (!level.PassedWithRock || MenuGameplay.main.gameFieldCTRL.CountRockBlocker <= 0) &&                                                         //���� ��� �������
                    (!level.PassedWithIce || MenuGameplay.main.gameFieldCTRL.CountIce <= 0) &&                                                                  //���� ��� ����
                    (!level.PassedWithPanel || MenuGameplay.main.gameFieldCTRL.CountInteractiveCells <= MenuGameplay.main.gameFieldCTRL.CountPanelSpread) &&    //���� ��� ��������� �����
                    (!level.PassedWithCrystal || level.NeedCrystal <= main.colorsCount[(int)level.NeedColor]) &&                                                //���� �������� ��� ��������
                    (!level.PassedWithUnderObj || MenuGameplay.main.gameFieldCTRL.listUnderIceObj.Count <= 0) &&                                                //���� ��� ���������� ������� ���������
                    (!level.PassedWithEnemy || score > enemyScore && EnemyController.MoveCount > EnemyController.MoveCountForPlayer && playerTurn)              //���� � ��� ������ ����� ��� � �����
                    )
                {

                    buffer.missionComplite = true;
                    LevelStatsController.main.playerScore = score;
                    LevelStatsController.main.SendPlayerStats();
                }
                //���� ����� ��� ��������
                if (movingCan <= 0 && (!level.PassedWithEnemy || (level.PassedWithEnemy && score <= enemyScore)))
                {
                    buffer.missionDefeat = true;
                    LevelStatsController.main.playerScore = score;                    
                    LevelStatsController.main.SendPlayerStats();
                }
            }

            if (Input.GetMouseButtonDown(0) && //���� ������ ������ �� �������� ����������� ������� 
                (buffer.missionComplite || buffer.missionDefeat)
                ) {
                //������� ��� ���������� ����������� ��������
                speedEnd = true;
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
        if (movingCan > 0) return;

        //���� ��� ��������� � ������
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
    /// ������� ������� ����� ����� ������� �� ������
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
    /// ������� ������� ����� ����� ������� �� ������
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
