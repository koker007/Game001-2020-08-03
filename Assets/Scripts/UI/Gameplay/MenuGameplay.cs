using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    // Start is called before the first frame update
    void Start()
    {
        startButtons();
    }

    // Update is called once per frame
    void Update()
    {
        updateButtons();
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
            PanelDown.pivot = new Vector2(PanelDown.pivot.x, downY);

            float upY = PanelUp.pivot.y;
            upY += (1 - upY) * Time.unscaledDeltaTime * 4;
            PanelUp.pivot = new Vector2(PanelUp.pivot.x, upY);
        }
    }
}
