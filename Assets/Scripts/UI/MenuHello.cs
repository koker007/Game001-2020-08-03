using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Семен
/// <summary>
/// Контроллер приветственного меню
/// </summary>
public class MenuHello : MonoBehaviour
{
    static public MenuHello main;

    [SerializeField]
    Text[] Version;

    [SerializeField]
    RectTransform PanelDown;

    [SerializeField]
    GameObject options;


    bool isNeedOpenOptions; //Нужно ли открыть или закрыть панель опций
    


    //Время когда в последний раз работало это меню
    float timeLastWork;


    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePanels();
        UpdatePanelOpttion();
    }

    /// <summary>
    /// Начать игру войти в главное меню
    /// </summary>
    public void clickButtonPlay() {
        UICTRL.main.OpenWorld();
    }

    /// <summary>
    /// Сохранить весь игровой прогресс
    /// </summary>
    public void clickButtonSave() {
    
    }

    /// <summary>
    /// Открыть настройки меню
    /// </summary>
    public void clickButtonOpttion()
    {
        isNeedOpenOptions = !isNeedOpenOptions;
    }


    /// <summary>
    /// Отвечает за состояние панели опций
    /// </summary>
    void UpdatePanelOpttion() {
        if (isNeedOpenOptions && !options.activeSelf)
        {
            options.SetActive(true);
        }
        else if(!isNeedOpenOptions && options.activeSelf) {
            options.SetActive(false);
        }
    }

    private void OnEnable()
    {
        OpenPanels();
    }


    void OpenPanels() {
        PanelDown.pivot = new Vector2(PanelDown.pivot.x,2);
    }

    //
    void UpdatePanels() {
        
        moving();

        void moving() {
            float y = PanelDown.pivot.y;

            y += (0 - y) * Time.unscaledDeltaTime * 4;

            PanelDown.pivot = new Vector2(PanelDown.pivot.x, y);
        }
    }
}
