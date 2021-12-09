using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������
//�����
public class TeleportController : MonoBehaviour
{
    public Image image;

    public CellCTRL cellIn; 
    public CellCTRL cellOut;
    public TeleportController secondTeleport;

    private void Update()
    {
        Teleport();
    }

    private void Teleport()
    {
        //���� �� ����� ���� ��� ���������� � ������ ����� � �����������
        if (cellIn.cellInternal != null &&
            cellIn.rock == 0)
        {
            //���� ����� ������� ��������� �������� � ��������, ���������� ������������
            if (secondTeleport.cellOut != null &&
                secondTeleport.cellOut.BlockingMove == 0 &&
                !secondTeleport.cellOut.cellInternal &&
                secondTeleport.cellOut.rock == 0)
            {
                //teleportIn.cellInternal.GetComponent<RectTransform>().pivot = secondTeleport.teleportOut.GetComponent<RectTransform>().pivot;
                //cellIn.cellInternal.StartMove(secondTeleport.cellOut);
                tele();
            }
        }

        void tele() {
            //������� ���������� ������ ��������
            GameObject dublicateObj = Instantiate(cellIn.myField.prefabInternal, cellIn.myField.parentOfInternals);
            CellInternalObject dublicate = dublicateObj.GetComponent<CellInternalObject>();

            //�������� ������ ��������� ��������� ���������
            dublicate.setColorAndType(cellIn.cellInternal.color, cellIn.cellInternal.type);

            //�������� ���������� �� ����� ���������
            dublicate.rectMy.pivot = cellIn.cellInternal.rectMy.pivot;
            //������� ������ ���������� ����� �� ���� ����������� ����
            dublicate.myMaskNeed.y = 100;

            //������� ��������� ��������� �� ������� ���������������
            dublicate.myDeath = cellOut.pos * -1;
            //�� ���������� �������� ������������
            dublicate.EndMoveAndRemove = true;
            dublicate.isMove = true;
            //������ ������ � ������� ����������
            dublicate.myCell = null;

            //������ �������� ��������� ��� � ���������
            dublicate.MovingSpeed = cellIn.cellInternal.MovingSpeed;

            //�������� ���������� �� ������� ��� ������ ������
            cellIn.cellInternal.rectMy.pivot = secondTeleport.cellIn.pos * -1;
            //�������� ������ ������
            cellIn.cellInternal.SetFullMask2D(CellInternalObject.MaskType.top);    
            //������� ������������ ��������� � ������� �����
            cellIn.cellInternal.StartMove(secondTeleport.cellOut);

        }
    }
}
