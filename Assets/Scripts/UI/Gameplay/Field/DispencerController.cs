using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������
//�����
//�������� ��������� ��������
public class DispencerController : MonoBehaviour
{
    private float primaryObjectSpawnChance = 10;                    //���� ��������� ��������� ��������
    public Vector2Int MyPosition;                   //������ � �����������
    public CellCTRL targetCell;                                     //������ ��� �����������

    public CellInternalObject.InternalColor primaryObjectColor;     //���� ��������� ��������, ������� �� ������� ������
    public CellInternalObject.Type primaryObjectType;               //��� ��������� ��������, ������� �� ������� ������

    private void Start()
    {
        Invoke("testDestroy", 0);
    }

    float testDestroyTime = 1;
    void testDestroy() {
        testDestroyTime *= 1.5f;
        //Debug.Log("DispenserController Test Destroy: " + testDestroyTime);

        //������� ���� ���� ������� ������ �� ����������
        if (targetCell == null) {
            Destroy(gameObject);
            return;
        }

        //������� ������ �� ������� ���������
        if (targetCell.myField.cellCTRLs[MyPosition.x, MyPosition.y] != null) {
            targetCell.myField.cellCTRLs[MyPosition.x, MyPosition.y].Destroy();
            Destroy(targetCell.myField.cellConturs[MyPosition.x, MyPosition.y]);
        }

        if(this != null)
            Invoke("testDestroy", testDestroyTime * Random.Range(0.1f, 0.5f));
    }

    private void Update()
    {
        SpawnInternal();
    }

    //C���� ����� + �� ������ + �� ������ ==> ������� ������
    private void SpawnInternal()
    {
        //���������, ���� �� ����� ����� ��� ������
        if (targetCell != null && 
            GameFieldCTRL.main.CheckObstaclesToMoveDown(targetCell, targetCell.myField.cellCTRLs[MyPosition.x, MyPosition.y]) && 
            targetCell.cellInternal == null)
        {
            //������� � ��������� �������
            GameObject internalObj = Instantiate(GameFieldCTRL.main.prefabInternal, GameFieldCTRL.main.parentOfInternals);
            CellInternalObject internalController = internalObj.GetComponent<CellInternalObject>();
            //������ ������ ��� ������, ��������� ������
            internalController.SetFullMask2D(CellInternalObject.MaskType.top);

            RectTransform internalRect = internalObj.GetComponent<RectTransform>();
            internalRect.pivot = -MyPosition;

            int currentChance = Random.Range(1, 101);

            //����� ��������� ��������
            if (primaryObjectSpawnChance >= currentChance) //��������� ����
            {            
                internalController.setColorAndType(primaryObjectColor, primaryObjectType);
            }
            //����� ���������� �����
            else
            {
                internalController.setColorAndType(internalController.GetRandomColor(false).color, CellInternalObject.Type.color);
            }
            
            internalController.StartMove(targetCell);
            internalController.myField = GameFieldCTRL.main;
            targetCell.setInternal(internalController);
        }      
    } 
}
