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
    


    public void setInternal(CellInternalObject internalObjectNew) {
        cellInternal = internalObjectNew;
        movingInternalNow = true;
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
