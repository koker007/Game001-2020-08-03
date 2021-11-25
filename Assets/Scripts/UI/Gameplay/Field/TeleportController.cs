using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public CellCTRL teleportIn;
    public CellCTRL teleportOut;
    public TeleportController secondTeleport;

    private void Update()
    {
        Teleport();
    }

    private void Teleport()
    {
        if (teleportIn.cellInternal != null)
        {
            //���� ����� ������� ��������� �������� � ��������, ������������� ������� �� ����� ����
            if (secondTeleport.teleportOut != null &&
                teleportIn.rock == 0 &&
                secondTeleport.teleportOut.BlockingMove == 0 &&
                !secondTeleport.teleportOut.cellInternal &&
                secondTeleport.teleportOut.rock == 0)
            {                
                //teleportIn.cellInternal.GetComponent<RectTransform>().pivot = secondTeleport.teleportOut.GetComponent<RectTransform>().pivot;
                teleportIn.cellInternal.StartMove(secondTeleport.teleportOut);
            }
        }
    }
}
