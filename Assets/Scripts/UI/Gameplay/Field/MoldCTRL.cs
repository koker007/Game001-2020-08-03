using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            Destroy(gameObject);
        }
    }

    //Спавним плесень в ближайщих точках
    public void TestSpawn() {
        
        //3 проверки подряд если ближайшая ячейка выдается пустой
        CellCTRL cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
        if (cellTarget == null || cellTarget.mold > 0) {
            cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
            if (cellTarget == null || cellTarget.mold > 0) {
                cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
            }
        }

        //Если этой ячейки нет, или если ячейка оказывается уже занята
        if (cellTarget == null || cellTarget.mold > 0)
            return;

        //Делаем здоровье если оно меньше 5-ти
        if(cellTarget.mold < 5)
            cellTarget.mold++;

        //Спавним на выбранной ячейке если еще нету
        if (cellTarget.moldCTRL == null) {
            GameObject moldObj = Instantiate(myPrefab, myCell.myField.parentOfMold);
            cellTarget.moldCTRL = moldObj.GetComponent<MoldCTRL>();
            cellTarget.moldCTRL.inicialize(cellTarget);
        }
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
