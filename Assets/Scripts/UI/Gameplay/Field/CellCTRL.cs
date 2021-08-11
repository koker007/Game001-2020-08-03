using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Ячейка и ее состояния
/// </summary>
public class CellCTRL : MonoBehaviour
{
    static int lastInternarNum = 0;

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
        myInternalNum = lastInternarNum;
        lastInternarNum++;
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
}
