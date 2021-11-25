using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Отвечает за карту
/// </summary>
public class PanelMap : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    RectTransform Up;

    //статистика
    [SerializeField]
    Text Health;
    [SerializeField]
    Text Ticket;
    [SerializeField]
    Text Gold;

    void Update()
    {
        updateButtons();

        UpdateText();
    }

    void UpdateText() {
        Health.text = System.Convert.ToString(PlayerProfile.main.Health.Amount);
        Ticket.text = System.Convert.ToString(PlayerProfile.main.Ticket.Amount);
        Gold.text = System.Convert.ToString(PlayerProfile.main.GoldAmount);
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
