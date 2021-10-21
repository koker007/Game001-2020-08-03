using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен

/// <summary>
/// Управляем поведением распространяющейся панели
/// </summary>
public class PanelSpreadCTRL : MonoBehaviour
{
    static float offsetY = 0;
    static float offsetYTimeOld = 0;

    CellCTRL myCell;
    [SerializeField]
    RawImage image;

    RectTransform myRect;

    public void inicialize(CellCTRL cell) {
        myCell = cell;

        myCell.myField.CountPanelSpread++;

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
        UpdateOffSet();
    }

    //Если панель по какой-то причине будет удалена, уменьшаем счетчик
    ~PanelSpreadCTRL() {
        myCell.myField.CountPanelSpread--;
    }

    static public void TestOffSet() {
        if (offsetYTimeOld != Time.unscaledTime) {
            offsetYTimeOld = Time.unscaledTime;

            offsetY += Time.unscaledDeltaTime * 0.1f;
            
        }
    }

    void UpdateOffSet() {
        image.uvRect = new Rect(0, offsetY, image.uvRect.width, image.uvRect.height);
    }
}
