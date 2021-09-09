using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен

/// <summary>
/// Контролирует префаб плесени
/// </summary>
public class MoldCTRL : MonoBehaviour
{
    CellCTRL myCell;
    RectTransform myRect;

    [SerializeField]
    GameObject myPrefab;

    [SerializeField]
    RawImage image;


    bool isInicialize = false;
    public void inicialize(CellCTRL cellIni) {
        if (isInicialize) return;

        myCell = cellIni;

        //Добавляем в список эту плесень
        myCell.myField.moldCTRLs.Add(this);
        myCell.myField.CountMold = myCell.myField.moldCTRLs.Count;
        IniRect();

        isInicialize = true;
    }

    void IniRect() {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-myCell.pos.x, -myCell.pos.y);
    }


    int HealthOld = -1;
    void UpdateLife() {
        //Если здоровье не менялось
        if (HealthOld == myCell.mold)
            return;

        ChangeImage();

        DestroyMold();
        HealthOld = myCell.mold;

        void ChangeImage() {

        }

        //Уничтожить если жизни кончились
        void DestroyMold() {
            if (myCell.mold > 0) return;
            Gameplay.main.MoldUpdate();

            myCell.myField.ReCalcMoldList();

            Destroy(gameObject);

            //Пересоздать список плесени исключая отсутствующиие
        }
    }

    //Спавним плесень в ближайщих точках
    public bool TestSpawn() {

        bool isSpawned = false;
        
        //3 проверки подряд если ближайшая ячейка выдается пустой
        CellCTRL cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);

        //Если ячейки нет, или плесень уже есть
        if (cellTarget == null || cellTarget.mold > 0 || cellTarget.panel) {
            cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
            if (cellTarget == null || cellTarget.mold > 0 || cellTarget.panel) {
                cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
            }
        }

        //Если этой ячейки нет, или если ячейка оказывается уже занята или на ячейке панель
        if (cellTarget == null || cellTarget.mold > 0 || cellTarget.panel)
            return isSpawned;

        //Делаем здоровье если оно меньше 5-ти и там нет панели.
        if(cellTarget.mold < 5 && !cellTarget.panel)
            cellTarget.mold++;

        //Если плесень должна быть но ее нет, Спавним на выбранной ячейке
        if (cellTarget.mold > 0 && cellTarget.moldCTRL == null) {
            GameObject moldObj = Instantiate(myPrefab, myCell.myField.parentOfMold);
            cellTarget.moldCTRL = moldObj.GetComponent<MoldCTRL>();
            cellTarget.moldCTRL.inicialize(cellTarget);
        }
        
        //Если дошли до конца значит выполнили
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLife();
    }
}
