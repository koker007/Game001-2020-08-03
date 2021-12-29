using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Андрей
//Семен
public class TeleportController : MonoBehaviour
{
    public Image image;
    public RectTransform rectMigleImage;

    public GameFieldCTRL myField;

    public CellCTRL cellIn; 
    public CellCTRL cellOut;
    public TeleportController secondTeleport;

    public Image ImageUP;
    public Image imageDown;

    //ID текущего телепорта
    int ID = 0;

    [SerializeField]
    Color[] colors;

    private void Update()
    {
        
        Teleport();
        UpdateImage();
    }

    private void Teleport()
    {
        if (myField == null) Destroy(gameObject);

        //Расчет только на расчетном кадре
        if (myField.buffer.CalculateFrameNow != 0) {
            return;
        }

        //Если во входе есть что перемещать и объект готов к перемещению
        if (cellIn != null &&
            cellIn.cellInternal != null && //Есть что перемещать
            cellIn.rock == 0 && //объект не заперт
            !cellIn.cellInternal.isMove //Объект бездействует
            )
        {
            //Если выход второго телепорта свободен и доступен, инициируем телепортацию
            if (secondTeleport.cellOut != null && //точка выхода существует
                secondTeleport.cellOut.BlockingMove == 0 && //Движение на выходе не заблокировано
                !secondTeleport.cellOut.cellInternal && //Место выхода не занято
                secondTeleport.cellOut.rock == 0 //на месте выхода нет камня
                                                 ) 
            {
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
            dublicate.myDeath = new Vector2Int(cellIn.pos.x, cellIn.pos.y - 1) * -1;
            //dublicate.myDeath = cellOut.pos * -1;
            //По завершении движения уничтожиться
            dublicate.EndMoveAndRemove = true;
            dublicate.isMove = true;
            //Забыть ячейку в которой существует
            dublicate.myCell = null;

            //Делаем скорость дупликату как у оригинала
            dublicate.MovingSpeed = cellIn.cellInternal.MovingSpeed;

            Vector2 pos = new Vector2();
            if (secondTeleport.cellIn)
            {
                pos = secondTeleport.cellIn.pos;
            }
            else if (secondTeleport.cellOut)
            {
                pos = new Vector2(secondTeleport.cellOut.pos.x, secondTeleport.cellOut.pos.y + 1);
            }
            else
            {
                Debug.Log("Teleport not have in and out");
            }

            //Оригинал перемещаем на позицию над точкой выхода
            cellIn.cellInternal.rectMy.pivot = pos * -1;
            //Скрываем объект сверху
            cellIn.cellInternal.SetFullMask2D(CellInternalObject.MaskType.top);
            cellIn.cellInternal.myMaskNeed = new Vector4();
            //Говорим внутренности двигаться в позицию снизу
            cellIn.cellInternal.StartMove(secondTeleport.cellOut);


            //Поджигаем изображения верхние и нижние свечение
            ImageUPFull();
            secondTeleport.ImageDownFull();
        }
    }

    //Установить id телепорта и поставить его цвет
    public void setIDAndColor(int IDnew) {
        ID = IDnew;

        image.color = colors[ID];
    }

    
    //Поставить показ верхнего изображения на полную
    void ImageUPFull() {
        Color colorUP = ImageUP.color;
        colorUP.a = 1;
        ImageUP.color = colorUP;
    }

    //поставить показ нижнего изображения на полную
    void ImageDownFull() {
        Color colorDown = imageDown.color;
        colorDown.a = 1;
        imageDown.color = colorDown;
    }

    void UpdateImage() {
        //Крутим портал
        Quaternion rectMidleImage = rectMigleImage.localRotation;
        Vector3 euler = rectMigleImage.eulerAngles;

        euler.z += 90 * Time.unscaledDeltaTime;

        rectMidleImage.eulerAngles = euler;
        rectMigleImage.localRotation = rectMidleImage;

        if (ImageUP.color.a > 0) {
            Color colorUP = ImageUP.color;
            colorUP.a -= Time.unscaledDeltaTime;
            ImageUP.color = colorUP;
        }

        if (imageDown.color.a > 0) {
            Color colorDown = imageDown.color;
            colorDown.a -= Time.unscaledDeltaTime;
            imageDown.color = colorDown;
        }
        
    }

}
