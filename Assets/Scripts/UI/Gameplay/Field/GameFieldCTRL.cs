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
    bool isMoving = false; //��������� �� � �������� ������� ������
    //��������� �� � �������� ����� ���� ������� �� ����
    void TestMoving() {
        bool movingNow = false;
        for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
            if (movingNow) break;

            for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                if (movingNow) break;

                //���� ������ ���, ������� ������
                if (!cellCTRLs[x,y]) continue;

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

    //������ ������ ������� ���� ������� ��������
    List<Swap> BufferSwap = new List<Swap>();


    public CellCTRL[,] cellCTRLs; //������
    List<CellInternalObject> cellInternalObjects; //������������ �����

    //���������������� ������� ����
    public void inicializeField(int sizeX, int sizeY) {
        //���������� ���� � �����
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f,0.5f);

        //������� ������������ �������� ����
        cellCTRLs = new CellCTRL[sizeX, sizeY];
        cellInternalObjects = new List<CellInternalObject>();

        AddAllCells();

        //��������� ��� ���� ��������
        void AddAllCells() {
            for(int x = 0; x < cellCTRLs.GetLength(0); x++) {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                    if (!cellCTRLs[x,y]) {
                        GameObject cellObj = Instantiate(prefabCell, parentOfCells);
                        

                        //���� ���������
                        cellCTRLs[x, y] = cellObj.GetComponent<CellCTRL>();
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

        TestSpawn(); //�������
        TestMoving(); //��������� ������� �������� ��� ������ �����
        TestFieldCombination(); //������ ����������

        TestStartSwap(); //�������� �����
        TestReturnSwap(); //���������� �����
    }

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

                        //���� ������ ������ ��������� ������������
                        if (!cellCTRLs[x,y].cellInternal) {
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
                if (cellCTRLs[x,y] && !cellCTRLs[x, y].cellInternal && cellCTRLs[x,y].dontMoving == 0) {

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
                        else if (y + plusY < cellCTRLs.GetLength(1) && !cellCTRLs[x, y + plusY])
                        {
                            break;
                        }

                        //�� ����������� ���� �������� ��� ������������ ������

                        //���� ������ ���� ������ � ������������� � ��� �� �����������
                        else if (cellCTRLs[x, y + plusY].dontMoving <= 0 && cellCTRLs[x, y + plusY].cellInternal) {

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
        //������� ����� ������ ����������
        listCombinations = new List<Combination>();

        //������� ������ ��������� ��� ������ �� ����������
        for (int y = cellCTRLs.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                TestCellCombination2(cellCTRLs[x, y]);
            }
        }

        TestDamageAndSpawn();

        ///////////////////////////////////////////////////////////////
        //��������� ������ �� ����������
        void TestCellCombination(CellCTRL Cell)
        {
            /*

            //������� ���� ������ ���, ��� ��� ������������, ��� ������������ � ��������, ��� ��� ������ ������
            if (!Cell || !Cell.cellInternal || Cell.cellInternal.isMove)
            {
                return;
            }

            List<CellCTRL> cellLineRight = new List<CellCTRL>();
            List<CellCTRL> cellLineLeft = new List<CellCTRL>();
            List<CellCTRL> cellLineDown = new List<CellCTRL>();
            List<CellCTRL> cellLineUp = new List<CellCTRL>();

            List<CellCTRL> cellSquare = new List<CellCTRL>();

            //������ ����� ������� ������ �������� ����
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

                Line5();
                Line4();
                Line3();
                Square();
                Cross();

                //TestActivate();

                //��������� �� ����� �� 5
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

                //��������� �� ����� �� 4
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

                //��������� �� ����� �� 3
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

                //�������� �� �������
                void Square()
                {
                    if (cellSquare.Count >= 3)
                    {
                        square = true;

                        AddDamage(Cell);
                        foreach (CellCTRL c in cellSquare) AddDamage(c);
                    }
                }

                //�������� �� �����
                void Cross()
                {
                    if (horizontal && vertical)
                    {
                        cross = true;
                    }
                }

                //�������� �� ���������
                void TestActivate() {

                    foreach (Swap swap in BufferSwap)
                    {
                        //���� ���� ��� �� � �������� �������
                        if (swap.first.cellInternal.isMove || swap.second.cellInternal.isMove)
                            continue;

                        bool activeFirst = isActivate(swap.first);
                        bool activeSecond = isActivate(swap.second);


                        //���� ������ ��������� ������������
                        CellCTRL activate = swap.first;
                        CellCTRL partner = swap.second;


                        //�����������
                        //���� ������ �� �������� � ������ ��
                        if (!activeFirst && activeSecond) {
                            activate = swap.second;
                            partner = swap.first;

                            activeFirst = true;
                            activeSecond = false;
                        }

                        //������� ���� ������ ��� � �� ���� �� ��������
                        if (!activeFirst) continue;

                        //���� ������ �� �������� �� ������ ������� �������� ������ � ������ �����
                        if (!activeSecond)
                        {
                            AddDamage(activate);
                        }
                        //����� ���� ����������� ������� ��� ������ �� ���������� �� �������
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

            //������� ���� ���� �������� ������� � ������
            void Damage()
            {
                //���� ���������� �� ����, �������
                if (cellDamage.Count == 0)
                {
                    return;
                }

                //���������� ������ � ���������� ������������
                CellCTRL cellLast = cellDamage[0];
                CellInternalObject.InternalColor internalColor = CellInternalObject.InternalColor.Red;
                if (cellLast.cellInternal) internalColor = cellLast.cellInternal.color;


                //��������� �� �� ��� ������ �� ���� ���������� ���� � ������
                List<CellCTRL> CombinationFounded = null;
                foreach (List<CellCTRL> combination in listCombinations) {
                    if (CombinationFounded != null) break; //������� ���� ���������� ���� �������

                    //��������� ������ �� �� ��� �� ��� � ������ ���� ����������
                    foreach (CellCTRL cellCombOld in combination) {
                        if (CombinationFounded != null) break; //������� ���� ���������� ���� �������

                        foreach (CellCTRL cellCombNow in cellDamage) {
                            if (CombinationFounded != null) break; //������� ���� ���������� ���� �������

                            //���� ����� ������ � ������ ���������� �� ������ ������� ���������� �������� ������������ ������
                            if (cellCombNow == cellCombOld) {
                                CombinationFounded = combination;
                            }
                        }
                    }
                }

                

                //������� ������� ���� ��� ���������� ���� ���� ��� �� ��������
                foreach (CellCTRL c in cellDamage)
                {
                    bool cellCombinationFound = false;
                    //���� ���� ���������� ���������� �� ��������� �� �� ��� ������� ������ � ������ ��� ���������� ���
                    if (CombinationFounded != null) {
                        foreach (CellCTRL cellCombOld in CombinationFounded)
                        {
                            if (cellCombOld == c) {
                                cellCombinationFound = true;
                            }
                        }
                    }

                    //���� ��� ������ ��� ���� � ������ ���������� �������� ��� ���
                    if (cellCombinationFound) continue;
                    //���� ������ ���������� ����, ��������� ������
                    else if(CombinationFounded != null) {
                        CombinationFounded.Add(c);
                    }

                    //���� ��� ������ ��������� ������������
                    if (cellLast.myInternalNum < c.myInternalNum)
                    {
                        //����������
                        cellLast = c;
                        if (cellLast.cellInternal && cellLast.cellInternal.type == CellInternalObject.Type.color) {
                            internalColor = cellLast.cellInternal.color;
                        }
                    }


                    CellInternalObject partner = c.cellInternal;
                    //�������� ��� ���� ���������� ���������� ��������� ������������ ������
                    List<Swap> BufferSwapNew = new List<Swap>();
                    foreach (Swap swap in BufferSwap)
                    {
                        if (swap.first == c || swap.second == c)
                        {

                            Gameplay.main.MinusMoving();

                            //���������  �������� �� ������������
                            if (swap.first != c) partner = c.cellInternal;
                            else if (swap.second != c) partner = c.cellInternal; 

                            continue;
                        }
                        BufferSwapNew.Add(swap);
                    }
                    BufferSwap = BufferSwapNew;

                    //������� ���� �� ������
                    c.Damage(partner);


                }

                //���� ����� ���������� ������, �� ������ ��� ��������� � ������ ���, ��������� ���������� � ������ ����������
                if (CombinationFounded == null) {
                    listCombinations.Add(cellDamage);
                }

                //
                foundCombination = true;

                //��������� ����� ������ �� �����
                //���� ����� �� 5
                if (line5) {
                    //������� ����� �������� �����
                    //CreateSuperColor(cellLast, internalColor);
                }
                //���� �����
                else if (cross) {
                    //������� �����
                    //CreateBomb(cellLast, internalColor);
                }
                //���� �������������� �� 4
                else if (line4 && horizontal) {
                    CreateRocketVertical(cellLast, internalColor);
                }
                //���� ������������ �� 3
                else if (line4 && vertical) {
                    CreateRocketHorizontal(cellLast, internalColor);
                }
                //���� �������
                else if (square) {
                    //����� ��������
                    CreateFly(cellLast, internalColor);
                }
            }


            //��������� ������ �� ���������� �����
            bool TestCellColor(CellCTRL SecondCell)
            {

                //������ ����
                if (
                    !SecondCell || //���� ����� ������ ���
                    !SecondCell.cellInternal ||
                    !Cell.cellInternal || //���� ������� � ������ ���
                    SecondCell.cellInternal.isMove || //���� ��� ������������ ��������� � ��������
                    SecondCell.cellInternal.color != Cell.cellInternal.color)
                {
                    //�� ���� ����������� �������
                    return true;
                }


                return false;
            }

            //�������� ������ � ������, �������� ��� �� ��� ���
            void AddDamage(CellCTRL cellDamageNew)
            {
                //��������� ��� ����� ������� ��� � ������
                foreach (CellCTRL c in cellDamage)
                {
                    //������ ��� ���� � ������, �������
                    if (c == cellDamageNew) { 
                        return;
                    
                    }
                }

                cellDamage.Add(cellDamageNew);
            }

            
            //������� ����� �� ����� ������ ������
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
        //��������� ������ �� ����������. ������� 2021.08.18
        void TestCellCombination2(CellCTRL Cell)
        {

            //������� ���� ������ ���, ��� ��� ������������, ��� ������������ � ��������, ��� ��� ������ ������
            if (!Cell || !Cell.cellInternal || Cell.cellInternal.isMove)
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


            //��������� ������ �� ���������� �����
            bool TestCellColor(CellCTRL SecondCell)
            {

                //������ ����
                if (
                    !SecondCell || //���� ����� ������ ���
                    !SecondCell.cellInternal ||
                    !Cell.cellInternal || //���� ������� � ������ ���
                    SecondCell.cellInternal.isMove || //���� ��� ������������ ��������� � ��������
                    SecondCell.cellInternal.color != Cell.cellInternal.color)
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

                //����� ����� + ����� �����
                //if () {
                
                //}
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
                        CellInternalObject partner = c.cellInternal;
                        //�������� ��� ���� ���������� ���������� ��������� ������������ ������
                        List<Swap> BufferSwapNew = new List<Swap>();
                        foreach (Swap swap in BufferSwap)
                        {
                            if (swap.first == c || swap.second == c)
                            {

                                Gameplay.main.MinusMoving();

                                //���������  �������� �� ������������
                                if (swap.first != c) partner = c.cellInternal;
                                else if (swap.second != c) partner = c.cellInternal;

                                continue;
                            }
                            BufferSwapNew.Add(swap);
                        }
                        BufferSwap = BufferSwapNew;

                        //������� ���� �� ������
                        c.Damage(partner);
                    }
                }

                //����� ������ ����, ������ ��������� ��� ����� ��������
                //��������� ����� ������ �� �����
                if (Gameplay.main.movingCount > 0) {
                    //���� ����� �� 5
                    if (comb.line5)
                    {
                        //������� ����� �������� �����
                        CreateSuperColor(CellSpawn, color);
                    }
                    //���� �����
                    else if (comb.cross)
                    {
                        //������� �����
                        CreateBomb(CellSpawn, color);
                    }
                    //���� �������������� �� 4
                    else if (comb.line4 && comb.horizontal)
                    {
                        CreateRocketVertical(CellSpawn, color);
                    }
                    //���� ������������ �� 3
                    else if (comb.line4 && comb.vertical)
                    {
                        CreateRocketHorizontal(CellSpawn, color);
                    }
                    //���� �������
                    else if (comb.square)
                    {
                        //����� ��������
                        CreateFly(CellSpawn, color);
                    }

                    //���������� ��������� �������� �����
                    ComboCount++;
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            //������� ����� �� ����� ������ ������
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
            CellSelect.dontMoving > 0 || //���� ������ ����������
            CellSwap.dontMoving > 0 ||
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

    //������� ������ ���������� ��� ������� ��� �����������
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
