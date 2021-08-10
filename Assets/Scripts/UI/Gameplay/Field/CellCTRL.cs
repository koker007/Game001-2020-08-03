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
    


    public void setInternal(CellInternalObject internalObjectNew) {
        cellInternal = internalObjectNew;
        movingInternalNow = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
