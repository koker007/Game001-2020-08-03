using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Андрей
//Семен
//Является спавнером объектов
public class DispencerController : MonoBehaviour
{


    private float primaryObjectSpawnChance = 10;                    //Шанс выпадения основного предмета
    public Vector2Int MyPosition;                                   //Клетка с раздатчиком
    public CellCTRL targetCell;                                     //Клетка под раздатчиком

    public CellInternalObject.InternalColor primaryObjectColor;     //Цвет основного предмета, берется из массива уровня
    public CellInternalObject.Type primaryObjectType;               //Тип основного предмета, берется из массива уровня

    [SerializeField]
    RawImage internalImage;
    [SerializeField]
    AnimatorCTRL animatorCTRL;

    [SerializeField]
    Texture2D internalRed;
    [SerializeField]
    Texture2D internalGreen;
    [SerializeField]
    Texture2D internalBlue;
    [SerializeField]
    Texture2D internalYellow;
    [SerializeField]
    Texture2D internalOrange;
    [SerializeField]
    Texture2D Ultimate;
    [SerializeField]
    Texture2D Corona;
    [SerializeField]
    Texture2D PillHorizont;
    [SerializeField]
    Texture2D PillVertical;
    [SerializeField]
    Texture2D Tablet;
    [SerializeField]
    Texture2D Fly;
    [SerializeField]
    Texture2D Blocker;



    private void Start()
    {
        Invoke("testDestroy", 0);

        iniImage();
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
            CellInternalObject internalController = GameFieldCTRL.main.FindFreeCellInternal();
            //прячем объект под маской, выпадение сверху
            internalController.SetFullMask2D(CellInternalObject.MaskType.top);

            RectTransform internalRect = internalController.GetComponent<RectTransform>();
            internalRect.pivot = -MyPosition;

            int currentChance = Random.Range(1, 101);

            //Если цели уровня выполнены то спавним с высокой вероятностью блокиратор
            if (Gameplay.main.isMissionComplite() && 75 >= currentChance) {
                internalController.setColorAndType(primaryObjectColor, CellInternalObject.Type.blocker);
            }
            //Спавн основного предмета
            else if (primaryObjectType > 0 && primaryObjectSpawnChance >= currentChance) //Проверяем шанс
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

            animatorCTRL.PlayAnimation("rotate");
        }      
    }

    void iniImage() {
        internalImage.gameObject.SetActive(true);
        internalImage.color = new Color(1,1,1,1);

        if (primaryObjectType == CellInternalObject.Type.color)
        {
            if (primaryObjectColor == CellInternalObject.InternalColor.Red) {
                internalImage.texture = internalRed;
            }
            else if (primaryObjectColor == CellInternalObject.InternalColor.Green) {
                internalImage.texture = internalGreen;
            }
            else if (primaryObjectColor == CellInternalObject.InternalColor.Blue) {
                internalImage.texture = internalBlue;
            }
            else if (primaryObjectColor == CellInternalObject.InternalColor.Yellow) {
                internalImage.texture = internalYellow;
            }
            else if (primaryObjectColor == CellInternalObject.InternalColor.Violet) {
                internalImage.texture = internalOrange;
            }
        }
        else if (primaryObjectType == CellInternalObject.Type.color5)
        {
            internalImage.texture = Corona;
        }
        else if (primaryObjectType == CellInternalObject.Type.rocketHorizontal)
        {
            internalImage.texture = PillHorizont;
        }
        else if (primaryObjectType == CellInternalObject.Type.rocketVertical)
        {
            internalImage.texture = PillVertical;
        }
        else if (primaryObjectType == CellInternalObject.Type.bomb)
        {
            internalImage.texture = Tablet;
        }
        else if (primaryObjectType == CellInternalObject.Type.airplane)
        {
            internalImage.texture = Fly;
        }
        else if (primaryObjectType == CellInternalObject.Type.blocker)
        {
            internalImage.texture = Blocker;
        }
        else {
            internalImage.gameObject.SetActive(false);
        }

    }
}
