using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Игровое поле
/// </summary>
public class GameFieldCTRL : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    GameObject prefabCell;
    [SerializeField]
    GameObject prefabInternal;

    [Header("Particles")]
    [SerializeField]
    public GameObject PrefabParticleDie;
    [SerializeField]
    public GameObject PrefabParticleScore;
    [SerializeField]
    public GameObject PrefabParticleSelect;

    [Header("Parents")]
    [SerializeField]
    Transform parentOfCells;
    [SerializeField]
    Transform parentOfInternals;
    [SerializeField]
    public Transform parentOfParticles;
    [SerializeField]
    public Transform parentOfScore;
    [SerializeField]
    public Transform parentOfSelect;

    [Header("Other")]
    [SerializeField]
    CellCTRL CellSelect;
    [SerializeField]
    CellCTRL CellSwap;

    [SerializeField]
    RectTransform rectParticleSelect;

    //Меняемые ячейки
    class Swap {
        public CellCTRL first;
        public CellCTRL second;

        public int stopSwap = 1;
    }

    //Хранит ячейки которые были недавно обменяны
    List<Swap> BufferSwap = new List<Swap>();


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
                        cellCTRLs[x, y].myField = this;
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
        StartInicialize();
    }

    // Update is called once per frame
    void Update()
    {
        TestSpawn();
        TestFieldCombination();
        TestStartSwap();
        TestReturnSwap();
    }

    void StartInicialize() {
        //Добавляем в поле все ячейки
        RandomInicialize();

        void RandomInicialize() {

            bool isComplite = false;
            int colors = Gameplay.main.colors;

            while (!isComplite) {
                //Ставим внизу вверх
                for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
                    //слева в право
                    for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                        //Если ячейки нет - выходим
                        if (!cellCTRLs[x,y]) {
                            break;
                        }

                        //если ячейка пустая добавляем внутренность
                        if (!cellCTRLs[x,y].cellInternal) {
                            //создаем внутренность
                            GameObject InternalObj = Instantiate(prefabInternal, parentOfInternals);
                            CellInternalObject Internal = InternalObj.GetComponent<CellInternalObject>();

                            Internal.myField = this;
                            Internal.myCell = cellCTRLs[x, y];
                            cellCTRLs[x, y].cellInternal = Internal;

                            //Устанавливаем позицию
                            Internal.IniRect();
                            Internal.PosToCell();

                            //Установить цвет
                            Internal.randColor();
                        }

                        
                    }
                }

                isComplite = true;
            }
        }
    }

    //Проверка ячеек на падение и совместимость
    void TestSpawn() {
        //Был ли спавн
        int[] countSpawned = new int[cellCTRLs.GetLength(0)];

        //Начиная снизу проверяем есть ли пустые ячейки
        for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++) {

                //Если эта ячейка есть, пустая и без блокировки движения и на ней сейчас нет движения
                if (cellCTRLs[x,y] && !cellCTRLs[x, y].cellInternal && cellCTRLs[x,y].dontMoving == 0) {

                    //Проверяем сверху на то есть ли там что-то что может упасть
                    for (int plusY = 0; plusY <= cellCTRLs.GetLength(1); plusY++) {
                        //Если достигли самого верха поля
                        if (y + plusY >= cellCTRLs.GetLength(1))
                        {
                            //Создаем префаб перемещаемого объекта
                            GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                            //ставим на позицию 
                            RectTransform rect = internalObj.GetComponent<RectTransform>();
                            
                            //Считаем количество ходов до верха
                            rect.pivot = new Vector2(-x, -y-(countSpawned[x]+cellCTRLs.GetLength(1)-y));

                            CellInternalObject internalCtrl = internalObj.GetComponent<CellInternalObject>();

                            //Установить цвет
                            internalCtrl.randColor();

                            //включаем падение
                            internalCtrl.StartMove(cellCTRLs[x, y]);

                            //Устанавливаем поле
                            internalCtrl.myField = this;
                            //Ставим ячейку куда надо двигаться
                            //internalCtrl.myCell = cellCTRLs[x, y];

                            cellCTRLs[x, y].setInternal(internalCtrl);

                            countSpawned[x]++;

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

                            break;
                        }
                    }

                        
                }
            }
        }
    }

    //Проверяем все ячейки на комбинаци
    void TestFieldCombination() {
        //Начиная сверху проверяем все ячейки на комбинацию
        for (int y = cellCTRLs.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                TestCellCombination(cellCTRLs[x, y]);
            }
        }

        ///////////////////////////////////////////////////////////////
        //Проверить ячейку на комбинации
        bool TestCellCombination(CellCTRL Cell)
        {

            //Выходим если ячейки нет, или нет внутренности, или внутренность в движении, или это просто эффект
            if (!Cell || !Cell.cellInternal || Cell.cellInternal.isMove)
            {
                return false;
            }

            List<CellCTRL> cellLineRight = new List<CellCTRL>();
            List<CellCTRL> cellLineLeft = new List<CellCTRL>();
            List<CellCTRL> cellLineDown = new List<CellCTRL>();
            List<CellCTRL> cellLineUp = new List<CellCTRL>();

            List<CellCTRL> cellSquare = new List<CellCTRL>();

            //Список ячеек которые должны получить урон
            List<CellCTRL> cellDamage = new List<CellCTRL>();

            bool horizontal = false;
            bool vertical = false;

            bool line5 = false;
            bool square = false;
            bool line4 = false;
            bool cross = false;

            bool foundCombination = false;

            TestLines();
            TestSquare();
            CalcResult();
            Damage();

            return foundCombination;

            ///////////////////////////////////////////////////////////////////
            void TestLines()
            {

                ///////////////////////////////////////////////////////
                //Проверка вправо
                for (int smeshenie = 1; smeshenie < 5; smeshenie++)
                {

                    if ((Cell.pos.x + smeshenie) > cellCTRLs.GetLength(0) - 1 || //Если вышли за пределы массива
                        TestCellColor(cellCTRLs[Cell.pos.x + smeshenie, Cell.pos.y]))
                    {
                        //На этом заканчиваем перебор
                        break;
                    }

                    //Добавляем ячейку в список линии
                    cellLineRight.Add(cellCTRLs[Cell.pos.x + smeshenie, Cell.pos.y]);
                }

                ////////////////////////////////////////////////////
                //Проверка влево
                for (int smeshenie = 1; smeshenie < 5; smeshenie++)
                {

                    if ((Cell.pos.x - smeshenie) < 0 || //Если вышли за пределы массива
                        TestCellColor(cellCTRLs[Cell.pos.x - smeshenie, Cell.pos.y]))
                    {
                        //На этом заканчиваем перебор
                        break;
                    }

                    //Добавляем ячейку в список линии
                    cellLineLeft.Add(cellCTRLs[Cell.pos.x - smeshenie, Cell.pos.y]);
                }

                /////////////////////////////////////////////////////
                //Проверка вниз
                for (int smeshenie = 1; smeshenie < 5; smeshenie++)
                {

                    if ((Cell.pos.y - smeshenie) < 0 || //Если вышли за пределы массива
                        TestCellColor(cellCTRLs[Cell.pos.x, Cell.pos.y - smeshenie]))
                    {
                        //На этом заканчиваем перебор
                        break;
                    }

                    //Добавляем ячейку в список линии
                    cellLineDown.Add(cellCTRLs[Cell.pos.x, Cell.pos.y - smeshenie]);
                }

                /////////////////////////////////////////////////////
                //Проверка вверх
                for (int smeshenie = 1; smeshenie < 5; smeshenie++)
                {

                    if ((Cell.pos.y + smeshenie) > cellCTRLs.GetLength(1) - 1 || //Если вышли за пределы массива
                        TestCellColor(cellCTRLs[Cell.pos.x, Cell.pos.y + smeshenie]))
                    {
                        //На этом заканчиваем перебор
                        break;
                    }

                    //Добавляем ячейку в список линии
                    cellLineUp.Add(cellCTRLs[Cell.pos.x, Cell.pos.y + smeshenie]);
                }
            }
            void TestSquare()
            {
                //Проверка на квадрат

                //Справа сверху
                if (cellLineRight.Count > 0 && cellLineUp.Count > 0 && !TestCellColor(cellCTRLs[Cell.pos.x + 1, Cell.pos.y + 1]))
                {
                    cellSquare.Add(cellLineRight[0]);
                    cellSquare.Add(cellLineUp[0]);
                    cellSquare.Add(cellCTRLs[Cell.pos.x + 1, Cell.pos.y + 1]);
                }

                //Справа снизу
                else if (cellLineRight.Count > 0 && cellLineDown.Count > 0 && !TestCellColor(cellCTRLs[Cell.pos.x + 1, Cell.pos.y - 1]))
                {
                    cellSquare.Add(cellLineRight[0]);
                    cellSquare.Add(cellLineDown[0]);
                    cellSquare.Add(cellCTRLs[Cell.pos.x + 1, Cell.pos.y - 1]);
                }

                //Слева снизу
                else if (cellLineLeft.Count > 0 && cellLineDown.Count > 0 && !TestCellColor(cellCTRLs[Cell.pos.x - 1, Cell.pos.y - 1]))
                {
                    cellSquare.Add(cellLineLeft[0]);
                    cellSquare.Add(cellLineDown[0]);
                    cellSquare.Add(cellCTRLs[Cell.pos.x - 1, Cell.pos.y - 1]);
                }

                //Слева сверху
                else if (cellLineLeft.Count > 0 && cellLineUp.Count > 0 && !TestCellColor(cellCTRLs[Cell.pos.x - 1, Cell.pos.y + 1]))
                {
                    cellSquare.Add(cellLineLeft[0]);
                    cellSquare.Add(cellLineUp[0]);
                    cellSquare.Add(cellCTRLs[Cell.pos.x - 1, Cell.pos.y + 1]);
                }
            }

            void CalcResult()
            {

                Line5();
                Line4();
                Line3();
                Square();
                Cross();

                //Собралась ли линия из 5
                void Line5()
                {
                    if (cellLineRight.Count + cellLineLeft.Count == 4)
                    {
                        horizontal = true;
                        line5 = true;

                        AddDamage(Cell);
                        foreach (CellCTRL c in cellLineRight) AddDamage(c);
                        foreach (CellCTRL c in cellLineLeft) AddDamage(c);
                    }

                    if (cellLineDown.Count + cellLineUp.Count == 4)
                    {
                        vertical = true;
                        line5 = true;

                        AddDamage(Cell);
                        foreach (CellCTRL c in cellLineDown) AddDamage(c);
                        foreach (CellCTRL c in cellLineUp) AddDamage(c);
                    }
                }

                //Собралась ли линия из 4
                void Line4()
                {
                    if (cellLineRight.Count + cellLineLeft.Count == 3)
                    {
                        horizontal = true;
                        line4 = true;

                        AddDamage(Cell);
                        foreach (CellCTRL c in cellLineRight) AddDamage(c);
                        foreach (CellCTRL c in cellLineLeft) AddDamage(c);
                    }

                    if (cellLineDown.Count + cellLineUp.Count == 3)
                    {
                        vertical = true;
                        line4 = true;

                        AddDamage(Cell);
                        foreach (CellCTRL c in cellLineDown) AddDamage(c);
                        foreach (CellCTRL c in cellLineUp) AddDamage(c);
                    }
                }

                //Собралась ли линия из 3
                void Line3()
                {
                    if (cellLineRight.Count + cellLineLeft.Count == 2)
                    {
                        horizontal = true;

                        AddDamage(Cell);
                        foreach (CellCTRL c in cellLineRight) AddDamage(c);
                        foreach (CellCTRL c in cellLineLeft) AddDamage(c);
                    }

                    if (cellLineDown.Count + cellLineUp.Count == 2)
                    {
                        vertical = true;

                        AddDamage(Cell);
                        foreach (CellCTRL c in cellLineDown) AddDamage(c);
                        foreach (CellCTRL c in cellLineUp) AddDamage(c);
                    }
                }

                //Собрался ли квадрат
                void Square()
                {
                    if (cellSquare.Count >= 3)
                    {
                        square = true;

                        AddDamage(Cell);
                        foreach (CellCTRL c in cellSquare) AddDamage(c);
                    }
                }

                //Собрался ли крест
                void Cross()
                {
                    if (horizontal && vertical)
                    {
                        cross = true;
                    }
                }
            }

            //Раздать урон всем обьектам которые в списке
            void Damage()
            {
                //Если комбинаций не было, выходим
                if (cellDamage.Count == 0)
                {
                    return;
                }

                //Запоминаем ячейку с наибольшим перемещением
                CellCTRL cellLast = cellDamage[0];
                CellInternalObject.InternalColor internalColor = CellInternalObject.InternalColor.Red;
                
                //Раздать ячейкам урон или перемешать если игра еще не началась
                foreach (CellCTRL c in cellDamage)
                {
                    //если эта ячейка последняя перемещаемая
                    if (cellLast.myInternalNum < c.myInternalNum)
                    {
                        //Запоминаем
                        cellLast = c;
                        if (cellLast.cellInternal && cellLast.cellInternal.type == CellInternalObject.Type.color) {
                            internalColor = cellLast.cellInternal.color;
                        }
                    }


                    //Наносим урон по клетке
                    c.Damage();

                    //Отнимаем ход если комбинация получилась благодаря перемещениям игрока
                    List<Swap> BufferSwapNew = new List<Swap>();
                    foreach (Swap swap in BufferSwap)
                    {
                        if (swap.first == c || swap.second == c)
                        {
                            Gameplay.main.movingCount++;
                            Gameplay.main.movingCan--;

                            continue;
                        }
                        BufferSwapNew.Add(swap);
                    }
                    BufferSwap = BufferSwapNew;

                }

                //
                foundCombination = true;

                //Поставить новый обьект на место
                //Эсли линия из 5
                if (line5) {
                    //Создаем супер цветовую боббу
                    CreateSuperColor(cellLast, internalColor);
                }
                //Если крест
                else if (cross) {
                    //Создаем бомбу
                    CreateBomb(cellLast, internalColor);
                }
                //Если горизонтальная из 4
                else if (line4 && horizontal) {

                }
                //Если вертикальная из 3
                else if (line4 && vertical) {

                }
                //Если квадрат
                else if (square) {
                    //нечто летающее
                    CreateFly(cellLast, internalColor);
                }
            }


            //Проверить ячейку на совпадение цвета
            bool TestCellColor(CellCTRL SecondCell)
            {

                //Отмена если
                if (
                    !SecondCell || //если самой ячейки нет
                    !SecondCell.cellInternal ||
                    !Cell.cellInternal || //если объекта в ячейке нет
                    SecondCell.cellInternal.isMove || //если эти внутренности находятся в движении
                    SecondCell.cellInternal.color != Cell.cellInternal.color)
                {
                    //На этом заканчиваем перебор
                    return true;
                }


                return false;
            }

            //Добавить ячейку в список, проверяя что ее там нет
            void AddDamage(CellCTRL cellDamageNew)
            {
                //Проверяем что этого обьекта нет в списке
                foreach (CellCTRL c in cellDamage)
                {
                    //Обьект уже есть в списке, выходим
                    if (c == cellDamageNew) return;
                }

                cellDamage.Add(cellDamageNew);
            }

            

            void CreateRocketHorizontal() {
                
            }
            void CreateRocketVertical() {
                
            }

            //Создать бомбу на месте другой ячейки
            CellInternalObject CreateBomb(CellCTRL cellLast, CellInternalObject.InternalColor internalColor) {
                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();

                cellInternal.IniBomb(cellLast, this, internalColor);


                return cellInternal;
            }
            CellInternalObject CreateFly(CellCTRL cellLast, CellInternalObject.InternalColor internalColor) {
                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();

                cellInternal.IniFly(cellLast, this, internalColor);


                return cellInternal;
            }
            CellInternalObject CreateSuperColor(CellCTRL cellLast, CellInternalObject.InternalColor internalColor)
            {
                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();

                cellInternal.IniSuperColor(cellLast, this, internalColor);


                return cellInternal;
            }

        }

    }

    //Проверка на то что нужно поменять местами объекты
    void TestStartSwap()
    {
        //Если есть выбранная ячейка
        if (CellSelect)
        {
            if (!rectParticleSelect)
            {
                GameObject select = Instantiate(PrefabParticleSelect, parentOfSelect);
                rectParticleSelect = select.GetComponent<RectTransform>();

            }

            rectParticleSelect.pivot = new Vector2(CellSelect.pos.x * -1, CellSelect.pos.y * -1);
        }
        else if (rectParticleSelect)
        {
            Destroy(rectParticleSelect.gameObject);
        }

        //Только есть с кем поменяться
        if (!CellSwap || !CellSelect)
            return;

        bool neibour = false;

        //Проверяем соседство
        //слева
        if (CellSelect.pos.x > 0 && CellSwap == cellCTRLs[CellSelect.pos.x - 1, CellSelect.pos.y])
            neibour = true;
        //справа
        else if (CellSelect.pos.x < cellCTRLs.GetLength(0) - 1 && CellSwap == cellCTRLs[CellSelect.pos.x + 1, CellSelect.pos.y])
            neibour = true;
        //Снизу
        else if (CellSelect.pos.y > 0 && CellSwap == cellCTRLs[CellSelect.pos.x, CellSelect.pos.y - 1])
            neibour = true;
        //сверху
        else if (CellSelect.pos.y < cellCTRLs.GetLength(1) - 1 && CellSwap == cellCTRLs[CellSelect.pos.x, CellSelect.pos.y + 1])
            neibour = true;

        //если не соседняя ячейка, выходим
        if (!neibour)
        {
            CellSelect = null;
            CellSwap = null;
            return;
        }

        //Если обмен не возможен
        if (!CellSelect.cellInternal || //Если у ячеек нечем меняться
            !CellSwap.cellInternal ||
            CellSelect.cellInternal.isMove || //Если в ячейки происходит движение
            CellSwap.cellInternal.isMove ||
            CellSelect.dontMoving > 0 || //Если ячейка заморожена
            CellSwap.dontMoving > 0 ||
            Gameplay.main.movingCan <= 0 //Если есть ходы
            )
        {
            CellSelect = null;
            CellSwap = null;
            return;
        }

        //Меняем внутренности
        CellInternalObject InternalSelect = CellSelect.cellInternal;
        CellInternalObject InternalSwap = CellSwap.cellInternal;

        //Открепляем привязку к ячейкам у обьектов
        CellSelect.cellInternal.myCell = null;
        CellSwap.cellInternal.myCell = null;

        //Открепляем привязку к обьектам у ячеек
        CellSelect.cellInternal = null;
        CellSwap.cellInternal = null;

        InternalSelect.StartMove(CellSwap);
        InternalSwap.StartMove(CellSelect);

        //Добавляем ячейки в список перемещаемых
        Swap swap = new Swap();

        swap.first = CellSelect;
        swap.second = CellSwap;
        BufferSwap.Add(swap);

        //Gameplay.main.movingCount++;
        //Gameplay.main.movingCan--;
    }

    void TestReturnSwap() {

        List<Swap> BufferSwapNew = new List<Swap>();

        foreach (Swap swap in BufferSwap) {

            if (swap == null) {
                continue;
            }
            //Если у ячеек есть внутренности и они не движутся, возвращаем на свои места
            else if (swap.first.cellInternal && swap.second.cellInternal &&
                !swap.first.cellInternal.isMove && !swap.second.cellInternal.isMove)
            {

                //Меняем внутренности
                CellInternalObject InternalFirst = swap.first.cellInternal;
                CellInternalObject InternalSecond = swap.second.cellInternal;

                //Открепляем привязку к ячейкам у обьектов
                swap.first.cellInternal.myCell = null;
                swap.second.cellInternal.myCell = null;

                //Открепляем привязку к обьектам у ячеек
                swap.first.cellInternal = null;
                swap.second.cellInternal = null;

                InternalFirst.StartMove(swap.second);
                InternalSecond.StartMove(swap.first);

                //Gameplay.main.movingCount++;
                //Gameplay.main.movingCan--;

                continue;
            }
            BufferSwapNew.Add(swap);

        }
        BufferSwap = BufferSwapNew;
    }

    //Сделать ячейку выделенной или целевой для перемещения
    public void SetSelectCell(CellCTRL CellClick) {
        if (!CellSelect) {
            CellSelect = CellClick;
        }
        else {
            //проверяем ячейку на соседство
            if (isNeibour())
            {
                //Это сосед меняем местами
                CellSwap = CellClick;
            }
            else {
                CellSwap = null;
                CellSelect = CellClick;
            }

            //проверить соседство
            bool isNeibour() {

                //Слева
                if (CellSelect.pos.x > 0 &&
                    cellCTRLs[CellSelect.pos.x - 1, CellSelect.pos.y] &&
                    cellCTRLs[CellSelect.pos.x - 1, CellSelect.pos.y] == CellClick) {
                    return true;
                }
                //Справа
                else if (CellSelect.pos.x < cellCTRLs.GetLength(0) - 1 &&
                    cellCTRLs[CellSelect.pos.x + 1, CellSelect.pos.y] &&
                    cellCTRLs[CellSelect.pos.x + 1, CellSelect.pos.y] == CellClick) {
                    return true;
                }
                //Снизу
                else if (CellSelect.pos.y > 0 &&
                    cellCTRLs[CellSelect.pos.x, CellSelect.pos.y - 1] &&
                    cellCTRLs[CellSelect.pos.x, CellSelect.pos.y - 1] == CellClick)
                {
                    return true;
                }
                //Сверху
                else if (CellSelect.pos.y < cellCTRLs.GetLength(1) - 1 &&
                    cellCTRLs[CellSelect.pos.x, CellSelect.pos.y + 1] &&
                    cellCTRLs[CellSelect.pos.x, CellSelect.pos.y + 1] == CellClick)
                {
                    return true;
                }

                return false;
            }
        }
    }


}
