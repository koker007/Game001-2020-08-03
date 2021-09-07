using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockCTRL : MonoBehaviour
{

    CellCTRL myCell;
    RectTransform myRect;

    [SerializeField]
    RawImage image;

    bool isInicialize = false;
    /// <summary>
    /// Инициализировать камень
    /// </summary>
    public void inicialize(CellCTRL cellIni)
    {
        if (isInicialize) return;

        myCell = cellIni;

        //Добавляем в список эту плесень
        myCell.myField.rockCTRLs[cellIni.pos.x, cellIni.pos.y] = this;
        IniRect();

        isInicialize = true;
    }

    void IniRect()
    {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-myCell.pos.x, -myCell.pos.y);
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
