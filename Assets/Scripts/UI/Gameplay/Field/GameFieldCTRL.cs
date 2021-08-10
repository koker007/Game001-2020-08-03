using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// ������������� �������������� ������
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
                            internalCtrl.dropStart(cellCTRLs[x, y]);

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
}
