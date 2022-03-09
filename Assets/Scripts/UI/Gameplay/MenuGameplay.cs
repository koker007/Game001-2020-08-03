using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// �������� �� ������� ��������
/// </summary>
public class MenuGameplay : MonoBehaviour
{
    public static MenuGameplay main;

    [Header("Panels")]
    [SerializeField]
    RectTransform PanelUp;
    [SerializeField]
    RectTransform PanelDown;

    [SerializeField]
    GameObject GameFieldPrefab;
    [SerializeField]
    GameObject MidleTextPrefab;

    [SerializeField]
    Transform GameFieldParent;
    [SerializeField]
    Transform MidleTextParent;

    [SerializeField]
    public RawImage Particles3D;

    public GameFieldCTRL gameFieldCTRL;

    [Header("Score")]
    [SerializeField]
    Text Level;
    [SerializeField]
    Text Score;
    [SerializeField]
    Text Move;
    [SerializeField]
    GameObject MoveClosedEnemy;

    [SerializeField]
    Slider ScoreSlider;
    float ScoreNeed = 0;
    [SerializeField]
    Image[] Stars = new Image[3];
    [SerializeField]
    public GameObject[] Goal = new GameObject[3];
    Text[] GoalText = new Text[3];
    Image[] GoalImage = new Image[3];


    [Header("PassedImages")]
    [SerializeField]
    public Sprite[] PassedCrystalImage = new Sprite[5];
    [SerializeField]
    public Sprite PassedBoxImage;
    [SerializeField]
    public Sprite PassedPanelImage;
    [SerializeField]
    public Sprite PassedMoldImage;
    [SerializeField]
    public Sprite PassedIceImage;
    [SerializeField]
    public Sprite PassedUnderObjImage;
    [SerializeField]
    public Sprite PassedRockImage;

    [SerializeField]
    public RawImage ImageFon;

    [SerializeField]
    Slider PassedEnemySlider;
    [SerializeField]
    GameObject PassedEnemy;

    [HideInInspector]
    public static GameObject GameField;

    private LevelsScript.Level level;


