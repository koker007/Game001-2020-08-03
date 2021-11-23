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
        if (teleportIn.cellInternal)
        {
            //Если выход второго телепорта свободен и доступен, телепортируем предмет на входе туда
            if (secondTeleport.teleportOut != null &&
                secondTeleport.teleportOut.BlockingMove == 0 &&
                !secondTeleport.teleportOut.cellInternal &&
                secondTeleport.teleportOut.rock == 0)
            {                
                teleportIn.cellInternal.GetComponent<RectTransform>().pivot = secondTeleport.teleportOut.GetComponent<RectTransform>().pivot;
                teleportIn.cellInternal.StartMove(secondTeleport.teleportOut);
                secondTeleport.teleportOut.setInternal(teleportIn.cellInternal);
            }
        }
    }
}
