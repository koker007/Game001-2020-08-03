using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Андрей
//Семен
//Является спавнером объектов
public class DispencerController : MonoBehaviour
{
    private float primaryObjectSpawnChance = 10;                    //Шанс выпадения основного предмета
    public CellCTRL myCell;                                         //Клетка с раздатчиком
    public CellCTRL targetCell;                                     //Клетка под раздатчиком

    public CellInternalObject.InternalColor primaryObjectColor;     //Цвет основного предмета, берется из массива уровня
    public CellInternalObject.Type primaryObjectType;               //Тип основного предмета, берется из массива уровня

    private void Update()
    {
        SpawnInternal();
    }

    //Cнизу пусто + не камень + не стенка ==> спавним объект
    private void SpawnInternal()
    {
        //Проверяем, есть ли снизу место для спавна
        if (targetCell != null && 
            GameFieldCTRL.main.CheckObstaclesToMoveDown(targetCell, myCell) && 
            targetCell.cellInternal == null)
        {
            //Создаем и размещаем предмет
            GameObject internalObj = Instantiate(GameFieldCTRL.main.prefabInternal, GameFieldCTRL.main.parentOfInternals);
            CellInternalObject internalController = internalObj.GetComponent<CellInternalObject>();
            //прячем объект под маской, выпадение сверху
            internalController.SetFullMask2D(CellInternalObject.MaskType.top);

            RectTransform internalRect = internalObj.GetComponent<RectTransform>();
            internalRect.pivot = myCell.GetComponent<RectTransform>().pivot;

            int currentChance = Random.Range(1, 101);

            //Спавн основного предмета
            if (primaryObjectSpawnChance >= currentChance) //Проверяем шанс
            {            
                internalController.setColorAndType(primaryObjectColor, primaryObjectType);
            }
            //Спавн случайного цвета
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
