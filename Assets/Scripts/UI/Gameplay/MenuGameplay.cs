using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Отвечает за текущий геймплей
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
    Transform GameFieldParent;

    GameFieldCTRL gameFieldCTRL;

    [Header("Score")]
    [SerializeField]
    Text Level;
    [SerializeField]
    Text Score;
    [SerializeField]
    Text Move;
    [SerializeField]
    Slider ScoreSlider;
    [SerializeField]
    Image[] Stars = new Image[3];
    [SerializeField]
    GameObject[] Goal = new GameObject[3];
    Text[] GoalText = new Text[3];

    public static GameObject GameField;

    private LevelsScript.Level level;


    private void Awake()
    {
        main = this;
        for (int i = 0; i < Goal.Length; i++)
        {
            GoalText[i] = Goal[i].GetComponentInChildren<Text>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startButtons();
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        updateButtons();
        updateGoal();
    }

    void startButtons()
    {
        PanelDown.pivot = new Vector2(PanelDown.pivot.x, 3);
        PanelUp.pivot = new Vector2(PanelUp.pivot.x, -3);
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
        updateUI();
    }
    private void OnDisable()
    {
        DestroyAllField();
    }

    //Создать игровое поле согласно параметрам игры
    void CreateGameField() {
        //Создание игрового поля
        GameField = Instantiate(GameFieldPrefab, GameFieldParent);
        gameFieldCTRL = GameField.GetComponent<GameFieldCTRL>();

        if (LevelsScript.main)
        {
            level = LevelsScript.main.ReturnLevel();
            gameFieldCTRL.inicializeField(level);
        }
        else {
            Destroy(GameField);
        }

        //Обнуление счета игры
        if (Gameplay.main)
        {
            Gameplay.main.StartGameplay(ref Stars);
        }
        else
        {
            Destroy(GameField);
        }
    }

    //Очисть все игровое поле
    void DestroyAllField() {
        GameFieldCTRL[] fields = GameFieldParent.GetComponentsInChildren<GameFieldCTRL>();
        foreach (GameFieldCTRL field in fields) {
            Destroy(field.gameObject);
        }
    }

    //обновление данных интерфейса
    void updateUI() {
        if (Gameplay.main) {
            Level.text = System.Convert.ToString(Gameplay.main.levelSelect);
            updateMoving();
            updateScore();
            updateGoal();
        }
    }

    public void updateGoal()
    {
        int i = 0;
        if (level.PassedWitScore)
        {
            GoalText[i].text = "S " + level.NeedScore.ToString();
            i++;
        }
        if (level.PassedWithCrystal)
        {
            GoalText[i].text = "C " + level.NeedCrystal.ToString();
            i++;
        }
        if (level.PassedWithBox)
        {
            GoalText[i].text = "B " + gameFieldCTRL.CountBoxBlocker.ToString();
            i++;
        }
        if (level.PassedWitMold)
        {
            GoalText[i].text = "M " + gameFieldCTRL.CountMold.ToString();
            i++;
        }
        if (level.PassedWitPanel)
        {
            GoalText[i].text = "P " + gameFieldCTRL.CountPanelSpread.ToString();
            i++;
        }

        for (int j = 0; j < Goal.Length; j++)
        {
            Goal[j].SetActive(false);
        }
        for (int j = 0; j < i; j++)
        {
            Goal[j].SetActive(true);
        }
    }

    public void updateScore()
    {
        Score.text = System.Convert.ToString(Gameplay.main.score);
        ScoreSlider.value = (float)Gameplay.main.score / (level.NeedScore * Gameplay.main.threeStartFactor);
        Gameplay.main.CountStars(ref Stars);
    }
    public void updateMoving()
    {
        Move.text = System.Convert.ToString(Gameplay.main.movingCan);
    }

    /// <summary>
    /// Открыть настройки меню
    /// </summary>
    public void clickButtoSettings()
    {
        GlobalMessage.Settings();
    }

    /// <summary>
    /// Открыть меню выхода из игры
    /// </summary>
    public void clickButtonExitLevel()
    {
        GlobalMessage.ExitLevel();
    }






    /////////////////////////////////////////////////////////////////////////////////////////////////
    ///Кнопки платных ударов
    public enum SuperHitType {
        none,
        internalObj,
        rosket2x,
        superColor
    }

    public SuperHitType SuperHitSelected = SuperHitType.none;

    public void ButtonClickDestroyInternal() {
        SuperHitSelected = SuperHitType.internalObj;
    }
    public void ButtonClickRosket() {
        SuperHitSelected = SuperHitType.rosket2x;
    }
    public void ButtonClickSuperColor() {
        SuperHitSelected = SuperHitType.superColor;
    }
    //Конец зоны кнопок платных ударов
    ///////////////////////////////////////////////////////////////////////////////////////////////////
}
