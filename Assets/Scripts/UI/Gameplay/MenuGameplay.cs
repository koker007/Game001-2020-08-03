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
    Transform GameFieldParent;

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

    public static GameObject GameField;

    private void Awake()
    {
        main = this;
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
    }

    void startButtons()
    {
        PanelDown.pivot = new Vector2(PanelDown.pivot.x, -3);
        PanelUp.pivot = new Vector2(PanelUp.pivot.x, 3);
    }

    void updateButtons()
    {
        moving();

        void moving()
        {
            float downY = PanelDown.pivot.y;
            downY += (0 - downY) * Time.unscaledDeltaTime * 4;
            PanelDown.pivot = new Vector2(PanelDown.pivot.x, downY);

            float upY = PanelUp.pivot.y;
            upY += (1 - upY) * Time.unscaledDeltaTime * 4;
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

    //������� ������� ���� �������� ���������� ����
    void CreateGameField() {
        //�������� �������� ����
        GameField = Instantiate(GameFieldPrefab, GameFieldParent);
        GameFieldCTRL gameFieldCTRL = GameField.GetComponent<GameFieldCTRL>();

        if (LevelsScript.main)
        {
            gameFieldCTRL.inicializeField(LevelsScript.main.ReturnLevel());
        }
        else {
            Destroy(GameField);
        }

        //��������� ����� ����
        if (Gameplay.main)
        {
            Gameplay.main.StartGameplay();
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
    void updateUI() {
        if (Gameplay.main) {
            Level.text = System.Convert.ToString(Gameplay.main.levelSelect);
            updateMoving();
            updateScore();
        }
    }

    public void updateScore()
    {
        Score.text = System.Convert.ToString(Gameplay.main.score);
        ScoreSlider.value = (float)Gameplay.main.score / LevelsScript.main.ReturnLevel().NeedScore;
        int star = Gameplay.main.CountStars();
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
}
