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

    public int ComboCount = 1; 
    bool isMoving = false; //находятся ли в движении объекты наполе
    //паходятся ли в движении какие либо объекты на поле
    void TestMoving() {
        bool movingNow = false;
        for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
            if (movingNow) break;

            for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                if (movingNow) break;

                //Если ячейки нет, смотрим дальше
                if (!cellCTRLs[x,y]) continue;

                if (!cellCTRLs[x,y].cellInternal && Time.unscaledTime - cellCTRLs[x, y].timeBoomOld > 0.5f) {
                    movingNow = true;
                }

                else if (cellCTRLs[x, y].cellInternal && cellCTRLs[x, y].cellInternal.isMove) {
                    movingNow = true;
                }
            }
        }

        //Если прекратили двигаться
        if (isMoving && !movingNow) {
            ComboCount = 1;
            Gameplay.main.CheckEndGame();
        }

        isMoving = movingNow;
    }

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

        TestSpawn(); //Спавним
        TestMoving(); //Проверяем наличие движения для отмены комбо
        TestFieldCombination(); //Тестим комбинации

        TestStartSwap(); //Начинаем обмен
        TestReturnSwap(); //Возвращяем обмен
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

    //Список линий которые взорвались
    List<Combination> listCombinations;
    //Проверяем все ячейки на комбинаци

    //Класс комбинации, для отслеживания что собралось
    class Combination {
        public List<CellCTRL> cells = new List<CellCTRL>();

        public bool horizontal = false;
        public bool vertical = false;

        public bool line5 = false;
        public bool square = false;
        public bool line4 = false;
        public bool cross = false;
    }
    void TestFieldCombination() {
        //Создаем новый список комбинаций
        listCombinations = new List<Combination>();

        //Начиная сверху проверяем все ячейки на комбинацию
        for (int y = cellCTRLs.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                TestCellCombination2(cellCTRLs[x, y]);
            }
        }

        TestDamageAndSpawn();

        ///////////////////////////////////////////////////////////////
        //Проверить ячейку на комбинации
        void TestCellCombination(CellCTRL Cell)
        {
            /*

            //Выходим если ячейки нет, или нет внутренности, или внутренность в движении, или это просто эффект
            if (!Cell || !Cell.cellInternal || Cell.cellInternal.isMove)
            {
                return;
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

            ///////////////////////////////////////////////////////////////////
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

                //TestActivate();

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

                //Проверка на активацию
                void TestActivate() {

                    foreach (Swap swap in BufferSwap)
                    {
                        //Если хоть кто то в движении выходим
                        if (swap.first.cellInternal.isMove || swap.second.cellInternal.isMove)
                            continue;

                        bool activeFirst = isActivate(swap.first);
                        bool activeSecond = isActivate(swap.second);


                        //Если первый компонент активируемый
                        CellCTRL activate = swap.first;
                        CellCTRL partner = swap.second;


                        //инвертируем
                        //если первый не активный а второй да
                        if (!activeFirst && activeSecond) {
                            activate = swap.second;
                            partner = swap.first;

                            activeFirst = true;
                            activeSecond = false;
                        }

                        //Выходим если первый так и не стал не активным
                        if (!activeFirst) continue;

                        //если второй не активный то просто заносим активную ячейку в список урона
                        if (!activeSecond)
                        {
                            AddDamage(activate);
                        }
                        //Иначе если оказывается активны обе ячейки то активируем по особому
                        else {
                            
                        }
                        

                    }

                    bool isActivate(CellCTRL cell)
                    {
                        bool result = false;
                        if (cell.cellInternal.type == CellInternalObject.Type.airplane)
                        {
                            result = true;
                        }
                        else if (cell.cellInternal.type == CellInternalObject.Type.bomb)
                        {
                            result = true;
                        }
                        else if (cell.cellInternal.type == CellInternalObject.Type.rocketHorizontal)
                        {
                            result = true;
                        }
                        else if (cell.cellInternal.type == CellInternalObject.Type.rocketVertical)
                        {
                            result = true;
                        }

                        else if (cell.cellInternal.type == CellInternalObject.Type.supercolor)
                        {
                            result = true;
                        }

                        return result;
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
                if (cellLast.cellInternal) internalColor = cellLast.cellInternal.color;


                //Проверяем на то что ячейки из этой комбинации нету в списке
                List<CellCTRL> CombinationFounded = null;
                foreach (List<CellCTRL> combination in listCombinations) {
                    if (CombinationFounded != null) break; //Выходим если комбинация была найдена

                    //Проверяем ячейку на то что ее нет в списке этой комбинации
                    foreach (CellCTRL cellCombOld in combination) {
                        if (CombinationFounded != null) break; //Выходим если комбинация была найдена

                        foreach (CellCTRL cellCombNow in cellDamage) {
                            if (CombinationFounded != null) break; //Выходим если комбинация была найдена

                            //Если нашли ячейку в списке комбинаций то значит текущая комбинация является продолжением старой
                            if (cellCombNow == cellCombOld) {
                                CombinationFounded = combination;
                            }
                        }
                    }
                }

                

                //Раздать ячейкам урон или перемешать если игра еще не началась
                foreach (CellCTRL c in cellDamage)
                {
                    bool cellCombinationFound = false;
                    //Если была обнаружена комбинация то проверяем на то что текущей ячейки в списке той комбинации нет
                    if (CombinationFounded != null) {
                        foreach (CellCTRL cellCombOld in CombinationFounded)
                        {
                            if (cellCombOld == c) {
                                cellCombinationFound = true;
                            }
                        }
                    }

                    //Если эта ячейка уже была в списке пропускаем действия над ней
                    if (cellCombinationFound) continue;
                    //Если список комбинации есть, добавляем ячейку
                    else if(CombinationFounded != null) {
                        CombinationFounded.Add(c);
                    }

                    //если эта ячейка последняя перемещаемая
                    if (cellLast.myInternalNum < c.myInternalNum)
                    {
                        //Запоминаем
                        cellLast = c;
                        if (cellLast.cellInternal && cellLast.cellInternal.type == CellInternalObject.Type.color) {
                            internalColor = cellLast.cellInternal.color;
                        }
                    }


                    CellInternalObject partner = c.cellInternal;
                    //Отнимаем ход если комбинация получилась благодаря перемещениям игрока
                    List<Swap> BufferSwapNew = new List<Swap>();
                    foreach (Swap swap in BufferSwap)
                    {
                        if (swap.first == c || swap.second == c)
                        {

                            Gameplay.main.MinusMoving();

                            //Запомнить  партнера по перемещениям
                            if (swap.first != c) partner = c.cellInternal;
                            else if (swap.second != c) partner = c.cellInternal; 

                            continue;
                        }
                        BufferSwapNew.Add(swap);
                    }
                    BufferSwap = BufferSwapNew;

                    //Наносим урон по клетке
                    c.Damage(partner);


                }

                //Если списа комбинации небыло, то значит это произошло в первый раз, добавляем комбинацию в список комбинаций
                if (CombinationFounded == null) {
                    listCombinations.Add(cellDamage);
                }

                //
                foundCombination = true;

                //Поставить новый обьект на место
                //если линия из 5
                if (line5) {
                    //Создаем супер цветовую боббу
                    //CreateSuperColor(cellLast, internalColor);
                }
                //Если крест
                else if (cross) {
                    //Создаем бомбу
                    //CreateBomb(cellLast, internalColor);
                }
                //Если горизонтальная из 4
                else if (line4 && horizontal) {
                    CreateRocketVertical(cellLast, internalColor);
                }
                //Если вертикальная из 3
                else if (line4 && vertical) {
                    CreateRocketHorizontal(cellLast, internalColor);
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
                    if (c == cellDamageNew) { 
                        return;
                    
                    }
                }

                cellDamage.Add(cellDamageNew);
            }

            
            //Создать бомбу на месте другой ячейки
            void CreateBomb(CellCTRL cellLast, CellInternalObject.InternalColor internalColor) {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniBomb(cellLast, this, internalColor);


            }
            void CreateFly(CellCTRL cellLast, CellInternalObject.InternalColor internalColor) {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniFly(cellLast, this, internalColor);


            }
            void CreateSuperColor(CellCTRL cellLast, CellInternalObject.InternalColor internalColor)
            {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniSuperColor(cellLast, this, internalColor);


            }
            void CreateRocketVertical(CellCTRL cellLast, CellInternalObject.InternalColor internalColor)
            {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniRocketVertical(cellLast, this, internalColor);

            }
            void CreateRocketHorizontal(CellCTRL cellLast, CellInternalObject.InternalColor internalColor)
            {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniRocketHorizontal(cellLast, this, internalColor);

            }

            */
        }
        //Проверить ячейку на комбинации. вариант 2021.08.18
        void TestCellCombination2(CellCTRL Cell)
        {

            //Выходим если ячейки нет, или нет внутренности, или внутренность в движении, или это просто эффект
            if (!Cell || !Cell.cellInternal || Cell.cellInternal.isMove)
            {
                return;
            }

            List<CellCTRL> cellLineRight = new List<CellCTRL>();
            List<CellCTRL> cellLineLeft = new List<CellCTRL>();
            List<CellCTRL> cellLineDown = new List<CellCTRL>();
            List<CellCTRL> cellLineUp = new List<CellCTRL>();

            List<CellCTRL> cellSquare = new List<CellCTRL>();

            //Список ячеек в комбинации
            Combination Combination = new Combination();

            //Сперва ищем свою комбинацию в списке
            GetCombination();

            TestLines();
            TestSquare();
            CalcResult();

            //Занести комбинацию в список
            SetCombination();

            ///////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////
            void GetCombination() {
                bool foundCombination = false;

                //Перебираем комбинации
                foreach (Combination comb in listCombinations) {
                    if (foundCombination) break; //если комбинация была найденна выходим

                    //Перебираем ячейки из списка комбинации
                    foreach (CellCTRL cellComb in comb.cells) {
                        if (foundCombination) break; //если комбинация была найденна выходим

                        //Если ячеейка была найденна в комбинации
                        if (cellComb == Cell) {
                            //то мы нашли комбинацию
                            foundCombination = true;
                            Combination = comb;
                        }
                    }
                }
            }
            void SetCombination() {
                if (Combination.cells.Count <= 0) {
                    return;
                }

                //Ищем есть ли в списке эта комбинация
                bool found = false;
                foreach (Combination comb in listCombinations) {
                    if (comb == Combination) {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    listCombinations.Add(Combination);
                }
            }

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
                        Combination.horizontal = true;
                        Combination.line5 = true;

                        AddCellToCombination(Cell);
                        foreach (CellCTRL c in cellLineRight) AddCellToCombination(c);
                        foreach (CellCTRL c in cellLineLeft) AddCellToCombination(c);
                    }

                    if (cellLineDown.Count + cellLineUp.Count == 4)
                    {
                        Combination.vertical = true;
                        Combination.line5 = true;

                        AddCellToCombination(Cell);
                        foreach (CellCTRL c in cellLineDown) AddCellToCombination(c);
                        foreach (CellCTRL c in cellLineUp) AddCellToCombination(c);
                    }
                }

                //Собралась ли линия из 4
                void Line4()
                {
                    if (cellLineRight.Count + cellLineLeft.Count == 3)
                    {
                        Combination.horizontal = true;
                        Combination.line4 = true;

                        AddCellToCombination(Cell);
                        foreach (CellCTRL c in cellLineRight) AddCellToCombination(c);
                        foreach (CellCTRL c in cellLineLeft) AddCellToCombination(c);
                    }

                    if (cellLineDown.Count + cellLineUp.Count == 3)
                    {
                        Combination.vertical = true;
                        Combination.line4 = true;

                        AddCellToCombination(Cell);
                        foreach (CellCTRL c in cellLineDown) AddCellToCombination(c);
                        foreach (CellCTRL c in cellLineUp) AddCellToCombination(c);
                    }
                }

                //Собралась ли линия из 3
                void Line3()
                {
                    if (cellLineRight.Count + cellLineLeft.Count == 2)
                    {
                        Combination.horizontal = true;

                        AddCellToCombination(Cell);
                        foreach (CellCTRL c in cellLineRight) AddCellToCombination(c);
                        foreach (CellCTRL c in cellLineLeft) AddCellToCombination(c);
                    }

                    if (cellLineDown.Count + cellLineUp.Count == 2)
                    {
                        Combination.vertical = true;

                        AddCellToCombination(Cell);
                        foreach (CellCTRL c in cellLineDown) AddCellToCombination(c);
                        foreach (CellCTRL c in cellLineUp) AddCellToCombination(c);
                    }
                }

                //Собрался ли квадрат
                void Square()
                {
                    if (cellSquare.Count >= 3)
                    {
                        Combination.square = true;

                        AddCellToCombination(Cell);
                        foreach (CellCTRL c in cellSquare) AddCellToCombination(c);
                    }
                }

                //Собрался ли крест
                void Cross()
                {
                    if (Combination.horizontal && Combination.vertical)
                    {
                        Combination.cross = true;
                    }
                }

            }

            void AddCellToCombination(CellCTRL cellAdd) {
                //Проверяем есть ли уже ячейка в списке
                bool found = false;
                foreach (CellCTRL cellComb in Combination.cells) {
                    //если найшли выходим
                    if (cellComb == cellAdd) {
                        found = true;
                        break;
                    }
                }

                //Если ячейка в списке не была обнаружена, добавляем в список
                if (!found) {
                    Combination.cells.Add(cellAdd);
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

        }

        //Проверить ячейки на супер комбинацию
        void TestSuperCombination() {
            foreach (Swap swap in BufferSwap) {
                //Пропускаем если они в движении
                if (swap.first.cellInternal.isMove || swap.second.cellInternal.isMove) {
                    continue;
                }

                //супер колор + супер колор
                //if () {
                
                //}
            }
        }
        //Раздать урон комбинациям и заспавнить объекты
        void TestDamageAndSpawn() {
            //Перебираем комбинации
            foreach (Combination comb in listCombinations) {
                //Если есть ячейки в комбинации
                if (comb.cells.Count > 0) {
                    //Выполняем действие над комбинацией
                    CalcCombination(comb);
                }
            }

            //Выполнить комбинацию
            void CalcCombination(Combination comb) {

                //Ищем наиболее подходяшую ячейку для спавна и запоминаем цвет
                CellCTRL CellSpawn = comb.cells[0];
                CellInternalObject.InternalColor color = CellInternalObject.InternalColor.Red;

                foreach (CellCTRL cell in comb.cells) {
                    if (CellSpawn.myInternalNum < cell.myInternalNum) {
                        CellSpawn = cell;

                        if (cell.cellInternal) {
                            color = cell.cellInternal.color;
                        }
                    }
                }

                if (CellSpawn.cellInternal) {
                    color = CellSpawn.cellInternal.color;
                }

                //Удаляем ячейки комбинации
                //Раздать ячейкам урон или перемешать если игра еще не началась
                foreach (CellCTRL c in comb.cells)
                {

                    if (Gameplay.main.movingCount <= 0)
                    {
                        mixColor();
                    }
                    else {
                        SetDamage();
                    }

                    //перемешать цвет
                    void mixColor() {
                        c.cellInternal.randColor();
                    }
                    //Нанести урон
                    void SetDamage() {
                        CellInternalObject partner = c.cellInternal;
                        //Отнимаем ход если комбинация получилась благодаря перемещениям игрока
                        List<Swap> BufferSwapNew = new List<Swap>();
                        foreach (Swap swap in BufferSwap)
                        {
                            if (swap.first == c || swap.second == c)
                            {

                                Gameplay.main.MinusMoving();

                                //Запомнить  партнера по перемещениям
                                if (swap.first != c) partner = c.cellInternal;
                                else if (swap.second != c) partner = c.cellInternal;

                                continue;
                            }
                            BufferSwapNew.Add(swap);
                        }
                        BufferSwap = BufferSwapNew;

                        //Наносим урон по клетке
                        c.Damage(partner);
                    }
                }

                //Точка спавна есть, теперь проверяем что нужно спавнить
                //Поставить новый обьект на место
                if (Gameplay.main.movingCount > 0) {
                    //если линия из 5
                    if (comb.line5)
                    {
                        //Создаем супер цветовую боббу
                        CreateSuperColor(CellSpawn, color);
                    }
                    //Если крест
                    else if (comb.cross)
                    {
                        //Создаем бомбу
                        CreateBomb(CellSpawn, color);
                    }
                    //Если горизонтальная из 4
                    else if (comb.line4 && comb.horizontal)
                    {
                        CreateRocketVertical(CellSpawn, color);
                    }
                    //Если вертикальная из 3
                    else if (comb.line4 && comb.vertical)
                    {
                        CreateRocketHorizontal(CellSpawn, color);
                    }
                    //Если квадрат
                    else if (comb.square)
                    {
                        //нечто летающее
                        CreateFly(CellSpawn, color);
                    }

                    //Комбинация завершена повышаем комбо
                    ComboCount++;
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            //Создать бомбу на месте другой ячейки
            void CreateBomb(CellCTRL cellLast, CellInternalObject.InternalColor internalColor)
            {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniBomb(cellLast, this, internalColor);


            }
            void CreateFly(CellCTRL cellLast, CellInternalObject.InternalColor internalColor)
            {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniFly(cellLast, this, internalColor);


            }
            void CreateSuperColor(CellCTRL cellLast, CellInternalObject.InternalColor internalColor)
            {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniSuperColor(cellLast, this, internalColor);


            }
            void CreateRocketVertical(CellCTRL cellLast, CellInternalObject.InternalColor internalColor)
            {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniRocketVertical(cellLast, this, internalColor);

            }
            void CreateRocketHorizontal(CellCTRL cellLast, CellInternalObject.InternalColor internalColor)
            {
                if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

                GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
                cellLast.timeAddInternalOld = Time.unscaledTime;

                cellInternal.IniRocketHorizontal(cellLast, this, internalColor);

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

        Gameplay.main.movingCount++;
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

                Gameplay.main.movingCount--;

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
