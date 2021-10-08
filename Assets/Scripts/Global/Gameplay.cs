using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// ���������� ������� ���������
/// </summary>
public class Gameplay : MonoBehaviour
{

    static public Gameplay main;


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

    [Header("Level parameters")]
    public int score = 0;
    public int scoreMax = 0;

    /// <summary>
    /// ���� ��������� ����
    /// </summary>
    public int[] colorsCount = new int[10];


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
        //���� ������� ������
        score = 0;
        movingCount = 0;
        movingMoldCount = 0;
        movingCan = LevelsScript.main.ReturnLevel().Move;
        CountStars(ref stars);


        for (int x = 0; x < colorsCount.Length; x++) {
            colorsCount[x] = 0;
        }

    }

    //������ ���������� ������� ���������� ��������� ��������� ������ � ���� ���� ��������
    List<GameFieldCTRL.Combination> combWaiting = new List<GameFieldCTRL.Combination>();
    
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

    //�������� ��� � ������ �������� �� 0 �����
    public void MinusMoving(GameFieldCTRL.Combination combination)
    {
        movingCount++;
        movingCan--;
        MenuGameplay.main.updateMoving();

        //��������� ���������� ��� ���� ��������
        combWaiting.Add(combination);

    }

    public void ScoreUpdate(int PlusScore)
    {
        score += PlusScore;
        MenuGameplay.main.updateScore();
    }

    public void CheckEndGame()
    {
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
            //���� ����� ��� ���������
            if (movingCan <= 0)
            {
                GlobalMessage.Lose();
                LevelsScript.main.ReturnLevel().MaxScore = score;
            }
        }

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
                //���� ����� ��� ���������
                if (movingCan <= 0)
                {
                    buffer.missionDefeat = true;
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
    /// ������� ������� ����� ����� ������� �� ������
    /// </summary>
    /// <param name="score"></param>
    /// <param name="stars"></param>
    public void CountStars(ref Image[] stars)
    {
        CountStars(score,ref stars);
    }
}
