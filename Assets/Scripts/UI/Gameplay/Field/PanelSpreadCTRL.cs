using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSpreadCTRL : MonoBehaviour
{
    CellCTRL myCell;
    [SerializeField]
    RawImage image;

    RectTransform myRect;

    public void inicialize(CellCTRL cell) {
        myCell = cell;

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
