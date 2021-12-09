using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Андрей
//Семен
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
        //Если во входе есть что перемещать и объект готов к перемещению
        if (cellIn.cellInternal != null &&
            cellIn.rock == 0)
        {
            //Если выход второго телепорта свободен и доступен, инициируем телепортацию
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
            //Создаем внутренний объект дубликат
            GameObject dublicateObj = Instantiate(cellIn.myField.prefabInternal, cellIn.myField.parentOfInternals);
            CellInternalObject dublicate = dublicateObj.GetComponent<CellInternalObject>();

            //Дубликат должен выглядеть идентично оригиналу
            dublicate.setColorAndType(cellIn.cellInternal.color, cellIn.cellInternal.type);

            //Дубликат перемещаем на место оригинала
            dublicate.rectMy.pivot = cellIn.cellInternal.rectMy.pivot;
            //говорим ячейке скрываться снизу по мере продвижения вниз
            dublicate.myMaskNeed.y = 100;

            //Говорим дубликату двигаться на позицию самоуничтожения
            dublicate.myDeath = cellOut.pos * -1;
            //По завершении движения уничтожиться
            dublicate.EndMoveAndRemove = true;
            dublicate.isMove = true;
            //Забыть ячейку в которой существует
            dublicate.myCell = null;

            //Делаем скорость дупликату как у оригинала
            dublicate.MovingSpeed = cellIn.cellInternal.MovingSpeed;

            //Оригинал перемещаем на позицию над точкой выхода
            cellIn.cellInternal.rectMy.pivot = secondTeleport.cellIn.pos * -1;
            //Скрываем объект сверху
            cellIn.cellInternal.SetFullMask2D(CellInternalObject.MaskType.top);    
            //Говорим внутренности двигаться в позицию снизу
            cellIn.cellInternal.StartMove(secondTeleport.cellOut);

        }
    }
}
