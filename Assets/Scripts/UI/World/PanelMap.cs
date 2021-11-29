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

    [SerializeField]
    Animator SpeedRotate;



    void Update()
    {
        updateButtons();

        UpdateText();

        updateRotate();
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

    //Градус изменения вращений
    float angleTimeRot = 0;
    //Время последней проверки градуса
    float angleTimeLastUpdate = 0;
    //Градус в предыдущем кадре
    float oldTimeRot = 0;
    void updateRotate() {
        //Если время с последнего кадра прошло очень много
        if (Time.unscaledTime - angleTimeLastUpdate > 10) {
            //обнуляем данные
            oldTimeRot = 0;
            SpeedRotate.SetBool("NeedOpen", false);
        }

        //Запоминаем время текущей проверки
        angleTimeLastUpdate = Time.unscaledTime;

        //прибавляем к углу
        if (oldTimeRot != 0) {
            //Узнаем разницу между текущим вращением и предыдущим
            float raznica = Mathf.Abs(WorldGenerateScene.main.rotationNow - oldTimeRot);
            angleTimeRot += raznica;
            angleTimeRot -= Time.unscaledDeltaTime;
        }

        //Раскрываем если еще не раскрыто и нужно раскрыть
        if (!SpeedRotate.GetBool("NeedOpen") && angleTimeRot > 30 && PlayerProfile.main.ProfilelevelOpen > 20) {
            SpeedRotate.SetBool("NeedOpen", true);
        }

        



        oldTimeRot = WorldGenerateScene.main.rotationNow;
    }
}
