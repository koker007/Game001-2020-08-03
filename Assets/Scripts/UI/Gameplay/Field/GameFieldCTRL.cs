using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// ������� ����
/// </summary>
public class GameFieldCTRL : MonoBehaviour
{
    [Header("Counters")]
    /// <summary>
    /// ������� ���������� ������� ����������� �������� �� �����
    /// </summary>
    [SerializeField]
    public int CountBoxBlocker = 0;
    /// <summary>
    /// ������� ���������� ������� �� �����
    /// </summary>
    [SerializeField]
    public int CountMold = 0;
    /// <summary>
    /// ������� ���������� ���������������� ������ �� �����
    /// </summary>
    [SerializeField]
    public int CountPanelSpread = 0;

    [Header("Prefabs")]
    [SerializeField]
    GameObject prefabCell;
    [SerializeField]
    public GameObject prefabInternal;
    [SerializeField]
    GameObject prefabBoxBlock;
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

    //������������ ������� ��� ������ ������ ������� ��������
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
    public Transform parentOfFly;
    [SerializeField]
    public Transform parentOfParticles;
    [SerializeField]
    public Transform parentOfScore;
    [SerializeField]
    public Transform parentOfSelect;


    [Header("Other")]
    [SerializeField]
    CellCTRL CellSelect; //������ ���������� ������������� ������
    [SerializeField]
    CellCTRL CellSwap; //������ ���������� ������������� ������

    [SerializeField]
    RectTransform rectParticleSelect;

    RectTransform myRect;

    /// <summary>
    /// ������ ������������ �������
    /// </summary>
    public List<MoldCTRL> moldCTRLs = new List<MoldCTRL>();

