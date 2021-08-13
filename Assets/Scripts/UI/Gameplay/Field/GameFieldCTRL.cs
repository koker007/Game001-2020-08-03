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
        TestSpawn();
        //TestLine();
        TestFieldCombination();
        //TestStartSwap();
        //TestReturnSwap();
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
                if (cellCTRLs[x,y] && !cellCTRLs[x, y].cellInternal && cellCTRLs[x,y].dontMoving == 0 && !cellCTRLs[x,y].movingInternalNow) {

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
                            //���������� ��
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

    //�������� �� ����� ���������� ��������
    void TestLine()
    {
        //������� ������ ��������� ��������� �� �����
        for (int y = cellCTRLs.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                line(x, y);
            }
        }

        void line(int x, int y) {
            //���� ������� ��� �������.
            if (!cellCTRLs[x,y].cellInternal) {
                return;
            }


            testRight();
            testDown();

            //�������� �����
            void testRight() {
                List<CellCTRL> cellLine = new List<CellCTRL>();

                for (int plusX = 0; plusX < 5; plusX++) {

                    if ((x + plusX) >= cellCTRLs.GetLength(0) || //���� ����� �� ������� �������
                        !cellCTRLs[x + plusX, y] || //���� ����� ������ ���
                        !cellCTRLs[x + plusX, y].cellInternal || //���� ������� � ������ ���
                        cellCTRLs[x + plusX, y].movingInternalNow || //���� ��� ������������ ��������� � ��������
                        cellCTRLs[x + plusX, y].cellInternal.color != cellCTRLs[x, y].cellInternal.color) {
                        //�� ���� ����������� �������
                        break;
                    }

                    //��������� ������ � ������ �����
                    cellLine.Add(cellCTRLs[x + plusX, y]);
                }

                //���� ��������� �����
                if (cellLine.Count >= 3) {
                    foreach (CellCTRL cell in cellLine) {
                        if (Gameplay.main.movingCount > 0)
                            cell.Damage();
                        else cell.cellInternal.randColor();
                    }
                }
            }
            //�������� ����
            void testDown() {
                List<CellCTRL> cellLine = new List<CellCTRL>();

                for (int minusY = 0; minusY < 5; minusY++)
                {

                    if ((y - minusY) < 0 || //���� ����� �� ������� �������
                        !cellCTRLs[x, y - minusY] || //���� ����� ������ ���
                        !cellCTRLs[x, y - minusY].cellInternal || //���� ������� � ������ ���
                        cellCTRLs[x, y - minusY].movingInternalNow || //���� ��� ������������ ��������� � ��������
                        cellCTRLs[x, y - minusY].cellInternal.color != cellCTRLs[x, y].cellInternal.color)
                    {
                        //�� ���� ����������� �������
                        break;
                    }

                    //��������� ������ � ������ �����
                    cellLine.Add(cellCTRLs[x, y - minusY]);
                }

                //���� ��������� �����
                if (cellLine.Count >= 3)
                {
                    foreach (CellCTRL cell in cellLine)
                    {
                        if (Gameplay.main.movingCount > 0)
                            cell.Damage();
                        else cell.cellInternal.randColor();
                    }
                }
            }

        }

    }

    //��������� ��� ������ �� ���������
    void TestFieldCombination() {
        //������� ������ ��������� ��������� �� �����
        for (int y = cellCTRLs.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                TestCellCombination(cellCTRLs[x, y]);
            }
        }
    }
    //��������� ������ �� �����������
    bool TestCellCombination(CellCTRL Cell) {
        if (!Cell) {
            return false;
        }

        if (!Cell.cellInternal) {
            Cell.movingInternalNow = false;
        }

        List<CellCTRL> cellLineRight = new List<CellCTRL>();
        List<CellCTRL> cellLineLeft = new List<CellCTRL>();
        List<CellCTRL> cellLineDown = new List<CellCTRL>();
        List<CellCTRL> cellLineUp = new List<CellCTRL>();

        List<CellCTRL> cellSquare = new List<CellCTRL>();

        //������ ����� ������� ������ �������� ����
        List<CellCTRL> cellDamage = new List<CellCTRL>();

        bool line5 = false;
        bool square = false;
        bool line4 = false;
        bool cross = false;

        bool foundCombination = false;

        TestLines();
        //TestSquare();
        CalcResult();
        Damage();

        return foundCombination;

        ///////////////////////////////////////////////////////////////////
        void TestLines() {

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
        void TestSquare() {
            //�������� �� �������

            //������ ������
            if (cellLineRight.Count > 0 && cellLineUp.Count > 0 && TestCellColor(cellCTRLs[Cell.pos.x + 1, Cell.pos.y + 1])) {
                cellSquare.Add(cellLineRight[0]);
                cellSquare.Add(cellLineUp[0]);
                cellSquare.Add(cellCTRLs[Cell.pos.x + 1, Cell.pos.y + 1]);
            }

            //������ �����
            else if (cellLineRight.Count > 0 && cellLineDown.Count > 0 && TestCellColor(cellCTRLs[Cell.pos.x + 1, Cell.pos.y - 1]))
            {
                cellSquare.Add(cellLineRight[0]);
                cellSquare.Add(cellLineDown[0]);
                cellSquare.Add(cellCTRLs[Cell.pos.x + 1, Cell.pos.y - 1]);
            }

            //����� �����
            else if (cellLineLeft.Count > 0 && cellLineDown.Count > 0 && TestCellColor(cellCTRLs[Cell.pos.x - 1, Cell.pos.y - 1]))
            {
                cellSquare.Add(cellLineLeft[0]);
                cellSquare.Add(cellLineDown[0]);
                cellSquare.Add(cellCTRLs[Cell.pos.x - 1, Cell.pos.y - 1]);
            }

            //����� ������
            else if (cellLineLeft.Count > 0 && cellLineUp.Count > 0 && TestCellColor(cellCTRLs[Cell.pos.x - 1, Cell.pos.y + 1]))
            {
                cellSquare.Add(cellLineLeft[0]);
                cellSquare.Add(cellLineUp[0]);
                cellSquare.Add(cellCTRLs[Cell.pos.x - 1, Cell.pos.y + 1]);
            }
        }

        void CalcResult() {

            bool horizontal = false;
            bool vertical = false;

            Line5();
            Line4();
            Line3();
            Square();
            Cross();

            //��������� �� ����� �� 5
            void Line5() {
                if (cellLineRight.Count + cellLineLeft.Count == 4) {
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
            void Line4() {
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
            void Square() {
                if (cellSquare.Count >= 3) {
                    square = true;

                    AddDamage(Cell);
                    foreach (CellCTRL c in cellSquare) AddDamage(c);
                }
            }

            //�������� �� �����
            void Cross() {
                if (horizontal && vertical) {
                    cross = true;
                }
            }
        }

        //������� ���� ���� �������� ������� � ������
        void Damage() {
            //���� ���������� �� ����, �������
            if (cellDamage.Count == 0) {
                return;
            }

            //������� ������� ���� ��� ���������� ���� ���� ��� �� ��������
            foreach (CellCTRL c in cellDamage) {
                c.Damage();
            }

            //
            foundCombination = true;

            //��������� ����� ������ �� �����

        }


        //��������� ������ �� ���������� �����
        bool TestCellColor(CellCTRL SecondCell) {

            
            if (
                !SecondCell || //���� ����� ������ ���
                !SecondCell.cellInternal ||
                !Cell.cellInternal || //���� ������� � ������ ���
                SecondCell.movingInternalNow || //���� ��� ������������ ��������� � ��������
                SecondCell.cellInternal.color != Cell.cellInternal.color)
            {
                //�� ���� ����������� �������
                return true;
            }


            return false;
        }

        //�������� ������ � ������, �������� ��� �� ��� ���
        void AddDamage(CellCTRL cellDamageNew) {
            //��������� ��� ����� ������� ��� � ������
            foreach (CellCTRL c in cellDamage) {
                //������ ��� ���� � ������, �������
                if (c == cellDamageNew) return;
            }

            cellDamage.Add(cellDamageNew);
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
            CellSelect.movingInternalNow || //���� � ������ ���������� ��������
            CellSwap.movingInternalNow ||
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
        Gameplay.main.movingCan--;
    }

    void TestReturnSwap() {

        List<Swap> BufferSwapNew = new List<Swap>();

        foreach (Swap swap in BufferSwap) {

            if (swap == null) {
                continue;
            }
            //���� � ����� ���� ������������ � ��� �� ��������, ���������� �� ���� �����
            else if (swap.first.cellInternal && swap.second.cellInternal &&
                !swap.first.movingInternalNow && !swap.second.movingInternalNow)
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

                Gameplay.main.movingCount++;
                Gameplay.main.movingCan--;

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
