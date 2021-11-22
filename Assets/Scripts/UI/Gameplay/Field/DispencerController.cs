using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispencerController : MonoBehaviour
{
    private float primaryObjectSpawnChance = 10;                    //���� ��������� ��������� ��������
    public CellCTRL myCell;                                         //������ � �����������
    public CellCTRL targetCell;                                     //������ ��� �����������

    public CellInternalObject.InternalColor primaryObjectColor;     //���� ��������� ��������, ������� �� ������� ������
    public CellInternalObject.Type primaryObjectType;               //��� ��������� ��������, ������� �� ������� ������

    private void Update()
    {
        SpawnInternal();
    }

    //C���� ����� + �� ������ + �� ������ ==> ������� ������
    private void SpawnInternal()
    {
        //���������, ���� �� ����� ����� ��� ������
        if (GameFieldCTRL.main.CheckObstaclesToMoveDown(targetCell, myCell) && targetCell.cellInternal == null)
        {
            //������� � ��������� �������
            GameObject internalObj = Instantiate(GameFieldCTRL.main.prefabInternal, GameFieldCTRL.main.parentOfInternals);
            CellInternalObject internalController = internalObj.GetComponent<CellInternalObject>();
            RectTransform internalRect = internalObj.GetComponent<RectTransform>();
            internalRect.pivot = myCell.GetComponent<RectTransform>().pivot;

            int currentChance = Random.Range(1, 101);

            //����� ��������� ��������
            if (primaryObjectSpawnChance >= currentChance) //��������� ����
            {            
                internalController.setColorAndType(primaryObjectColor, primaryObjectType);

                Debug.Log("SPAWN PRIMARY!");
            }
            //����� ���������� �����
            else
            {
                internalController.setColorAndType(internalController.GetRandomColor(false).color, CellInternalObject.Type.color);

                Debug.Log("SPAWN!");
            }
            
            internalController.StartMove(targetCell);
            internalController.myField = GameFieldCTRL.main;
            targetCell.setInternal(internalController);
        }      
    } 
}
