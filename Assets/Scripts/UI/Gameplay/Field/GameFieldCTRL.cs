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


    public CellCTRL[,] cellCTRLs; //Ячейки
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

                        cellCTRLs[x, y].pos = new Vector2Int(x, y);

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
        TestLine();
    }


    //Проверка ячеек на падение и совместимость
    void TestSpawn() {
        //Начиная снизу проверяем есть ли пустые ячейки
        for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++) {

                //Если эта ячейка есть, пустая и без блокировки движения и на ней сейчас нет движения
                if (cellCTRLs[x,y] && !cellCTRLs[x, y].cellInternal && cellCTRLs[x,y].dontMoving == 0 && !cellCTRLs[x,y].movingInternalNow) {

                    //Проверяем сверху на то есть ли там что-то что может упасть
                    for (int plusY = 0; plusY <= cellCTRLs.GetLength(1); plusY++) {
                        //Если достигли самого верха поля
                        if (y + plusY >= cellCTRLs.GetLength(1))
                        {
                            //Создаем префаб перемещаемого объекта
                            GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                            //ставим на позицию 
                            RectTransform rect = internalObj.GetComponent<RectTransform>();
                            rect.pivot = new Vector2(-x, -y-10);

                            CellInternalObject internalCtrl = internalObj.GetComponent<CellInternalObject>();

                            //Установить цвет
                            internalCtrl.randColor();

                            //включаем падение
                            internalCtrl.dropStart(cellCTRLs[x, y]);

                            //Устанавливаем поле
                            internalCtrl.myField = this;
                            //Ставим ячейку куда надо двигаться
                            //internalCtrl.myCell = cellCTRLs[x, y];

                            cellCTRLs[x, y].setInternal(internalCtrl);

                            break;
                        }
                        //или дошли до несуществующей йчейки, выходим
                        else if (y + plusY < cellCTRLs.GetLength(1) && !cellCTRLs[x, y + plusY])
                        {
                            break;
                        }

                        //За перемещение ниже отвечает сам перемещаемый объект

                        //Если сверху есть ячейка с внутренностью и она не блокирована
                        else if (cellCTRLs[x, y + plusY].dontMoving <= 0 && cellCTRLs[x, y + plusY].cellInternal) {
                            //Перемещаем ее
                            //cellCTRLs[x, y].cellInternal = cellCTRLs[x, y + plusY].cellInternal;
                            //cellCTRLs[x, y + plusY].cellInternal = null;

                            //cellCTRLs[x, y].cellInternal.dropStart(cellCTRLs[x, y]);

                            break;
                        }
                    }

                }
            }
        }
    }

    //Проверки на линию одинаковых объектов
    void TestLine()
    {
        //Начиная сверху проверяем собралась ли линия
        for (int y = cellCTRLs.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                line(x, y);
            }
        }

        void line(int x, int y) {
            //Если обьекта нет выходим.
            if (!cellCTRLs[x,y].cellInternal) {
                return;
            }


            testRight();
            testDown();

            //Проверка влево
            void testRight() {
                List<CellCTRL> cellLine = new List<CellCTRL>();

                for (int plusX = 0; plusX < 5; plusX++) {

                    if ((x + plusX) >= cellCTRLs.GetLength(0) || //Если вышли за пределы массива
                        !cellCTRLs[x + plusX, y] || //если самой ячейки нет
                        !cellCTRLs[x + plusX, y].cellInternal || //если объекта в ячейке нет
                        cellCTRLs[x + plusX, y].movingInternalNow || //если эти внутренности находятся в движении
                        cellCTRLs[x + plusX, y].cellInternal.color != cellCTRLs[x, y].cellInternal.color) {
                        //На этом заканчиваем перебор
                        break;
                    }

                    //Добавляем ячейку с список линии
                    cellLine.Add(cellCTRLs[x + plusX, y]);
                }

                //Если собралась линия
                if (cellLine.Count >= 3) {
                    foreach (CellCTRL cell in cellLine) {
                        cell.gettingScore();
                    }
                }
            }
            //Проверка вниз
            void testDown() {
                List<CellCTRL> cellLine = new List<CellCTRL>();

                for (int minusY = 0; minusY < 5; minusY++)
                {

                    if ((y - minusY) < 0 || //Если вышли за пределы массива
                        !cellCTRLs[x, y - minusY] || //если самой ячейки нет
                        !cellCTRLs[x, y - minusY].cellInternal || //если объекта в ячейке нет
                        cellCTRLs[x, y - minusY].movingInternalNow || //если эти внутренности находятся в движении
                        cellCTRLs[x, y - minusY].cellInternal.color != cellCTRLs[x, y].cellInternal.color)
                    {
                        //На этом заканчиваем перебор
                        break;
                    }

                    //Добавляем ячейку с список линии
                    cellLine.Add(cellCTRLs[x, y - minusY]);
                }

                //Если собралась линия
                if (cellLine.Count >= 3)
                {
                    foreach (CellCTRL cell in cellLine)
                    {
                        cell.gettingScore();
                    }
                }
            }

        }

    }
}
