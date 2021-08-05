using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен
/// <summary>
/// Отвечает за карту
/// </summary>
public class PanelMap : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    RectTransform Up;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateButtons();
    }

    void OnEnable()
    {
        startButtons();
    }
    void updateButtons()
    {
        moving();

        void moving()
        {
            float upY = Up.pivot.y;

            upY += (1 - upY) * Time.unscaledDeltaTime * 4;

            Up.pivot = new Vector2(Up.pivot.x, upY);
        }
    }

    //Поставить панели в стартовое положение
    void startButtons()
    {
        Up.pivot = new Vector2(Up.pivot.x, -3);
    }
}
