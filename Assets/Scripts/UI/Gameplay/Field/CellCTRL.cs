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
public class CellCTRL : MonoBehaviour, IPointerDownHandler
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

    [SerializeField]
    Image[] ramka;

    public Vector2Int pos = new Vector2Int();
    /// <summary>
    /// ������������ ������
    /// </summary>
    public CellInternalObject cellInternal;
    /// <summary>
    /// ��������� �� ������������ ������
    /// </summary>
    public bool movingInternalNow;

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
        movingInternalNow = true;
        myInternalNum = lastInternalNum;
        lastInternalNum++;
    }

    /// <summary>
    /// �������� ���� � ���������� �� ������������
    /// </summary>
    public void gettingScore() {

        //�����������
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

    //�������� ���� ���������� ������ �� ��������� �� ��� ������, �� �������� ��� ���
    void TestInternal() {
        if (cellInternal && cellInternal.myCell != this) {
            cellInternal = null;
            movingInternalNow = false;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //���� ���� �� ������
        MouseCTRL.main.click();

        //���� ������ ���� ������ � �������� ���
        if (cellInternal && !movingInternalNow) {

            //���� ��������� ������� ����
            if (MouseCTRL.main.ClickDouble)
            {
                cellInternal.ActivateObj();
            }

            //��������� ����
            else {
                myField.SetSelectCell(this);
            }
        }
    }
}
