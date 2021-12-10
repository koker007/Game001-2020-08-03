using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//—емен
//ѕреп€тствует перемещению через
public class WallController : MonoBehaviour
{
    [SerializeField]
    GameObject Up;
    [SerializeField]
    GameObject Down;
    [SerializeField]
    GameObject Left;
    [SerializeField]
    GameObject Right;

    CellCTRL MyCell;
    
    public void IniWall(CellCTRL parentCell) {
        MyCell = parentCell;
        MyCell.wallController = this;

        GetComponent<RectTransform>().pivot = new Vector2(-MyCell.pos.x, -MyCell.pos.y);

        SetID(MyCell.wallID);
    }

    void SetID(int wallID)
    {
        //¬ерх
        if (wallID == 1) {
            SetWall(true, false, false, false);
        }
        //право
        else if (wallID == 2)
        {
            SetWall(false, true, false, false);
        }
        //низ
        else if (wallID == 3)
        {
            SetWall(false, false, true , false);
        }
        //лево
        else if (wallID == 4)
        {
            SetWall(false, false, false, true);
        }

        ////////////////////////////////////////////
        //верх-право
        else if (wallID == 5)
        {
            SetWall(true, true, false, false);
        }
        //низ-право
        else if (wallID == 6)
        {
            SetWall(false, true, true, false);
        }
        //низ-лево
        else if (wallID == 7)
        {
            SetWall(false, false, true, true);
        }
        //верх-лево
        else if (wallID == 8)
        {
            SetWall(true, false, false, true);
        }

        /////////////////////////////////////////
        //верх-низ
        else if (wallID == 9)
        {
            SetWall(true, false, true, false);
        }
        //право-лево
        else if (wallID == 10)
        {
            SetWall(false, true, false, true);
        }

        ///////////////////////////////////////////
        //дырка сверху
        else if (wallID == 11)
        {
            SetWall(false, true, true, true);
        }
        //дырка справа
        else if (wallID == 12)
        {
            SetWall(true, false, true, true);
        }
        //дырка снизу
        else if (wallID == 13)
        {
            SetWall(true, true, false, true);
        }
        //дырка слева
        else if (wallID == 14)
        {
            SetWall(true, true, true, false);
        }
        //везде стены
        else if (wallID == 15)
        {
            SetWall(true, true, true, true);
        }

    }

    void SetWall(bool up, bool right, bool down, bool left) {
        if (up) Up.SetActive(true);
        else Up.SetActive(false);

        if (down) Down.SetActive(true);
        else Down.SetActive(false);

        if (left) Left.SetActive(true);
        else Left.SetActive(false);

        if (right) Right.SetActive(true);
        else Right.SetActive(false);
    }


    public bool isOpenUP() {
        bool result = true;

        if (MyCell.wallID == 1 ||
            MyCell.wallID == 5 ||
            MyCell.wallID == 8 ||
            MyCell.wallID == 9 ||
            MyCell.wallID == 12 ||
            MyCell.wallID == 13 ||
            MyCell.wallID == 14 ||
            MyCell.wallID == 15) 
            result = false;

        return result;
    }
    public bool isOpenDown() {
        bool result = true;

        if (MyCell.wallID == 3 ||
            MyCell.wallID == 6 ||
            MyCell.wallID == 7 ||
            MyCell.wallID == 9 ||
            MyCell.wallID == 11 ||
            MyCell.wallID == 12 ||
            MyCell.wallID == 14 ||
            MyCell.wallID == 15)
            result = false;

        return result;
    }

}
