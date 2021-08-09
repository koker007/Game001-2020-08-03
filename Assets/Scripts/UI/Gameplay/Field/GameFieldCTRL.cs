using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Передвигаемый внутриячеестый объект
/// </summary>
public class GameFieldCTRL : MonoBehaviour
{

    [SerializeField]
    GameObject prefabCell;
    [SerializeField]
    GameObject prefabInternal;

    [SerializeField]
    Transform parentOfCells;
    [SerializeField]
    Transform parentOfInternals;


    CellCTRL[,] cellCTRLs; //Ячейки
    List<CellInternalObject> cellInternalObjects; //Внутренности ячеек

    //Инициализировать игровое поле
    public void inicializeField(int sizeX, int sizeY) {
        //Перемещаем поле в центр
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f,0.5f);

        //Создаем пространство игрового поля
        cellCTRLs = new CellCTRL[sizeX, sizeY];
        cellInternalObjects = new List<CellInternalObject>();

        AddAllCells();

        //Заполняем все поля ячейками
        void AddAllCells() {
            for(int x = 0; x < cellCTRLs.GetLength(0); x++) {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                    if (!cellCTRLs[x,y]) {
                        GameObject cellObj = Instantiate(prefabCell, parentOfCells);
                        

                        //ищем компонент
                        cellCTRLs[x, y] = cellObj.GetComponent<CellCTRL>();
                        //Если компонент не нашелся удаляем этот мусор
                        if (!cellCTRLs[x,y]) {
                            Destroy(cellObj);
                            break;
                        }

                        //Перемещаем объект на свою позицию
                        RectTransform rect = cellObj.GetComponent<RectTransform>();
                        rect.pivot = new Vector2(-x,-y);
                    }
                }
            }
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TestSpawn();
    }

    //Проверка ячеек на падение и совместимость
    void TestSpawn() {
        //Начиная снизу проверяем есть ли пустые ячейки
        for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++) {

                //Если эта ячейка есть, пустая и без блокировки движения
                if (cellCTRLs[x,y] && !cellCTRLs[x, y].cellInternal && cellCTRLs[x,y].dontMoving == 0) {

                    //Проверяем сверху на то есть ли там что-то что может упасть
                    for (int plusY = 0; plusY < cellCTRLs.GetLength(1); plusY++) {
                        //Если достигли самого верха поля
                        if (y + plusY >= cellCTRLs.GetLength(1))
                        {
                            //Создаем префаб перемещаемого объекта
                            GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                            //ставим на позицию 
                            RectTransform rect = internalObj.GetComponent<RectTransform>();
                            rect.pivot = new Vector2(-x, -y);
                            cellCTRLs[x, y].cellInternal = internalObj.GetComponent<CellInternalObject>();
                            break;
                        }
                        //или дошли до несуществующей йчейки, выходим
                        else if (y + plusY < cellCTRLs.GetLength(1) && !cellCTRLs[x, y + plusY])
                        {
                            break;
                        }
                        //Если сверху есть ячейка с внутренностью и она не блокирована
                        else if (cellCTRLs[x, y + plusY].dontMoving <= 0 && cellCTRLs[x, y + plusY].cellInternal) {
                            //Перемещаем ее
                            cellCTRLs[x, y].cellInternal = cellCTRLs[x, y + plusY].cellInternal;
                            cellCTRLs[x, y + plusY].cellInternal = null;
                        }
                    }

                }
            }
        }
    }
}
