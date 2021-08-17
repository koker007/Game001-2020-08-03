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
public class CellCTRL : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
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

    static CellCTRL CellClickOld;
    static CellCTRL CellEnterOld;

    [SerializeField]
    Image[] ramka;

    public Vector2Int pos = new Vector2Int();
    /// <summary>
    /// Внутренность ячейки
    /// </summary>
    public CellInternalObject cellInternal;

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
        cellInternal.isMove = true;
        myInternalNum = lastInternalNum;
        lastInternalNum++;
    }

    /// <summary>
    /// получить очки и избавиться от внутренности
    /// </summary>
    public void Damage()
    {
        Damage(cellInternal);
    }
    public void Damage(CellInternalObject partner)
    {

        if (cellInternal)
        {

            cellInternal.Activate(cellInternal.type, partner);
            //Избавляемся
            cellInternal.DestroyObj();
        }
    }

    public void DamageInvoke(float time) {
        Invoke("Damage", time);
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
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //Если клик по ячейке
        MouseCTRL.main.click();

        //Если внутри есть объект и движения нет
        if (cellInternal && !cellInternal.isMove) {

            //если произошел двойной клик и клик по тойже ячейке
            if (MouseCTRL.main.ClickDouble && CellClickOld == this)
            {
                //cellInternal.Activate();
            }

            //Одинарный клик
            else {
                myField.SetSelectCell(this);
            }

            CellClickOld = this;
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {

        if (CellClickOld != this && CellEnterOld != this) {
            myField.SetSelectCell(this);
        }
        CellEnterOld = this;
    }
    public void OnPointerUp( PointerEventData eventData)
    {

    }

}
