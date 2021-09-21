using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����

/// <summary>
/// ������������ ������ �������
/// </summary>
public class MoldCTRL : MonoBehaviour
{

    [SerializeField]
    AnimatorCTRL animator;

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
        myCell.myField.CountMold = myCell.myField.moldCTRLs.Count;
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

            animator.PlayAnimation("Destroy");

        }
    }

    void Destroy()
    {
        myCell.myField.ReCalcMoldList();
        myCell.moldCTRL = null;
        Destroy(gameObject);
    }

    //������� ������� � ��������� ������
    public bool TestSpawn() {

        bool isSpawned = false;
        
        //3 �������� ������ ���� ��������� ������ �������� ������
        CellCTRL cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);

        //���� ������ ���, ��� ������� ��� ����
        if (cellTarget == null || cellTarget.mold > 0 || cellTarget.panel) {
            cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
            if (cellTarget == null || cellTarget.mold > 0 || cellTarget.panel) {
                cellTarget = GameFieldCTRL.GetRandomCellNearest(myCell);
            }
        }

        //���� ���� ������ ���, ��� ���� ������ ����������� ��� ������ ��� �� ������ ������
        if (cellTarget == null || cellTarget.mold > 0 || cellTarget.panel)
            return isSpawned;

        //������ �������� ���� ��� ������ 5-�� � ��� ��� ������.
        if(cellTarget.mold < 5 && !cellTarget.panel)
            cellTarget.mold++;

        //���� ������� ������ ���� �� �� ���, ������� �� ��������� ������
        if (cellTarget.mold > 0 && cellTarget.moldCTRL == null) {
            GameObject moldObj = Instantiate(myPrefab, myCell.myField.parentOfMold);
            cellTarget.moldCTRL = moldObj.GetComponent<MoldCTRL>();
            cellTarget.moldCTRL.inicialize(cellTarget);

            //���� ������ �������
            SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipAddMold, 0.5f, Random.Range(0.9f, 1.1f));
        }
        
        //���� ����� �� ����� ������ ���������
        return true;
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
