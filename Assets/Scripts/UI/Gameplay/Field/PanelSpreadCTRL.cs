using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����

/// <summary>
/// ��������� ���������� ������������������ ������
/// </summary>
public class PanelSpreadCTRL : MonoBehaviour
{
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
        
    }

    //���� ������ �� �����-�� ������� ����� �������, ��������� �������
    ~PanelSpreadCTRL() {
        myCell.myField.CountPanelSpread--;
    }
}
