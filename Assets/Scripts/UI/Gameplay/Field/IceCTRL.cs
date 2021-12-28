using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Контролирует ячейку льда
/// </summary>
public class IceCTRL : MonoBehaviour
{

    [SerializeField]
    Image image;

    CellCTRL myCell;
    RectTransform myRect;

    int HealthOld = -1;

    bool isInicialize = false;
    public void inicialize(CellCTRL cellIni)
    {
        if (isInicialize) return;

        myCell = cellIni;

        //Добавляем в список эту плесень
        myCell.myField.iceCTRLs[cellIni.pos.x, cellIni.pos.y] = this;

        IniRect();

        ReCalcIceCount();

        isInicialize = true;
    }

    void IniRect()
    {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-myCell.pos.x, -myCell.pos.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAlpha();
    }
    private void FixedUpdate()
    {
        UpdateLife2();
    }

    /*
    void UpdateLife()
    {
        //Если здоровье не менялось
        if (HealthOld == myCell.ice)
            return;


        //Если получили урон воспроизводим частицы разломов
        if (HealthOld > myCell.ice) {
            Particle3dCTRL.CreateDestroyIce(myCell.myField.transform, myCell);
        }

        HealthOld = myCell.ice;


        if (myCell.ice > 0)
        {
            return;
        }

    }
    */

    float alphaNeed = 1;
    float alphaSpeed = 0.2f;
    bool alphaCalc = false;
    void UpdateLife2() {
        //Если здоровье не менялось
        if (HealthOld == myCell.ice)
            return;

        //Воспроизводим частицы разломов
        if (HealthOld > myCell.ice)
        {
            Particle3dCTRL.CreateDestroyIce(myCell.myField.transform, myCell);
        }

        HealthOld = myCell.ice;

        //Перерасчет новой цели прозрачности
        calcColor();

        //Удаляем если здоровье ниже 0
        if (myCell.ice > 0)
        {
            return;
        }

        destroy();

        void destroy()
        {
            //Звук разбивания
            SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipDestroyRock, 0.5f, Random.Range(0.9f, 1.1f));

            myCell.myField.iceCTRLs[myCell.pos.x, myCell.pos.y] = null;
            ReCalcIceCount();
            Destroy(gameObject);
        }

        void calcColor() {

            alphaCalc = true;

            if (myCell.ice == 1)
            {
                alphaNeed = 0.7f;
            }
            else if (myCell.ice == 2)
            {
                alphaNeed = 0.8f;
            }
            else if (myCell.ice == 3)
            {
                alphaNeed = 0.9f;
            }
            else if (myCell.ice == 4)
            {
                alphaNeed = 0.95f;
            }
            else if (myCell.ice == 5) {
                alphaNeed = 1;
            }
        }
    }

    void UpdateAlpha() {
        if (alphaCalc) {
            //вытаскиваем цвет
            Color color = image.color;

            //проверка надобности вычислений
            if (color.a == alphaNeed) {
                //Достигли цели выходим
                alphaCalc = false;
                return;
            }


            //нужно прибавлять чтобы достигнуть цели
            bool plusNeed = false;
            if (alphaNeed - color.a > 0) {
                plusNeed = true;
            }

            //прибавляем
            if (plusNeed)
            {
                color.a += alphaSpeed * Time.deltaTime;
            }
            //Вычитаем
            else {
                color.a -= alphaSpeed * Time.deltaTime;
            }

            //Проверяем на достижение цели
            bool plusNow = false;
            if (alphaNeed - color.a > 0)
            {
                plusNow = true;
            }

            //Если перескочили
            if (plusNeed != plusNow) {
                color.a = alphaNeed;
                alphaCalc = false;
            }

            image.color = color;
        }
    }

    void ReCalcIceCount()
    {

        int count = 0;

        //Перебираем все игровое поле
        for (int x = 0; x < myCell.myField.iceCTRLs.GetLength(0); x++)
        {
            for (int y = 0; y < myCell.myField.iceCTRLs.GetLength(1); y++)
            {
                if (myCell.myField.iceCTRLs[x, y])
                {
                    count++;
                }
            }
        }

        myCell.myField.CountIce = count;
    }
}
