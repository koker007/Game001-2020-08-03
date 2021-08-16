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

    public static GameObject GameField;

    // Start is called before the first frame update
    void Start()
    {
        startButtons();
    }

    // Update is called once per frame
    void Update()
    {
        updateButtons();
        updateUI();
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
        CreateGameField();
    }

    //Создать игровое поле согласно параметрам игры
    void CreateGameField() {
        //Создание игрового поля
        GameField = Instantiate(GameFieldPrefab, GameFieldParent);
        GameFieldCTRL gameFieldCTRL = GameField.GetComponent<GameFieldCTRL>();
        gameFieldCTRL.inicializeField(10,10);

        //Обнуление счета игры
        Gameplay.main.StartGameplay();
    }

    //Проверка данных интерфейса
    void updateUI() {

        Level.text = System.Convert.ToString(Gameplay.main.levelSelect);
        Move.text = System.Convert.ToString(Gameplay.main.movingCan);
        Score.text = System.Convert.ToString(Gameplay.main.score);
        
        
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
}
