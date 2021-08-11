using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

//Семен
/// <summary>
/// Ячейка и ее состояния
/// </summary>
public class CellCTRL : MonoBehaviour, IPointerDownHandler
{
    static int lastInternalNum = 0; //нужно чтобы ячека знала насколько давно в ней происходило какое либо действие
    public int LastInternalNum {
        get {
            int num = lastInternalNum;
            lastInternalNum++;

            return num;
        }
    }

    public GameFieldCTRL myField;

    [SerializeField]
    Image[] ramka;

    public Vector2Int pos = new Vector2Int();
    /// <summary>
    /// Внутренность ячейки
    /// </summary>
    public CellInternalObject cellInternal;
    /// <summary>
    /// Двигаются ли внутренности сейчас
    /// </summary>
    public bool movingInternalNow;

    /// <summary>
    /// Степень запрета на перемещение объекта
    /// </summary>
    public int dontMoving;
    /// <summary>
    /// Степень гели
    /// </summary>
    public int gel;
    
    public int myInternalNum = 0;

    public void setInternal(CellInternalObject internalObjectNew) {
        cellInternal = internalObjectNew;
        movingInternalNow = true;
        myInternalNum = lastInternalNum;
        lastInternalNum++;
    }

    /// <summary>
    /// получить очки и избавиться от внутренности
    /// </summary>
    public void gettingScore() {

        //Избавляемся
        cellInternal.DestroyObj();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TestInternal();
    }

    //Проверка если внутренний обьект не ссылается на эту ячейку, мы забываем про нее
    void TestInternal() {
        if (cellInternal && cellInternal.myCell != this) {
            cellInternal = null;
            movingInternalNow = false;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //Если клик по ячейке
        MouseCTRL.main.click();

        //Если внутри есть объект и движения нет
        if (cellInternal && !movingInternalNow) {

            //если произошел двойной клик
            if (MouseCTRL.main.ClickDouble)
            {
                cellInternal.ActivateObj();
            }

            //Одинарный клик
            else {
                myField.SetSelectCell(this);
            }
        }
    }
}
