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
    AnimatorCTRL animatorCTRL;
        


    //Время когда в последний раз работало это меню
    float timeLastWork;


    // Start is called before the first frame update
    void Start()
    {
        main = this;
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
        GlobalMessage.ComingSoon();
    }

    /// <summary>
    /// Открыть настройки меню
    /// </summary>
    public void clickButtonOpttion()
    {
        GlobalMessage.Settings();
    }

    private void OnEnable()
    {
        OpenPanels();
    }


    void OpenPanels() {
        animatorCTRL.PlayAnimation("restart");
        //PanelDown.pivot = new Vector2(PanelDown.pivot.x,2);
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
