using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispencerController : MonoBehaviour
{
    private float primaryObjectSpawnChance = 30;        //���� ��������� ��������� ��������
    public CellCTRL myCell;                        //������ � �����������
    public CellCTRL targetCell;                    //������ ��� �����������

    private void Update()
    {
        SpawnInternal();
    }

    //C���� ����� + �� ������ + �� ������ ==> ������� ������
    private void SpawnInternal()
    {
        if (GameFieldCTRL.main.CheckWallsToMoveDown(targetCell, myCell) && targetCell.cellInternal == null)
        {
            int currentChance = Random.Range(1, 101);
            //if (primaryObjectSpawnChance >= currentChance) //��������� ����
            //{
            //GameObject internalObj = Instantiate(GameFieldCTRL.main.prefabInternal, GameFieldCTRL.main.parentOfInternals);
            Debug.Log("SPAWN!");
            //}
        }      
    } 
}
