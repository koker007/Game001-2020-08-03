using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

//�����
/// <summary>
/// ������ � �� ���������
/// </summary>
public class CellCTRL : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    static int lastInternalNum = 0; //����� ����� ����� ����� ��������� ����� � ��� ����������� ����� ���� ��������
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
    /// ������������ ������
    /// </summary>
    public CellInternalObject cellInternal;

    /// <summary>
    /// ������� ������� �� ����������� �������
    /// </summary>
    public int dontMoving;
    /// <summary>
    /// ������� ����
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
    /// �������� ���� � ���������� �� ������������
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
            //�����������
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

    //�������� ���� ���������� ������ �� ��������� �� ��� ������, �� �������� ��� ���
    void TestInternal() {
        if (cellInternal && cellInternal.myCell != this) {
            cellInternal = null;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //���� ���� �� ������
        MouseCTRL.main.click();

        //���� ������ ���� ������ � �������� ���
        if (cellInternal && !cellInternal.isMove) {

            //���� ��������� ������� ���� � ���� �� ����� ������
            if (MouseCTRL.main.ClickDouble && CellClickOld == this)
            {
                //cellInternal.Activate();
            }

            //��������� ����
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
