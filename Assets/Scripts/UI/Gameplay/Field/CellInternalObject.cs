using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Врутренняя перемещаемая часть ячеек
/// </summary>
public class CellInternalObject : MonoBehaviour
{
    //мое поле
    public GameFieldCTRL myField;
    //Моя ячейка
    public CellCTRL myCell;

    RectTransform rectMy;
    RectTransform rectCell;

    [SerializeField]
    RawImage Image;

    [SerializeField]
    Texture2D TextureRed;
    [SerializeField]
    Texture2D TextureGreen;
    [SerializeField]
    Texture2D TextureBlue;
    [SerializeField]
    Texture2D TextureYellow;
    [SerializeField]
    Texture2D textureViolet;

    public enum InternalColor {
        Red,
        Green,
        Blue,
        Yellow,
        Violet
    }
    public enum Type {
        color,
        rocketHorizontal,
        rocketVertical,
        airplane
    }

    public InternalColor color;
    public Type type;

    void GetRect() {
        rectMy = GetComponent<RectTransform>();
        rectCell = myCell.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetRect();
    }

    // Update is called once per frame
    void Update()
    {
        dropped();
        //Moving();
    }

    public bool isDropped = false;
    float DroppedSpeed = 0;
    void dropped() {
        if (isDropped)
        {
            DroppedSpeed += Time.unscaledDeltaTime * 4;

            float posYnew = rectMy.pivot.y + 0.05f + DroppedSpeed;

            //Если позиция ниже чем та куда надо
            if (posYnew > rectCell.pivot.y)
            {
                CellCTRL cellMove = GetFreeCellDown();
                if (cellMove)
                {
                    dropStart(cellMove);
                }
                else
                {
                    posYnew = rectCell.pivot.y;
                    isDropped = false; //падение закончиось
                    myCell.movingInternalNow = false; //Ячейка освободилась для дейсвия
                }
            }

            //Устанавливаем позицию
            rectMy.pivot = new Vector2(rectMy.pivot.x, posYnew);
        }
        //Проверяем снизу наличие свободной ячейки
        else {
            CellCTRL cellMove = GetFreeCellDown();
            if (cellMove)
                dropStart(cellMove);
        }
        
    }

    //Получить свободную ячейку снизу
    CellCTRL GetFreeCellDown() {
        CellCTRL returnCell = null;
        for (int minusY = 1; minusY < myField.cellCTRLs.GetLength(1); minusY++) {
            if (myCell.pos.y - minusY >= 0 && //если не вышли за массив
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY] && //Если есть ячейка
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].cellInternal && //И она свободна
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].movingInternalNow && //Туда никто не движется
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].dontMoving == 0 //И можно двигаться
                                                                                  )
            {
                //Ставим такую ячейку как нижнюю
                returnCell = myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY];
            }
            //Дальше таких ячеек нет
            else { 
                //Выходим
            }
        }
        return returnCell;
    }

    public void dropStart(CellCTRL cellNew) {
        if (myCell)
        {
            myCell.movingInternalNow = false;
            myCell.cellInternal = null;
        }

        myCell = cellNew;

        if (!isDropped)
        {
            DroppedSpeed = 0;
            isDropped = true;
        }
        //говорим текущей ячейке что к ней происходит движение
        myCell.movingInternalNow = true;
        myCell.cellInternal = this;

        GetRect();
    }

    /// <summary>
    /// Удалить объект
    /// </summary>
    public void DestroyObj() {
        Destroy(gameObject);
    }

    public void randColor() {
        color = GetRandomColor();
        if (type == Type.color) {
            if (color == InternalColor.Red) {
                Image.texture = TextureRed;
            }
            else if (color == InternalColor.Green) {
                Image.texture = TextureGreen;
            }
            else if (color == InternalColor.Blue)
            {
                Image.texture = TextureBlue;
            }
            else if (color == InternalColor.Yellow)
            {
                Image.texture = TextureYellow;
            }
            else if (color == InternalColor.Violet)
            {
                Image.texture = textureViolet;
            }
        }
    }

    InternalColor GetRandomColor()
    {
        InternalColor colorReturn = InternalColor.Red;

        int random = Random.Range(0, 3);
        if (random == 0)
        {
            colorReturn = InternalColor.Red;
        }
        else if (random == 1)
        {
            colorReturn = InternalColor.Green;
        }
        else if (random == 2)
        {
            colorReturn = InternalColor.Blue;
        }
        else if (random == 3)
        {
            colorReturn = InternalColor.Yellow;
        }
        else if (random == 4)
        {
            colorReturn = InternalColor.Violet;
        }

        return colorReturn;
    }

}
