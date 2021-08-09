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

    [SerializeField]
    Image[] ramka;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
