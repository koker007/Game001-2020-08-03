using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен / алексадр
/// <summary>
/// Контролирует UI главного меню
/// </summary>
public class MenuWorld : MonoBehaviour
{
    static public MenuWorld main;

    [Header("Panels")]
    public RectTransform Up;
    public RectTransform Down;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        updatePanels();
    }

    void OnEnable()
    {
        startPanels();
    }


    void updatePanels() {
        moving();

        void moving() {
            float upY = Up.pivot.y;
            float downY = Down.pivot.y;

            upY += (1 - upY) * Time.unscaledDeltaTime * 4;
            downY += (0 - downY) * Time.unscaledDeltaTime * 4;

            Up.pivot = new Vector2(Up.pivot.x, upY);
            Down.pivot = new Vector2(Down.pivot.x, downY);
        }
    }

    //Поставить панели в стартовое положение
    void startPanels() {
        Up.pivot = new Vector2(Up.pivot.x, -3);
        Down.pivot = new Vector2(Down.pivot.x, 3);
    }
}
