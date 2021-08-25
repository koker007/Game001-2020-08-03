using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoldCTRL : MonoBehaviour
{
    CellCTRL myCell;
    RectTransform myRect;

    [SerializeField]
    GameObject myPrefab;

    [SerializeField]
    RawImage image;


    bool isInicialize = false;
    public void inicialize(CellCTRL cellIni) {
        if (isInicialize) return;

        myCell = cellIni;

        //��������� � ������ ��� �������
        myCell.myField.moldCTRLs.Add(this);
        IniRect();

        isInicialize = true;
    }

    void IniRect() {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-myCell.pos.x, -myCell.pos.y);
    }


    int HealthOld = -1;
    void UpdateLife() {
        //���� �������� �� ��������
        if (HealthOld == myCell.mold)
            return;

        ChangeImage();

        DestroyMold();
        HealthOld = myCell.mold;

        void ChangeImage() {

        }

        //���������� ���� ����� ���������
        void DestroyMold() {
            if (myCell.mold > 0) return;
            Gameplay.main.MoldUpdate();
            Destroy(gameObject);
        }
    }

    //������� ������� � ��������� ������
    public void TestSpawn() {
        
        //3 �������� ������ ���� ��������� ������ �������� ������
        CellCTRL cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
        if (cellTarget == null || cellTarget.mold > 0) {
            cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
            if (cellTarget == null || cellTarget.mold > 0) {
                cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
            }
        }

        //���� ���� ������ ���, ��� ���� ������ ����������� ��� ������
        if (cellTarget == null || cellTarget.mold > 0)
            return;

        //������ �������� ���� ��� ������ 5-��
        if(cellTarget.mold < 5)
            cellTarget.mold++;

        //������� �� ��������� ������ ���� ��� ����
        if (cellTarget.moldCTRL == null) {
            GameObject moldObj = Instantiate(myPrefab, myCell.myField.parentOfMold);
            cellTarget.moldCTRL = moldObj.GetComponent<MoldCTRL>();
            cellTarget.moldCTRL.inicialize(cellTarget);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLife();
    }
}
