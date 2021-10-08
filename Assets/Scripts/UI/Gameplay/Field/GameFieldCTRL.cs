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
    [Header("Counters")]
    /// <summary>
    /// Текущее количество коробок блокирующее движение на карте
    /// </summary>
    [SerializeField]
    public int CountBoxBlocker = 0;

    /// <summary>
    /// Текущее количество камней блокирующее движение на карте
    /// </summary>
    [SerializeField]
    public int CountRockBlocker = 0;
    /// <summary>
    /// Текущее количество плесени на карте
    /// </summary>
    [SerializeField]
    public int CountMold = 0;
    /// <summary>
    /// Текущее количество распространяемой панели на карте
    /// </summary>
    [SerializeField]
    public int CountPanelSpread = 0;
    /// <summary>
    /// Количество игровых клеток в которых могут быть обьекты
    /// </summary>
    [SerializeField]
    public int CountInteractiveCells = 0;
    [SerializeField]
    public int[] CountDestroyCrystals;

    [Header("Prefabs")]
    [SerializeField]
    GameObject prefabCell;
    [SerializeField]
    public GameObject prefabInternal;
    [SerializeField]
    GameObject prefabBoxBlock;
    [SerializeField]
    GameObject prefabRock;
    [SerializeField]
    public GameObject prefabPanel;
    [SerializeField]
    GameObject prefabMold;

    [Header("Particles")]
    [SerializeField]
    public GameObject PrefabParticleDie;
    [SerializeField]
    public GameObject PrefabParticleScore;
    [SerializeField]
    public GameObject PrefabParticleSelect;

    //Родительские позиции для спавна внутри игровых объектов
    [Header("Parents")]
    [SerializeField]
    Transform parentOfCells;
    [SerializeField]
    public Transform parentOfInternals;
    [SerializeField]
    public Transform parentOfBoxBlock;
    [SerializeField]
    public Transform parentOfPanels;
    [SerializeField]
    public Transform parentOfMold;
    [SerializeField]
    public Transform parentOfRock;
    [SerializeField]
    public Transform parentOfFly;
    [SerializeField]
    public Transform parentOfParticles;
    [SerializeField]
    public Transform parentOfScore;
    [SerializeField]
    public Transform parentOfSelect;


    [Header("Other")]
    [SerializeField]
    public CellCTRL CellSelect; //Первый выделенный пользователем объект
    [SerializeField]
    CellCTRL CellSwap; //Второй выделенный пользователем объект

    [SerializeField]
    RectTransform rectParticleSelect;

    public float timeLastBoom = 0;
    public float timeLastMove = 0;

    RectTransform myRect;

    /// <summary>
    /// Список контроллеров плесени
    /// </summary>
    public List<MoldCTRL> moldCTRLs = new List<MoldCTRL>();

    /// <summary>
    /// Пересчитать текущее количество плесени, исключить удаленные из списка
    /// </summary>
    public void ReCalcMoldList() {
        List<MoldCTRL> moldCTRLsNew = new List<MoldCTRL>();

        foreach (MoldCTRL moldCTRL in moldCTRLs) {
            if (moldCTRL) {
                moldCTRLsNew.Add(moldCTRL);
            }
        }

        moldCTRLs = moldCTRLsNew;
        CountMold = moldCTRLs.Count;
    }

    /// <summary>
    /// Получить рандомную ячейку с боков этой, может возвратить Null
    /// </summary>
    static public CellCTRL GetRandomCellNearest(CellCTRL cellFrom)
    {
        //Выбираем место
        int place = Random.Range(0, 4);

        CellCTRL result = null;
        //Сверху
        if (place == 0)
        {
            //Если не вышли за пределы и ячейка существует
            if (cellFrom.pos.y + 1 < cellFrom.myField.cellCTRLs.GetLength(1) && cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y])
                result = cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y + 1];
        }
        //Снизу
        else if (place == 1)
        {
            //Если не вышли за пределы и ячейка существует
            if (cellFrom.pos.y - 1 >= 0 && cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y])
                result = cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y - 1];
        }
        //Справа
        else if (place == 2)
        {
            //Если не вышли за пределы и ячейка существует
            if (cellFrom.pos.x + 1 < cellFrom.myField.cellCTRLs.GetLength(0) && cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y])
                result = cellFrom.myField.cellCTRLs[cellFrom.pos.x + 1, cellFrom.pos.y];
        }
        //слева
        else if (place == 3)
        {
            //Если не вышли за пределы и ячейка существует
            if (cellFrom.pos.x - 1 >= 0 && cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y])
                result = cellFrom.myField.cellCTRLs[cellFrom.pos.x - 1, cellFrom.pos.y];
        }

        return result;
    }

    public int ComboCount = 1; 

    //Меняемые ячейки
    class Swap {
        public CellCTRL first;
        public CellCTRL second;

        public int stopSwap = 1;
    }

    //Хранит ячейки которые были недавно обменяны, пользователем
    List<Swap> BufferSwap = new List<Swap>();

    /// <summary>
    /// Ячейки
    /// </summary>
    public CellCTRL[,] cellCTRLs;
    /// <summary>
    /// препятствия движения
    /// </summary>
    public BoxBlockCTRL[,] BoxBlockCTRLs;

    /// <summary>
    /// контроллеры камня
    /// </summary>
    public RockCTRL[,] rockCTRLs;

    /// <summary>
    /// Инициализировать игровое поле, на основе данных уровня, или рандомно, если уровня нет
    /// </summary>
    public void inicializeField(LevelsScript.Level level) {
        

        //Перемещаем поле в центр
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f,0.5f);

        //Если уровень есть в базе
        if (level != null)
        {
            AddAllCellLevel();
        }
        else {
            AddAllCellsRandom();
        }

        ScaleField();

        //Заполняем все поля ячейками рандомно
        void AddAllCellsRandom() {
            //Создаем пространство игрового поля
            cellCTRLs = new CellCTRL[10, 10];
            cellsPriority = new CellCTRL[cellCTRLs.GetLength(0) * cellCTRLs.GetLength(1)];

            for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                    if (!cellCTRLs[x,y]) {
                        GameObject cellObj = Instantiate(prefabCell, parentOfCells);
                        

                        //ищем компонент
                        cellCTRLs[x, y] = cellObj.GetComponent<CellCTRL>();
                        cellsPriority[x * cellCTRLs.GetLength(1) + y] = cellCTRLs[x, y];
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

                        //Перерасчет приоритера
                        cellCTRLs[x, y].CalcMyPriority();
                    }
                }
            }
        }

        //заполняем все поля ячейками по шаблону уровня
        void AddAllCellLevel() {

            CountDestroyCrystals = new int[level.NumColors];

            //Создаем пространство игрового поля
            cellCTRLs = new CellCTRL[level.Width, level.Height];
            BoxBlockCTRLs = new BoxBlockCTRL[level.Width, level.Height];
            rockCTRLs = new RockCTRL[level.Width, level.Height];
            cellsPriority = new CellCTRL[cellCTRLs.GetLength(0) * cellCTRLs.GetLength(1)];

            Gameplay.main.colors = level.NumColors;
            Gameplay.main.superColorPercent = level.SuperColorPercent;

            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++)
                {
                    //Нужно ли создавать ячейку
                    LevelsScript.CellInfo cellInfo = level.ReturneCell(new Vector2Int(x,y));
                    
                    //Если этой ячейки не существует
                    if (cellInfo == null)
                    {
                        continue;
                    }
                    else {
                        CountInteractiveCells++;
                    }

                    //Если ячейки нет, создаем
                    if (!cellCTRLs[x, y]) {

                        GameObject cellObj = Instantiate(prefabCell, parentOfCells);
                        //ищем компонент
                        cellCTRLs[x, y] = cellObj.GetComponent<CellCTRL>();
                        cellsPriority[x * cellCTRLs.GetLength(1) + y] = cellCTRLs[x, y];
                        if (!cellCTRLs[x, y])
                        {
                            Destroy(cellObj);
                            break;
                        }

                        cellCTRLs[x, y].pos = new Vector2Int(x, y);
                        cellCTRLs[x, y].myField = this;
                        //Перемещаем объект на свою позицию
                        RectTransform rect = cellObj.GetComponent<RectTransform>();
                        rect.pivot = new Vector2(-x, -y);
                        cellCTRLs[x, y].BlockingMove = level.cells[x, y].boxHealth;
                        cellCTRLs[x, y].rock = level.cells[x, y].rock;
                        cellCTRLs[x, y].mold = level.cells[x, y].moldHealth;
                        
                        if(level.cells[x, y].Panel > 0)
                            cellCTRLs[x, y].panel = true;

                        //рандомизация для тестирования
                        //if (Random.Range(0,100) > 90) {
                        //    cellCTRLs[x, y].BlockingMove = 5;
                        //}
                        //if (Random.Range(0, 100) > 90) {
                        //    cellCTRLs[x, y].mold = 5;
                        //}
                        //if (Random.Range(0, 100) > 90) {
                        //    cellCTRLs[x, y].panel = true;
                        //}
                    }

                    //Нужно ли создать ящик
                    if (cellCTRLs[x, y].BlockingMove > 0) {
                        GameObject BoxBlockObj = Instantiate(prefabBoxBlock, parentOfBoxBlock);
                        BoxBlockCTRL boxBlockCTRL = BoxBlockObj.GetComponent<BoxBlockCTRL>();
                        cellCTRLs[x, y].BoxBlockCTRL = boxBlockCTRL;

                        //Инициализируем коробку
                        boxBlockCTRL.Inicialize(cellCTRLs[x, y]);
                    }

                    //Нужно ли создать плесень
                    if (cellCTRLs[x, y].mold > 0) {
                        GameObject MoldObj = Instantiate(prefabMold, parentOfMold);
                        MoldCTRL moldCTRL = MoldObj.GetComponent<MoldCTRL>();
                        cellCTRLs[x, y].moldCTRL = moldCTRL;

                        //Инициализация плесени
                        moldCTRL.inicialize(cellCTRLs[x, y]);
                    }
                    //Нужно ли создать панель
                    if (cellCTRLs[x,y].panel) {
                        GameObject panelObj = Instantiate(prefabPanel, parentOfPanels);
                        cellCTRLs[x, y].panelCTRL = panelObj.GetComponent<PanelSpreadCTRL>();

                        //Инициализация плесени
                        cellCTRLs[x, y].panelCTRL.inicialize(cellCTRLs[x, y]);
                    }
                    //Нужно ли создать камень
                    if (cellCTRLs[x, y].rock > 0) {
                        GameObject rockObj = Instantiate(prefabRock, parentOfRock);
                        cellCTRLs[x, y].rockCTRL = rockObj.GetComponent<RockCTRL>();

                        //Инициализация камня
                        cellCTRLs[x, y].rockCTRL.inicialize(cellCTRLs[x, y]);
                    }

                    //Создаем подвижные объекты
                    if (level.cells[x, y].typeCell != CellInternalObject.Type.none &&
                        cellCTRLs[x, y].BlockingMove == 0 //Если нету ящика
                        )
                    {
                        if (level.cells[x, y].typeCell == CellInternalObject.Type.color) {
                            //Создаем объект и перемещаем
                            GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                            CellInternalObject internalCtrl = internalObj.GetComponent<CellInternalObject>();
                            internalCtrl.myField = this;
                            internalCtrl.StartMove(cellCTRLs[x, y]);
                            internalCtrl.EndMove();

                            //Меняем тип объекта
                            internalCtrl.setColorAndType(cellInfo.colorCell, level.cells[x, y].typeCell);

                            internalCtrl.color = cellInfo.colorCell;

                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.airplane) {
                            CreateFly(cellCTRLs[x, y], cellInfo.colorCell, 0);
                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.bomb) {
                            CreateBomb(cellCTRLs[x, y], cellInfo.colorCell, 0);
                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.rocketHorizontal) {
                            CreateRocketHorizontal(cellCTRLs[x, y], cellInfo.colorCell, 0);
                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.rocketVertical) {
                            CreateRocketVertical(cellCTRLs[x, y], cellInfo.colorCell, 0);
                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.color5) {
                            CreateSuperColor(cellCTRLs[x, y], cellInfo.colorCell, 0);
                        }
                        
                    }

                    //Перерасчет приоритера
                    cellCTRLs[x, y].CalcMyPriority();

                }
            }
        }

        //Подогнать размер поля в зависимости от количества ячеек
        void ScaleField() {
            //100% размер это поле с 10-ю ячейками

            if (!myRect) myRect = GetComponent<RectTransform>();

            //вычисляем какого размера должно быть поле
            float cellsWeight = (float)cellCTRLs.GetLength(0);

            float sizeNeed = 10 / cellsWeight;

            //Устанавливаем размер поля
            myRect.localScale = new Vector3(sizeNeed, sizeNeed, 1);
            myRect.sizeDelta = new Vector2(100 * cellCTRLs.GetLength(0), 100 * cellCTRLs.GetLength(1));
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
        isMoving();
        TestFieldCombination(true); //Тестим комбинации с применением урона

        TestFieldPotencial(); //ищем потенциальные ходы
        TestRandomNotPotencial(); //Рандомизируем если ходов не обнаружено

        TestStartSwap(); //Начинаем обмен
        TestReturnSwap(); //Возвращяем обмен

        TestMold(); //Выполняем действия после хода

        TestCalcPriorityCells(); //Вычисление приоритета ячеек
    }

    //Стартовая инициализация игрового поля
    void StartInicialize() {
        //Добавляем в поле все ячейки

        timeLastMove = Time.unscaledTime;

        //RandomInicialize();

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

                        //если ячейка пустая добавляем внутренность и без коробки
                        if (!cellCTRLs[x,y].cellInternal && cellCTRLs[x,y].BlockingMove <= 0) {
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
                if (cellCTRLs[x,y] && !cellCTRLs[x, y].cellInternal && 
                    cellCTRLs[x,y].BlockingMove == 0 &&
                    cellCTRLs[x,y].rock == 0) {

                    //Проверяем сверху на то есть ли там что-то что может упасть
                    for (int plusY = 0; plusY <= cellCTRLs.GetLength(1); plusY++) {

                        //Если достигли самого верха поля
                        if (y + plusY >= cellCTRLs.GetLength(1) &&
                            Time.unscaledTime - timeLastBoom > 0.1f)
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
                        else if (y + plusY < cellCTRLs.GetLength(1) && (
                            !cellCTRLs[x, y + plusY] ||
                            cellCTRLs[x, y + plusY].BlockingMove > 0 ||
                            cellCTRLs[x, y + plusY].rock > 0
                            ))
                        {
                            break;
                        }

                        //За перемещение ниже отвечает сам перемещаемый объект

                        //Если сверху есть ячейка с внутренностью и она не блокирована
                        else if (y + plusY < cellCTRLs.GetLength(1) && cellCTRLs[x, y + plusY].BlockingMove <= 0 && cellCTRLs[x, y + plusY].cellInternal && cellCTRLs[x, y + plusY].rock <= 0) {

                            break;
                        }
                    }

                        
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///Поиск и анализ комбинаций

    //Список линий которые взорвались
    List<Combination> listCombinations;
    //Проверяем все ячейки на комбинаци

    //Класс комбинации, для отслеживания что собралось
    /// <summary>
    /// Класс комбинации для отслеживания что 
    /// </summary>
    public class Combination {
        static int IDLast = 0; //последняя созданная комбинация

        public List<CellCTRL> cells = new List<CellCTRL>();

        public int ID = 0;

        public bool horizontal = false;
        public bool vertical = false;

        public bool line5 = false;
        public bool square = false;
        public bool line4 = false;
        public bool cross = false;

        public float timeLastAction = 0;

        public bool foundPanel = false;
        public bool foundBenefit = false;

        public Combination(){
            IDLast++; //Прибавляем id комбинации
            ID = IDLast; //Это наш номер
        }

        public Combination(Combination ParentComb) {

            if (ParentComb != null)
            {
                ID = ParentComb.ID; //Тотже id Поскольку являемся продолжением комбинации родителя
                cells = ParentComb.cells;

                horizontal = ParentComb.horizontal;
                vertical = ParentComb.vertical;
                line5 = ParentComb.line5;
                square = ParentComb.square;
                line4 = ParentComb.line4;
                cross = ParentComb.cross;


                if (ParentComb.foundPanel)
                {
                    foundPanel = ParentComb.foundPanel;
                }
                if (ParentComb.foundBenefit)
                {
                    foundBenefit = ParentComb.foundBenefit;
                }
            }
            else {

                IDLast++; //Прибавляем id комбинации
                ID = IDLast; //Это наш номер

            }
            
        }
    }
    void TestFieldCombination(bool damageOnCombinations) {
        //Создаем новый список комбинаций
        listCombinations = new List<Combination>();

        //Начиная сверху проверяем все ячейки на комбинацию
        for (int y = cellCTRLs.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                bool test = false;
                if (x == 2 && y == 1) {
                    test = true;
                }
                TestCellCombination(cellCTRLs[x, y]);
            }
        }

        DestroyDuplicateCombinations();

        //Если не нужно раздавать урон выходим
        if (!damageOnCombinations) return;


        TestHitShop();

        bool isFoundSuperComb = false;
        TestSuperCombination();

        if (!isFoundSuperComb)
        {
            TestDamageAndSpawn();
        }
        //TestSuperCombination();

        ///////////////////////////////////////////////////////////////
        //Проверить ячейку на комбинации. вариант 2 2021.08.18
        void TestCellCombination(CellCTRL Cell)
        {

            //Выходим если ячейки нет, или нет внутренности, или внутренность в движении, или это кристал, который не должен собираться в комбинации
            if (!Cell || !Cell.cellInternal || (Cell.cellInternal.isMove && damageOnCombinations) || Cell.cellInternal.type == CellInternalObject.Type.color5)
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
                bool test = false;
                if (cellLineRight.Count + cellLineLeft.Count > 1 ||
                    cellLineDown.Count + cellLineUp.Count > 1) {
                    test = true;
                }

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


            //Проверить ячейку на совпадение цвета и возможность добавления в комбинацию
            bool TestCellColor(CellCTRL SecondCell)
            {
                bool test = false;
                if (SecondCell && SecondCell.cellInternal != null && SecondCell.cellInternal.type == CellInternalObject.Type.color5) {
                    test = true;
                }

                //Отмена если
                if (
                    !SecondCell || //если самой ячейки нет
                    !SecondCell.cellInternal || //В ячейке нет внутренности
                    !Cell.cellInternal || //если объекта в ячейке нет
                    (SecondCell.cellInternal.isMove && damageOnCombinations) || //если эти внутренности находятся в движении
                    SecondCell.cellInternal.color != Cell.cellInternal.color || //Цвет не совпал
                    SecondCell.cellInternal.type == CellInternalObject.Type.color5 || //Тип ячейки супер кристал, он не должет сам активироваться оп этому его в комбинации нет
                    (SecondCell.cellInternal.type == CellInternalObject.Type.bomb && SecondCell.cellInternal.activateNeed) //Активированаая бомба не участвует в комбинациях
                    )
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
                if (swap.first.cellInternal == null || swap.second.cellInternal == null || 
                    swap.first.cellInternal.isMove || swap.second.cellInternal.isMove) {
                    continue;
                }

                Combination comb = new Combination();
                comb.cells.Add(swap.first);
                comb.cells.Add(swap.second);
                if (swap.first.panel || swap.second.panel)
                    comb.foundPanel = true;

                //супер колор + что-то еще
                if (swap.first.cellInternal.type == CellInternalObject.Type.color5 ||
                    swap.second.cellInternal.type == CellInternalObject.Type.color5)
                {

                    if (swap.first.cellInternal.type == CellInternalObject.Type.color5)
                    {
                        swap.first.cellInternal.Activate(CellInternalObject.Type.color5, swap.second.cellInternal, comb);
                        isFoundSuperComb = true;
                    }
                    else
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.color5, swap.first.cellInternal, comb);
                        isFoundSuperComb = true;
                    }

                }

                //Самолет + самолет
                else if (swap.first.cellInternal.type == CellInternalObject.Type.airplane &&
                    swap.second.cellInternal.type == CellInternalObject.Type.airplane)
                {
                    swap.first.cellInternal.Activate(CellInternalObject.Type.airplane, swap.second.cellInternal, comb);
                    isFoundSuperComb = true;
                }


                //Самолет + бомба
                else if (swap.first.cellInternal.type == CellInternalObject.Type.airplane && swap.second.cellInternal.type == CellInternalObject.Type.bomb)
                {
                    swap.first.cellInternal.Activate(CellInternalObject.Type.airplane, swap.second.cellInternal, comb);
                    isFoundSuperComb = true;
                }
                //бомба + самолет
                else if (swap.second.cellInternal.type == CellInternalObject.Type.airplane && swap.first.cellInternal.type == CellInternalObject.Type.bomb)
                {
                    swap.second.cellInternal.Activate(CellInternalObject.Type.airplane, swap.first.cellInternal, comb);
                    isFoundSuperComb = true;
                }


                //самолет + ракета
                else if (swap.first.cellInternal.type == CellInternalObject.Type.airplane && (swap.second.cellInternal.type == CellInternalObject.Type.rocketVertical || swap.second.cellInternal.type == CellInternalObject.Type.rocketHorizontal))
                {
                    swap.first.cellInternal.Activate(CellInternalObject.Type.airplane, swap.second.cellInternal, comb);
                    isFoundSuperComb = true;
                }
                //Ракета + самолет
                else if (swap.second.cellInternal.type == CellInternalObject.Type.airplane && (swap.first.cellInternal.type == CellInternalObject.Type.rocketVertical || swap.first.cellInternal.type == CellInternalObject.Type.rocketHorizontal))
                {
                    swap.second.cellInternal.Activate(CellInternalObject.Type.airplane, swap.first.cellInternal, comb);
                    isFoundSuperComb = true;
                }

                //Бомба + что-то но не цвет
                else if ((swap.first.cellInternal.type == CellInternalObject.Type.bomb && swap.second.cellInternal.type != CellInternalObject.Type.color) ||
                    (swap.second.cellInternal.type == CellInternalObject.Type.bomb && swap.first.cellInternal.type != CellInternalObject.Type.color))
                {

                    if (swap.second.cellInternal.type == CellInternalObject.Type.bomb)
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.bomb, swap.first.cellInternal, comb);
                        isFoundSuperComb = true;
                    }
                    else
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.rocketHorizontal, swap.first.cellInternal, comb);
                        isFoundSuperComb = true;
                    }
                }


                //Ракета + ракета
                else if ((swap.first.cellInternal.type == CellInternalObject.Type.rocketHorizontal || swap.first.cellInternal.type == CellInternalObject.Type.rocketVertical) &&
                    (swap.second.cellInternal.type == CellInternalObject.Type.rocketHorizontal || swap.second.cellInternal.type == CellInternalObject.Type.rocketVertical))
                {

                    if (swap.first.cellInternal.type == CellInternalObject.Type.rocketHorizontal)
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.rocketHorizontal, swap.first.cellInternal, comb);
                        isFoundSuperComb = true;
                    }
                    else if (swap.first.cellInternal.type == CellInternalObject.Type.rocketVertical)
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.rocketVertical, swap.first.cellInternal, comb);
                        isFoundSuperComb = true;
                    }
                    else if (swap.second.cellInternal.type == CellInternalObject.Type.rocketHorizontal)
                    {
                        swap.first.cellInternal.Activate(CellInternalObject.Type.rocketHorizontal, swap.second.cellInternal, comb);
                        isFoundSuperComb = true;
                    }
                    else if (swap.second.cellInternal.type == CellInternalObject.Type.rocketVertical)
                    {
                        swap.first.cellInternal.Activate(CellInternalObject.Type.rocketVertical, swap.second.cellInternal, comb);
                        isFoundSuperComb = true;
                    }

                }


                //Прибавляем счетчик если нашласть комбинация
                if (isFoundSuperComb) {
                    Gameplay.main.MinusMoving(comb);
                }

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
                float timelastMove = 0;
                CellInternalObject.InternalColor color = CellInternalObject.InternalColor.Red;

                //Перебираем все ячейки чтобы получить общую информацию об комбинации
                foreach (CellCTRL cell in comb.cells) {

                    //Выбираем текущую ячейку как место для спавна, выбираем среди цветом потому что эти ячейки исчезнут 100%
                    //Если последняя выбранная ячейка не типа цвет
                    //Или
                    //Если номер проверяемой ячейки больще чем выбранной и тип ячейки цвет
                    if ((CellSpawn.cellInternal.type != CellInternalObject.Type.color && cell.cellInternal.type == CellInternalObject.Type.color) ||
                        (CellSpawn.myInternalNum < cell.myInternalNum && cell.cellInternal.type == CellInternalObject.Type.color)) {
                        
                        CellSpawn = cell;

                        if (cell.cellInternal) {
                            color = cell.cellInternal.color;

                            //Ищем как давно существует этот стак
                            if (timelastMove < cell.cellInternal.timeLastMoving) {
                                timelastMove = cell.cellInternal.timeLastMoving;
                            }
                        }

                    }

                    //Если нашли панель
                    if (cell.panel)
                        comb.foundPanel = true;
                    if (cell.mold > 0)
                        comb.foundBenefit = true;
                }


                if (CellSpawn.cellInternal) {
                    color = CellSpawn.cellInternal.color;
                }


                //Если цвет ультимативный
                if (comb.cells[0].cellInternal.color == CellInternalObject.InternalColor.Ultimate) {
                    ActivateCombUltimate();
                }
                //Иначе обычное поведение
                else {
                    ActivateCombNormal();
                }

                void ActivateCombNormal() {
                    //Удаляем ячейки комбинации
                    //Раздать ячейкам урон или перемешать если игра еще не началась
                    foreach (CellCTRL c in comb.cells)
                    {

                        if (Gameplay.main.movingCount <= 0)
                        {
                            mixColor();
                        }
                        else
                        {
                            SetDamage();
                        }

                        //перемешать цвет
                        void mixColor()
                        {
                            c.cellInternal.randColor();
                        }
                        //Нанести урон
                        void SetDamage()
                        {

                            CellInternalObject partner = null;
                            //Отнимаем ход если комбинация получилась благодаря перемещениям игрока
                            List<Swap> BufferSwapNew = new List<Swap>();
                            foreach (Swap swap in BufferSwap)
                            {
                                //Если комбинация получилась благодаря перемещению игрока
                                if (swap.first == c || swap.second == c)
                                {

                                    Gameplay.main.MinusMoving(comb);

                                    //Запомнить  партнера по перемещениям
                                    //if (swap.first != c) partner = c.cellInternal;
                                    //else if (swap.second != c) partner = c.cellInternal;

                                    continue;
                                }
                                BufferSwapNew.Add(swap);
                            }
                            BufferSwap = BufferSwapNew;

                            //Наносим урон по клетке
                            c.Damage(partner, comb);

                        }
                    }

                    //Точка спавна есть, теперь проверяем что нужно спавнить
                    //Поставить новый обьект на место
                    if (Gameplay.main.movingCount > 0)
                    {
                        //если линия из 5
                        if (comb.line5)
                        {
                            //Создаем супер цветовую бомбу
                            CreateSuperColor(CellSpawn, color, comb.ID);
                        }
                        //Если крест
                        else if (comb.cross)
                        {
                            //Создаем бомбу
                            CreateBomb(CellSpawn, color, comb.ID);
                        }
                        //Если горизонтальная из 4
                        else if (comb.line4 && comb.horizontal)
                        {
                            CreateRocketVertical(CellSpawn, color, comb.ID);
                        }
                        //Если вертикальная из 3
                        else if (comb.line4 && comb.vertical)
                        {
                            CreateRocketHorizontal(CellSpawn, color, comb.ID);
                        }
                        //Если квадрат
                        else if (comb.square)
                        {
                            //нечто летающее
                            CreateFly(CellSpawn, color, comb.ID);
                        }

                        //Комбинация завершена повышаем комбо
                        ComboCount++;
                    }
                }

                void ActivateCombUltimate() {
                    //Удаляем ячейки комбинации
                    //Раздать ячейкам урон или перемешать если игра еще не началась
                    foreach (CellCTRL c in comb.cells)
                    {

                        if (Gameplay.main.movingCount <= 0)
                        {
                            mixColor();
                        }
                        else
                        {
                            SetDamage();
                        }

                        //перемешать цвет
                        void mixColor()
                        {
                            c.cellInternal.randColor();
                        }
                        //Нанести урон
                        void SetDamage()
                        {

                            CellInternalObject partner = null;
                            //Отнимаем ход если комбинация получилась благодаря перемещениям игрока
                            List<Swap> BufferSwapNew = new List<Swap>();
                            foreach (Swap swap in BufferSwap)
                            {
                                //Если комбинация получилась благодаря перемещению игрока
                                if (swap.first == c || swap.second == c)
                                {

                                    Gameplay.main.MinusMoving(comb);

                                    //Запомнить  партнера по перемещениям
                                    //if (swap.first != c) partner = c.cellInternal;
                                    //else if (swap.second != c) partner = c.cellInternal;

                                    continue;
                                }
                                BufferSwapNew.Add(swap);
                            }
                            BufferSwap = BufferSwapNew;

                            //Наносим урон по клетке
                            c.Damage(partner, comb);
                        }
                    }


                    //Перебираем ячейки и раздаем указанный урон
                    //если линия из 5 уничтожить все
                    if (comb.line5)
                    {
                        //ищем
                        //Перебираем ячейки в поисках переклестной ячейки
                        CellCTRL cellCross = comb.cells[0];
                        foreach (CellCTRL cell in comb.cells)
                        {
                            if (cell.myInternalNum > cellCross.myInternalNum)
                            {
                                cellCross = cell;
                            }
                        }

                        //Перебираем всю карту
                        for (int x = 0; x < cellCTRLs.GetLength(0); x++)
                        {
                            for (int y = 0; y < cellCTRLs.GetLength(1); y++)
                            {
                                //Если ячейки нет выходим
                                if (!cellCTRLs[x, y])
                                {
                                    continue;
                                }

                                //находим растояние от ячейки от куда должен распространяться взрыв
                                float dist = Vector2.Distance(cellCross.pos, cellCTRLs[x, y].pos);

                                cellCross.myField.cellCTRLs[x, y].BufferCombination = comb;
                                cellCross.myField.cellCTRLs[x, y].BufferNearDamage = false;
                                cellCTRLs[x, y].DamageInvoke(dist * 0.05f);
                            }
                        }

                    }
                    //Если собрался крест
                    else if (comb.cross)
                    {
                        //Перебираем ячейки в поисках переклестной ячейки
                        //Крест собирается только в месте куда переместили последнюю ячейку
                        CellCTRL cellCross = comb.cells[0];
                        foreach (CellCTRL cell in comb.cells)
                        {
                            if (cell.myInternalNum > cellCross.myInternalNum)
                            {
                                cellCross = cell;
                            }
                        }

                        int radius = 3;
                        //Ячейку выбрали.. Взрываем на 3 ячейки влево вправо вверх вниз
                        for (int x = -radius; x <= radius; x++)
                        {
                            for (int y = -radius; y <= radius; y++)
                            {
                                int fieldPosX = cellCross.pos.x + x;
                                int fieldPosY = cellCross.pos.y + y;
                                //Если вышли за пределы карты или этой ячейки нету
                                if (fieldPosX < 0 || fieldPosX >= cellCross.myField.cellCTRLs.GetLength(0) ||
                                    fieldPosY < 0 || fieldPosY >= cellCross.myField.cellCTRLs.GetLength(1) ||
                                    !cellCross.myField.cellCTRLs[fieldPosX, fieldPosY]
                                    )
                                {
                                    continue;
                                }

                                //Считаем время задержки взрыва этой ячейки
                                float time = Vector2.Distance(new Vector2(), new Vector2(x, y)) * 0.1f;
                                //
                                cellCTRLs[fieldPosX, fieldPosY].BufferCombination = comb;
                                cellCTRLs[fieldPosX, fieldPosY].BufferNearDamage = false;
                                cellCTRLs[fieldPosX, fieldPosY].DamageInvoke(time);
                            }
                        }

                        //Создаем частицы взрыва
                        Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateBoomBomb(gameObject, cellCross, radius);
                        particle3DCTRL.SetSpeed(radius);
                        particle3DCTRL.SetSize(radius * 3);
                        particle3DCTRL.SetColor(cellCross.cellInternal.GetColor(color) * 0.5f);

                    }
                    //Если собрался квадрат
                    else if (comb.square)
                    {
                        //Проверяем ячейки
                        int count = 0;
                        foreach (CellCTRL cellCTRL in comb.cells)
                        {
                            if (cellCTRL)
                            {
                                count++;
                                //Если было больще 4 запусков выходим
                                if (count > 4)
                                {
                                    break;
                                }

                                //если там уже есть самолет, запускаем
                                if (cellCTRL.cellInternal && cellCTRL.cellInternal.type == CellInternalObject.Type.airplane)
                                {
                                    cellCTRL.cellInternal.Activate(CellInternalObject.Type.airplane, null, comb);
                                    break;
                                }

                                //Удаляем внутренний объект
                                Destroy(cellCTRL.cellInternal.gameObject);

                                //Запускаем с этой ячейки самолет
                                //Создаем самолет
                                CreateFly(cellCTRL, cellCTRL.cellInternal.color, comb.ID);
                                //Запускаем замолет
                                cellCTRL.cellInternal.Activate(CellInternalObject.Type.airplane, null, comb);
                            }
                        }
                    }
                    //Если собралась горизонталь
                    else if (comb.horizontal && comb.line4)
                    {
                        foreach (CellCTRL c in comb.cells)
                        {
                            //Вертикально запускаем взрыв
                            c.explosion = new CellCTRL.Explosion(false, false, true, true, 0.1f, comb);
                            c.BufferCombination = comb;
                            c.BufferNearDamage = false;
                            c.ExplosionBoomInvoke(c.explosion, c.explosion.timer);
                        }
                    }
                    //Если собралась вертикаль
                    else if (comb.vertical && comb.line4) {
                        foreach (CellCTRL c in comb.cells)
                        {
                            //Вертикально запускаем взрыв
                            c.explosion = new CellCTRL.Explosion(true, true, false, false, 0.1f, comb);
                            c.BufferCombination = comb;
                            c.BufferNearDamage = false;
                            c.ExplosionBoomInvoke(c.explosion, c.explosion.timer);
                        }
                    }

                }
            }

        }
        

        //Уничтожить повторяющиеся комбинации
        void DestroyDuplicateCombinations() {

            //Если списка нет сразу выходим
            if (listCombinations.Count == 0) return;


            //список комбинаций на удаление из списка так как повтряющиеся
            List<Combination> listCombinationsDelete = new List<Combination>();

            //Перебираем текущие комбинации в поисках совпадений
            foreach (Combination combinationFirst in listCombinations) {

                foreach (Combination combinationSecond in listCombinations)
                {
                    //переключаемся если это таже самая комбинация
                    if (combinationFirst == combinationSecond) continue;


                    bool foundIdenticalCells = false;
                    //Перебираем ячейки здесь и там в поисках совпадения
                    foreach (CellCTRL cell in combinationFirst.cells) {
                        if (foundIdenticalCells) break;
                        foreach (CellCTRL cellNew in combinationSecond.cells) {
                            //Если ячейка из прошлого списка совпадает с ячейкой из другого
                            if (cell == cellNew) {
                                //То это значит что это должна была быть одна комбинация но она разделилась
                                foundIdenticalCells = true;
                                break;
                            }
                        }
                    }

                    //Если было найденно ячеистое совпадение
                    if (foundIdenticalCells)
                    {
                        //Если в первой ячеек меньше добавляем на удаление
                        if (combinationFirst.cells.Count < combinationSecond.cells.Count)
                        {
                            listCombinationsDelete.Add(combinationFirst);
                        }
                        //Если во второй ячеек меньше, добавляем ее на удаление
                        else if(combinationFirst.cells.Count > combinationSecond.cells.Count) {
                            listCombinationsDelete.Add(combinationSecond);
                        }
                    }
                }
            }

            //Создаем новый список ячеек и заполняем только комбинациями без повторений
            List<Combination> combinationsNew = new List<Combination>();
            foreach (Combination combTest in listCombinations) {
                bool needAdd = true;
                foreach (Combination combIgnore in listCombinationsDelete) {
                    //Если это комбинация которой нужно избежать
                    if (combTest == combIgnore) {
                        needAdd = false;
                        break;
                    }
                }

                //Добавляем если можно
                if (needAdd) combinationsNew.Add(combTest);

            }

            //Заменяем старый лист новым
            listCombinations = combinationsNew;

        }

        //Проверка на использование платного супер удара
        void TestHitShop() {
            //Если только что поменялся супер удар
            if (buffer.superHit != MenuGameplay.main.SuperHitSelected) {
                //Снимаем выделение с ячейки
                CellSelect = null;

                buffer.superHit = MenuGameplay.main.SuperHitSelected;
            }

            //Если сейчас активен супер удар
            if (buffer.superHit != MenuGameplay.SuperHitType.none) {
                //Если не выбранна, выходим
                if (CellSelect == null) {
                    return;
                }

                if (buffer.superHit == MenuGameplay.SuperHitType.internalObj)
                    TestShopInternal();
                else if (buffer.superHit == MenuGameplay.SuperHitType.rosket2x)
                    TestShopRocket();
                else if(buffer.superHit == MenuGameplay.SuperHitType.superColor)
                    TestShopSuperColor();

                //Активировали, возвращаем ничего
                MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;
            }

            void TestShopInternal() {
                if (Gameplay.main.buttonDestroyInternal > 0) {
                    Gameplay.main.buttonDestroyInternal--;

                    //Создаем комбинацию
                    Combination comb = new Combination();
                    comb.cells.Add(CellSelect);

                    //если ячейки есть
                    if (CellSelect.panel)
                        comb.foundPanel = true;
                    if (CellSelect.mold > 0)
                        comb.foundBenefit = true;

                    CellSelect.BufferCombination = comb;
                    CellSelect.DamageInvoke(0.25f);
                    MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;
                }
            }
            void TestShopRocket() {
                if (Gameplay.main.buttonRosket > 0) {
                    Gameplay.main.buttonRosket--;

                    //Создаем комбинацию
                    Combination comb = new Combination();
                    comb.cells.Add(CellSelect);

                    //если ячейки есть
                    if (CellSelect.panel)
                        comb.foundPanel = true;
                    if (CellSelect.mold > 0)
                        comb.foundBenefit = true;

                    CellSelect.BufferCombination = comb;

                    CellSelect.explosion = new CellCTRL.Explosion(true, true, true, true, 0.05f, comb);
                    CellSelect.ExplosionBoomInvoke(CellSelect.explosion, CellSelect.explosion.timer);

                    MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;
                }

            }
            void TestShopSuperColor() {

            }
        }

    }


    List<PotencialComb> listPotencial = new List<PotencialComb>();
    PotencialComb potencialBest = null;
    public class PotencialComb {
        public CellCTRL Moving = null; //Ячейка которую необходимо передвинуть
        public CellCTRL Target = null; //Ячейка относитенльно которой проверяем комбинации

        public List<CellCTRL> cells = new List<CellCTRL>();

        public int priority = 0;
        
        public bool line5 = false;
        public bool square = false;
        public bool line4 = false;
        public bool cross = false;

        public bool foundNotPanel = false;
        public bool foundPanel = false;
        public bool foundMold = false;
        public bool foundRock = false;

        public void CalcPriority() {
            //Проверяем приоритет ячеек
            foreach (CellCTRL cell in cells) {
                PlusPriority(cell);
            }
            PlusPriority(Target);

            if (foundPanel) {
                priority += 90;
            }
            else if (foundPanel && foundNotPanel) {
                priority += 110;
            }

            if (foundMold) {
                priority += 80;
            }
            if (foundRock) {
                priority += 100;
            }

            if (cells.Count == 4)
                priority += 100;
            if (cells.Count == 3)
                priority += 90;

            void PlusPriority(CellCTRL cell) {
                priority += cell.MyPriority;
                if (cell.panel)
                {
                    foundPanel = true;
                }
                else foundNotPanel = true;

                if (cell.mold > 0)
                {
                    foundMold = true;
                }
                if (cell.rock > 0)
                {
                    foundRock = true;
                }

            }
        }
    }

    [SerializeField]
    public Vector2Int testCell = new Vector2Int();
    /// <summary>
    /// Поиск потенциальных комбинаций
    /// </summary>
    /// 
    void TestFieldPotencial() {

        //Воспроизводим анимацию на ячейках с потенциалом
        if (Gameplay.main.isMissionComplite() || Gameplay.main.isMissionDefeat())
        {
            potencialBest = null;
        }
        AnimationPlayPotencial();

        float timeToTest = 2;
        //Если недавно было движение то обнуляем лист
        if (Time.unscaledTime - timeLastMove < timeToTest) {
            listPotencial = new List<PotencialComb>();
            potencialBest = null;

            Debug.Log("Waiting Time " + Time.unscaledTime);

            return;
        }

        //ВЫходим если список уже есть или миссия провалена или выполнена
        if (listPotencial.Count > 0 || Gameplay.main.isMissionComplite() || Gameplay.main.isMissionDefeat()) {
            return;
        }

        //Начинаем поиск потенциальных комбинаций
        ReCalcListPotencial();

        //Если комбинации нашлись
        if (listPotencial.Count > 0) {
            //ВЫбираем лучшую
            if (potencialBest == null) {
                potencialBest = listPotencial[0];

                foreach (PotencialComb potencial in listPotencial) {
                    if (potencialBest.priority < potencial.priority) {
                        potencialBest = potencial;
                    }
                }
            }

            Debug.Log("Found potencial: " + potencialBest.priority + " Pos: " + potencialBest.Target.pos + " Target:" + potencialBest.Target.pos + " Moving:" + potencialBest.Moving.pos);
        }
        //если комбинаций не нашлось и лист нужных к перемещению объектов еще не создан
        else if(ListWaitingInternals.Count <= 0){
            CreateRandomInternalList();
            Debug.Log("Not Found potencial comb " + Time.unscaledTime);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        //Воспроизвести анимацию потенциала если есть
        void AnimationPlayPotencial()
        {
            if (potencialBest == null)
                return;

            //Если потенциал есть
            //Перебираем ячейки и воспроизводим анимацию
            foreach (CellCTRL potencialCell in potencialBest.cells)
            {
                potencialCell.cellInternal.animatorObject.PlayAnimation("ApperanceCombinationObject");
            }
            potencialBest.Moving.cellInternal.animatorObject.PlayAnimation("ApperanceCombinationObject");
        }

        void CreateRandomInternalList() {
            //перебираем все ячейки
            for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                    if (isCellCanRandomizade(cellCTRLs[x,y])) {
                        ListWaitingInternals.Add(cellCTRLs[x, y].cellInternal);
                    }
                }
            }

            bool isCellCanRandomizade(CellCTRL select) {
                bool result = false;

                if (select != null && select.cellInternal &&
                    select.rock == 0) {
                    result = true;
                }

                return result;
            }
        }
    }

    //Перестроить новый список потенциальных комбинаций
    void ReCalcListPotencial() {

        

        //Начинаем поиск потенциальных комбинаций
        for (int x = 0; x < cellCTRLs.GetLength(0); x++)
        {
            for (int y = 0; y < cellCTRLs.GetLength(1); y++)
            {
                testPotencialCell(cellCTRLs[x, y]);
            }
        }

        void testPotencialCell(CellCTRL cell)
        {

            bool test = false;
            if (cell != null &&
                cell.pos == testCell)
            {
                test = true;
            }

            //Выходим если этой ячейки нет или она заморожена
            if (cell == null ||
                cell.cellInternal == null ||
                cell.rock > 0)
            {
                return;
            }

            PotencialComb potencialLeft = new PotencialComb();
            PotencialComb potencialRight = new PotencialComb();
            PotencialComb potencialUp = new PotencialComb();
            PotencialComb potencialDown = new PotencialComb();



            //Поиск ряда
            //слева
            if (isCellOK(new Vector2Int(cell.pos.x - 1, cell.pos.y)))
            {

                potencialLeft.Target = cell;
                potencialLeft.cells.Add(cellCTRLs[cell.pos.x - 1, cell.pos.y]);


                //Запоминаем цвет
                CellInternalObject.InternalColor color = cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.color;

                //Двигаем дальше пока данный цвет не пропадет
                for (int n = 1; n < cellCTRLs.GetLength(0); n++)
                {
                    //если условия соблюдены и цвет ячейки совпадает
                    if (isCellOK(new Vector2Int(cell.pos.x - 1 - n, cell.pos.y)) &&
                        cellCTRLs[cell.pos.x - 1 - n, cell.pos.y].cellInternal.color == color)
                    {
                        //Добавляем ячейку в список
                        potencialLeft.cells.Add(cellCTRLs[cell.pos.x - 1 - n, cell.pos.y]);
                    }
                    //Иначе заверша ем цикл поиска ряда
                    else
                    {
                        break;
                    }
                }
            }
            //Права
            if (isCellOK(new Vector2Int(cell.pos.x + 1, cell.pos.y)))
            {

                potencialRight.Target = cell;
                potencialRight.cells.Add(cellCTRLs[cell.pos.x + 1, cell.pos.y]);

                //Запоминаем цвет
                CellInternalObject.InternalColor color = cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.color;

                //Двигаем дальше пока данный цвет не пропадет
                for (int n = 1; n < cellCTRLs.GetLength(0); n++)
                {
                    //если условия соблюдены и цвет ячейки совпадает
                    if (isCellOK(new Vector2Int(cell.pos.x + 1 + n, cell.pos.y)) &&
                        cellCTRLs[cell.pos.x + 1 + n, cell.pos.y].cellInternal.color == color)
                    {
                        //Добавляем ячейку в список
                        potencialRight.cells.Add(cellCTRLs[cell.pos.x + 1 + n, cell.pos.y]);
                    }
                    //Иначе заверша ем цикл поиска ряда
                    else
                    {
                        break;
                    }
                }
            }
            //Сверху
            if (isCellOK(new Vector2Int(cell.pos.x, cell.pos.y + 1)))
            {

                potencialUp.Target = cell;
                potencialUp.cells.Add(cellCTRLs[cell.pos.x, cell.pos.y + 1]);

                //Запоминаем цвет
                CellInternalObject.InternalColor color = cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.color;

                //Двигаем дальше пока данный цвет не пропадет
                for (int n = 1; n < cellCTRLs.GetLength(0); n++)
                {
                    //если условия соблюдены и цвет ячейки совпадает
                    if (isCellOK(new Vector2Int(cell.pos.x, cell.pos.y + 1 + n)) &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1 + n].cellInternal.color == color)
                    {
                        //Добавляем ячейку в список
                        potencialUp.cells.Add(cellCTRLs[cell.pos.x, cell.pos.y + 1 + n]);
                    }
                    //Иначе заверша ем цикл поиска ряда
                    else
                    {
                        break;
                    }
                }
            }
            //Снизу
            if (isCellOK(new Vector2Int(cell.pos.x, cell.pos.y - 1)))
            {

                potencialDown.Target = cell;
                potencialDown.cells.Add(cellCTRLs[cell.pos.x, cell.pos.y - 1]);

                //Запоминаем цвет
                CellInternalObject.InternalColor color = cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.color;

                //Двигаем дальше пока данный цвет не пропадет
                for (int n = 1; n < cellCTRLs.GetLength(0); n++)
                {
                    //если условия соблюдены и цвет ячейки совпадает
                    if (isCellOK(new Vector2Int(cell.pos.x, cell.pos.y - 1 - n)) &&
                        cellCTRLs[cell.pos.x, cell.pos.y - 1 - n].cellInternal.color == color)
                    {
                        //Добавляем ячейку в список
                        potencialDown.cells.Add(cellCTRLs[cell.pos.x, cell.pos.y - 1 - n]);
                    }
                    //Иначе заверша ем цикл поиска ряда
                    else
                    {
                        break;
                    }
                }
            }

            //Теперь проверяем соседние ячейки
            //Если обнаружен потенциал, добавляем в список потенциалов
            isHavePotencial(potencialLeft);
            isHavePotencial(potencialRight);
            isHavePotencial(potencialUp);
            isHavePotencial(potencialDown);

            //Проверяем на супер комбинацию
            CombinatePotencialSuper();

            //проверка квадрата
            CombinatePotencialSquare();

            //Теперь проверяем на супер потенциалы
            CombinatePotencialHorizontal(potencialLeft, potencialRight);
            CombinatePotencialVertical(potencialUp, potencialDown);

            CombinatePotencialsAngle(potencialLeft, potencialUp); //Лево верх
            CombinatePotencialsAngle(potencialRight, potencialUp); //Право верх
            CombinatePotencialsAngle(potencialRight, potencialDown); //Право низ
            CombinatePotencialsAngle(potencialLeft, potencialDown); //Лево низ

            //Проверить ячейку на ее существование
            bool isCellOK(Vector2Int cellPos)
            {
                bool result = false;

                //Если ячейка не вышла за пределы
                //Она существует
                //Есть внутренность
                //Внутренность не равна супер цвету
                if (cellPos.x >= 0 && cellPos.x < cellCTRLs.GetLength(0) && cellPos.y >= 0 && cellPos.y < cellCTRLs.GetLength(1) &&
                    cellCTRLs[cellPos.x, cellPos.y] != null &&
                    cellCTRLs[cellPos.x, cellPos.y].cellInternal != null &&
                    cellCTRLs[cellPos.x, cellPos.y].cellInternal.type != CellInternalObject.Type.color5
                    )
                {

                    //Выходим если это бомба которая готова активироваться
                    if (cellCTRLs[cellPos.x, cellPos.y].cellInternal.type == CellInternalObject.Type.bomb && 
                        cellCTRLs[cellPos.x, cellPos.y].cellInternal.activateNeed) {
                        return false;
                    }

                    result = true;
                }

                return result;
            }

            //проверяем комбинацию на потенциал
            bool isHavePotencial(PotencialComb potTest)
            {
                bool result = false;

                //Если ряд больше или равно 1
                if (potTest.cells.Count >= 1)
                {
                    //Проверяем наличие одинаковых цветов по сторонам

                    //Если ячейка есть
                    //Если это не таже самая ячейка из ряда
                    //Если у ячейки есть внутренность
                    //Если у внутренности совпадает цвет
                    //Если внутренность не супер цвет

                    //Слева
                    if (potTest.Target.pos.x - 1 >= 0 &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y] != null &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y] != potTest.cells[0] &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].rock == 0 &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal != null &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal.color == potTest.cells[0].cellInternal.color &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal.type != CellInternalObject.Type.color5 &&
                        !(cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal.activateNeed)
                        )
                    {

                        potTest.Moving = cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y];

                    }
                    //Справа
                    else if (potTest.Target.pos.x + 1 < cellCTRLs.GetLength(0) &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y] != null &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y] != potTest.cells[0] &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].rock == 0 &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal != null &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal.color == potTest.cells[0].cellInternal.color &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal.type != CellInternalObject.Type.color5 &&
                        !(cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal.activateNeed)
                        )
                    {

                        potTest.Moving = cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y];

                    }
                    //Сверху
                    else if (potTest.Target.pos.y + 1 < cellCTRLs.GetLength(1) &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1] != null &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1] != potTest.cells[0] &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].rock == 0 &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal != null &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal.color == potTest.cells[0].cellInternal.color &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal.type != CellInternalObject.Type.color5 &&
                        !(cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal.activateNeed)
                        )
                    {

                        potTest.Moving = cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1];

                    }
                    //Снизу
                    else if (potTest.Target.pos.y - 1 >= 0 &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1] != null &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1] != potTest.cells[0] &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].rock == 0 &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal != null &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal.color == potTest.cells[0].cellInternal.color &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal.type != CellInternalObject.Type.color5 &&
                        !(cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal.activateNeed)
                        )
                    {

                        potTest.Moving = cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1];

                    }
                }

                //
                if (potTest.Moving != null && potTest.cells.Count == 2)
                {
                    listPotencial.Add(potTest);
                    result = true;
                }

                if (potTest.Moving)
                {
                    potTest.CalcPriority();
                }

                return result;

            }

            //Проверка соседних ячеек на супер комбинацию
            void CombinatePotencialSuper()
            {
                //если текущая ячейка не цвет
                if (cell.cellInternal.type == CellInternalObject.Type.color)
                {
                    //выходим
                    return;
                }

                CellCTRL left = null;
                CellCTRL right = null;
                CellCTRL up = null;
                CellCTRL down = null;

                //Проверяем соседние ячейки на то что они тоже не цвет
                //Слева
                if (cell.pos.x - 1 >= 0 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y] != null &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y].rock == 0 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal != null &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.type != CellInternalObject.Type.color &&
                    !(cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.activateNeed)
                    )
                {

                    left = cellCTRLs[cell.pos.x - 1, cell.pos.y];

                }
                //Справа
                if (cell.pos.x + 1 < cellCTRLs.GetLength(0) &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y] != null &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y].rock == 0 &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal != null &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.type != CellInternalObject.Type.color &&
                    !(cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.activateNeed)
                    )
                {

                    right = cellCTRLs[cell.pos.x + 1, cell.pos.y];

                }
                //Сверху
                if (cell.pos.y + 1 < cellCTRLs.GetLength(1) &&
                    cellCTRLs[cell.pos.x, cell.pos.y + 1] != null &&
                    cellCTRLs[cell.pos.x, cell.pos.y + 1].rock == 0 &&
                    cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal != null &&
                    cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.type != CellInternalObject.Type.color &&
                    !(cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.activateNeed)
                    )
                {

                    up = cellCTRLs[cell.pos.x, cell.pos.y + 1];

                }
                //Снизу
                if (cell.pos.y - 1 >= 0 &&
                    cellCTRLs[cell.pos.x, cell.pos.y - 1] != null &&
                    cellCTRLs[cell.pos.x, cell.pos.y - 1] != null &&
                    cellCTRLs[cell.pos.x, cell.pos.y - 1].rock == 0 &&
                    cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal != null &&
                    cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.type != CellInternalObject.Type.color &&
                    !(cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.activateNeed)
                                        )
                {

                    down = cellCTRLs[cell.pos.x, cell.pos.y - 1];
                }


                //Если текущая ячейка супер цвет
                if (cell.cellInternal.type == CellInternalObject.Type.color5)
                {
                    //проверяем соседей на то что там обычный цвет
                    //Слева
                    if (cell.pos.x - 1 >= 0 &&
                        cellCTRLs[cell.pos.x - 1, cell.pos.y] != null &&
                        cellCTRLs[cell.pos.x - 1, cell.pos.y].rock == 0 &&
                        cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal != null &&
                        cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.color)
                    {

                        left = cellCTRLs[cell.pos.x - 1, cell.pos.y];

                    }
                    //Справа
                    if (cell.pos.x + 1 < cellCTRLs.GetLength(0) &&
                        cellCTRLs[cell.pos.x + 1, cell.pos.y] != null &&
                        cellCTRLs[cell.pos.x + 1, cell.pos.y].rock == 0 &&
                        cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal != null &&
                        cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.color)
                    {

                        right = cellCTRLs[cell.pos.x + 1, cell.pos.y];

                    }
                    //Сверху
                    if (cell.pos.y + 1 < cellCTRLs.GetLength(1) &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1] != null &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1].rock == 0 &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal != null &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.type == CellInternalObject.Type.color)
                    {

                        up = cellCTRLs[cell.pos.x, cell.pos.y + 1];

                    }
                    //Снизу
                    if (cell.pos.y - 1 >= 0 &&
                        cellCTRLs[cell.pos.x, cell.pos.y - 1] != null &&
                        cellCTRLs[cell.pos.x, cell.pos.y - 1] != null &&
                        cellCTRLs[cell.pos.x, cell.pos.y - 1].rock == 0 &&
                        cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal != null &&
                        cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.type == CellInternalObject.Type.color)
                    {

                        down = cellCTRLs[cell.pos.x, cell.pos.y - 1];
                    }
                }

                int superPriority = 200;
                //если сбоку обнаружена подходящая ячейка
                if (left != null)
                {
                    PotencialComb super = new PotencialComb();

                    super.Moving = cell;
                    super.cells.Add(left);
                    super.Target = left;

                    //Теперь комбинация собрана, считаем ее приоритет
                    super.CalcPriority();
                    super.priority += superPriority;

                    listPotencial.Add(super);
                }
                if (right != null)
                {
                    PotencialComb super = new PotencialComb();

                    super.Moving = cell;
                    super.cells.Add(right);
                    super.Target = right;

                    //Теперь комбинация собрана, считаем ее приоритет
                    super.CalcPriority();
                    super.priority += superPriority;

                    listPotencial.Add(super);
                }
                if (up != null)
                {
                    PotencialComb super = new PotencialComb();

                    super.Moving = cell;
                    super.cells.Add(up);
                    super.Target = up;

                    //Теперь комбинация собрана, считаем ее приоритет
                    super.CalcPriority();
                    super.priority += superPriority;

                    listPotencial.Add(super);
                }
                if (down != null)
                {
                    PotencialComb super = new PotencialComb();

                    super.Moving = cell;
                    super.cells.Add(down);
                    super.Target = down;

                    //Теперь комбинация собрана, считаем ее приоритет
                    super.CalcPriority();
                    super.priority += superPriority;

                    listPotencial.Add(super);
                }
            }

            //Потенциальный квадрат
            void CombinatePotencialSquare()
            {


                //верх лево
                if (potencialUp.cells.Count > 0 && potencialLeft.cells.Count > 0 &&
                    potencialUp.cells[0].cellInternal.color == potencialLeft.cells[0].cellInternal.color &&
                    ((potencialUp.cells[0] != potencialLeft.Moving && potencialLeft.Moving) || (potencialUp.Moving != potencialLeft.cells[0] && potencialUp.Moving)) &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1] != null &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1].rock == 0 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1].cellInternal != null &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1].cellInternal.color == potencialLeft.cells[0].cellInternal.color
                    )
                {

                    PotencialComb super = new PotencialComb();

                    foreach (CellCTRL cellAdd in potencialLeft.cells)
                        super.cells.Add(cellAdd);

                    foreach (CellCTRL cellAdd in potencialUp.cells)
                        super.cells.Add(cellAdd);


                    //Добавляем крайнюю
                    super.cells.Add(cellCTRLs[cell.pos.x - 1, cell.pos.y + 1]);

                    super.Target = cell;

                    if (potencialUp.cells[0] != potencialLeft.Moving && potencialLeft.Moving)
                    {
                        super.Moving = potencialLeft.Moving;
                    }
                    else
                    {
                        super.Moving = potencialUp.Moving;
                    }

                    //Теперь комбинация собрана, считаем ее приоритет
                    super.CalcPriority();
                    super.priority += 20;

                    listPotencial.Add(super);
                }

                //верх право
                if (potencialUp.cells.Count > 0 && potencialRight.cells.Count > 0 &&
                    potencialUp.cells[0].cellInternal.color == potencialRight.cells[0].cellInternal.color &&
                    ((potencialUp.cells[0] != potencialRight.Moving && potencialRight.Moving) || (potencialUp.Moving != potencialRight.cells[0] && potencialUp.Moving)) &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1] != null &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1].rock == 0 &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1].cellInternal != null &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1].cellInternal.color == potencialRight.cells[0].cellInternal.color)
                {

                    PotencialComb super = new PotencialComb();

                    foreach (CellCTRL cellAdd in potencialRight.cells)
                        super.cells.Add(cellAdd);

                    foreach (CellCTRL cellAdd in potencialUp.cells)
                        super.cells.Add(cellAdd);


                    //Добавляем крайнюю
                    super.cells.Add(cellCTRLs[cell.pos.x + 1, cell.pos.y + 1]);

                    super.Target = cell;

                    if (potencialUp.cells[0] != potencialRight.Moving && potencialRight.Moving)
                    {
                        super.Moving = potencialRight.Moving;
                    }
                    else
                    {
                        super.Moving = potencialUp.Moving;
                    }

                    //Теперь комбинация собрана, считаем ее приоритет
                    super.CalcPriority();
                    super.priority += 20;

                    listPotencial.Add(super);
                }

                //низ право
                if (potencialDown.cells.Count > 0 && potencialRight.cells.Count > 0 &&
                    potencialDown.cells[0].cellInternal.color == potencialRight.cells[0].cellInternal.color &&
                    ((potencialDown.cells[0] != potencialRight.Moving && potencialRight.Moving) || (potencialDown.Moving != potencialRight.cells[0] && potencialDown.Moving)) &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1] != null &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1].rock == 0 &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1].cellInternal != null &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1].cellInternal.color == potencialRight.cells[0].cellInternal.color)
                {

                    PotencialComb super = new PotencialComb();

                    foreach (CellCTRL cellAdd in potencialRight.cells)
                        super.cells.Add(cellAdd);

                    foreach (CellCTRL cellAdd in potencialDown.cells)
                        super.cells.Add(cellAdd);


                    //Добавляем крайнюю
                    super.cells.Add(cellCTRLs[cell.pos.x + 1, cell.pos.y - 1]);

                    super.Target = cell;

                    if (potencialDown.cells[0] != potencialRight.Moving && potencialRight.Moving)
                    {
                        super.Moving = potencialRight.Moving;
                    }
                    else
                    {
                        super.Moving = potencialDown.Moving;
                    }

                    //Теперь комбинация собрана, считаем ее приоритет
                    super.CalcPriority();
                    super.priority += 20;

                    listPotencial.Add(super);
                }

                //низ лево
                if (potencialDown.cells.Count > 0 && potencialLeft.cells.Count > 0 &&
                    potencialDown.cells[0].cellInternal.color == potencialLeft.cells[0].cellInternal.color &&
                    ((potencialDown.cells[0] != potencialLeft.Moving && potencialLeft.Moving) || (potencialDown.Moving != potencialLeft.cells[0] && potencialDown.Moving)) &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1] != null &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1].rock == 0 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1].cellInternal != null &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1].cellInternal.color == potencialLeft.cells[0].cellInternal.color)
                {

                    PotencialComb super = new PotencialComb();

                    foreach (CellCTRL cellAdd in potencialLeft.cells)
                        super.cells.Add(cellAdd);

                    foreach (CellCTRL cellAdd in potencialDown.cells)
                        super.cells.Add(cellAdd);


                    //Добавляем крайнюю
                    super.cells.Add(cellCTRLs[cell.pos.x - 1, cell.pos.y - 1]);

                    super.Target = cell;

                    if (potencialDown.cells[0] != potencialLeft.Moving && potencialLeft.Moving)
                    {
                        super.Moving = potencialLeft.Moving;
                    }
                    else
                    {
                        super.Moving = potencialDown.Moving;
                    }

                    //Теперь комбинация собрана, считаем ее приоритет
                    super.CalcPriority();
                    super.priority += 20;

                    listPotencial.Add(super);
                }

            }

            void CombinatePotencialHorizontal(PotencialComb first, PotencialComb second)
            {
                PotencialComb potencialNew = new PotencialComb();

                //У комбинаций должны совпадать целевые ячейки,
                //у комбинаций должна быть хотябы одна ячейка
                //первые ячейки должны быть одного цвета
                //Долна быть определена перемещаемая ячейка
                if (first.Target == second.Target &&
                    first.cells.Count >= 1 && second.cells.Count >= 1 &&
                    first.cells[0].cellInternal.color == second.cells[0].cellInternal.color &&
                    first.Moving && second.Moving)
                {

                    CellCTRL moving = null;

                    CellCTRL up = null;
                    CellCTRL down = null;
                    //проверка сверху
                    if (first.Target.pos.y + 1 < cellCTRLs.GetLength(1) &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1] != null &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1] != first.cells[0] &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1].rock == 0 &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1].cellInternal != null &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1].cellInternal.color == first.cells[0].cellInternal.color &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1].cellInternal.type != CellInternalObject.Type.color5)
                    {

                        up = cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1];
                    }
                    if (first.Target.pos.y - 1 >= 0 &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1] != null &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1] != first.cells[0] &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1].rock == 0 &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1].cellInternal != null &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1].cellInternal.color == first.cells[0].cellInternal.color &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1].cellInternal.type != CellInternalObject.Type.color5)
                    {
                        down = cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1];
                    }

                    if (down != null && up != null)
                    {
                        if (up.MyPriority < down.MyPriority)
                            moving = down;
                        else moving = up;
                    }
                    else if (up != null) moving = up;
                    else if (down != null) moving = down;

                    //Если нет перемещаемой ячейки выходим
                    if (moving == null) return;



                    foreach (CellCTRL c in first.cells)
                        potencialNew.cells.Add(c);

                    foreach (CellCTRL c in second.cells)
                        potencialNew.cells.Add(c);

                    potencialNew.Target = first.Target;

                    if (first.Moving.MyPriority > second.Moving.MyPriority)
                        potencialNew.Moving = first.Moving;
                    else
                        potencialNew.Moving = second.Moving;

                    potencialNew.Moving = moving;

                    //Теперь комбинация собрана, считаем ее приоритет
                    potencialNew.CalcPriority();

                    if (potencialNew.cells.Count >= 4)
                        potencialNew.priority += 80;

                    listPotencial.Add(potencialNew);
                }

            }

            void CombinatePotencialVertical(PotencialComb first, PotencialComb second)
            {
                PotencialComb potencialNew = new PotencialComb();

                //У комбинаций должны совпадать целевые ячейки,
                //у комбинаций должна быть хотябы одна ячейка
                //первые ячейки должны быть одного цвета
                //Долна быть определена перемещаемая ячейка
                if (first.Target == second.Target &&
                    first.cells.Count >= 1 && second.cells.Count >= 1 &&
                    first.cells[0].cellInternal.color == second.cells[0].cellInternal.color &&
                    first.Moving && second.Moving)
                {

                    CellCTRL moving = null;

                    CellCTRL right = null;
                    CellCTRL left = null;
                    //проверка сверху
                    if (first.Target.pos.x + 1 < cellCTRLs.GetLength(0) &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y] != null &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y] != first.cells[0] &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y].rock == 0 &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y].cellInternal != null &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y].cellInternal.color == first.cells[0].cellInternal.color &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y].cellInternal.type != CellInternalObject.Type.color5)
                    {

                        right = cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y];
                    }
                    if (first.Target.pos.x - 1 >= 0 &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y] != null &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y] != first.cells[0] &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y].rock == 0 &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y].cellInternal != null &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y].cellInternal.color == first.cells[0].cellInternal.color &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y].cellInternal.type != CellInternalObject.Type.color5)
                    {
                        left = cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y];
                    }

                    if (left != null && right != null)
                    {
                        if (right.MyPriority < left.MyPriority)
                            moving = left;
                        else moving = right;
                    }
                    else if (right != null) moving = right;
                    else if (left != null) moving = left;

                    //Если нет перемещаемой ячейки выходим
                    if (moving == null) return;

                    foreach (CellCTRL c in first.cells)
                        potencialNew.cells.Add(c);

                    foreach (CellCTRL c in second.cells)
                        potencialNew.cells.Add(c);

                    potencialNew.Target = first.Target;

                    potencialNew.Moving = moving;


                    //Теперь комбинация собрана, считаем ее приоритет
                    potencialNew.CalcPriority();

                    if (potencialNew.cells.Count >= 4)
                        potencialNew.priority += 80;

                    listPotencial.Add(potencialNew);
                }

            }

            void CombinatePotencialsAngle(PotencialComb first, PotencialComb second)
            {
                PotencialComb potencialNew = new PotencialComb();

                //У комбинаций должны совпадать целевые ячейки,
                //у комбинаций должна быть по 2 ячейки
                //первые ячейки должны быть одного цвета
                //Долна быть определена перемещаемая ячейка
                if (first.Target == second.Target &&
                    first.cells.Count >= 2 && second.cells.Count >= 2 &&
                    first.cells[0] != second.Moving && second.cells[0] != first.Moving &&
                    first.cells[0].cellInternal.color == second.cells[0].cellInternal.color &&
                    (first.Moving || second.Moving))
                {


                    foreach (CellCTRL c in first.cells)
                        potencialNew.cells.Add(c);

                    foreach (CellCTRL c in second.cells)
                        potencialNew.cells.Add(c);

                    potencialNew.Target = first.Target;

                    if (first.Moving.MyPriority > second.Moving.MyPriority)
                        potencialNew.Moving = first.Moving;
                    else
                        potencialNew.Moving = second.Moving;


                    //Теперь комбинация собрана, считаем ее приоритет
                    potencialNew.CalcPriority();
                    potencialNew.priority += 30;

                    listPotencial.Add(potencialNew);
                }
            }
        }
    }

    //Список ячеек ожидающих перемешивание
    List<CellInternalObject> ListWaitingInternals = new List<CellInternalObject>();

    //Перемешиваем ячейки которые ожидают перемешивания
    void TestRandomNotPotencial() {
        //выходим если нет ячеек для перемешивания
        if (ListWaitingInternals.Count == 0)
            return;

        //Двигаем к центру и если кто-то не достиг центра, выходим
        if (!isDoneTransformToCenter()) {
            return;
        }

        //Запретить перемещение игроком, пока идет перемешивание
        CellSelect = null;
        CellSwap = null;

        //Рандомизируем новые позиции
        RandomizadeNewPos();


        bool isDoneTransformToCenter() {

            Vector2 pivotNeed = new Vector2((cellCTRLs.GetLength(0)/2)*-1, (cellCTRLs.GetLength(1)/2)*-1);

            foreach (CellInternalObject cellInternal in ListWaitingInternals) {

                cellInternal.rectMy.pivot += (pivotNeed - cellInternal.rectMy.pivot) * Time.deltaTime * 4;

            }
            

            //проверяем все внутренние на близость к центру
            foreach (CellInternalObject cellInternal in ListWaitingInternals) {
                if (Vector2.Distance(pivotNeed, cellInternal.rectMy.pivot) > 0.1f) {
                    return false;
                }
            }

            //Если все объекты достаточно близко, прошли проверку
            return true;

        }
        
        //Перемешиваем
        void RandomizadeNewPos() {
            //Запоминаем ячейки этих внутренностей
            List<CellCTRL> cellsList = new List<CellCTRL>();
            foreach (CellInternalObject cellInternal in ListWaitingInternals) {
                cellsList.Add(cellInternal.myCell);
            }

            //Запомнили ячейки

            //Тест
            int testNow = 0;
            int testMax = 100;
            //Выходим если тестов было больше 100 и ничего не нашлось
            while (ListWaitingInternals.Count > 0 && testNow < testMax) {
                if (testNow < testMax / 4)
                {
                    SetAllCellInternal();
                }
                else if (testNow < (testMax / 4) * 2)
                {
                    //Рандомизировать цвет у цветных
                    SetAllCellInternalRandomColor(testNow - (testMax / 4));
                }
                else if (testNow < (testMax / 4) * 3)
                {
                    //Рандомизировать цвет не только у цветных объектов
                    SetAllCellInternalRandom(testNow - (testMax / 4) * 2);
                }
                else {
                    SetAllCellInternalRandomType(testNow - (testMax / 4) * 3);
                }



                testNow++;
            }

            void SetAllCellNull() {
                foreach (CellCTRL cell in cellsList)
                {
                    cell.cellInternal = null;
                }
            }

            //Установить всем внутренностям по ячейке
            void SetAllCellInternal() {

                //открепляем связь внутренности с ячейкой
                SetAllCellNull();

                for (int x = 0; x < ListWaitingInternals.Count; x++) {
                    CellCTRL selectedCell = null;

                    //Удаляем память о ячейке в перемещаемом объекте
                    ListWaitingInternals[x].myCell = null;

                    while (!selectedCell) {
                        CellCTRL randomCell = cellsList[Random.Range(0, cellsList.Count)];

                        //Если ячейка существует и у нее нету внутренности
                        if (randomCell && randomCell.cellInternal == null) {
                            selectedCell = randomCell;
                        }
                    }


                    //Выбрали ячейку указываем путь
                    ListWaitingInternals[x].StartMove(selectedCell);
                    
                }

                TestFieldCombination(false);

                //Очищаем список если есть потенциальная комбинация и нет текущей комбинации
                if (isPotencialFound() && listCombinations.Count <= 0) {
                    //Очищаем список
                    ListWaitingInternals = new List<CellInternalObject>();
                }

            }

            void SetAllCellInternalRandomColor(int countRandomMax) {
                //открепляем связь внутренности с ячейкой
                SetAllCellNull();

                int countRandomNow = 0;
                for (int x = 0; x < ListWaitingInternals.Count; x++)
                {
                    CellCTRL selectedCell = null;

                    //Удаляем память о ячейке в перемещаемом объекте
                    ListWaitingInternals[x].myCell = null;
                    if (countRandomNow < countRandomMax && ListWaitingInternals[x].type == CellInternalObject.Type.color) {
                        countRandomNow++;

                        ListWaitingInternals[x].randColor();
                    }

                    while (!selectedCell)
                    {
                        CellCTRL randomCell = cellsList[Random.Range(0, cellsList.Count)];

                        //Если ячейка существует и у нее нету внутренности
                        if (randomCell && randomCell.cellInternal == null)
                        {
                            selectedCell = randomCell;
                        }
                    }


                    //Выбрали ячейку указываем путь
                    ListWaitingInternals[x].StartMove(selectedCell);

                }

                //проверяем на комбинации сейчас
                TestFieldCombination(false);

                //Очищаем список есть потенциальная комбинация и нет текущей комбинации
                if (isPotencialFound() && listCombinations.Count <= 0)
                {
                    //Очищаем список
                    ListWaitingInternals = new List<CellInternalObject>();
                }
            }

            void SetAllCellInternalRandom(int countRandomMax)
            {
                //открепляем связь внутренности с ячейкой
                SetAllCellNull();

                int countRandomNow = 0;
                for (int x = 0; x < ListWaitingInternals.Count; x++)
                {
                    CellCTRL selectedCell = null;

                    //Удаляем память о ячейке в перемещаемом объекте
                    ListWaitingInternals[x].myCell = null;
                    if (countRandomNow < countRandomMax)
                    {
                        countRandomNow++;

                        ListWaitingInternals[x].randColor();
                    }

                    while (!selectedCell)
                    {
                        CellCTRL randomCell = cellsList[Random.Range(0, cellsList.Count)];

                        //Если ячейка существует и у нее нету внутренности
                        if (randomCell && randomCell.cellInternal == null)
                        {
                            selectedCell = randomCell;
                        }
                    }


                    //Выбрали ячейку указываем путь
                    ListWaitingInternals[x].StartMove(selectedCell);

                }

                //проверяем на комбинации сейчас
                TestFieldCombination(false);

                //Очищаем список если есть потенциальная комбинация и нет текущей комбинации
                if (isPotencialFound() && listCombinations.Count <= 0)
                {
                    //Очищаем список
                    ListWaitingInternals = new List<CellInternalObject>();
                }
            }

            void SetAllCellInternalRandomType(int countRandomMax)
            {
                //открепляем связь внутренности с ячейкой
                SetAllCellNull();

                int countRandomNow = 0;
                for (int x = 0; x < ListWaitingInternals.Count; x++)
                {
                    CellCTRL selectedCell = null;

                    //Удаляем память о ячейке в перемещаемом объекте
                    ListWaitingInternals[x].myCell = null;
                    if (countRandomNow < countRandomMax)
                    {
                        countRandomNow++;

                        ListWaitingInternals[x].randType();
                        ListWaitingInternals[x].randColor();
                        ListWaitingInternals[x].setColorAndType(ListWaitingInternals[x].color, ListWaitingInternals[x].type);
                    }

                    while (!selectedCell)
                    {
                        CellCTRL randomCell = cellsList[Random.Range(0, cellsList.Count)];

                        //Если ячейка существует и у нее нету внутренности
                        if (randomCell && randomCell.cellInternal == null)
                        {
                            selectedCell = randomCell;
                        }
                    }


                    //Выбрали ячейку указываем путь
                    ListWaitingInternals[x].StartMove(selectedCell);

                }

                TestFieldCombination(false);

                //Очищаем список если есть потенциальная комбинация и нет текущей комбинации
                if (isPotencialFound() && listCombinations.Count <= 0)
                {
                    //Очищаем список
                    ListWaitingInternals = new List<CellInternalObject>();
                }
            }

            //Проверяем текушую растановку ячеек на комбинации
            bool isCombinationFound()
            {
                bool result = false;

                //Перебираем каждую ячейку
                for (int x = 0; x < cellCTRLs.GetLength(0); x++)
                {
                    for (int y = 0; y < cellCTRLs.GetLength(1); y++)
                    {

                    }
                }



                return result;
            }
            bool isPotencialFound()
            {
                bool result = false;

                ReCalcListPotencial();
                if (listPotencial.Count > 0) result = true;



                return result;
            }
        }
    }

    //Для групировки данных
    public struct Buffer{
        public MenuGameplay.SuperHitType superHit;
    }
    Buffer buffer;

    //Конец комбинаций
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Создание перемещаемых объектов

    /// <summary>
    /// Создать бомбу на месте другой ячейки
    /// </summary>
    public void CreateBomb(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        cellLast.DeleteInternalNoDamage();

        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniBomb(cellLast, this, internalColor, combID);


    }
    /// <summary>
    /// Создать самолет на месте другой ячейки
    /// </summary>
    public void CreateFly(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        cellLast.DeleteInternalNoDamage();

        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniFly(cellLast, this, internalColor, combID);


    }
    /// <summary>
    /// Создать СуперЦветовую на месте другой ячейки
    /// </summary>
    public void CreateSuperColor(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        cellLast.DeleteInternalNoDamage();

        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniSuperColor(cellLast, this, internalColor, combID);


    }
    /// <summary>
    /// Создать вертикальную ракету на месте другой ячейки
    /// </summary>
    public void CreateRocketVertical(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        cellLast.DeleteInternalNoDamage();

        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniRocketVertical(cellLast, this, internalColor, combID);

    }
    /// <summary>
    /// Создать горизонтальную ракету на месте другой ячейки
    /// </summary>
    public void CreateRocketHorizontal(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        cellLast.DeleteInternalNoDamage();

        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniRocketHorizontal(cellLast, this, internalColor, combID);

    }

    float timeCellSelect = 0;
    //Проверка на то что нужно поменять местами объекты
    void TestStartSwap()
    {
        //Если выделенная ячейка есть и прошло больще 5 секунд снимаем выделение
        if (CellSelect && Time.unscaledTime - timeCellSelect > 4) {
            CellSelect = null;
        }

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
            CellSelect.BlockingMove > 0 || //Если в ячейке коробка
            CellSwap.BlockingMove > 0 ||
            CellSelect.rock > 0 || //Если в ячейке камень
            CellSwap.rock > 0 ||
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

    int lastStepCount = 0;
    float lastTime = 0;
    //Если количетсво ходов изменилось
    void TestMold() {

        if (Gameplay.main.movingMoldCount <= lastStepCount || //если количество ходов игрока меньше чем сделала плесень
            Gameplay.main.combo > 0 || //Если идет набор комбинации и ход не закончен
            isMovingInternalObj
            ) {
            lastTime = Time.unscaledTime; //Запоминаем последнее пропущенное время
            return;
        }

        //Если ожидание меньше секунды выходим
        if (Time.unscaledTime - lastTime < 0.5f) {
            return;
        }

        StepMold();

        //Ход плесени
        void StepMold() {
            //Выбираем рандомную плесень из всего списка
            int num = Random.Range(0, moldCTRLs.Count);
            //Если плесень существует, если выбранна конкретная плесень, если спавн удачный
            if (moldCTRLs.Count > 0 && moldCTRLs[num] && moldCTRLs[num].TestSpawn()) {
                //прибавляем ход плесени
                lastStepCount++;
            }
        }
    }

    /// <summary>
    /// Cписок всех ячеек в порядке приоритета от высокого к низкому
    /// </summary>
    public CellCTRL[] cellsPriority;
    void TestCalcPriorityCells() {
        //Вычисление приоритетов
        bool isComplite = false;
        for (int testCount = 0; testCount < 50 && !isComplite; testCount++) {

            //Если этот цикл пройдет без смещений то значит навершено
            isComplite = true;

            //Перебираем все ячейки и перемещаем поплавком на одну позицию
            for (int num = 1; num < cellsPriority.Length; num++)
            {
                //если ячейки нет, то пропускаем
                if (cellsPriority[num] == null) continue;

                //Меняем местами с предыдущим если, предыдущей ячейки нет или ее приориет ниже
                if (cellsPriority[num - 1] == null || cellsPriority[num - 1].MyPriority < cellsPriority[num].MyPriority)
                {
                    //CellCTRL Now = cellsPriority[num];
                    CellCTRL Previously = cellsPriority[num - 1];

                    cellsPriority[num - 1] = cellsPriority[num];
                    cellsPriority[num] = Previously;

                    //Произошло смещение, рано завершать перебор
                    isComplite = false;
                }
            }
        }

        //Перебираем все ячейки и перемещаем поплавком на одну позицию
        for (int num = 1; num < cellsPriority.Length; num++) {
            //если ячейки нет, то пропускаем
            if (cellsPriority[num] == null) continue;

            //Меняем местами с предыдущим если, предыдущей ячейки нет или ее приориет ниже
            if (cellsPriority[num - 1] == null || cellsPriority[num - 1].MyPriority < cellsPriority[num].MyPriority) {
                //CellCTRL Now = cellsPriority[num];
                CellCTRL Previously = cellsPriority[num - 1];

                cellsPriority[num - 1] = cellsPriority[num];
                cellsPriority[num] = Previously;
            }
        }

        
    }

    /// <summary>
    /// Сделать ячейку выделенной или целевой для перемещения
    /// </summary>
    public void SetSelectCell(CellCTRL CellClick) {
        if (!CellSelect) {
            CellSelect = CellClick;
            timeCellSelect = Time.unscaledTime;
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

    //Проверить ячейку на блокирувку распространения взрыва
    public bool isBlockingBoomDamage(CellCTRL cell) {
        bool result = false;

        if (cell == null) return result;


        if (cell.rock > 0) {
            result = true;
        }

        return result;
    }



    float timelastMoveTest = 0;

    bool isMovingOld = false;
    bool isMovingInternalObj = false; //находятся ли в движении объекты наполе
    bool isMovingFly = false;

    bool isMoving() {
        if (timelastMoveTest != Time.unscaledTime)
        {
            bool isMovingNow = false;

            timelastMoveTest = Time.unscaledTime;

            //Если мисия уже завершилась включаем все возможные цвета
            if (Gameplay.main.isMissionComplite()) {
                Gameplay.main.colors = 6;
                Gameplay.main.superColorPercent = 20;

                CellSelect = null;
            }

            TestMoveInternalObj(); //Проверяем наличие движения для отмены комбо
            TestMoveFly();

            //Если происходит движение
            if (isMovingInternalObj || isMovingFly || Time.unscaledTime - movingObjLastTime < 1 ) {
                isMovingNow = true;
            }
            else {
                isMovingNow = false;
            }


            //Если прекратили двигаться
            if (isMovingOld && !isMovingNow)
            {

                //Вызываем сообжение о комбинации
                MidleMessageCombo();

                //Если миссию можно закончить прохождением
                if (Gameplay.main.isMissionComplite()) {
                    //Активируем по очереди все доступные супер бомбы ракеты и остальные активационные штуки
                    List<CellInternalObject> roskets = new List<CellInternalObject>();
                    List<CellInternalObject> bombs = new List<CellInternalObject>();
                    List<CellInternalObject> flys = new List<CellInternalObject>();
                    List<CellInternalObject> color5 = new List<CellInternalObject>();

                    for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                        for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
                            if (cellCTRLs[x,y] != null && cellCTRLs[x,y].cellInternal) {

                                if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.rocketHorizontal ||
                                    cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.rocketVertical) {
                                    roskets.Add(cellCTRLs[x, y].cellInternal);
                                }
                                else if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.bomb) {
                                    bombs.Add(cellCTRLs[x, y].cellInternal);
                                }
                                else if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.airplane) {
                                    flys.Add(cellCTRLs[x, y].cellInternal);
                                }
                                else if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.color5) {
                                    color5.Add(cellCTRLs[x, y].cellInternal);
                                }
                            }
                        }
                    }

                    if (roskets.Count > 0)
                    {
                        foreach (CellInternalObject inside in roskets)
                            inside.myCell.Damage();
                    }
                    else if (bombs.Count > 0)
                    {
                        foreach (CellInternalObject inside in bombs)
                            inside.myCell.Damage();
                    }
                    else if (flys.Count > 0)
                    {
                        foreach (CellInternalObject inside in flys)
                            inside.myCell.Damage();
                    }
                    else if (color5.Count > 0) {
                        foreach (CellInternalObject inside in color5)
                            inside.myCell.Damage();
                    }
                    //Если остались ходы
                    else if (Gameplay.main.movingCan > 0) {
                        while (Gameplay.main.movingCan > 0) {
                            //Выбираем рандомный объект
                            int x = Random.Range(0, cellCTRLs.GetLength(0));
                            int y = Random.Range(0, cellCTRLs.GetLength(1));

                            //Есди эта ячейка не подходит выходим
                            if (cellCTRLs[x,y] == null || cellCTRLs[x,y].cellInternal == null || cellCTRLs[x,y].cellInternal.type != CellInternalObject.Type.color) {
                                continue;
                            }

                            //Выбираем тип
                            CellInternalObject.Type typeNew = CellInternalObject.Type.rocketHorizontal;
                            if (Random.Range(0, 100) < 50) typeNew = CellInternalObject.Type.rocketVertical;
                            

                            //применяем изменения
                            cellCTRLs[x, y].cellInternal.setColorAndType(cellCTRLs[x,y].cellInternal.color, typeNew);
                            Combination comb = new Combination();
                            comb.cells.Add(cellCTRLs[x, y]);
                            if (cellCTRLs[x, y].panel) {
                                comb.foundPanel = true;
                            }
                            //Активируем
                            cellCTRLs[x, y].cellInternal.Activate(cellCTRLs[x,y].cellInternal.type, null, comb);
                            

                            //вычитаем ход
                            Gameplay.main.movingCan--;
                        }

                        MenuGameplay.main.updateMoving();
                        isMovingOld = true;

                    }
                    else
                    {
                        Gameplay.main.OpenMessageComplite();
                    }

                }
                else if (Gameplay.main.isMissionDefeat()) Gameplay.main.OpenMessageDefeat();
            }
            else if (!isMovingOld && isMovingNow)
            {
                SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipMoveCrystal, 0.75f, Random.Range(0.9f, 1.1f));
            }

            //Обновляем время движения
            if (isMovingNow)
            {
                timeLastMove = Time.unscaledTime;
            }

            isMovingOld = isMovingNow;
        }

        return isMovingOld;

        void MidleMessageCombo() {

            if (ComboCount > 10) {
                MenuGameplay.main.CreateMidleMessage("Ultimative");
            }
            else if (ComboCount > 5) {
                MenuGameplay.main.CreateMidleMessage("incredible");
            }
            else if (ComboCount > 3) {
                MenuGameplay.main.CreateMidleMessage("Not bad");
            }

            ComboCount = 1;
        }
    }

    //находятся ли в движении какие либо объекты на поле

    float movingObjLastTime = 0;
    void TestMoveInternalObj()
    {
        bool movingNow = false;
        for (int x = 0; x < cellCTRLs.GetLength(0) && !movingNow; x++)
        {
            if (movingNow) break;

            for (int y = 0; y < cellCTRLs.GetLength(1) && !movingNow; y++)
            {
                if (movingNow) break;

                //смотрим дальше
                if (!cellCTRLs[x, y] || //Нет ячейки
                    !cellCTRLs[x, y].cellInternal || //Нет внутреннего объекта
                    cellCTRLs[x, y].BlockingMove > 0 || //присутствует коробка
                    cellCTRLs[x, y].rock > 0 //|| //Или есть камень
                    //(cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[x,y].cellInternal.activateNeed) //или есть активированная бомба
                    ) continue;

                //Если снизу есть куда двигаться, то двигаемся
                if (y - 1 >= 0 &&
                    cellCTRLs[x, y - 1] &&
                    !cellCTRLs[x, y - 1].cellInternal &&
                    cellCTRLs[x, y - 1].rock == 0 &&
                    cellCTRLs[x, y - 1].BlockingMove == 0
                    ) {
                    movingNow = true;
                }

                if (!cellCTRLs[x, y].cellInternal && Time.unscaledTime - cellCTRLs[x, y].timeBoomOld > 0.5f)
                {
                    movingNow = true;
                }

                else if (cellCTRLs[x, y].cellInternal && cellCTRLs[x, y].cellInternal.isMove)
                {
                    movingNow = true;
                }

                if (movingNow) {
                    movingObjLastTime = Time.unscaledTime;
                }
            }
        }

        isMovingInternalObj = movingNow;
    }
    void TestMoveFly() {

        List<FlyCTRL> flyCTRLsNew = new List<FlyCTRL>();
        foreach (FlyCTRL flyCTRL in FlyCTRL.flyCTRLs) {
            if (flyCTRL != null)
                flyCTRLsNew.Add(flyCTRL);
        }
        FlyCTRL.flyCTRLs = flyCTRLsNew;

        if (FlyCTRL.flyCTRLs.Count > 0) {
            isMovingFly = true;
        }
        else {
            isMovingFly = false;
        }
    }
}
