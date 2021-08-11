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

    [Header("Other")]
    [SerializeField]
    CellCTRL CellSelect;
    [SerializeField]
    CellCTRL CellSwap;



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
        
    }

    // Update is called once per frame
    void Update()
    {
        TestSpawn();
        TestLine();
        TestSwap();
    }


    //�������� ����� �� ������� � �������������
    void TestSpawn() {
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
                            rect.pivot = new Vector2(-x, -y-10);

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
                        cell.gettingScore();
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
                        cell.gettingScore();
                    }
                }
            }

        }

    }

    //�������� �� �� ��� ����� �������� ������� �������
    void TestSwap()
    {
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
            CellSwap.dontMoving > 0
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
                    cellCTRLs[CellSelect.pos.y, CellSelect.pos.y - 1] &&
                    cellCTRLs[CellSelect.pos.y, CellSelect.pos.y - 1] == CellClick)
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
