using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Андрей
//Семен
//Является спавнером объектов
public class DispencerController : MonoBehaviour
{
    private float primaryObjectSpawnChance = 10;                    //Шанс выпадения основного предмета
    public Vector2Int MyPosition;                   //Клетка с раздатчиком
    public CellCTRL targetCell;                                     //Клетка под раздатчиком

    public CellInternalObject.InternalColor primaryObjectColor;     //Цвет основного предмета, берется из массива уровня
    public CellInternalObject.Type primaryObjectType;               //Тип основного предмета, берется из массива уровня

    private void Start()
    {
        Invoke("testDestroy", 0);
    }

    float testDestroyTime = 1;
    void testDestroy() {
        testDestroyTime *= 1.5f;
        //Debug.Log("DispenserController Test Destroy: " + testDestroyTime);

        //Удаляем себя если целевой ячейки не существует
        if (targetCell == null) {
            Destroy(gameObject);
            return;
        }

        //Удаляем ячейку на которой находимся
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

    //Cнизу пусто + не камень + не стенка ==> спавним объект
    private void SpawnInternal()
    {
        //Проверяем, есть ли снизу место для спавна
        if (targetCell != null && 
            GameFieldCTRL.main.CheckObstaclesToMoveDown(targetCell, targetCell.myField.cellCTRLs[MyPosition.x, MyPosition.y]) && 
            targetCell.cellInternal == null)
        {
            //Создаем и размещаем предмет
            GameObject internalObj = Instantiate(GameFieldCTRL.main.prefabInternal, GameFieldCTRL.main.parentOfInternals);
            CellInternalObject internalController = internalObj.GetComponent<CellInternalObject>();
            //прячем объект под маской, выпадение сверху
            internalController.SetFullMask2D(CellInternalObject.MaskType.top);

            RectTransform internalRect = internalObj.GetComponent<RectTransform>();
            internalRect.pivot = -MyPosition;

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