    /// <summary>
    /// ����������� ������� ���������� �������, ��������� ��������� �� ������
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
    /// �������� ��������� ������ � ����� ����, ����� ���������� Null
    /// </summary>
    static public CellCTRL GetRandomCellNearest(CellCTRL cellFrom)
    {
        //�������� �����
        int place = Random.Range(0, 4);

        CellCTRL result = null;
        //������
        if (place == 0)
        {
            //���� �� ����� �� ������� � ������ ����������
            if (cellFrom.pos.y + 1 < cellFrom.myField.cellCTRLs.GetLength(1) && cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y])
                result = cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y + 1];
        }
        //�����
        else if (place == 1)
        {
            //���� �� ����� �� ������� � ������ ����������
            if (cellFrom.pos.y - 1 >= 0 && cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y])
                result = cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y - 1];
        }
        //������
        else if (place == 2)
        {
            //���� �� ����� �� ������� � ������ ����������
            if (cellFrom.pos.x + 1 < cellFrom.myField.cellCTRLs.GetLength(0) && cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y])
                result = cellFrom.myField.cellCTRLs[cellFrom.pos.x + 1, cellFrom.pos.y];
        }
        //�����
        else if (place == 3)
        {
            //���� �� ����� �� ������� � ������ ����������
            if (cellFrom.pos.x - 1 >= 0 && cellFrom.myField.cellCTRLs[cellFrom.pos.x, cellFrom.pos.y])
                result = cellFrom.myField.cellCTRLs[cellFrom.pos.x - 1, cellFrom.pos.y];
        }

        return result;
    }

    public int ComboCount = 1; 
    bool isMoving = false; //��������� �� � �������� ������� ������
    //��������� �� � �������� ����� ���� ������� �� ����
    void TestMoving() {
        bool movingNow = false;
        for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
            if (movingNow) break;

            for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                if (movingNow) break;

                //������� ������
                if (!cellCTRLs[x, y] || //��� ������
                    !cellCTRLs[x, y].cellInternal || //��� ����������� �������
                    cellCTRLs[x, y].BlockingMove > 0 //������������ �������
                    ) continue;

                if (!cellCTRLs[x,y].cellInternal && Time.unscaledTime - cellCTRLs[x, y].timeBoomOld > 0.5f) {
                    movingNow = true;
                }

                else if (cellCTRLs[x, y].cellInternal && cellCTRLs[x, y].cellInternal.isMove) {
                    movingNow = true;
                }
            }
        }

        //���� ���������� ���������
        if (isMoving && !movingNow) {
            ComboCount = 1;
            Gameplay.main.CheckEndGame();
        }

        isMoving = movingNow;
    }

    //�������� ������
    class Swap {
        public CellCTRL first;
        public CellCTRL second;

        public int stopSwap = 1;
    }

    //������ ������ ������� ���� ������� ��������, �������������
    List<Swap> BufferSwap = new List<Swap>();

    /// <summary>
    /// ������
    /// </summary>
    public CellCTRL[,] cellCTRLs;
    /// <summary>
    /// ����������� ��������
    /// </summary>
    public BoxBlockCTRL[,] BoxBlockCTRLs;

    /// <summary>
    /// ���������������� ������� ����, �� ������ ������ ������, ��� ��������, ���� ������ ���
    /// </summary>
    public void inicializeField(LevelsScript.Level level) {
        

        //���������� ���� � �����
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f,0.5f);

        //���� ������� ���� � ����
        if (level != null)
        {
            AddAllCellLevel();
        }
        else {
            AddAllCellsRandom();
        }

        ScaleField();

        //��������� ��� ���� �������� ��������
        void AddAllCellsRandom() {
            //������� ������������ �������� ����
            cellCTRLs = new CellCTRL[10, 10];
            cellsPriority = new CellCTRL[cellCTRLs.GetLength(0) * cellCTRLs.GetLength(1)];

            for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                    if (!cellCTRLs[x,y]) {
                        GameObject cellObj = Instantiate(prefabCell, parentOfCells);
                        

                        //���� ���������
                        cellCTRLs[x, y] = cellObj.GetComponent<CellCTRL>();
                        cellsPriority[x * cellCTRLs.GetLength(1) + y] = cellCTRLs[x, y];
                        //���� ��������� �� ������� ������� ���� �����
                        if (!cellCTRLs[x,y]) {
                            Destroy(cellObj);
                            break;
                        }

                        cellCTRLs[x, y].pos = new Vector2Int(x, y);
                        cellCTRLs[x, y].myField = this;
                        //���������� ������ �� ���� �������
                        RectTransform rect = cellObj.GetComponent<RectTransform>();
                        rect.pivot = new Vector2(-x,-y);

                        //���������� ����������
                        cellCTRLs[x, y].CalcMyPriority();
                    }
                }
            }
        }

        //��������� ��� ���� �������� �� ������� ������
        void AddAllCellLevel() {
            //������� ������������ �������� ����
            cellCTRLs = new CellCTRL[level.Width, level.Height];
            cellsPriority = new CellCTRL[cellCTRLs.GetLength(0) * cellCTRLs.GetLength(1)];

            Gameplay.main.colors = level.NumColors;

            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++)
                {
                    //����� �� ��������� ������
                    LevelsScript.CellInfo cellInfo = level.ReturneCell(new Vector2Int(x,y));
                    
                    //���� ���� ������ �� ����������
                    if (cellInfo == null)
                    {
                        continue;
                    }

                    //���� ������ ���, �������
                    if (!cellCTRLs[x, y]) {
                        GameObject cellObj = Instantiate(prefabCell, parentOfCells);
                        //���� ���������
                        cellCTRLs[x, y] = cellObj.GetComponent<CellCTRL>();
                        cellsPriority[x * cellCTRLs.GetLength(1) + y] = cellCTRLs[x, y];
                        if (!cellCTRLs[x, y])
                        {
                            Destroy(cellObj);
                            break;
                        }

                        cellCTRLs[x, y].pos = new Vector2Int(x, y);
                        cellCTRLs[x, y].myField = this;
                        //���������� ������ �� ���� �������
                        RectTransform rect = cellObj.GetComponent<RectTransform>();
                        rect.pivot = new Vector2(-x, -y);
                        cellCTRLs[x, y].BlockingMove = level.cells[x, y].boxHealth;
                        cellCTRLs[x, y].mold = level.cells[x, y].moldHealth;
                        
                        if(level.cells[x, y].Panel > 0)
                            cellCTRLs[x, y].panel = true;

                        //������������ ��� ������������
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

                    //����� �� ������� ����
                    if (cellCTRLs[x, y].BlockingMove > 0) {
                        GameObject BoxBlockObj = Instantiate(prefabBoxBlock, parentOfBoxBlock);
                        BoxBlockCTRL boxBlockCTRL = BoxBlockObj.GetComponent<BoxBlockCTRL>();
                        cellCTRLs[x, y].BoxBlockCTRL = boxBlockCTRL;

                        //�������������� �������
                        boxBlockCTRL.Inicialize(cellCTRLs[x, y]);
                    }

                    //����� �� ������� �������
                    if (cellCTRLs[x, y].mold > 0) {
                        GameObject MoldObj = Instantiate(prefabMold, parentOfMold);
                        MoldCTRL moldCTRL = MoldObj.GetComponent<MoldCTRL>();
                        cellCTRLs[x, y].moldCTRL = moldCTRL;

                        //������������� �������
                        moldCTRL.inicialize(cellCTRLs[x, y]);
                    }
                    //����� �� ������� ������
                    if (cellCTRLs[x,y].panel) {
                        GameObject panelObj = Instantiate(prefabPanel, parentOfPanels);
                        cellCTRLs[x, y].panelCTRL = panelObj.GetComponent<PanelSpreadCTRL>();

                        //������������� �������
                        cellCTRLs[x, y].panelCTRL.inicialize(cellCTRLs[x, y]);
                    }

                    //������� ��������� �������
                    if (cellCTRLs[x, y].BlockingMove == 0 //���� ���� �����
                        )
                    {
                        if (level.cells[x, y].typeCell == CellInternalObject.Type.color) {
                            //������� ������ � ����������
                            GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                            CellInternalObject internalCtrl = internalObj.GetComponent<CellInternalObject>();
                            internalCtrl.myField = this;
                            internalCtrl.StartMove(cellCTRLs[x, y]);
                            internalCtrl.EndMove();

                            //������ ��� �������
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
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.supercolor) {
                            CreateSuperColor(cellCTRLs[x, y], cellInfo.colorCell, 0);
                        }
                        
                    }

                    //���������� ����������
                    cellCTRLs[x, y].CalcMyPriority();

                }
            }
        }

        //��������� ������ ���� � ����������� �� ���������� �����
        void ScaleField() {
            //100% ������ ��� ���� � 10-� ��������

            if (!myRect) myRect = GetComponent<RectTransform>();

            //��������� ������ ������� ������ ���� ����
            float cellsWeight = (float)cellCTRLs.GetLength(0);

            float sizeNeed = 10 / cellsWeight;

            //������������� ������ ����
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

        TestSpawn(); //�������
        TestMoving(); //��������� ������� �������� ��� ������ �����
        TestFieldCombination(); //������ ����������

        TestStartSwap(); //�������� �����
        TestReturnSwap(); //���������� �����

        TestMold(); //��������� �������� ����� ����

        TestCalcPriorityCells(); //���������� ���������� �����
    }

    //��������� ������������� �������� ����
    void StartInicialize() {
        //��������� � ���� ��� ������
        RandomInicialize();

        void RandomInicialize() {

            bool isComplite = false;
            int colors = Gameplay.main.colors;

            while (!isComplite) {
                //������ ����� �����
                for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
                    //����� � �����
                    for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                        //���� ������ ��� - �������
                        if (!cellCTRLs[x,y]) {
                            break;
                        }

                        //���� ������ ������ ��������� ������������ � ��� �������
                        if (!cellCTRLs[x,y].cellInternal && cellCTRLs[x,y].BlockingMove <= 0) {
                            //������� ������������
                            GameObject InternalObj = Instantiate(prefabInternal, parentOfInternals);
                            CellInternalObject Internal = InternalObj.GetComponent<CellInternalObject>();

                            Internal.myField = this;
                            Internal.myCell = cellCTRLs[x, y];
                            cellCTRLs[x, y].cellInternal = Internal;

                            //������������� �������
                            Internal.IniRect();
                            Internal.PosToCell();

                            //���������� ����
                            Internal.randColor();
                        }

                        
                    }
                }

                isComplite = true;
            }
        }


    }

    //�������� ����� �� ������� � �������������
    void TestSpawn() {
        //��� �� �����
        int[] countSpawned = new int[cellCTRLs.GetLength(0)];

        //������� ����� ��������� ���� �� ������ ������
        for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++) {

                //���� ��� ������ ����, ������ � ��� ���������� �������� � �� ��� ������ ��� ��������
                if (cellCTRLs[x,y] && !cellCTRLs[x, y].cellInternal && cellCTRLs[x,y].BlockingMove == 0) {

                    //��������� ������ �� �� ���� �� ��� ���-�� ��� ����� ������
                    for (int plusY = 0; plusY <= cellCTRLs.GetLength(1); plusY++) {

                        //���� �������� ������ ����� ����
                        if (y + plusY >= cellCTRLs.GetLength(1))
                        {
                            //������� ������ ������������� �������
                            GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                            //������ �� ������� 
                            RectTransform rect = internalObj.GetComponent<RectTransform>();
                            
                            //������� ���������� ����� �� �����
                            rect.pivot = new Vector2(-x, -y-(countSpawned[x]+cellCTRLs.GetLength(1)-y));

                            CellInternalObject internalCtrl = internalObj.GetComponent<CellInternalObject>();

                            //���������� ����
                            internalCtrl.randColor();

                            //�������� �������
                            internalCtrl.StartMove(cellCTRLs[x, y]);

                            //������������� ����
                            internalCtrl.myField = this;
                            //������ ������ ���� ���� ���������
                            //internalCtrl.myCell = cellCTRLs[x, y];

                            cellCTRLs[x, y].setInternal(internalCtrl);

                            countSpawned[x]++;

                            break;
                        }
                        //��� ����� �� �������������� ������, �������
                        else if (y + plusY < cellCTRLs.GetLength(1) && (
                            !cellCTRLs[x, y + plusY] ||
                            cellCTRLs[x, y + plusY].BlockingMove > 0
                            ))
                        {
                            break;
                        }

                        //�� ����������� ���� �������� ��� ������������ ������

                        //���� ������ ���� ������ � ������������� � ��� �� �����������
                        else if (cellCTRLs[x, y + plusY].BlockingMove <= 0 && cellCTRLs[x, y + plusY].cellInternal) {

                            break;
                        }
                    }

                        
                }
            }
        }
    }

    //������ ����� ������� ����������
    List<Combination> listCombinations;
    //��������� ��� ������ �� ���������

    //����� ����������, ��� ������������ ��� ���������
    /// <summary>
    /// ����� ���������� ��� ������������ ��� 
    /// </summary>
    public class Combination {
        static int IDLast = 0; //��������� ��������� ����������

        public List<CellCTRL> cells = new List<CellCTRL>();

        public int ID = 0;

        public bool horizontal = false;
        public bool vertical = false;

        public bool line5 = false;
        public bool square = false;
        public bool line4 = false;
        public bool cross = false;

        public bool foundPanel = false;
        public bool foundMold = false;

        public Combination(){
            IDLast++; //���������� id ����������
            ID = IDLast; //��� ��� �����
        }
    }
    void TestFieldCombination() {
        //������� ����� ������ ����������
        listCombinations = new List<Combination>();

        //������� ������ ��������� ��� ������ �� ����������
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
        TestDamageAndSpawn();

        TestSuperCombination();

        ///////////////////////////////////////////////////////////////
        //��������� ������ �� ����������. ������� 2021.08.18
        void TestCellCombination(CellCTRL Cell)
        {

            //������� ���� ������ ���, ��� ��� ������������, ��� ������������ � ��������, ��� ��� �������, ������� �� ������ ���������� � ����������
            if (!Cell || !Cell.cellInternal || Cell.cellInternal.isMove || Cell.cellInternal.type == CellInternalObject.Type.supercolor)
            {
                return;
            }

            List<CellCTRL> cellLineRight = new List<CellCTRL>();
            List<CellCTRL> cellLineLeft = new List<CellCTRL>();
            List<CellCTRL> cellLineDown = new List<CellCTRL>();
            List<CellCTRL> cellLineUp = new List<CellCTRL>();

            List<CellCTRL> cellSquare = new List<CellCTRL>();

            //������ ����� � ����������
            Combination Combination = new Combination();

            //������ ���� ���� ���������� � ������
            GetCombination();

            TestLines();
            TestSquare();
            CalcResult();

            //������� ���������� � ������
            SetCombination();

            ///////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////
            void GetCombination() {
                bool foundCombination = false;

                //���������� ����������
                foreach (Combination comb in listCombinations) {
                    if (foundCombination) break; //���� ���������� ���� �������� �������

                    //���������� ������ �� ������ ����������
                    foreach (CellCTRL cellComb in comb.cells) {
                        if (foundCombination) break; //���� ���������� ���� �������� �������

                        //���� ������� ���� �������� � ����������
                        if (cellComb == Cell) {
                            //�� �� ����� ����������
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

                //���� ���� �� � ������ ��� ����������
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
                //�������� ������
                for (int smeshenie = 1; smeshenie < 5; smeshenie++)
                {

                    if ((Cell.pos.x + smeshenie) > cellCTRLs.GetLength(0) - 1 || //���� ����� �� ������� �������
                        TestCellColor(cellCTRLs[Cell.pos.x + smeshenie, Cell.pos.y]))
                    {
                        //�� ���� ����������� �������
                        break;
                    }

                    //��������� ������ � ������ �����
                    cellLineRight.Add(cellCTRLs[Cell.pos.x + smeshenie, Cell.pos.y]);
                }

                ////////////////////////////////////////////////////
                //�������� �����
                for (int smeshenie = 1; smeshenie < 5; smeshenie++)
                {

                    if ((Cell.pos.x - smeshenie) < 0 || //���� ����� �� ������� �������
                        TestCellColor(cellCTRLs[Cell.pos.x - smeshenie, Cell.pos.y]))
                    {
                        //�� ���� ����������� �������
                        break;
                    }

                    //��������� ������ � ������ �����
                    cellLineLeft.Add(cellCTRLs[Cell.pos.x - smeshenie, Cell.pos.y]);
                }

                /////////////////////////////////////////////////////
                //�������� ����
                for (int smeshenie = 1; smeshenie < 5; smeshenie++)
                {

                    if ((Cell.pos.y - smeshenie) < 0 || //���� ����� �� ������� �������
                        TestCellColor(cellCTRLs[Cell.pos.x, Cell.pos.y - smeshenie]))
                    {
                        //�� ���� ����������� �������
                        break;
                    }

                    //��������� ������ � ������ �����
                    cellLineDown.Add(cellCTRLs[Cell.pos.x, Cell.pos.y - smeshenie]);
                }

                /////////////////////////////////////////////////////
                //�������� �����
                for (int smeshenie = 1; smeshenie < 5; smeshenie++)
                {

                    if ((Cell.pos.y + smeshenie) > cellCTRLs.GetLength(1) - 1 || //���� ����� �� ������� �������
                        TestCellColor(cellCTRLs[Cell.pos.x, Cell.pos.y + smeshenie]))
                    {
                        //�� ���� ����������� �������
                        break;
                    }

                    //��������� ������ � ������ �����
                    cellLineUp.Add(cellCTRLs[Cell.pos.x, Cell.pos.y + smeshenie]);
                }
            }
            void TestSquare()
            {
                //�������� �� �������

                //������ ������
                if (cellLineRight.Count > 0 && cellLineUp.Count > 0 && !TestCellColor(cellCTRLs[Cell.pos.x + 1, Cell.pos.y + 1]))
                {
                    cellSquare.Add(cellLineRight[0]);
                    cellSquare.Add(cellLineUp[0]);
                    cellSquare.Add(cellCTRLs[Cell.pos.x + 1, Cell.pos.y + 1]);
                }

                //������ �����
                else if (cellLineRight.Count > 0 && cellLineDown.Count > 0 && !TestCellColor(cellCTRLs[Cell.pos.x + 1, Cell.pos.y - 1]))
                {
                    cellSquare.Add(cellLineRight[0]);
                    cellSquare.Add(cellLineDown[0]);
                    cellSquare.Add(cellCTRLs[Cell.pos.x + 1, Cell.pos.y - 1]);
                }

                //����� �����
                else if (cellLineLeft.Count > 0 && cellLineDown.Count > 0 && !TestCellColor(cellCTRLs[Cell.pos.x - 1, Cell.pos.y - 1]))
                {
                    cellSquare.Add(cellLineLeft[0]);
                    cellSquare.Add(cellLineDown[0]);
                    cellSquare.Add(cellCTRLs[Cell.pos.x - 1, Cell.pos.y - 1]);
                }

                //����� ������
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


                //��������� �� ����� �� 5
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

                //��������� �� ����� �� 4
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

                //��������� �� ����� �� 3
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

                //�������� �� �������
                void Square()
                {
                    if (cellSquare.Count >= 3)
                    {
                        Combination.square = true;

                        AddCellToCombination(Cell);
                        foreach (CellCTRL c in cellSquare) AddCellToCombination(c);
                    }
                }

                //�������� �� �����
                void Cross()
                {
                    if (Combination.horizontal && Combination.vertical)
                    {
                        Combination.cross = true;
                    }
                }

            }

            void AddCellToCombination(CellCTRL cellAdd) {
                //��������� ���� �� ��� ������ � ������
                bool found = false;
                foreach (CellCTRL cellComb in Combination.cells) {
                    //���� ������ �������
                    if (cellComb == cellAdd) {
                        found = true;
                        break;
                    }
                }

                //���� ������ � ������ �� ���� ����������, ��������� � ������
                if (!found) {
                    Combination.cells.Add(cellAdd);
                }
            }


            //��������� ������ �� ���������� ����� � ����������� ���������� � ����������
            bool TestCellColor(CellCTRL SecondCell)
            {
                bool test = false;
                if (SecondCell && SecondCell.cellInternal != null && SecondCell.cellInternal.type == CellInternalObject.Type.supercolor) {
                    test = true;
                }

                //������ ����
                if (
                    !SecondCell || //���� ����� ������ ���
                    !SecondCell.cellInternal || //� ������ ��� ������������
                    !Cell.cellInternal || //���� ������� � ������ ���
                    SecondCell.cellInternal.isMove || //���� ��� ������������ ��������� � ��������
                    SecondCell.cellInternal.color != Cell.cellInternal.color || //���� �� ������
                    SecondCell.cellInternal.type == CellInternalObject.Type.supercolor //��� ������ ����� �������, �� �� ������ ��� �������������� �� ����� ��� � ���������� ���
                    )
                {
                    //�� ���� ����������� �������
                    return true;
                }


                return false;
            }

        }

        //��������� ������ �� ����� ����������
        void TestSuperCombination() {
            foreach (Swap swap in BufferSwap) {
                //���������� ���� ��� � ��������
                if (swap.first.cellInternal.isMove || swap.second.cellInternal.isMove) {
                    continue;
                }

                Combination comb = new Combination();
                comb.cells.Add(swap.first);
                comb.cells.Add(swap.second);
                if (swap.first.panel || swap.second.panel)
                    comb.foundPanel = true;

                //����� ����� + ���-�� ���
                if (swap.first.cellInternal.type == CellInternalObject.Type.supercolor ||
                    swap.second.cellInternal.type == CellInternalObject.Type.supercolor)
                {

                    if (swap.first.cellInternal.type == CellInternalObject.Type.supercolor)
                    {
                        swap.first.cellInternal.Activate(CellInternalObject.Type.supercolor, swap.second.cellInternal, comb);
                    }
                    else
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.supercolor, swap.first.cellInternal, comb);
                    }

                }

                //������� + �������
                else if (swap.first.cellInternal.type == CellInternalObject.Type.airplane &&
                    swap.second.cellInternal.type == CellInternalObject.Type.airplane)
                {
                    swap.first.cellInternal.Activate(CellInternalObject.Type.airplane, swap.second.cellInternal, comb);
                }


                //������� + �����
                else if (swap.first.cellInternal.type == CellInternalObject.Type.airplane && swap.second.cellInternal.type == CellInternalObject.Type.bomb)
                {
                    swap.first.cellInternal.Activate(CellInternalObject.Type.airplane, swap.second.cellInternal, comb);
                }
                //����� + �������
                else if (swap.second.cellInternal.type == CellInternalObject.Type.airplane && swap.first.cellInternal.type == CellInternalObject.Type.bomb)
                {
                    swap.second.cellInternal.Activate(CellInternalObject.Type.airplane, swap.first.cellInternal, comb);
                }


                //������� + ������
                else if (swap.first.cellInternal.type == CellInternalObject.Type.airplane && (swap.second.cellInternal.type == CellInternalObject.Type.rocketVertical || swap.second.cellInternal.type == CellInternalObject.Type.rocketHorizontal))
                {
                    swap.first.cellInternal.Activate(CellInternalObject.Type.airplane, swap.second.cellInternal, comb);
                }
                //������ + �������
                else if (swap.second.cellInternal.type == CellInternalObject.Type.airplane && (swap.first.cellInternal.type == CellInternalObject.Type.rocketVertical || swap.first.cellInternal.type == CellInternalObject.Type.rocketHorizontal))
                {
                    swap.second.cellInternal.Activate(CellInternalObject.Type.airplane, swap.first.cellInternal, comb);
                }

                //����� + ���-�� �� �� ����
                else if ((swap.first.cellInternal.type == CellInternalObject.Type.bomb && swap.second.cellInternal.type != CellInternalObject.Type.color) ||
                    (swap.second.cellInternal.type == CellInternalObject.Type.bomb && swap.first.cellInternal.type != CellInternalObject.Type.color))
                {

                    if (swap.second.cellInternal.type == CellInternalObject.Type.bomb)
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.bomb, swap.first.cellInternal, comb);
                    }
                    else
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.rocketHorizontal, swap.first.cellInternal, comb);
                    }
                }


                //������ + ������
                else if ((swap.first.cellInternal.type == CellInternalObject.Type.rocketHorizontal || swap.first.cellInternal.type == CellInternalObject.Type.rocketVertical) &&
                    (swap.second.cellInternal.type == CellInternalObject.Type.rocketHorizontal || swap.second.cellInternal.type == CellInternalObject.Type.rocketVertical))
                {

                    if (swap.first.cellInternal.type == CellInternalObject.Type.rocketHorizontal)
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.rocketHorizontal, swap.first.cellInternal, comb);
                    }
                    else if (swap.first.cellInternal.type == CellInternalObject.Type.rocketVertical)
                    {
                        swap.second.cellInternal.Activate(CellInternalObject.Type.rocketVertical, swap.first.cellInternal, comb);
                    }
                    else if (swap.second.cellInternal.type == CellInternalObject.Type.rocketHorizontal)
                    {
                        swap.first.cellInternal.Activate(CellInternalObject.Type.rocketHorizontal, swap.second.cellInternal, comb);
                    }
                    else if (swap.second.cellInternal.type == CellInternalObject.Type.rocketVertical)
                    {
                        swap.first.cellInternal.Activate(CellInternalObject.Type.rocketVertical, swap.second.cellInternal, comb);
                    }

                }


            }
        }
        //������� ���� ����������� � ���������� �������
        void TestDamageAndSpawn() {
            //���������� ����������
            foreach (Combination comb in listCombinations) {
                //���� ���� ������ � ����������
                if (comb.cells.Count > 0) {
                    //��������� �������� ��� �����������
                    CalcCombination(comb);
                }
            }

            //��������� ����������
            void CalcCombination(Combination comb) {


                //���� �������� ���������� ������ ��� ������ � ���������� ����
                CellCTRL CellSpawn = comb.cells[0];
                float timelastMove = 0;
                CellInternalObject.InternalColor color = CellInternalObject.InternalColor.Red;

                //���������� ��� ������ ����� �������� ����� ���������� �� ����������
                foreach (CellCTRL cell in comb.cells) {

                    //�������� ������� ������ ��� ����� ��� ������, �������� ����� ������ ������ ��� ��� ������ �������� 100%
                    //���� ��������� ��������� ������ �� ���� ����
                    //���
                    //���� ����� ����������� ������ ������ ��� ��������� � ��� ������ ����
                    if ((CellSpawn.cellInternal.type != CellInternalObject.Type.color && cell.cellInternal.type == CellInternalObject.Type.color) ||
                        (CellSpawn.myInternalNum < cell.myInternalNum && cell.cellInternal.type == CellInternalObject.Type.color)) {
                        CellSpawn = cell;

                        if (cell.cellInternal) {
                            color = cell.cellInternal.color;

                            //���� ��� ����� ���������� ���� ����
                            if (timelastMove < cell.cellInternal.timeLastMoving) {
                                timelastMove = cell.cellInternal.timeLastMoving;
                            }
                        }

                    }

                    //���� ����� ������
                    if (cell.panel)
                        comb.foundPanel = true;
                    if (cell.mold > 0)
                        comb.foundMold = true;
                }


                if (CellSpawn.cellInternal) {
                    color = CellSpawn.cellInternal.color;
                }

                //������� ������ ����������
                //������� ������� ���� ��� ���������� ���� ���� ��� �� ��������
                foreach (CellCTRL c in comb.cells)
                {

                    if (Gameplay.main.movingCount <= 0)
                    {
                        mixColor();
                    }
                    else {
                        SetDamage();
                    }

                    //���������� ����
                    void mixColor() {
                        c.cellInternal.randColor();
                    }
                    //������� ����
                    void SetDamage() {

                        CellInternalObject partner = null;
                        //�������� ��� ���� ���������� ���������� ��������� ������������ ������
                        List<Swap> BufferSwapNew = new List<Swap>();
                        foreach (Swap swap in BufferSwap)
                        {
                            //���� ���������� ���������� ��������� ����������� ������
                            if (swap.first == c || swap.second == c)
                            {

                                Gameplay.main.MinusMoving(comb);

                                //���������  �������� �� ������������
                                //if (swap.first != c) partner = c.cellInternal;
                                //else if (swap.second != c) partner = c.cellInternal;

                                continue;
                            }
                            BufferSwapNew.Add(swap);
                        }
                        BufferSwap = BufferSwapNew;

                        //������� ���� �� ������
                        c.Damage(partner, comb);
                    }
                }

                //����� ������ ����, ������ ��������� ��� ����� ��������
                //��������� ����� ������ �� �����
                if (Gameplay.main.movingCount > 0) {
                    //���� ����� �� 5
                    if (comb.line5)
                    {
                        //������� ����� �������� �����
                        CreateSuperColor(CellSpawn, color, comb.ID);
                    }
                    //���� �����
                    else if (comb.cross)
                    {
                        //������� �����
                        CreateBomb(CellSpawn, color, comb.ID);
                    }
                    //���� �������������� �� 4
                    else if (comb.line4 && comb.horizontal)
                    {
                        CreateRocketVertical(CellSpawn, color, comb.ID);
                    }
                    //���� ������������ �� 3
                    else if (comb.line4 && comb.vertical)
                    {
                        CreateRocketHorizontal(CellSpawn, color, comb.ID);
                    }
                    //���� �������
                    else if (comb.square)
                    {
                        //����� ��������
                        CreateFly(CellSpawn, color, comb.ID);
                    }

                    //���������� ��������� �������� �����
                    ComboCount++;
                }
            }

        }
        
        //���������� ������������� ����������
        void DestroyDuplicateCombinations() {

            //���� ������ ��� ����� �������
            if (listCombinations.Count == 0) return;


            //������ ���������� �� �������� �� ������ ��� ��� ������������
            List<Combination> listCombinationsDelete = new List<Combination>();

            //���������� ������� ���������� � ������� ����������
            foreach (Combination combinationFirst in listCombinations) {

                foreach (Combination combinationSecond in listCombinations)
                {
                    //������������� ���� ��� ���� ����� ����������
                    if (combinationFirst == combinationSecond) continue;


                    bool foundIdenticalCells = false;
                    //���������� ������ ����� � ��� � ������� ����������
                    foreach (CellCTRL cell in combinationFirst.cells) {
                        if (foundIdenticalCells) break;
                        foreach (CellCTRL cellNew in combinationSecond.cells) {
                            //���� ������ �� �������� ������ ��������� � ������� �� �������
                            if (cell == cellNew) {
                                //�� ��� ������ ��� ��� ������ ���� ���� ���� ���������� �� ��� �����������
                                foundIdenticalCells = true;
                                break;
                            }
                        }
                    }

                    //���� ���� �������� �������� ����������
                    if (foundIdenticalCells)
                    {
                        //���� � ������ ����� ������ ��������� �� ��������
                        if (combinationFirst.cells.Count < combinationSecond.cells.Count)
                        {
                            listCombinationsDelete.Add(combinationFirst);
                        }
                        //���� �� ������ ����� ������, ��������� �� �� ��������
                        else if(combinationFirst.cells.Count > combinationSecond.cells.Count) {
                            listCombinationsDelete.Add(combinationSecond);
                        }
                    }
                }
            }

            //������� ����� ������ ����� � ��������� ������ ������������ ��� ����������
            List<Combination> combinationsNew = new List<Combination>();
            foreach (Combination combTest in listCombinations) {
                bool needAdd = true;
                foreach (Combination combIgnore in listCombinationsDelete) {
                    //���� ��� ���������� ������� ����� ��������
                    if (combTest == combIgnore) {
                        needAdd = false;
                        break;
                    }
                }

                //��������� ���� �����
                if (needAdd) combinationsNew.Add(combTest);

            }

            //�������� ������ ���� �����
            listCombinations = combinationsNew;

        }

    }


    /// <summary>
    /// ������� ����� �� ����� ������ ������
    /// </summary>
    public void CreateBomb(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniBomb(cellLast, this, internalColor, combID);


    }
    /// <summary>
    /// ������� ������� �� ����� ������ ������
    /// </summary>
    public void CreateFly(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniFly(cellLast, this, internalColor, combID);


    }
    /// <summary>
    /// ������� ������������� �� ����� ������ ������
    /// </summary>
    public void CreateSuperColor(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniSuperColor(cellLast, this, internalColor, combID);


    }
    /// <summary>
    /// ������� ������������ ������ �� ����� ������ ������
    /// </summary>
    public void CreateRocketVertical(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniRocketVertical(cellLast, this, internalColor, combID);

    }
    /// <summary>
    /// ������� �������������� ������ �� ����� ������ ������
    /// </summary>
    public void CreateRocketHorizontal(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniRocketHorizontal(cellLast, this, internalColor, combID);

    }

    //�������� �� �� ��� ����� �������� ������� �������
    void TestStartSwap()
    {
        //���� ���� ��������� ������
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

        //������ ���� � ��� ����������
        if (!CellSwap || !CellSelect)
            return;

        bool neibour = false;

        //��������� ���������
        //�����
        if (CellSelect.pos.x > 0 && CellSwap == cellCTRLs[CellSelect.pos.x - 1, CellSelect.pos.y])
            neibour = true;
        //������
        else if (CellSelect.pos.x < cellCTRLs.GetLength(0) - 1 && CellSwap == cellCTRLs[CellSelect.pos.x + 1, CellSelect.pos.y])
            neibour = true;
        //�����
        else if (CellSelect.pos.y > 0 && CellSwap == cellCTRLs[CellSelect.pos.x, CellSelect.pos.y - 1])
            neibour = true;
        //������
        else if (CellSelect.pos.y < cellCTRLs.GetLength(1) - 1 && CellSwap == cellCTRLs[CellSelect.pos.x, CellSelect.pos.y + 1])
            neibour = true;

        //���� �� �������� ������, �������
        if (!neibour)
        {
            CellSelect = null;
            CellSwap = null;
            return;
        }

        //���� ����� �� ��������
        if (!CellSelect.cellInternal || //���� � ����� ����� ��������
            !CellSwap.cellInternal ||
            CellSelect.cellInternal.isMove || //���� � ������ ���������� ��������
            CellSwap.cellInternal.isMove ||
            CellSelect.BlockingMove > 0 || //���� ������ ����������
            CellSwap.BlockingMove > 0 ||
            Gameplay.main.movingCan <= 0 //���� ���� ����
            )
        {
            CellSelect = null;
            CellSwap = null;
            return;
        }

        //������ ������������
        CellInternalObject InternalSelect = CellSelect.cellInternal;
        CellInternalObject InternalSwap = CellSwap.cellInternal;

        //���������� �������� � ������� � ��������
        CellSelect.cellInternal.myCell = null;
        CellSwap.cellInternal.myCell = null;

        //���������� �������� � �������� � �����
        CellSelect.cellInternal = null;
        CellSwap.cellInternal = null;

        InternalSelect.StartMove(CellSwap);
        InternalSwap.StartMove(CellSelect);

        //��������� ������ � ������ ������������
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
            //���� � ����� ���� ������������ � ��� �� ��������, ���������� �� ���� �����
            else if (swap.first.cellInternal && swap.second.cellInternal &&
                !swap.first.cellInternal.isMove && !swap.second.cellInternal.isMove)
            {

                //������ ������������
                CellInternalObject InternalFirst = swap.first.cellInternal;
                CellInternalObject InternalSecond = swap.second.cellInternal;

                //���������� �������� � ������� � ��������
                swap.first.cellInternal.myCell = null;
                swap.second.cellInternal.myCell = null;

                //���������� �������� � �������� � �����
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
    //���� ���������� ����� ����������
    void TestMold() {

        if (Gameplay.main.movingMoldCount <= lastStepCount || //���� ���������� ����� ������ ������ ��� ������� �������
            Gameplay.main.combo > 0 || //���� ���� ����� ���������� � ��� �� ��������
            isMoving
            ) {
            lastTime = Time.unscaledTime; //���������� ��������� ����������� �����
            return;
        }

        //���� �������� ������ ������� �������
        if (Time.unscaledTime - lastTime < 0.5f) {
            return;
        }

        //���������� ���
        lastStepCount++;

        StepMold();

        //��� �������
        void StepMold() {
            //�������� ��������� ������� �� ����� ������
            int num = Random.Range(0, moldCTRLs.Count);
            if (moldCTRLs.Count > 0 && moldCTRLs[num]) {
                moldCTRLs[num].TestSpawn();
            }
        }
    }

    /// <summary>
    /// C����� ���� ����� � ������� ���������� �� �������� � �������
    /// </summary>
    public CellCTRL[] cellsPriority;
    void TestCalcPriorityCells() {
        //���������� �����������

        //���������� ��� ������ � ���������� ��������� �� ���� �������
        for (int num = 1; num < cellsPriority.Length; num++) {
            //���� ������ ���, �� ����������
            if (cellsPriority[num] == null) continue;

            //������ ������� � ���������� ����, ���������� ������ ��� ��� �� �������� ����
            if (cellsPriority[num - 1] == null || cellsPriority[num - 1].MyPriority < cellsPriority[num].MyPriority) {
                //CellCTRL Now = cellsPriority[num];
                CellCTRL Previously = cellsPriority[num - 1];

                cellsPriority[num - 1] = cellsPriority[num];
                cellsPriority[num] = Previously;
            }
        }

        
    }

    /// <summary>
    /// ������� ������ ���������� ��� ������� ��� �����������
    /// </summary>
    public void SetSelectCell(CellCTRL CellClick) {
        if (!CellSelect) {
            CellSelect = CellClick;
        }
        else {
            //��������� ������ �� ���������
            if (isNeibour())
            {
                //��� ����� ������ �������
                CellSwap = CellClick;
            }
            else {
                CellSwap = null;
                CellSelect = CellClick;
            }

            //��������� ���������
            bool isNeibour() {

                //�����
                if (CellSelect.pos.x > 0 &&
                    cellCTRLs[CellSelect.pos.x - 1, CellSelect.pos.y] &&
                    cellCTRLs[CellSelect.pos.x - 1, CellSelect.pos.y] == CellClick) {
                    return true;
                }
                //������
                else if (CellSelect.pos.x < cellCTRLs.GetLength(0) - 1 &&
                    cellCTRLs[CellSelect.pos.x + 1, CellSelect.pos.y] &&
                    cellCTRLs[CellSelect.pos.x + 1, CellSelect.pos.y] == CellClick) {
                    return true;
                }
                //�����
                else if (CellSelect.pos.y > 0 &&
                    cellCTRLs[CellSelect.pos.x, CellSelect.pos.y - 1] &&
                    cellCTRLs[CellSelect.pos.x, CellSelect.pos.y - 1] == CellClick)
                {
                    return true;
                }
                //������
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
