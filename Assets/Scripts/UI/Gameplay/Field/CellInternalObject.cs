using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен
/// <summary>
/// Врутренняя перемещаемая часть ячеек
/// </summary>
public class CellInternalObject : MonoBehaviour
{
    //Моя ячейка
    public CellCTRL myCell;

    RectTransform rectMy;
    RectTransform rectCell;

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
    }

    public bool isDropped = false;
    float DroppedSpeed = 0;
    void dropped() {
        if (isDropped) {
            DroppedSpeed += Time.unscaledDeltaTime * 4;

            float posYnew = rectMy.pivot.y + 0.05f + DroppedSpeed;

            //Если позиция ниже чем та куда надо, останавливаемся
            if (posYnew > rectCell.pivot.y) {
                posYnew = rectCell.pivot.y;
                isDropped = false; //падение закончиось
                myCell.movingInternalNow = false; //Ячейка освободилась для дейсвия
            }

            //Устанавливаем позицию
            rectMy.pivot = new Vector2(rectMy.pivot.x, posYnew);
        }
    }
    
}