    private void Awake()
    {
        main = this;
        for (int i = 0; i < Goal.Length; i++)
        {
            GoalText[i] = Goal[i].GetComponentInChildren<Text>();
            GoalImage[i] = Goal[i].GetComponentInChildren<Image>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //startButtons();
        EnableUI();
    }

    // Update is called once per frame
    void Update()
    {
        //updateButtons();
        updateGoal();

#if PLATFORM_ANDROID
        if (Input.GetKey(KeyCode.Escape))
        {
            clickButtonExitLevel();
        }
#endif

        UpdateUI();
    }

    void startButtons()
    {
        //PanelDown.pivot = new Vector2(PanelDown.pivot.x, 3);
        //PanelUp.pivot = new Vector2(PanelUp.pivot.x, -3);
    }

    void updateButtons()
    {
        moving();

        void moving()
        {
            float downY = PanelDown.pivot.y;
            downY += (0 - downY) * Time.unscaledDeltaTime * 4;
            if (downY < 0) downY = 0;

            PanelDown.pivot = new Vector2(PanelDown.pivot.x, downY);

            float upY = PanelUp.pivot.y;
            upY += (1 - upY) * Time.unscaledDeltaTime * 4;
            if (upY > 1) upY = 1;

            PanelUp.pivot = new Vector2(PanelUp.pivot.x, upY);
        }
    }

    private void OnEnable()
    {
        DestroyAllField();
        CreateGameField();
        EnableUI();
        ClearGoal();

        if (!GameplayParticles3D.main.gameObject.activeSelf) {
            GameplayParticles3D.main.gameObject.SetActive(true);
        }
    }
    private void OnDisable()
    {
        DestroyAllField();

        if (GameplayParticles3D.main.gameObject.activeSelf)
        {
            GameplayParticles3D.main.gameObject.SetActive(false);
        }

    }

    //������� ������� ���� �������� ���������� ����
    void CreateGameField() {

        SuperHitSelected = SuperHitType.none;


        //�������� �������� ����
        GameField = Instantiate(GameFieldPrefab, GameFieldParent);
        gameFieldCTRL = GameField.GetComponent<GameFieldCTRL>();

        if (LevelsScript.main)
        {
            LevelGenerator.main.GenerateLevel(Gameplay.main.levelSelect);
            level = LevelsScript.main.ReturnLevel();
            gameFieldCTRL.inicializeField(level);
        }
        else {
            Destroy(GameField);
        }

        //��������� ����� ����
        if (Gameplay.main)
        {
            Gameplay.main.StartGameplay(ref Stars);
        }
        else
        {
            Destroy(GameField);
        }
    }

    //������ ��� ������� ����
    void DestroyAllField() {
        GameFieldCTRL[] fields = GameFieldParent.GetComponentsInChildren<GameFieldCTRL>();
        foreach (GameFieldCTRL field in fields) {
            Destroy(field.gameObject);
        }
    }

    //���������� ������ ����������
    void EnableUI() {
        if (Gameplay.main) {
            Level.text = System.Convert.ToString(Gameplay.main.levelSelect);
            updateMoving();
            updateScore();
            updateGoal();

            //��������� �������� ������ �� ������
            if (!Particles3D.texture || Particles3D.texture.texelSize.x != Screen.width || Particles3D.texture.texelSize.y != Screen.height)
            {
                Particles3D.texture = GameplayParticles3D.main.texture;
                GameplayParticles3D.main.SetCameraPos(gameFieldCTRL.cellCTRLs.GetLength(0), gameFieldCTRL.cellCTRLs.GetLength(1));
            }
        }
    }

    void UpdateUI() {

        //Score();

        void Score() {
            //ScoreSlider.value += (ScoreNeed - ScoreSlider.value) * Time.deltaTime;
        }

        updateMoving();
    }

    /// <summary>
    /// ��������� ���������� � ���� ������
    /// </summary>
    public void updateGoal()
    {

        //�������� ���������� �����
        if (level.PassedWithEnemy)
        {

            PassedEnemySlider.gameObject.SetActive(true);
            PassedEnemy.SetActive(true);
            MoveClosedEnemy.SetActive(true);

            if (EnemyController.MoveCount > 5)
            {
                MoveClosedEnemy.SetActive(false);
            }
            else {
                MoveClosedEnemy.SetActive(true);
            }

        }
        else {
            PassedEnemySlider.gameObject.SetActive(false);
            PassedEnemy.SetActive(false);
            MoveClosedEnemy.SetActive(false);

            int i = 0;
            if (level.PassedWithScore)
            {
                GoalImage[i].sprite = PassedPanelImage;
                GoalText[i].text = level.NeedScore.ToString();
                i++;
            }
            if (level.PassedWithCrystal)
            {
                GoalImage[i].sprite = PassedCrystalImage[(int)level.NeedColor];
                int countColor = level.NeedCrystal - Gameplay.main.colorsCount[(int)level.NeedColor];
                if (countColor < 0) countColor = 0;
                GoalText[i].text = (countColor).ToString();
                i++;
            }
            if (level.PassedWithBox)
            {
                GoalImage[i].sprite = PassedBoxImage;
                GoalText[i].text = gameFieldCTRL.CountBoxBlocker.ToString();
                i++;
            }
            if (level.PassedWithMold)
            {
                GoalImage[i].sprite = PassedMoldImage;
                GoalText[i].text = gameFieldCTRL.CountMold.ToString();
                i++;
            }
            if (level.PassedWithIce)
            {
                GoalImage[i].sprite = PassedIceImage;
                GoalText[i].text = gameFieldCTRL.CountIce.ToString();
                i++;
            }
            if (level.PassedWithUnderObj)
            {
                GoalImage[i].sprite = PassedUnderObjImage;
                GoalText[i].text = gameFieldCTRL.listUnderIceObj.Count.ToString();
                i++;
            }
            if (level.PassedWithPanel)
            {
                GoalImage[i].sprite = PassedPanelImage;
                GoalText[i].text = (gameFieldCTRL.CountInteractiveCells - gameFieldCTRL.CountPanelSpread).ToString();
                i++;
            }
            if (level.PassedWithRock)
            {
                GoalImage[i].sprite = PassedRockImage;
                GoalText[i].text = gameFieldCTRL.CountRockBlocker.ToString();
                i++;
            }

            for (int j = 0; j < i; j++)
            {
                Goal[j].SetActive(true);
            }

        }
    }

    private void ClearGoal()
    {
        for (int j = 0; j < Goal.Length; j++)
        {
            Goal[j].SetActive(false);
        }
    }

    /// <summary>
    /// ��������� ���� �����
    /// </summary>
    public void updateScore()
    {
        Score.text = System.Convert.ToString(Gameplay.main.score);

        ScoreSlider.value = (float)Gameplay.main.score / (level.NeedScore * Gameplay.main.StarFactor3);


        Gameplay.main.CountStars(ref Stars);
    }
    public void updateMoving()
    {
        Move.text = System.Convert.ToString(Gameplay.main.movingCan);
    }

    /// <summary>
    /// ������� ��������� ����
    /// </summary>
    public void clickButtoSettings()
    {
        GlobalMessage.Settings();
    }

    /// <summary>
    /// ������� ���� ������ �� ����
    /// </summary>
    public void clickButtonExitLevel()
    {
        GlobalMessage.ExitLevel();
    }

    public void CreateMidleMessage(string text, Color color) {
        EnemyController.canEnemyMove = false;
        GameFieldCTRL.main.canPassTurn = false;
        GameObject MidleTextObj = Instantiate(MidleTextPrefab, MidleTextParent);
        MidleTextGameplay MidleText = MidleTextObj.GetComponent<MidleTextGameplay>();

        //�������� �� ������� �������������� �����
        if (text == MidleTextGameplay.strMixed)
        {
            MidleText.SetMixed();
        }
        else if (text == MidleTextGameplay.strEnemyTurn)
        {
            MidleText.SetTextEnemyMove();
        }
        else if (text == MidleTextGameplay.strYourTurn)
        {
            MidleText.SetTextYourMove();
        }
        else if (text == MidleTextGameplay.strCombo)
        {
            MidleText.SetTextCombo();
        }
        else if (text == MidleTextGameplay.strNotBad)
        {
            MidleText.SetTextNotBad();
        }
        else if (text == MidleTextGameplay.strCongratulations)
        {
            MidleText.SetTextCongratulations();
        }
        else if (text == MidleTextGameplay.strLastMove)
        {
            MidleText.SetTextLastMove();
        }
        else if (text == MidleTextGameplay.strIncredible) {
            MidleText.SetTextIncredible();
        }

        //����� ������� �����
        else
        {
            MidleText.SetText(text);
        }

        if (color != Color.black)
            MidleText.SetColorFon(color);
    }
    public void CreateMidleMessage(string text) {
        CreateMidleMessage(text, Color.black);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////
    ///������ ������� ������
    public enum SuperHitType {
        none,
        internalObj,
        rosket2x,
        bomb,
        Color5,
        mixed          
    }

    public SuperHitType SuperHitSelected = SuperHitType.none;

    public void ButtonClickDestroyInternal() {
        SuperHitSelected = SuperHitType.none;

        //��������� �����
        if (PlayerProfile.main.ShopInternal.Amount > 0)
        {
            SuperHitSelected = SuperHitType.internalObj;
        }
        //��������� ���� �������
        else {
            GlobalMessage.ShopBuyInternal();
        }
    }
    public void ButtonClickRosket() {
        SuperHitSelected = SuperHitType.none;

        //���������� �����
        if (PlayerProfile.main.ShopRocket.Amount > 0)
        {
            SuperHitSelected = SuperHitType.rosket2x;

        }
        //��������� ���� �������
        else {
            GlobalMessage.ShopBuyRocket();
        }
    }

    public void ButtonClickBomb() {
        SuperHitSelected = SuperHitType.none;

        //���������� �����
        if (PlayerProfile.main.ShopBomb.Amount > 0)
        {
            SuperHitSelected = SuperHitType.bomb;

        }
        //��������� ���� �������
        else
        {
            GlobalMessage.ShopBuyBomb();
        }
    }

    public void ButtonClickSuperColor() {
        SuperHitSelected = SuperHitType.none;

        //���������� �����
        if (PlayerProfile.main.ShopColor5.Amount > 0) {
            SuperHitSelected = SuperHitType.Color5;
        }
        //��������� ���� �������
        else {
            GlobalMessage.ShopBuyColor5();
        }
    }

    //������ ������������ ��������
    public void ButtonClickMixed() {
        SuperHitSelected = SuperHitType.none;

        //���������� �����
        if (PlayerProfile.main.ShopMixed.Amount > 0)
        {
            SuperHitSelected = SuperHitType.mixed;
        }
        //��������� ���� �������
        else
        {
            GlobalMessage.ShopBuyMixed();
        }
    }
    //����� ���� ������ ������� ������
    ///////////////////////////////////////////////////////////////////////////////////////////////////
}
