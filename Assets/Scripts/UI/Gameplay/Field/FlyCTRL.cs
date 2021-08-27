using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Контролирует полет объекта, например иньекции
/// </summary>
public class FlyCTRL : MonoBehaviour
{
    [SerializeField]
    Image image;

    Vector2 PivotTarget;

    float SpeedRotate = 0;
    float SpeedMove = 0;

    RectTransform myRect;
    Vector2 PivotStart;

    // Start is called before the first frame update
    void Start()
    {
        inicialize();
    }

    void inicialize() {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = PivotStart;
    }

    // Update is called once per frame
    void Update()
    {
        CalcTransform();
    }

    void CalcTransform() {

        Rotate();
        Move();

        void Rotate() {
            //ищем какой угл нужен чтобы повернуться к цели
            float angleTarget = 0;

            //Направление к цели относительно объекта
            Vector2 vectorTarget = PivotTarget - myRect.pivot;
            Vector2 vectorTargetNormalized = vectorTarget.normalized;

            //Ищем арккосинус
            //float radianSin = Mathf.Asin(vectorTargetNormalized.x);
            //На основе арк косинуса находим угл на который наобходимо повернуть
            float radianCos = Mathf.Acos(vectorTargetNormalized.y);
            //на основе того отрицательный или положительный X узнаем положительный или отрицательный поворот

            //Нашли угол в радианах
            //Нашли вектор до цели
            //Узнаем какой это угл поворота
            



        }
        void Move() {
        
        }
    }
}
