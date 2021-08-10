using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// ������ � �� ���������
/// </summary>
public class CellCTRL : MonoBehaviour
{
    static int lastInternarNum = 0;

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
        myInternalNum = lastInternarNum;
        lastInternarNum++;
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
}
