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
    /// ������� ���������� ������ ����������� �������� �� �����
    /// </summary>
    [SerializeField]
    public int CountRockBlocker = 0;
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
    /// <summary>
    /// ���������� ������� ������ � ������� ����� ���� �������
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
    public CellCTRL CellSelect; //������ ���������� ������������� ������
    [SerializeField]
    CellCTRL CellSwap; //������ ���������� ������������� ������

    [SerializeField]
    RectTransform rectParticleSelect;

    public float timeLastBoom = 0;
    public float timeLastMove = 0;

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
    /// ����������� �����
    /// </summary>
    public RockCTRL[,] rockCTRLs;

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

            CountDestroyCrystals = new int[level.NumColors];

            //������� ������������ �������� ����
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
                    //����� �� ��������� ������
                    LevelsScript.CellInfo cellInfo = level.ReturneCell(new Vector2Int(x,y));
                    
                    //���� ���� ������ �� ����������
                    if (cellInfo == null)
                    {
                        continue;
                    }
                    else {
                        CountInteractiveCells++;
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
                        cellCTRLs[x, y].rock = level.cells[x, y].rock;
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
                    //����� �� ������� ������
                    if (cellCTRLs[x, y].rock > 0) {
                        GameObject rockObj = Instantiate(prefabRock, parentOfRock);
                        cellCTRLs[x, y].rockCTRL = rockObj.GetComponent<RockCTRL>();

                        //������������� �����
                        cellCTRLs[x, y].rockCTRL.inicialize(cellCTRLs[x, y]);
                    }

                    //������� ��������� �������
                    if (level.cells[x, y].typeCell != CellInternalObject.Type.none &&
                        cellCTRLs[x, y].BlockingMove == 0 //���� ���� �����
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
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.color5) {
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
        isMoving();
        TestFieldCombination(true); //������ ���������� � ����������� �����

        TestFieldPotencial(); //���� ������������� ����
        TestRandomNotPotencial(); //������������� ���� ����� �� ����������

        TestStartSwap(); //�������� �����
        TestReturnSwap(); //���������� �����

        TestMold(); //��������� �������� ����� ����

        TestCalcPriorityCells(); //���������� ���������� �����
    }

    //��������� ������������� �������� ����
    void StartInicialize() {
        //��������� � ���� ��� ������

        timeLastMove = Time.unscaledTime;

        //RandomInicialize();

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
                if (cellCTRLs[x,y] && !cellCTRLs[x, y].cellInternal && 
                    cellCTRLs[x,y].BlockingMove == 0 &&
                    cellCTRLs[x,y].rock == 0) {

                    //��������� ������ �� �� ���� �� ��� ���-�� ��� ����� ������
                    for (int plusY = 0; plusY <= cellCTRLs.GetLength(1); plusY++) {

                        //���� �������� ������ ����� ����
                        if (y + plusY >= cellCTRLs.GetLength(1) &&
                            Time.unscaledTime - timeLastBoom > 0.1f)
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
                            cellCTRLs[x, y + plusY].BlockingMove > 0 ||
                            cellCTRLs[x, y + plusY].rock > 0
                            ))
                        {
                            break;
                        }

                        //�� ����������� ���� �������� ��� ������������ ������

                        //���� ������ ���� ������ � ������������� � ��� �� �����������
                        else if (y + plusY < cellCTRLs.GetLength(1) && cellCTRLs[x, y + plusY].BlockingMove <= 0 && cellCTRLs[x, y + plusY].cellInternal && cellCTRLs[x, y + plusY].rock <= 0) {

                            break;
                        }
                    }

                        
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///����� � ������ ����������

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

        public float timeLastAction = 0;

        public bool foundPanel = false;
        public bool foundBenefit = false;

        public Combination(){
            IDLast++; //���������� id ����������
            ID = IDLast; //��� ��� �����
        }

        public Combination(Combination ParentComb) {

            if (ParentComb != null)
            {
                ID = ParentComb.ID; //����� id ��������� �������� ������������ ���������� ��������
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

                IDLast++; //���������� id ����������
                ID = IDLast; //��� ��� �����

            }
            
        }
    }
    void TestFieldCombination(bool damageOnCombinations) {
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

        //���� �� ����� ��������� ���� �������
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
        //��������� ������ �� ����������. ������� 2 2021.08.18
        void TestCellCombination(CellCTRL Cell)
        {

            //������� ���� ������ ���, ��� ��� ������������, ��� ������������ � ��������, ��� ��� �������, ������� �� ������ ���������� � ����������
            if (!Cell || !Cell.cellInternal || (Cell.cellInternal.isMove && damageOnCombinations) || Cell.cellInternal.type == CellInternalObject.Type.color5)
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
                if (SecondCell && SecondCell.cellInternal != null && SecondCell.cellInternal.type == CellInternalObject.Type.color5) {
                    test = true;
                }

                //������ ����
                if (
                    !SecondCell || //���� ����� ������ ���
                    !SecondCell.cellInternal || //� ������ ��� ������������
                    !Cell.cellInternal || //���� ������� � ������ ���
                    (SecondCell.cellInternal.isMove && damageOnCombinations) || //���� ��� ������������ ��������� � ��������
                    SecondCell.cellInternal.color != Cell.cellInternal.color || //���� �� ������
                    SecondCell.cellInternal.type == CellInternalObject.Type.color5 || //��� ������ ����� �������, �� �� ������ ��� �������������� �� ����� ��� � ���������� ���
                    (SecondCell.cellInternal.type == CellInternalObject.Type.bomb && SecondCell.cellInternal.activateNeed) //�������������� ����� �� ��������� � �����������
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
                if (swap.first.cellInternal == null || swap.second.cellInternal == null || 
                    swap.first.cellInternal.isMove || swap.second.cellInternal.isMove) {
                    continue;
                }

                Combination comb = new Combination();
                comb.cells.Add(swap.first);
                comb.cells.Add(swap.second);
                if (swap.first.panel || swap.second.panel)
                    comb.foundPanel = true;

                //����� ����� + ���-�� ���
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

                //������� + �������
                else if (swap.first.cellInternal.type == CellInternalObject.Type.airplane &&
                    swap.second.cellInternal.type == CellInternalObject.Type.airplane)
                {
                    swap.first.cellInternal.Activate(CellInternalObject.Type.airplane, swap.second.cellInternal, comb);
                    isFoundSuperComb = true;
                }


                //������� + �����
                else if (swap.first.cellInternal.type == CellInternalObject.Type.airplane && swap.second.cellInternal.type == CellInternalObject.Type.bomb)
                {
                    swap.first.cellInternal.Activate(CellInternalObject.Type.airplane, swap.second.cellInternal, comb);
                    isFoundSuperComb = true;
                }
                //����� + �������
                else if (swap.second.cellInternal.type == CellInternalObject.Type.airplane && swap.first.cellInternal.type == CellInternalObject.Type.bomb)
                {
                    swap.second.cellInternal.Activate(CellInternalObject.Type.airplane, swap.first.cellInternal, comb);
                    isFoundSuperComb = true;
                }


                //������� + ������
                else if (swap.first.cellInternal.type == CellInternalObject.Type.airplane && (swap.second.cellInternal.type == CellInternalObject.Type.rocketVertical || swap.second.cellInternal.type == CellInternalObject.Type.rocketHorizontal))
                {
                    swap.first.cellInternal.Activate(CellInternalObject.Type.airplane, swap.second.cellInternal, comb);
                    isFoundSuperComb = true;
                }
                //������ + �������
                else if (swap.second.cellInternal.type == CellInternalObject.Type.airplane && (swap.first.cellInternal.type == CellInternalObject.Type.rocketVertical || swap.first.cellInternal.type == CellInternalObject.Type.rocketHorizontal))
                {
                    swap.second.cellInternal.Activate(CellInternalObject.Type.airplane, swap.first.cellInternal, comb);
                    isFoundSuperComb = true;
                }

                //����� + ���-�� �� �� ����
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


                //������ + ������
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


                //���������� ������� ���� �������� ����������
                if (isFoundSuperComb) {
                    Gameplay.main.MinusMoving(comb);
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
                        comb.foundBenefit = true;
                }


                if (CellSpawn.cellInternal) {
                    color = CellSpawn.cellInternal.color;
                }


                //���� ���� �������������
                if (comb.cells[0].cellInternal.color == CellInternalObject.InternalColor.Ultimate) {
                    ActivateCombUltimate();
                }
                //����� ������� ���������
                else {
                    ActivateCombNormal();
                }

                void ActivateCombNormal() {
                    //������� ������ ����������
                    //������� ������� ���� ��� ���������� ���� ���� ��� �� ��������
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

                        //���������� ����
                        void mixColor()
                        {
                            c.cellInternal.randColor();
                        }
                        //������� ����
                        void SetDamage()
                        {

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
                    if (Gameplay.main.movingCount > 0)
                    {
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

                void ActivateCombUltimate() {
                    //������� ������ ����������
                    //������� ������� ���� ��� ���������� ���� ���� ��� �� ��������
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

                        //���������� ����
                        void mixColor()
                        {
                            c.cellInternal.randColor();
                        }
                        //������� ����
                        void SetDamage()
                        {

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


                    //���������� ������ � ������� ��������� ����
                    //���� ����� �� 5 ���������� ���
                    if (comb.line5)
                    {
                        //����
                        //���������� ������ � ������� ������������ ������
                        CellCTRL cellCross = comb.cells[0];
                        foreach (CellCTRL cell in comb.cells)
                        {
                            if (cell.myInternalNum > cellCross.myInternalNum)
                            {
                                cellCross = cell;
                            }
                        }

                        //���������� ��� �����
                        for (int x = 0; x < cellCTRLs.GetLength(0); x++)
                        {
                            for (int y = 0; y < cellCTRLs.GetLength(1); y++)
                            {
                                //���� ������ ��� �������
                                if (!cellCTRLs[x, y])
                                {
                                    continue;
                                }

                                //������� ��������� �� ������ �� ���� ������ ���������������� �����
                                float dist = Vector2.Distance(cellCross.pos, cellCTRLs[x, y].pos);

                                cellCross.myField.cellCTRLs[x, y].BufferCombination = comb;
                                cellCross.myField.cellCTRLs[x, y].BufferNearDamage = false;
                                cellCTRLs[x, y].DamageInvoke(dist * 0.05f);
                            }
                        }

                    }
                    //���� �������� �����
                    else if (comb.cross)
                    {
                        //���������� ������ � ������� ������������ ������
                        //����� ���������� ������ � ����� ���� ����������� ��������� ������
                        CellCTRL cellCross = comb.cells[0];
                        foreach (CellCTRL cell in comb.cells)
                        {
                            if (cell.myInternalNum > cellCross.myInternalNum)
                            {
                                cellCross = cell;
                            }
                        }

                        int radius = 3;
                        //������ �������.. �������� �� 3 ������ ����� ������ ����� ����
                        for (int x = -radius; x <= radius; x++)
                        {
                            for (int y = -radius; y <= radius; y++)
                            {
                                int fieldPosX = cellCross.pos.x + x;
                                int fieldPosY = cellCross.pos.y + y;
                                //���� ����� �� ������� ����� ��� ���� ������ ����
                                if (fieldPosX < 0 || fieldPosX >= cellCross.myField.cellCTRLs.GetLength(0) ||
                                    fieldPosY < 0 || fieldPosY >= cellCross.myField.cellCTRLs.GetLength(1) ||
                                    !cellCross.myField.cellCTRLs[fieldPosX, fieldPosY]
                                    )
                                {
                                    continue;
                                }

                                //������� ����� �������� ������ ���� ������
                                float time = Vector2.Distance(new Vector2(), new Vector2(x, y)) * 0.1f;
                                //
                                cellCTRLs[fieldPosX, fieldPosY].BufferCombination = comb;
                                cellCTRLs[fieldPosX, fieldPosY].BufferNearDamage = false;
                                cellCTRLs[fieldPosX, fieldPosY].DamageInvoke(time);
                            }
                        }

                        //������� ������� ������
                        Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateBoomBomb(gameObject, cellCross, radius);
                        particle3DCTRL.SetSpeed(radius);
                        particle3DCTRL.SetSize(radius * 3);
                        particle3DCTRL.SetColor(cellCross.cellInternal.GetColor(color) * 0.5f);

                    }
                    //���� �������� �������
                    else if (comb.square)
                    {
                        //��������� ������
                        int count = 0;
                        foreach (CellCTRL cellCTRL in comb.cells)
                        {
                            if (cellCTRL)
                            {
                                count++;
                                //���� ���� ������ 4 �������� �������
                                if (count > 4)
                                {
                                    break;
                                }

                                //���� ��� ��� ���� �������, ���������
                                if (cellCTRL.cellInternal && cellCTRL.cellInternal.type == CellInternalObject.Type.airplane)
                                {
                                    cellCTRL.cellInternal.Activate(CellInternalObject.Type.airplane, null, comb);
                                    break;
                                }

                                //������� ���������� ������
                                Destroy(cellCTRL.cellInternal.gameObject);

                                //��������� � ���� ������ �������
                                //������� �������
                                CreateFly(cellCTRL, cellCTRL.cellInternal.color, comb.ID);
                                //��������� �������
                                cellCTRL.cellInternal.Activate(CellInternalObject.Type.airplane, null, comb);
                            }
                        }
                    }
                    //���� ��������� �����������
                    else if (comb.horizontal && comb.line4)
                    {
                        foreach (CellCTRL c in comb.cells)
                        {
                            //����������� ��������� �����
                            c.explosion = new CellCTRL.Explosion(false, false, true, true, 0.1f, comb);
                            c.BufferCombination = comb;
                            c.BufferNearDamage = false;
                            c.ExplosionBoomInvoke(c.explosion, c.explosion.timer);
                        }
                    }
                    //���� ��������� ���������
                    else if (comb.vertical && comb.line4) {
                        foreach (CellCTRL c in comb.cells)
                        {
                            //����������� ��������� �����
                            c.explosion = new CellCTRL.Explosion(true, true, false, false, 0.1f, comb);
                            c.BufferCombination = comb;
                            c.BufferNearDamage = false;
                            c.ExplosionBoomInvoke(c.explosion, c.explosion.timer);
                        }
                    }

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

        //�������� �� ������������� �������� ����� �����
        void TestHitShop() {
            //���� ������ ��� ��������� ����� ����
            if (buffer.superHit != MenuGameplay.main.SuperHitSelected) {
                //������� ��������� � ������
                CellSelect = null;

                buffer.superHit = MenuGameplay.main.SuperHitSelected;
            }

            //���� ������ ������� ����� ����
            if (buffer.superHit != MenuGameplay.SuperHitType.none) {
                //���� �� ��������, �������
                if (CellSelect == null) {
                    return;
                }

                if (buffer.superHit == MenuGameplay.SuperHitType.internalObj)
                    TestShopInternal();
                else if (buffer.superHit == MenuGameplay.SuperHitType.rosket2x)
                    TestShopRocket();
                else if(buffer.superHit == MenuGameplay.SuperHitType.superColor)
                    TestShopSuperColor();

                //������������, ���������� ������
                MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;
            }

            void TestShopInternal() {
                if (Gameplay.main.buttonDestroyInternal > 0) {
                    Gameplay.main.buttonDestroyInternal--;

                    //������� ����������
                    Combination comb = new Combination();
                    comb.cells.Add(CellSelect);

                    //���� ������ ����
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

                    //������� ����������
                    Combination comb = new Combination();
                    comb.cells.Add(CellSelect);

                    //���� ������ ����
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
        public CellCTRL Moving = null; //������ ������� ���������� �����������
        public CellCTRL Target = null; //������ ������������� ������� ��������� ����������

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
            //��������� ��������� �����
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
    /// ����� ������������� ����������
    /// </summary>
    /// 
    void TestFieldPotencial() {

        //������������� �������� �� ������� � �����������
        if (Gameplay.main.isMissionComplite() || Gameplay.main.isMissionDefeat())
        {
            potencialBest = null;
        }
        AnimationPlayPotencial();

        float timeToTest = 2;
        //���� ������� ���� �������� �� �������� ����
        if (Time.unscaledTime - timeLastMove < timeToTest) {
            listPotencial = new List<PotencialComb>();
            potencialBest = null;

            Debug.Log("Waiting Time " + Time.unscaledTime);

            return;
        }

        //������� ���� ������ ��� ���� ��� ������ ��������� ��� ���������
        if (listPotencial.Count > 0 || Gameplay.main.isMissionComplite() || Gameplay.main.isMissionDefeat()) {
            return;
        }

        //�������� ����� ������������� ����������
        ReCalcListPotencial();

        //���� ���������� �������
        if (listPotencial.Count > 0) {
            //�������� ������
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
        //���� ���������� �� ������� � ���� ������ � ����������� �������� ��� �� ������
        else if(ListWaitingInternals.Count <= 0){
            CreateRandomInternalList();
            Debug.Log("Not Found potencial comb " + Time.unscaledTime);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        //������������� �������� ���������� ���� ����
        void AnimationPlayPotencial()
        {
            if (potencialBest == null)
                return;

            //���� ��������� ����
            //���������� ������ � ������������� ��������
            foreach (CellCTRL potencialCell in potencialBest.cells)
            {
                potencialCell.cellInternal.animatorObject.PlayAnimation("ApperanceCombinationObject");
            }
            potencialBest.Moving.cellInternal.animatorObject.PlayAnimation("ApperanceCombinationObject");
        }

        void CreateRandomInternalList() {
            //���������� ��� ������
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

    //����������� ����� ������ ������������� ����������
    void ReCalcListPotencial() {

        

        //�������� ����� ������������� ����������
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

            //������� ���� ���� ������ ��� ��� ��� ����������
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



            //����� ����
            //�����
            if (isCellOK(new Vector2Int(cell.pos.x - 1, cell.pos.y)))
            {

                potencialLeft.Target = cell;
                potencialLeft.cells.Add(cellCTRLs[cell.pos.x - 1, cell.pos.y]);


                //���������� ����
                CellInternalObject.InternalColor color = cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.color;

                //������� ������ ���� ������ ���� �� ��������
                for (int n = 1; n < cellCTRLs.GetLength(0); n++)
                {
                    //���� ������� ��������� � ���� ������ ���������
                    if (isCellOK(new Vector2Int(cell.pos.x - 1 - n, cell.pos.y)) &&
                        cellCTRLs[cell.pos.x - 1 - n, cell.pos.y].cellInternal.color == color)
                    {
                        //��������� ������ � ������
                        potencialLeft.cells.Add(cellCTRLs[cell.pos.x - 1 - n, cell.pos.y]);
                    }
                    //����� ������� �� ���� ������ ����
                    else
                    {
                        break;
                    }
                }
            }
            //�����
            if (isCellOK(new Vector2Int(cell.pos.x + 1, cell.pos.y)))
            {

                potencialRight.Target = cell;
                potencialRight.cells.Add(cellCTRLs[cell.pos.x + 1, cell.pos.y]);

                //���������� ����
                CellInternalObject.InternalColor color = cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.color;

                //������� ������ ���� ������ ���� �� ��������
                for (int n = 1; n < cellCTRLs.GetLength(0); n++)
                {
                    //���� ������� ��������� � ���� ������ ���������
                    if (isCellOK(new Vector2Int(cell.pos.x + 1 + n, cell.pos.y)) &&
                        cellCTRLs[cell.pos.x + 1 + n, cell.pos.y].cellInternal.color == color)
                    {
                        //��������� ������ � ������
                        potencialRight.cells.Add(cellCTRLs[cell.pos.x + 1 + n, cell.pos.y]);
                    }
                    //����� ������� �� ���� ������ ����
                    else
                    {
                        break;
                    }
                }
            }
            //������
            if (isCellOK(new Vector2Int(cell.pos.x, cell.pos.y + 1)))
            {

                potencialUp.Target = cell;
                potencialUp.cells.Add(cellCTRLs[cell.pos.x, cell.pos.y + 1]);

                //���������� ����
                CellInternalObject.InternalColor color = cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.color;

                //������� ������ ���� ������ ���� �� ��������
                for (int n = 1; n < cellCTRLs.GetLength(0); n++)
                {
                    //���� ������� ��������� � ���� ������ ���������
                    if (isCellOK(new Vector2Int(cell.pos.x, cell.pos.y + 1 + n)) &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1 + n].cellInternal.color == color)
                    {
                        //��������� ������ � ������
                        potencialUp.cells.Add(cellCTRLs[cell.pos.x, cell.pos.y + 1 + n]);
                    }
                    //����� ������� �� ���� ������ ����
                    else
                    {
                        break;
                    }
                }
            }
            //�����
            if (isCellOK(new Vector2Int(cell.pos.x, cell.pos.y - 1)))
            {

                potencialDown.Target = cell;
                potencialDown.cells.Add(cellCTRLs[cell.pos.x, cell.pos.y - 1]);

                //���������� ����
                CellInternalObject.InternalColor color = cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.color;

                //������� ������ ���� ������ ���� �� ��������
                for (int n = 1; n < cellCTRLs.GetLength(0); n++)
                {
                    //���� ������� ��������� � ���� ������ ���������
                    if (isCellOK(new Vector2Int(cell.pos.x, cell.pos.y - 1 - n)) &&
                        cellCTRLs[cell.pos.x, cell.pos.y - 1 - n].cellInternal.color == color)
                    {
                        //��������� ������ � ������
                        potencialDown.cells.Add(cellCTRLs[cell.pos.x, cell.pos.y - 1 - n]);
                    }
                    //����� ������� �� ���� ������ ����
                    else
                    {
                        break;
                    }
                }
            }

            //������ ��������� �������� ������
            //���� ��������� ���������, ��������� � ������ �����������
            isHavePotencial(potencialLeft);
            isHavePotencial(potencialRight);
            isHavePotencial(potencialUp);
            isHavePotencial(potencialDown);

            //��������� �� ����� ����������
            CombinatePotencialSuper();

            //�������� ��������
            CombinatePotencialSquare();

            //������ ��������� �� ����� ����������
            CombinatePotencialHorizontal(potencialLeft, potencialRight);
            CombinatePotencialVertical(potencialUp, potencialDown);

            CombinatePotencialsAngle(potencialLeft, potencialUp); //���� ����
            CombinatePotencialsAngle(potencialRight, potencialUp); //����� ����
            CombinatePotencialsAngle(potencialRight, potencialDown); //����� ���
            CombinatePotencialsAngle(potencialLeft, potencialDown); //���� ���

            //��������� ������ �� �� �������������
            bool isCellOK(Vector2Int cellPos)
            {
                bool result = false;

                //���� ������ �� ����� �� �������
                //��� ����������
                //���� ������������
                //������������ �� ����� ����� �����
                if (cellPos.x >= 0 && cellPos.x < cellCTRLs.GetLength(0) && cellPos.y >= 0 && cellPos.y < cellCTRLs.GetLength(1) &&
                    cellCTRLs[cellPos.x, cellPos.y] != null &&
                    cellCTRLs[cellPos.x, cellPos.y].cellInternal != null &&
                    cellCTRLs[cellPos.x, cellPos.y].cellInternal.type != CellInternalObject.Type.color5
                    )
                {

                    //������� ���� ��� ����� ������� ������ ��������������
                    if (cellCTRLs[cellPos.x, cellPos.y].cellInternal.type == CellInternalObject.Type.bomb && 
                        cellCTRLs[cellPos.x, cellPos.y].cellInternal.activateNeed) {
                        return false;
                    }

                    result = true;
                }

                return result;
            }

            //��������� ���������� �� ���������
            bool isHavePotencial(PotencialComb potTest)
            {
                bool result = false;

                //���� ��� ������ ��� ����� 1
                if (potTest.cells.Count >= 1)
                {
                    //��������� ������� ���������� ������ �� ��������

                    //���� ������ ����
                    //���� ��� �� ���� ����� ������ �� ����
                    //���� � ������ ���� ������������
                    //���� � ������������ ��������� ����
                    //���� ������������ �� ����� ����

                    //�����
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
                    //������
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
                    //������
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
                    //�����
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

            //�������� �������� ����� �� ����� ����������
            void CombinatePotencialSuper()
            {
                //���� ������� ������ �� ����
                if (cell.cellInternal.type == CellInternalObject.Type.color)
                {
                    //�������
                    return;
                }

                CellCTRL left = null;
                CellCTRL right = null;
                CellCTRL up = null;
                CellCTRL down = null;

                //��������� �������� ������ �� �� ��� ��� ���� �� ����
                //�����
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
                //������
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
                //������
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
                //�����
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


                //���� ������� ������ ����� ����
                if (cell.cellInternal.type == CellInternalObject.Type.color5)
                {
                    //��������� ������� �� �� ��� ��� ������� ����
                    //�����
                    if (cell.pos.x - 1 >= 0 &&
                        cellCTRLs[cell.pos.x - 1, cell.pos.y] != null &&
                        cellCTRLs[cell.pos.x - 1, cell.pos.y].rock == 0 &&
                        cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal != null &&
                        cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.color)
                    {

                        left = cellCTRLs[cell.pos.x - 1, cell.pos.y];

                    }
                    //������
                    if (cell.pos.x + 1 < cellCTRLs.GetLength(0) &&
                        cellCTRLs[cell.pos.x + 1, cell.pos.y] != null &&
                        cellCTRLs[cell.pos.x + 1, cell.pos.y].rock == 0 &&
                        cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal != null &&
                        cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.color)
                    {

                        right = cellCTRLs[cell.pos.x + 1, cell.pos.y];

                    }
                    //������
                    if (cell.pos.y + 1 < cellCTRLs.GetLength(1) &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1] != null &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1].rock == 0 &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal != null &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.type == CellInternalObject.Type.color)
                    {

                        up = cellCTRLs[cell.pos.x, cell.pos.y + 1];

                    }
                    //�����
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
                //���� ����� ���������� ���������� ������
                if (left != null)
                {
                    PotencialComb super = new PotencialComb();

                    super.Moving = cell;
                    super.cells.Add(left);
                    super.Target = left;

                    //������ ���������� �������, ������� �� ���������
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

                    //������ ���������� �������, ������� �� ���������
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

                    //������ ���������� �������, ������� �� ���������
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

                    //������ ���������� �������, ������� �� ���������
                    super.CalcPriority();
                    super.priority += superPriority;

                    listPotencial.Add(super);
                }
            }

            //������������� �������
            void CombinatePotencialSquare()
            {


                //���� ����
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


                    //��������� �������
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

                    //������ ���������� �������, ������� �� ���������
                    super.CalcPriority();
                    super.priority += 20;

                    listPotencial.Add(super);
                }

                //���� �����
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


                    //��������� �������
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

                    //������ ���������� �������, ������� �� ���������
                    super.CalcPriority();
                    super.priority += 20;

                    listPotencial.Add(super);
                }

                //��� �����
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


                    //��������� �������
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

                    //������ ���������� �������, ������� �� ���������
                    super.CalcPriority();
                    super.priority += 20;

                    listPotencial.Add(super);
                }

                //��� ����
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


                    //��������� �������
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

                    //������ ���������� �������, ������� �� ���������
                    super.CalcPriority();
                    super.priority += 20;

                    listPotencial.Add(super);
                }

            }

            void CombinatePotencialHorizontal(PotencialComb first, PotencialComb second)
            {
                PotencialComb potencialNew = new PotencialComb();

                //� ���������� ������ ��������� ������� ������,
                //� ���������� ������ ���� ������ ���� ������
                //������ ������ ������ ���� ������ �����
                //����� ���� ���������� ������������ ������
                if (first.Target == second.Target &&
                    first.cells.Count >= 1 && second.cells.Count >= 1 &&
                    first.cells[0].cellInternal.color == second.cells[0].cellInternal.color &&
                    first.Moving && second.Moving)
                {

                    CellCTRL moving = null;

                    CellCTRL up = null;
                    CellCTRL down = null;
                    //�������� ������
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

                    //���� ��� ������������ ������ �������
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

                    //������ ���������� �������, ������� �� ���������
                    potencialNew.CalcPriority();

                    if (potencialNew.cells.Count >= 4)
                        potencialNew.priority += 80;

                    listPotencial.Add(potencialNew);
                }

            }

            void CombinatePotencialVertical(PotencialComb first, PotencialComb second)
            {
                PotencialComb potencialNew = new PotencialComb();

                //� ���������� ������ ��������� ������� ������,
                //� ���������� ������ ���� ������ ���� ������
                //������ ������ ������ ���� ������ �����
                //����� ���� ���������� ������������ ������
                if (first.Target == second.Target &&
                    first.cells.Count >= 1 && second.cells.Count >= 1 &&
                    first.cells[0].cellInternal.color == second.cells[0].cellInternal.color &&
                    first.Moving && second.Moving)
                {

                    CellCTRL moving = null;

                    CellCTRL right = null;
                    CellCTRL left = null;
                    //�������� ������
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

                    //���� ��� ������������ ������ �������
                    if (moving == null) return;

                    foreach (CellCTRL c in first.cells)
                        potencialNew.cells.Add(c);

                    foreach (CellCTRL c in second.cells)
                        potencialNew.cells.Add(c);

                    potencialNew.Target = first.Target;

                    potencialNew.Moving = moving;


                    //������ ���������� �������, ������� �� ���������
                    potencialNew.CalcPriority();

                    if (potencialNew.cells.Count >= 4)
                        potencialNew.priority += 80;

                    listPotencial.Add(potencialNew);
                }

            }

            void CombinatePotencialsAngle(PotencialComb first, PotencialComb second)
            {
                PotencialComb potencialNew = new PotencialComb();

                //� ���������� ������ ��������� ������� ������,
                //� ���������� ������ ���� �� 2 ������
                //������ ������ ������ ���� ������ �����
                //����� ���� ���������� ������������ ������
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


                    //������ ���������� �������, ������� �� ���������
                    potencialNew.CalcPriority();
                    potencialNew.priority += 30;

                    listPotencial.Add(potencialNew);
                }
            }
        }
    }

    //������ ����� ��������� �������������
    List<CellInternalObject> ListWaitingInternals = new List<CellInternalObject>();

    //������������ ������ ������� ������� �������������
    void TestRandomNotPotencial() {
        //������� ���� ��� ����� ��� �������������
        if (ListWaitingInternals.Count == 0)
            return;

        //������� � ������ � ���� ���-�� �� ������ ������, �������
        if (!isDoneTransformToCenter()) {
            return;
        }

        //��������� ����������� �������, ���� ���� �������������
        CellSelect = null;
        CellSwap = null;

        //������������� ����� �������
        RandomizadeNewPos();


        bool isDoneTransformToCenter() {

            Vector2 pivotNeed = new Vector2((cellCTRLs.GetLength(0)/2)*-1, (cellCTRLs.GetLength(1)/2)*-1);

            foreach (CellInternalObject cellInternal in ListWaitingInternals) {

                cellInternal.rectMy.pivot += (pivotNeed - cellInternal.rectMy.pivot) * Time.deltaTime * 4;

            }
            

            //��������� ��� ���������� �� �������� � ������
            foreach (CellInternalObject cellInternal in ListWaitingInternals) {
                if (Vector2.Distance(pivotNeed, cellInternal.rectMy.pivot) > 0.1f) {
                    return false;
                }
            }

            //���� ��� ������� ���������� ������, ������ ��������
            return true;

        }
        
        //������������
        void RandomizadeNewPos() {
            //���������� ������ ���� �������������
            List<CellCTRL> cellsList = new List<CellCTRL>();
            foreach (CellInternalObject cellInternal in ListWaitingInternals) {
                cellsList.Add(cellInternal.myCell);
            }

            //��������� ������

            //����
            int testNow = 0;
            int testMax = 100;
            //������� ���� ������ ���� ������ 100 � ������ �� �������
            while (ListWaitingInternals.Count > 0 && testNow < testMax) {
                if (testNow < testMax / 4)
                {
                    SetAllCellInternal();
                }
                else if (testNow < (testMax / 4) * 2)
                {
                    //��������������� ���� � �������
                    SetAllCellInternalRandomColor(testNow - (testMax / 4));
                }
                else if (testNow < (testMax / 4) * 3)
                {
                    //��������������� ���� �� ������ � ������� ��������
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

            //���������� ���� ������������� �� ������
            void SetAllCellInternal() {

                //���������� ����� ������������ � �������
                SetAllCellNull();

                for (int x = 0; x < ListWaitingInternals.Count; x++) {
                    CellCTRL selectedCell = null;

                    //������� ������ � ������ � ������������ �������
                    ListWaitingInternals[x].myCell = null;

                    while (!selectedCell) {
                        CellCTRL randomCell = cellsList[Random.Range(0, cellsList.Count)];

                        //���� ������ ���������� � � ��� ���� ������������
                        if (randomCell && randomCell.cellInternal == null) {
                            selectedCell = randomCell;
                        }
                    }


                    //������� ������ ��������� ����
                    ListWaitingInternals[x].StartMove(selectedCell);
                    
                }

                TestFieldCombination(false);

                //������� ������ ���� ���� ������������� ���������� � ��� ������� ����������
                if (isPotencialFound() && listCombinations.Count <= 0) {
                    //������� ������
                    ListWaitingInternals = new List<CellInternalObject>();
                }

            }

            void SetAllCellInternalRandomColor(int countRandomMax) {
                //���������� ����� ������������ � �������
                SetAllCellNull();

                int countRandomNow = 0;
                for (int x = 0; x < ListWaitingInternals.Count; x++)
                {
                    CellCTRL selectedCell = null;

                    //������� ������ � ������ � ������������ �������
                    ListWaitingInternals[x].myCell = null;
                    if (countRandomNow < countRandomMax && ListWaitingInternals[x].type == CellInternalObject.Type.color) {
                        countRandomNow++;

                        ListWaitingInternals[x].randColor();
                    }

                    while (!selectedCell)
                    {
                        CellCTRL randomCell = cellsList[Random.Range(0, cellsList.Count)];

                        //���� ������ ���������� � � ��� ���� ������������
                        if (randomCell && randomCell.cellInternal == null)
                        {
                            selectedCell = randomCell;
                        }
                    }


                    //������� ������ ��������� ����
                    ListWaitingInternals[x].StartMove(selectedCell);

                }

                //��������� �� ���������� ������
                TestFieldCombination(false);

                //������� ������ ���� ������������� ���������� � ��� ������� ����������
                if (isPotencialFound() && listCombinations.Count <= 0)
                {
                    //������� ������
                    ListWaitingInternals = new List<CellInternalObject>();
                }
            }

            void SetAllCellInternalRandom(int countRandomMax)
            {
                //���������� ����� ������������ � �������
                SetAllCellNull();

                int countRandomNow = 0;
                for (int x = 0; x < ListWaitingInternals.Count; x++)
                {
                    CellCTRL selectedCell = null;

                    //������� ������ � ������ � ������������ �������
                    ListWaitingInternals[x].myCell = null;
                    if (countRandomNow < countRandomMax)
                    {
                        countRandomNow++;

                        ListWaitingInternals[x].randColor();
                    }

                    while (!selectedCell)
                    {
                        CellCTRL randomCell = cellsList[Random.Range(0, cellsList.Count)];

                        //���� ������ ���������� � � ��� ���� ������������
                        if (randomCell && randomCell.cellInternal == null)
                        {
                            selectedCell = randomCell;
                        }
                    }


                    //������� ������ ��������� ����
                    ListWaitingInternals[x].StartMove(selectedCell);

                }

                //��������� �� ���������� ������
                TestFieldCombination(false);

                //������� ������ ���� ���� ������������� ���������� � ��� ������� ����������
                if (isPotencialFound() && listCombinations.Count <= 0)
                {
                    //������� ������
                    ListWaitingInternals = new List<CellInternalObject>();
                }
            }

            void SetAllCellInternalRandomType(int countRandomMax)
            {
                //���������� ����� ������������ � �������
                SetAllCellNull();

                int countRandomNow = 0;
                for (int x = 0; x < ListWaitingInternals.Count; x++)
                {
                    CellCTRL selectedCell = null;

                    //������� ������ � ������ � ������������ �������
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

                        //���� ������ ���������� � � ��� ���� ������������
                        if (randomCell && randomCell.cellInternal == null)
                        {
                            selectedCell = randomCell;
                        }
                    }


                    //������� ������ ��������� ����
                    ListWaitingInternals[x].StartMove(selectedCell);

                }

                TestFieldCombination(false);

                //������� ������ ���� ���� ������������� ���������� � ��� ������� ����������
                if (isPotencialFound() && listCombinations.Count <= 0)
                {
                    //������� ������
                    ListWaitingInternals = new List<CellInternalObject>();
                }
            }

            //��������� ������� ���������� ����� �� ����������
            bool isCombinationFound()
            {
                bool result = false;

                //���������� ������ ������
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

    //��� ���������� ������
    public struct Buffer{
        public MenuGameplay.SuperHitType superHit;
    }
    Buffer buffer;

    //����� ����������
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //�������� ������������ ��������

    /// <summary>
    /// ������� ����� �� ����� ������ ������
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
    /// ������� ������� �� ����� ������ ������
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
    /// ������� ������������� �� ����� ������ ������
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
    /// ������� ������������ ������ �� ����� ������ ������
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
    /// ������� �������������� ������ �� ����� ������ ������
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
    //�������� �� �� ��� ����� �������� ������� �������
    void TestStartSwap()
    {
        //���� ���������� ������ ���� � ������ ������ 5 ������ ������� ���������
        if (CellSelect && Time.unscaledTime - timeCellSelect > 4) {
            CellSelect = null;
        }

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
            CellSelect.BlockingMove > 0 || //���� � ������ �������
            CellSwap.BlockingMove > 0 ||
            CellSelect.rock > 0 || //���� � ������ ������
            CellSwap.rock > 0 ||
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
            isMovingInternalObj
            ) {
            lastTime = Time.unscaledTime; //���������� ��������� ����������� �����
            return;
        }

        //���� �������� ������ ������� �������
        if (Time.unscaledTime - lastTime < 0.5f) {
            return;
        }

        StepMold();

        //��� �������
        void StepMold() {
            //�������� ��������� ������� �� ����� ������
            int num = Random.Range(0, moldCTRLs.Count);
            //���� ������� ����������, ���� �������� ���������� �������, ���� ����� �������
            if (moldCTRLs.Count > 0 && moldCTRLs[num] && moldCTRLs[num].TestSpawn()) {
                //���������� ��� �������
                lastStepCount++;
            }
        }
    }

    /// <summary>
    /// C����� ���� ����� � ������� ���������� �� �������� � �������
    /// </summary>
    public CellCTRL[] cellsPriority;
    void TestCalcPriorityCells() {
        //���������� �����������
        bool isComplite = false;
        for (int testCount = 0; testCount < 50 && !isComplite; testCount++) {

            //���� ���� ���� ������� ��� �������� �� ������ ���������
            isComplite = true;

            //���������� ��� ������ � ���������� ��������� �� ���� �������
            for (int num = 1; num < cellsPriority.Length; num++)
            {
                //���� ������ ���, �� ����������
                if (cellsPriority[num] == null) continue;

                //������ ������� � ���������� ����, ���������� ������ ��� ��� �� �������� ����
                if (cellsPriority[num - 1] == null || cellsPriority[num - 1].MyPriority < cellsPriority[num].MyPriority)
                {
                    //CellCTRL Now = cellsPriority[num];
                    CellCTRL Previously = cellsPriority[num - 1];

                    cellsPriority[num - 1] = cellsPriority[num];
                    cellsPriority[num] = Previously;

                    //��������� ��������, ���� ��������� �������
                    isComplite = false;
                }
            }
        }

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
            timeCellSelect = Time.unscaledTime;
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

    //��������� ������ �� ���������� ��������������� ������
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
    bool isMovingInternalObj = false; //��������� �� � �������� ������� ������
    bool isMovingFly = false;

    bool isMoving() {
        if (timelastMoveTest != Time.unscaledTime)
        {
            bool isMovingNow = false;

            timelastMoveTest = Time.unscaledTime;

            //���� ����� ��� ����������� �������� ��� ��������� �����
            if (Gameplay.main.isMissionComplite()) {
                Gameplay.main.colors = 6;
                Gameplay.main.superColorPercent = 20;

                CellSelect = null;
            }

            TestMoveInternalObj(); //��������� ������� �������� ��� ������ �����
            TestMoveFly();

            //���� ���������� ��������
            if (isMovingInternalObj || isMovingFly || Time.unscaledTime - movingObjLastTime < 1 ) {
                isMovingNow = true;
            }
            else {
                isMovingNow = false;
            }


            //���� ���������� ���������
            if (isMovingOld && !isMovingNow)
            {

                //�������� ��������� � ����������
                MidleMessageCombo();

                //���� ������ ����� ��������� ������������
                if (Gameplay.main.isMissionComplite()) {
                    //���������� �� ������� ��� ��������� ����� ����� ������ � ��������� ������������� �����
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
                    //���� �������� ����
                    else if (Gameplay.main.movingCan > 0) {
                        while (Gameplay.main.movingCan > 0) {
                            //�������� ��������� ������
                            int x = Random.Range(0, cellCTRLs.GetLength(0));
                            int y = Random.Range(0, cellCTRLs.GetLength(1));

                            //���� ��� ������ �� �������� �������
                            if (cellCTRLs[x,y] == null || cellCTRLs[x,y].cellInternal == null || cellCTRLs[x,y].cellInternal.type != CellInternalObject.Type.color) {
                                continue;
                            }

                            //�������� ���
                            CellInternalObject.Type typeNew = CellInternalObject.Type.rocketHorizontal;
                            if (Random.Range(0, 100) < 50) typeNew = CellInternalObject.Type.rocketVertical;
                            

                            //��������� ���������
                            cellCTRLs[x, y].cellInternal.setColorAndType(cellCTRLs[x,y].cellInternal.color, typeNew);
                            Combination comb = new Combination();
                            comb.cells.Add(cellCTRLs[x, y]);
                            if (cellCTRLs[x, y].panel) {
                                comb.foundPanel = true;
                            }
                            //����������
                            cellCTRLs[x, y].cellInternal.Activate(cellCTRLs[x,y].cellInternal.type, null, comb);
                            

                            //�������� ���
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

            //��������� ����� ��������
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

    //��������� �� � �������� ����� ���� ������� �� ����

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

                //������� ������
                if (!cellCTRLs[x, y] || //��� ������
                    !cellCTRLs[x, y].cellInternal || //��� ����������� �������
                    cellCTRLs[x, y].BlockingMove > 0 || //������������ �������
                    cellCTRLs[x, y].rock > 0 //|| //��� ���� ������
                    //(cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[x,y].cellInternal.activateNeed) //��� ���� �������������� �����
                    ) continue;

                //���� ����� ���� ���� ���������, �� ���������
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
