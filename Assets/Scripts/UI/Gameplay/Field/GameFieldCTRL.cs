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
    public static GameFieldCTRL main;

    private TeleportController[] firstTeleports = new TeleportController[100]; //fix

    [SerializeField]
    bool NeedOpenComplite = false;
    [SerializeField]
    bool NeedOpenDefeat = false;
    public bool canPassTurn = true;
    private bool readyToPass = false;
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
    /// ���������� ���� �� �����
    /// </summary>
    [SerializeField]
    public int CountIce = 0;

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
    [SerializeField]
    GameObject prefabIce;
    [SerializeField]
    GameObject prefabMarker;                                //������
    [SerializeField]
    GameObject prefabWall;                                  //������
    [SerializeField]
    GameObject prefabTeleport;                              //��������
    [SerializeField]
    GameObject prefabDispencer;                             //���������
    [SerializeField]
    GameObject prefabContur;

    LevelsScript.Level currentLevel;

    [Header("Particles")]
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
    public Transform parentOfIce;
    [SerializeField]
    public Transform parentOfRock;
    [SerializeField]
    public Transform parentOfContur;
    [SerializeField]
    public Transform parentOfWall;                          //������
    [SerializeField]
    public Transform parentOfFly;
    [SerializeField]
    public Transform parentOfParticles;
    [SerializeField]
    public Transform parentOfScore;
    [SerializeField]
    public Transform parentOfSelect;
    [SerializeField]
    public Transform parentOfMarkers;                       //������� ���� (���������)
    [SerializeField]
    public Transform parentOfDispencers;                    //����������
    [SerializeField]
    public Transform parentOfTeleport;                    //����������

    [Header("Other")]
    public CellCTRL CellSelect; //������ ���������� ������������� ������
    public CellCTRL CellSwap; //������ ���������� ������������� ������

    public RectTransform PosSpawnInternals;

    [SerializeField]
    RectTransform rectParticleSelect;

    public float timeLastBoom = 0;
    public float timeLastMove = 0;
    public float timeLastMarker = 0;

    RectTransform myRect;

    /// <summary>
    /// ������ ������������ �������
    /// </summary>
    public List<MoldCTRL> moldCTRLs = new List<MoldCTRL>();

    private GameObject[,] markers;


    private void Start()
    {
        main = this;
        buffer.Ini(); //������������������� ������
    }

    void Update()
    {
        if (NeedOpenDefeat && Gameplay.main.movingCan > 0)
        {
            NeedOpenDefeat = false;
        }
        TestSpawn(); //�������

        UpdateAddPlayBonus();

        isMoving();

        TestFieldCombination(true); //������ ���������� � ����������� �����

        TestFieldPotencial(); //���� ������������� ����
        TestRandomNotPotencial(); //������������� ���� ����� �� ����������

        ActivateMarkers(); //�������� ������� ����

        TestStartSwap(); //�������� �����
        TestReturnSwap(); //���������� �����

        TestMold(); //��������� �������� ����� ����

        TestCalcPriorityCells(); //���������� ���������� �����

        TurnPass();

        //�������� �� ���������� ����
        TestEndMessage();

        //������ �������� ����������.. � �������� ����� ��� ���������� �� �����, �� ��� ������ ����..
        buffer.Calculate();

        PanelSpreadCTRL.TestOffSet();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// ���������������� ������� ����, �� ������ ������ ������, ��� ��������, ���� ������ ���
    /// </summary>
    public void inicializeField(LevelsScript.Level level)
    {
        currentLevel = level;
        timeLastMove = Time.unscaledTime;
        timeLastMarker = Time.unscaledTime;

        potencialBest = null; //����� ������������� ������

        enemyPotencialBest = null;

        //���������� ���� � �����
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f, 0.5f);

        //���� ������� ���� � ����
        if (level != null)
        {
            AddAllCellLevel();
        }
        else
        {
            AddAllCellsRandom();
        }

        ScaleField();

        //��������� ��� ���� �������� ��������
        void AddAllCellsRandom()
        {
            //������� ������������ �������� ����
            cellCTRLs = new CellCTRL[10, 10];
            cellsPriority = new CellCTRL[cellCTRLs.GetLength(0) * cellCTRLs.GetLength(1)];

            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++)
                {
                    if (!cellCTRLs[x, y])
                    {
                        GameObject cellObj = Instantiate(prefabCell, parentOfCells);


                        //���� ���������
                        cellCTRLs[x, y] = cellObj.GetComponent<CellCTRL>();
                        cellsPriority[x * cellCTRLs.GetLength(1) + y] = cellCTRLs[x, y];
                        //���� ��������� �� ������� ������� ���� �����
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

                        //���������� ����������
                        cellCTRLs[x, y].CalcMyPriority();

                        //��������� ������
                        GameObject cellContur = Instantiate(prefabContur, parentOfContur);
                        RectTransform rectContur = cellContur.GetComponent<RectTransform>();
                        rectContur.pivot = new Vector2(-x, -y);

                    }
                }
            }
        }

        //��������� ��� ���� �������� �� ������� ������
        void AddAllCellLevel()
        {
            CountDestroyCrystals = new int[level.NumColors];

            //������� ������������ �������� ����
            cellCTRLs = new CellCTRL[level.Width, level.Height];
            cellConturs = new GameObject[level.Width, level.Height];

            BoxBlockCTRLs = new BoxBlockCTRL[level.Width, level.Height];
            rockCTRLs = new RockCTRL[level.Width, level.Height];
            iceCTRLs = new IceCTRL[cellCTRLs.GetLength(0), cellCTRLs.GetLength(1)];
            cellsPriority = new CellCTRL[cellCTRLs.GetLength(0) * cellCTRLs.GetLength(1)];
            markers = new GameObject[level.Width, level.Height];

            Gameplay.main.colors = level.NumColors;
            Gameplay.main.superColorPercent = level.SuperColorPercent;
            Gameplay.main.typeBlockerPercent = level.TypeBlockerPercent;

            //���������� ������� ������ ��������
            PosSpawnInternals.pivot = new Vector2(0.5f, -level.Height);

            //���������� ������
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++)
                {

                    //���� ���� ������ �� ���������� � ��� ��������� � ��� ����������
                    if (level.cells[x, y].Exist == 0 && //���� ������ ���
                        level.cells[x, y].teleport == 0 && //��� ���������
                        !level.cells[x, y].dispencer) //��� ����������
                    {
                        //�� ���������� �������� ���� ������
                        continue;
                    }


                    //��� ��� ��������� �� ����� ���� ������, �� �����

                    //���� ���� ������
                    //���� ��� ����������
                    else if (level.cells[x,y].Exist != 0 &&
                        !level.cells[x,y].dispencer)
                    {
                        //��������� ������������� ������ ���� ��� ����������
                        CountInteractiveCells++;
                    }


                    //���� ������ ���, ��� ������ ��� ��� ���������, �������
                    if (!cellCTRLs[x, y] && level.cells[x,y].Exist == 1 || level.cells[x,y].dispencer)
                    {
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
                        cellCTRLs[x, y].IniFon();

                        //��������� ������
                        cellConturs[x,y] = Instantiate(prefabContur, parentOfContur);
                        RectTransform rectContur = cellConturs[x, y].GetComponent<RectTransform>();
                        rectContur.pivot = new Vector2(-x, -y);


                        cellCTRLs[x, y].Box = level.cells[x, y].HealthBox;
                        cellCTRLs[x, y].rock = level.cells[x, y].rock;
                        cellCTRLs[x, y].mold = level.cells[x, y].HealthMold;
                        cellCTRLs[x, y].wallID = level.cells[x, y].wall;
                        cellCTRLs[x, y].teleport = level.cells[x, y].teleport;
                        if (cellCTRLs[x, y].mold <= 0 && cellCTRLs[x, y].Box > 0)
                        {
                            cellCTRLs[x, y].mold = 1;
                        }
                        cellCTRLs[x, y].ice = level.cells[x, y].HealthIce;

                        if (level.cells[x, y].Panel > 0)
                            cellCTRLs[x, y].panel = true;
                    }

                    //���� ����� ������� ���������
                    if (level.cells[x, y].dispencer)
                    {
                        GameObject dispencerObj = Instantiate(prefabDispencer, parentOfDispencers);
                        dispencerObj.GetComponent<RectTransform>().pivot = new Vector2(-x, -y);
                        //������ � �����������
                        dispencerObj.GetComponent<DispencerController>().MyPosition = new Vector2Int(x,y);
                        //������, � ������� ��������� ������ �������
                        if (y - 1 >= 0)
                        {
                            dispencerObj.GetComponent<DispencerController>().targetCell = cellCTRLs[x, y - 1];
                        }
                        //���� ��������� (�����������) �������, ����� �� ������� ������
                        dispencerObj.GetComponent<DispencerController>().primaryObjectColor = level.cells[x, y].colorCell;
                        //��� ��������� (�����������) �������, ����� �� ������� �����
                        dispencerObj.GetComponent<DispencerController>().primaryObjectType = level.cells[x, y].typeCell;
                    }


                    //���� ����� ������� ��������
                    if (level.cells[x, y].teleport > 0)
                    {
                        int teleportID = level.cells[x, y].teleport;
                        GameObject teleportObj = Instantiate(prefabTeleport, parentOfTeleport);                        
                        teleportObj.GetComponent<RectTransform>().pivot = new Vector2(-x, -y);

                        //����� ����� � ��������
                        teleportObj.GetComponent<TeleportController>().cellIn = cellCTRLs[x, y];
                        
                        //����� ������ �� ���������
                        if (y > 0)
                        {
                            teleportObj.GetComponent<TeleportController>().cellOut = cellCTRLs[x, y - 1];
                        }

                        //���� ������ �������� � �������� ID ����������� � �������, ��������� �������
                        if (firstTeleports[teleportID] == null)
                        {
                            firstTeleports[teleportID] = teleportObj.GetComponent<TeleportController>();
                        }
                        //����� ������� ���� ����������
                        else
                        {
                            //firstTeleports[teleportID].secondTeleport = teleportObj.GetComponent<TeleportController>();
                            //teleportObj.GetComponent<TeleportController>().secondTeleport = firstTeleports[teleportID];
                            //firstTeleports[teleportID].setIDAndColor(teleportID);


                            firstTeleports[teleportID].secondTeleport = teleportObj.GetComponent<TeleportController>();
                            firstTeleports[teleportID].secondTeleport.secondTeleport = firstTeleports[teleportID];

                            //���� ���������� ��������, ������ ����
                            firstTeleports[teleportID].setIDAndColor(teleportID);
                            firstTeleports[teleportID].secondTeleport.setIDAndColor(teleportID);

                            firstTeleports[teleportID].myField = this;
                            firstTeleports[teleportID].secondTeleport.myField = this;
                        }


                        //��������� ����� �� 
                    }

                    ////////////////////////////////////////////////////////////////////////////
                    //����� ��������� ����������� ����� ��� ������� ������� ������ - ����������
                    //�� ����� ���� ������ ��� �� ������������� �� ��������� ������
                    if (!cellCTRLs[x, y])
                        continue;
                    ////////////////////////////////////////////////////////////////////////////

                    //����� �� ������� ����
                    if (cellCTRLs[x, y].Box > 0)
                    {
                        GameObject BoxBlockObj = Instantiate(prefabBoxBlock, parentOfBoxBlock);
                        BoxBlockCTRL boxBlockCTRL = BoxBlockObj.GetComponent<BoxBlockCTRL>();
                        cellCTRLs[x, y].BoxBlockCTRL = boxBlockCTRL;

                        //�������������� �������
                        boxBlockCTRL.Inicialize(cellCTRLs[x, y]);
                    }

                    //����� �� ������� ������� //���� ���� ����
                    if (!level.PassedWithPanel && (cellCTRLs[x, y].mold > 0 || cellCTRLs[x, y].Box > 0))
                    {
                        GameObject MoldObj = Instantiate(prefabMold, parentOfMold);
                        MoldCTRL moldCTRL = MoldObj.GetComponent<MoldCTRL>();
                        cellCTRLs[x, y].moldCTRL = moldCTRL;

                        //������������� �������
                        moldCTRL.inicialize(cellCTRLs[x, y]);
                    }
                    else
                    {
                        cellCTRLs[x, y].mold = 0;
                    }

                    //����� �� ������� ���
                    if (cellCTRLs[x, y].ice > 0)
                    {
                        GameObject iceObj = Instantiate(prefabIce, parentOfIce);
                        IceCTRL iceCTRL = iceObj.GetComponent<IceCTRL>();
                        cellCTRLs[x, y].iceCTRL = iceCTRL;

                        //������������� �������
                        iceCTRL.inicialize(cellCTRLs[x, y]);
                    }

                    //����� �� ������� ������
                    if (cellCTRLs[x, y].panel)
                    {
                        GameObject panelObj = Instantiate(prefabPanel, parentOfPanels);
                        cellCTRLs[x, y].panelCTRL = panelObj.GetComponent<PanelSpreadCTRL>();

                        //������������� �������
                        cellCTRLs[x, y].panelCTRL.inicialize(cellCTRLs[x, y]);
                    }
                    //����� �� ������� ������ � ��� �����
                    if (cellCTRLs[x, y].rock > 0 && cellCTRLs[x, y].Box <= 0)
                    {
                        GameObject rockObj = Instantiate(prefabRock, parentOfRock);
                        cellCTRLs[x, y].rockCTRL = rockObj.GetComponent<RockCTRL>();

                        //������������� �����
                        cellCTRLs[x, y].rockCTRL.inicialize(cellCTRLs[x, y]);
                    }
                    else
                    {
                        cellCTRLs[x, y].rock = 0;
                    }

                    //����� �� ������� �����
                    if (cellCTRLs[x, y].wallID > 0)
                    {
                        GameObject wallObj = Instantiate(prefabWall, parentOfWall);
                        WallController wallController = wallObj.GetComponent<WallController>();
                        //wallObj.GetComponent<RectTransform>().pivot = new Vector2(-cellCTRLs[x, y].pos.x, -cellCTRLs[x, y].pos.y);
                        wallController.IniWall(cellCTRLs[x, y]);
                    }

                    //������� ��������� �������
                    if (level.cells[x, y].typeCell != CellInternalObject.Type.none &&
                        cellCTRLs[x, y].Box == 0 && //���� ���� �����
                        !level.cells[x, y].dispencer
                        )
                    {
                        if (level.cells[x, y].typeCell == CellInternalObject.Type.color)
                        {
                            //������� ������ � ����������
                            GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                            CellInternalObject internalCtrl = internalObj.GetComponent<CellInternalObject>();
                            internalCtrl.myField = this;
                            internalCtrl.StartMove(cellCTRLs[x, y]);
                            internalCtrl.EndMove();

                            //������ ��� �������
                            internalCtrl.setColorAndType(level.cells[x, y].colorCell, level.cells[x, y].typeCell);

                            internalCtrl.color = level.cells[x, y].colorCell;

                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.airplane)
                        {
                            CreateFly(cellCTRLs[x, y], level.cells[x, y].colorCell, 0);
                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.bomb)
                        {
                            CreateBomb(cellCTRLs[x, y], level.cells[x, y].colorCell, 0);
                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.rocketHorizontal)
                        {
                            CreateRocketHorizontal(cellCTRLs[x, y], level.cells[x, y].colorCell, 0);
                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.rocketVertical)
                        {
                            CreateRocketVertical(cellCTRLs[x, y], level.cells[x, y].colorCell, 0);
                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.color5)
                        {
                            CreateSuperColor(cellCTRLs[x, y], level.cells[x, y].colorCell, 0);
                        }
                        else if (level.cells[x, y].typeCell == CellInternalObject.Type.blocker)
                        {
                            CreateBlocker(cellCTRLs[x, y], level.cells[x, y].colorCell, 0);
                        }
                    }

                    //���������� ����������
                    cellCTRLs[x, y].CalcMyPriority();

                }
            }


        }

        //��������� ������ ���� � ����������� �� ���������� �����
        void ScaleField()
        {
            //100% ������ ��� ���� � 10-� ��������

            if (!myRect) myRect = GetComponent<RectTransform>();

            //��������� ������ ������� ������ ���� ����
            //������� ���� ������������ �� 9 ������
            float cellsWeight = 9;//(float)cellCTRLs.GetLength(0);

            float sizeNeed = 10 / cellsWeight;

            //������������� ������ ����
            myRect.localScale = new Vector3(sizeNeed, sizeNeed, 1);
            myRect.sizeDelta = new Vector2(100 * cellCTRLs.GetLength(0), 100 * cellCTRLs.GetLength(1));
        }
    }

    /// <summary>
    /// ����������� ������� ���������� �������, ��������� ��������� �� ������
    /// </summary>
    public void ReCalcMoldList()
    {
        List<MoldCTRL> moldCTRLsNew = new List<MoldCTRL>();

        foreach (MoldCTRL moldCTRL in moldCTRLs)
        {
            if (moldCTRL)
            {
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
    public int ComboInternal = 0;

    //�������� ������
    class Swap
    {
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
    public GameObject[,] cellConturs;
    /// <summary>
    /// ����������� ��������
    /// </summary>
    public BoxBlockCTRL[,] BoxBlockCTRLs;

    /// <summary>
    /// ����������� �����
    /// </summary>
    public RockCTRL[,] rockCTRLs;

    /// <summary>
    /// ����������� ����
    /// </summary>
    public IceCTRL[,] iceCTRLs;


    //�������� ����� �� ������� � �������������
    void TestSpawn()
    {
        //��� �� �����
        int[] countSpawned = new int[cellCTRLs.GetLength(0)];

        //������� ����� ��������� ���� �� ������ ������
        for (int y = 0; y < cellCTRLs.GetLength(1); y++)
        {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {

                //���� ��� ������ ����, ������ � ��� ���������� �������� � �� ��� ������ ��� ��������
                if (cellCTRLs[x, y] && !cellCTRLs[x, y].cellInternal &&
                    cellCTRLs[x, y].Box == 0 &&
                    cellCTRLs[x, y].rock == 0 &&
                    !cellCTRLs[x, y].dispencer)
                {
                    //��������� ������ �� �� ���� �� ��� ���-�� ��� ����� ������
                    for (int plusY = 0; plusY <= cellCTRLs.GetLength(1); plusY++)
                    {

                        //������ ����� ������ ����
                        CellCTRL upCell = null;
                        //���� ���� ������������ ���� ������ 
                        if (y + plusY + 1 < cellCTRLs.GetLength(1)) {
                            upCell = cellCTRLs[x, y + plusY + 1];
                        }

                        //���� �������� ������ ����� ����
                        if (y + plusY >= cellCTRLs.GetLength(1) &&
                            Time.unscaledTime - timeLastBoom > 0.1f)
                        {
                            //������� ������ ������������� �������
                            GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
                            //������ �� ������� 
                            RectTransform rect = internalObj.GetComponent<RectTransform>();

                            //������� ���������� ����� �� �����
                            rect.pivot = new Vector2(-x, -y - (countSpawned[x] + cellCTRLs.GetLength(1) - y));

                            CellInternalObject internalCtrl = internalObj.GetComponent<CellInternalObject>();

                            //���������� ����
                            internalCtrl.randSpawnType(true);

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
                        else if (y + plusY < cellCTRLs.GetLength(1) &&
                            (//!CheckObstaclesToMoveUp(cellCTRLs[x, y + plusY], null) || //�� ����� ������� �� ����� ��������
                            !CheckObstaclesToMoveDown(cellCTRLs[x, y + plusY], upCell)))
                        {
                            break;
                        }

                        //�� ����������� ���� �������� ��� ������������ ������

                        //���� ������ ���� ������ � ������������� � ��� �� �����������
                        else if (y + plusY < cellCTRLs.GetLength(1) &&
                            //CheckObstaclesToMoveUp(cellCTRLs[x, y + plusY], cellCTRLs[x, y]) &&
                            //CheckObstaclesToMoveDown(cellCTRLs[x, y + plusY], cellCTRLs[x, y]) &&
                            cellCTRLs[x, y + plusY].cellInternal)
                        {

                            break;
                        }

                        //�� ������������� �� �������� ���� ��� ���� ������
                        bool canMoveToTargetFromUP(CellCTRL target, CellCTRL from) {
                            bool result = false;

                            //����� ������������� ������ ����
                            if (target != null && //���� ���� ����
                                target.rock == 0 &&
                                target.Box == 0 &&
                                !target.dispencer &&
                                target.wallID != 1 &&
                                target.wallID != 5 &&
                                target.wallID != 8 &&
                                target.wallID != 9 &&
                                target.wallID != 12 &&
                                target.wallID != 13 &&
                                target.wallID != 14 &&
                                target.wallID != 15 &&
                                ((from != null &&
                                from.teleport == 0 &&
                                from.wallID != 3 &&
                                from.wallID != 6 &&
                                from.wallID != 7 &&
                                from.wallID != 9 &&
                                from.wallID != 11 &&
                                from.wallID != 12 &&
                                from.wallID != 14 &&
                                from.wallID != 15) ||
                                (from == null)))
                            {
                                result = true;
                            }

                            return result;
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
    public class Combination
    {
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
        public bool foundMould = false;

        public Combination()
        {
            IDLast++; //���������� id ����������
            ID = IDLast; //��� ��� �����
        }

        public Combination(Combination ParentComb)
        {

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
                    foundPanel = ParentComb.foundPanel;
                if (ParentComb.foundMould)
                    foundMould = ParentComb.foundMould;
            }
            else
            {

                IDLast++; //���������� id ����������
                ID = IDLast; //��� ��� �����

            }

        }
        public void TestCells()
        {
            foreach (CellCTRL cell in cells)
            {
                if (cell.panel)
                    foundPanel = true;

                if (cell.mold > 0)
                    foundMould = true;
            }
        }
    }
    void TestFieldCombination(bool damageOnCombinations)
    {
        //������� ����� ������ ����������
        listCombinations = new List<Combination>();

        //������� ������ ��������� ��� ������ �� ����������
        for (int y = cellCTRLs.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
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

        ///////////////////////////////////////////////////////////////
        //��������� ������ �� ����������. ������� 2 2021.08.18
        void TestCellCombination(CellCTRL Cell)
        {

            //������� ���� ������ ���, ��� ��� ������������, ��� ������������ � ��������, ��� ��� �������, ������� �� ������ ���������� � ���������� ��� ��� ���������� ����������
            if (!Cell || !Cell.cellInternal || (Cell.cellInternal.isMove && damageOnCombinations) || Cell.cellInternal.type == CellInternalObject.Type.color5 || Cell.cellInternal.type == CellInternalObject.Type.blocker)
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
            void GetCombination()
            {
                bool foundCombination = false;

                //���������� ����������
                foreach (Combination comb in listCombinations)
                {
                    if (foundCombination) break; //���� ���������� ���� �������� �������

                    //���������� ������ �� ������ ����������
                    foreach (CellCTRL cellComb in comb.cells)
                    {
                        if (foundCombination) break; //���� ���������� ���� �������� �������

                        //���� ������� ���� �������� � ����������
                        if (cellComb == Cell)
                        {
                            //�� �� ����� ����������
                            foundCombination = true;
                            Combination = comb;
                        }
                    }
                }
            }

            void SetCombination()
            {
                if (Combination.cells.Count <= 0)
                {
                    return;
                }

                //���� ���� �� � ������ ��� ����������
                bool found = false;
                foreach (Combination comb in listCombinations)
                {
                    if (comb == Combination)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
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

            void AddCellToCombination(CellCTRL cellAdd)
            {
                //��������� ���� �� ��� ������ � ������
                bool found = false;
                foreach (CellCTRL cellComb in Combination.cells)
                {
                    //���� ������ �������
                    if (cellComb == cellAdd)
                    {
                        found = true;
                        break;
                    }
                }

                //���� ������ � ������ �� ���� ����������, ��������� � ������
                if (!found)
                {
                    Combination.cells.Add(cellAdd);
                }
            }


            //��������� ������ �� ���������� ����� � ����������� ���������� � ����������
            bool TestCellColor(CellCTRL SecondCell)
            {
                //������ ����
                if (
                    !SecondCell || //���� ����� ������ ���
                    !SecondCell.cellInternal || //� ������ ��� ������������
                    !Cell.cellInternal || //���� ������� � ������ ���
                    (SecondCell.cellInternal.isMove && damageOnCombinations) || //���� ��� ������������ ��������� � ��������
                    SecondCell.cellInternal.color != Cell.cellInternal.color || //���� �� ������
                    SecondCell.cellInternal.type == CellInternalObject.Type.color5 || //��� ������ ����� �������, �� �� ������ ��� �������������� �� ����� ��� � ���������� ���
                    SecondCell.cellInternal.type == CellInternalObject.Type.blocker ||
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
        void TestSuperCombination()
        {
            foreach (Swap swap in BufferSwap)
            {
                //���������� ���� ��� � ��������
                if (swap.first.cellInternal == null || swap.second.cellInternal == null ||
                    swap.first.cellInternal.isMove || swap.second.cellInternal.isMove)
                {
                    continue;
                }

                //��������
                //���� � ���������� ���� ����������
                if (swap.first.cellInternal.type == CellInternalObject.Type.blocker ||
                    swap.second.cellInternal.type == CellInternalObject.Type.blocker)
                {
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
                if (isFoundSuperComb)
                {
                    Gameplay.main.MinusMoving(comb);
                }

            }
        }

        //������� ���� ����������� � ���������� �������
        void TestDamageAndSpawn()
        {
            //���������� ����������
            foreach (Combination comb in listCombinations)
            {
                //���� ���� ������ � ����������
                if (comb.cells.Count > 0)
                {
                    //��������� �������� ��� �����������
                    CalcCombination(comb);
                }
            }

            //��������� ����������
            void CalcCombination(Combination comb)
            {


                //���� �������� ���������� ������ ��� ������ � ���������� ����
                CellCTRL CellSpawn = comb.cells[0];
                float timelastMove = 0;
                CellInternalObject.InternalColor color = CellInternalObject.InternalColor.Red;

                //���������� ��� ������ ����� �������� ����� ���������� �� ����������
                foreach (CellCTRL cell in comb.cells)
                {

                    //�������� ������� ������ ��� ����� ��� ������, �������� ����� ������ ������ ��� ��� ������ �������� 100%
                    //���� ��������� ��������� ������ �� ���� ����
                    //���
                    //���� ����� ����������� ������ ������ ��� ��������� � ��� ������ ����
                    if ((CellSpawn.cellInternal.type != CellInternalObject.Type.color && cell.cellInternal.type == CellInternalObject.Type.color) ||
                        (CellSpawn.myInternalNum < cell.myInternalNum && cell.cellInternal.type == CellInternalObject.Type.color))
                    {

                        CellSpawn = cell;

                        if (cell.cellInternal)
                        {
                            color = cell.cellInternal.color;

                            //���� ��� ����� ���������� ���� ����
                            if (timelastMove < cell.cellInternal.timeLastMoving)
                            {
                                timelastMove = cell.cellInternal.timeLastMoving;
                            }
                        }

                    }

                    //���� ����� ������
                    if (cell.panel)
                        comb.foundPanel = true;
                    if (cell.mold > 0)
                        comb.foundMould = true;
                }


                if (CellSpawn.cellInternal)
                {
                    color = CellSpawn.cellInternal.color;
                }


                //���� ���� �������������
                if (comb.cells[0].cellInternal.color == CellInternalObject.InternalColor.Ultimate)
                {
                    ActivateCombUltimate();
                }
                //����� ������� ���������
                else
                {
                    ActivateCombNormal();
                }

                void ActivateCombNormal()
                {
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
                            c.cellInternal.randSpawnType(false, false);
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

                void ActivateCombUltimate()
                {
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
                            c.cellInternal.randSpawnType(false);
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
                        Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateBoomBomb(gameObject.transform, cellCross);
                        particle3DCTRL.SetSpeed(radius);
                        particle3DCTRL.SetSize(radius);
                        Color color1 = cellCross.cellInternal.GetColor(color);
                        Vector3 colorVec = new Vector3(color1.r, color1.g, color1.b);
                        colorVec.Normalize();
                        color1 = new Color(color1.r, color1.g, color1.b);
                        particle3DCTRL.SetColor(color1);

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
                    else if (comb.vertical && comb.line4)
                    {
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
        void DestroyDuplicateCombinations()
        {

            //���� ������ ��� ����� �������
            if (listCombinations.Count == 0) return;


            //������ ���������� �� �������� �� ������ ��� ��� ������������
            List<Combination> listCombinationsDelete = new List<Combination>();

            //���������� ������� ���������� � ������� ����������
            foreach (Combination combinationFirst in listCombinations)
            {

                foreach (Combination combinationSecond in listCombinations)
                {
                    //������������� ���� ��� ���� ����� ����������
                    if (combinationFirst == combinationSecond) continue;


                    bool foundIdenticalCells = false;
                    //���������� ������ ����� � ��� � ������� ����������
                    foreach (CellCTRL cell in combinationFirst.cells)
                    {
                        if (foundIdenticalCells) break;
                        foreach (CellCTRL cellNew in combinationSecond.cells)
                        {
                            //���� ������ �� �������� ������ ��������� � ������� �� �������
                            if (cell == cellNew)
                            {
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
                        else if (combinationFirst.cells.Count > combinationSecond.cells.Count)
                        {
                            listCombinationsDelete.Add(combinationSecond);
                        }
                    }
                }
            }

            //������� ����� ������ ����� � ��������� ������ ������������ ��� ����������
            List<Combination> combinationsNew = new List<Combination>();
            foreach (Combination combTest in listCombinations)
            {
                bool needAdd = true;
                foreach (Combination combIgnore in listCombinationsDelete)
                {
                    //���� ��� ���������� ������� ����� ��������
                    if (combTest == combIgnore)
                    {
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
        void TestHitShop()
        {
            //���� ������ ��� ��������� ����� ����
            if (buffer.superHit != MenuGameplay.main.SuperHitSelected)
            {
                //������� ��������� � ������
                CellSelect = null;

                buffer.superHit = MenuGameplay.main.SuperHitSelected;
                Gameplay.main.movingCount++;
            }

            //���� ������ ������� ����� ����
            if (buffer.superHit != MenuGameplay.SuperHitType.none)
            {
                //���� �� ��������, �������
                if (CellSelect == null)
                {
                    return;
                }

                if (buffer.superHit == MenuGameplay.SuperHitType.internalObj)
                    TestShopInternal();
                else if (buffer.superHit == MenuGameplay.SuperHitType.rosket2x)
                    TestShopRocket();
                else if (buffer.superHit == MenuGameplay.SuperHitType.bomb)
                    TestShopBomb();
                else if (buffer.superHit == MenuGameplay.SuperHitType.Color5)
                    TestShopSuperColor();
                else if (buffer.superHit == MenuGameplay.SuperHitType.mixed)
                    TestShopMixed();

                //������������, ���������� ������
                MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;

            }

            void TestShopInternal()
            {
                if (PlayerProfile.main.ShopInternal.Amount > 0)
                {
                    PlayerProfile.main.ShopInternal.Amount--;

                    //������� ����������
                    Combination comb = new Combination();
                    comb.cells.Add(CellSelect);

                    //���� ������ ����
                    if (CellSelect.panel)
                        comb.foundPanel = true;
                    if (CellSelect.mold > 0)
                        comb.foundMould = true;

                    CellSelect.BufferCombination = comb;
                    CellSelect.DamageInvoke(0.25f);
                    MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;
                }
            }
            void TestShopRocket()
            {
                if (PlayerProfile.main.ShopRocket.Amount > 0)
                {
                    PlayerProfile.main.ShopRocket.Amount--;

                    //������� ����������
                    Combination comb = new Combination();
                    comb.cells.Add(CellSelect);
                    comb.TestCells();

                    //���� ���� ���������� ������ � ��� �����5
                    if (CellSelect.cellInternal && CellSelect.cellInternal.type == CellInternalObject.Type.color5)
                    {

                        CellSelect.cellInternal.Activate(CellInternalObject.Type.rocketHorizontal, CellSelect.cellInternal, comb);
                    }
                    else
                    {

                        CellSelect.BufferCombination = comb;

                        CellSelect.explosion = new CellCTRL.Explosion(true, true, true, true, 0.05f, comb);
                        CellSelect.ExplosionBoomInvoke(CellSelect.explosion, CellSelect.explosion.timer);
                    }

                    MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;
                }

            }
            void TestShopBomb()
            {
                if (PlayerProfile.main.ShopBomb.Amount <= 0 || CellSelect.Box > 0)
                {
                    return;
                }

                PlayerProfile.main.ShopBomb.Amount--;

                if (CellSelect.cellInternal != null)
                {
                    if (CellSelect.cellInternal.type == CellInternalObject.Type.airplane ||
                        CellSelect.cellInternal.type == CellInternalObject.Type.bomb ||
                        CellSelect.cellInternal.type == CellInternalObject.Type.color5 ||
                        CellSelect.cellInternal.type == CellInternalObject.Type.rocketHorizontal ||
                        CellSelect.cellInternal.type == CellInternalObject.Type.rocketVertical)
                    {
                        CellSelect.cellInternal.Activate(CellInternalObject.Type.bomb, CellSelect.cellInternal, null);
                    }
                    else
                    {
                        CellSelect.cellInternal.setColorAndType(CellSelect.cellInternal.color, CellInternalObject.Type.bomb);
                    }
                }
                else
                {
                    CreateBomb(CellSelect, CellInternalObject.InternalColor.Red, 0);
                }

                CellSelect.Damage();
            }

            void TestShopSuperColor()
            {
                if (PlayerProfile.main.ShopColor5.Amount <= 0 || CellSelect.cellInternal == null)
                {
                    return;
                }

                PlayerProfile.main.ShopColor5.Amount--;

                //������� ����������
                Combination comb = new Combination();
                comb.cells.Add(CellSelect);
                comb.TestCells();


                CellSelect.cellInternal.Activate(CellInternalObject.Type.color5, CellSelect.cellInternal, comb);

                MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;
            }

            void TestShopMixed()
            {
                if (PlayerProfile.main.ShopMixed.Amount <= 0 || CellSelect.cellInternal == null)
                {
                    return;
                }

                PlayerProfile.main.ShopMixed.Amount--;

                //����� ���������..
                BuyMixedNeed = true;

                MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;
            }
        }

    }


    private List<PotencialComb> listPotencial = new List<PotencialComb>();
    public static PotencialComb potencialBest = null;
    //������ ���������� ��� ������ (����� ������)
    public static PotencialComb enemyPotencialBest = null;
    public class PotencialComb
    {
        public static PotencialComb main;
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

        public void CalcPriority()
        {
            //��������� ��������� �����
            foreach (CellCTRL cell in cells)
            {
                PlusPriority(cell);
            }
            PlusPriority(Target);

            if (foundPanel)
            {
                priority += 90;
            }
            else if (foundPanel && foundNotPanel)
            {
                priority += 110;
            }

            if (foundMold)
            {
                priority += 80;
            }
            if (foundRock)
            {
                priority += 100;
            }

            if (cells.Count == 4)
                priority += 100;
            if (cells.Count == 3)
                priority += 90;

            void PlusPriority(CellCTRL cell)
            {
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

    public bool BuyMixedNeed = false;

    [SerializeField]
    public Vector2Int testCell = new Vector2Int();
    /// <summary>
    /// ����� ������������� ����������
    /// </summary>
    /// 
    void TestFieldPotencial()
    {
        //���� ���� ������ ��������
        if ((Gameplay.main.movingCount <= 0 && Time.unscaledTime - timeLastMove < 3) ||
            isMovingInternalObj ||
            Time.unscaledTime - timeLastBoom < 0.2f
            )
        {
            potencialBest = null;
            return;
        }

        //������������� �������� �� ������� � �����������
        if (Gameplay.main.isMissionComplite() || Gameplay.main.isMissionDefeat())
        {
            potencialBest = null;
            enemyPotencialBest = null;
        }
        AnimationPlayPotencial();

        /*
        float timeToTest = 0.5f;
        //���� ������� ���� �������� �� �������� ����
        if (Time.unscaledTime - timeLastMove < timeToTest)
        {
            listPotencial = new List<PotencialComb>();
            potencialBest = null;
            enemyPotencialBest = null;

            //Debug.Log("Waiting Time " + Time.unscaledTime);

            return;
        }
        */
        //������� ���� ������ ��� ���� ��� ������ ��������� ��� ���������
        if ((listPotencial.Count > 0 && !BuyMixedNeed) || Gameplay.main.isMissionComplite() || Gameplay.main.isMissionDefeat())
        {
            return;
        }

        //�������� ����� ������������� ����������
        ReCalcListPotencial();

        //���� ���������� �������
        if (listPotencial.Count > 0 && !BuyMixedNeed)
        {
            //�������� ������
            if (potencialBest == null)
            {
                potencialBest = listPotencial[0];

                foreach (PotencialComb potencial in listPotencial)
                {
                    if (potencialBest.priority < potencial.priority)
                    {
                        potencialBest = potencial;

                    }
                }
            }

            //������� ����� ������ ���������� ��� ������ (����� ������)
            if (enemyPotencialBest == null)
            {
                enemyPotencialBest = listPotencial[0];

                foreach (PotencialComb potencial in listPotencial)
                {
                    //������ ������� ������ ����������
                    //if (enemyPotencialBest.priority > potencial.priority)
                    if (enemyPotencialBest.priority < potencial.priority)
                        {
                        enemyPotencialBest = potencial;

                    }
                }
            }
        }
        //���� ���������� �� ������� � ���� ������ � ����������� �������� ��� �� ������ � ��� �� ������ ���
        else if (ListWaitingInternals.Count <= 0)
        {

            CreateRandomInternalList();

            //���� ������ ������������ �������� ��� ������ //������� �������� ��� ������������
            if (ListWaitingInternals.Count > 0) MenuGameplay.main.CreateMidleMessage(MidleTextGameplay.strMixed);

            //������������� ���������
            BuyMixedNeed = false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        //������������� �������� ���������� ���� ����
        void AnimationPlayPotencial()
        {
            //������������� �������� �������������� ���� ����� 10 ������
            if (potencialBest == null || Time.unscaledTime - timeLastMove < 10)
                return;

            //���� ��������� ����
            //���������� ������ � ������������� ��������
            foreach (CellCTRL potencialCell in potencialBest.cells)
            {
                potencialCell.cellInternal.animatorCTRL.PlayAnimation("ApperanceCombinationObject");
            }
            potencialBest.Moving.cellInternal.animatorCTRL.PlayAnimation("ApperanceCombinationObject");
        }

        void CreateRandomInternalList()
        {
            //���������� ��� ������
            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++)
                {
                    if (isCellCanRandomizade(cellCTRLs[x, y]))
                    {
                        ListWaitingInternals.Add(cellCTRLs[x, y].cellInternal);
                    }
                }
            }

            bool isCellCanRandomizade(CellCTRL select)
            {
                bool result = false;

                if (select != null && select.cellInternal &&
                    select.rock == 0 &&
                    select.cellInternal.type != CellInternalObject.Type.blocker)
                {
                    result = true;
                }

                return result;
            }
        }
    }

    //�������� �� ������������� ������� ������ ���� �����
    private void ActivateMarkers()
    {
        if (Time.unscaledTime - timeLastMove >= 10 && Time.unscaledTime - timeLastMarker >= 10)
        {

            for (int x = 0; x < cellCTRLs.GetLength(0); x++)
            {
                for (int y = 0; y < cellCTRLs.GetLength(1); y++)
                {
                    if (currentLevel.PassedWithBox && CountBoxBlocker < 10 && cellCTRLs[x, y] != null && cellCTRLs[x, y].BoxBlockCTRL != null)
                    {
                        TestCreateMarker();
                    }
                    if (currentLevel.PassedWithIce && CountIce < 10 && cellCTRLs[x, y] != null && cellCTRLs[x, y].iceCTRL != null)
                    {
                        TestCreateMarker();
                    }
                    if (currentLevel.PassedWithMold && CountMold < 10 && cellCTRLs[x, y] != null && cellCTRLs[x, y].moldCTRL != null)
                    {
                        TestCreateMarker();
                    }
                    if (currentLevel.PassedWithPanel && CountPanelSpread < 10 && cellCTRLs[x, y] != null && cellCTRLs[x, y].panelCTRL == null)
                    {
                        TestCreateMarker();
                    }
                    if (currentLevel.PassedWithRock && CountRockBlocker < 10 && cellCTRLs[x, y] != null && cellCTRLs[x, y].rockCTRL != null)
                    {
                        TestCreateMarker();
                    }

                    if (currentLevel.PassedWithCrystal && cellCTRLs[x, y] != null && cellCTRLs[x, y].cellInternal != null && cellCTRLs[x, y].cellInternal.color == currentLevel.NeedColor)
                    {
                        TestCreateMarker();
                    }
                    

                    void TestCreateMarker()
                    {
                        //���� ������ ��� ���� �� �������
                        if (markers[x, y] != null) {
                            return;
                        }
                        //������� ������
                        markers[x, y] = Instantiate(prefabMarker, parentOfMarkers);
                        //���������� ������
                        RectTransform markerRect = markers[x, y].GetComponent<RectTransform>();
                        markerRect.pivot = new Vector2(-x, -y);
                    }
                }
            }

            timeLastMarker = Time.unscaledTime;
        }
    }

    //����������� ����� ������ ������������� ����������
    void ReCalcListPotencial()
    {



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
                //������������ �� ����� �����������
                if (cellPos.x >= 0 && cellPos.x < cellCTRLs.GetLength(0) && cellPos.y >= 0 && cellPos.y < cellCTRLs.GetLength(1) &&
                    cellCTRLs[cellPos.x, cellPos.y] != null &&
                    cellCTRLs[cellPos.x, cellPos.y].cellInternal != null &&
                    cellCTRLs[cellPos.x, cellPos.y].cellInternal.type != CellInternalObject.Type.color5 &&
                    cellCTRLs[cellPos.x, cellPos.y].cellInternal.type != CellInternalObject.Type.blocker
                    )
                {

                    //������� ���� ��� ����� ������� ������ ��������������
                    if (cellCTRLs[cellPos.x, cellPos.y].cellInternal.type == CellInternalObject.Type.bomb &&
                        cellCTRLs[cellPos.x, cellPos.y].cellInternal.activateNeed)
                    {
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
                        CheckObstaclesToMoveLeft(cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y], cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y]) &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y] != potTest.cells[0] &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal != null &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal.color == potTest.cells[0].cellInternal.color &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal.type != CellInternalObject.Type.color5 &&
                        cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal.type != CellInternalObject.Type.blocker &&
                        !(cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y].cellInternal.activateNeed))
                        
                    {

                        potTest.Moving = cellCTRLs[potTest.Target.pos.x - 1, potTest.Target.pos.y];

                    }
                    //������
                    else if (potTest.Target.pos.x + 1 < cellCTRLs.GetLength(0) &&
                        CheckObstaclesToMoveRight(cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y], cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y]) &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y] != potTest.cells[0] &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal != null &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal.color == potTest.cells[0].cellInternal.color &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal.type != CellInternalObject.Type.color5 &&
                        cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal.type != CellInternalObject.Type.blocker &&
                        !(cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y].cellInternal.activateNeed))
                        
                    {

                        potTest.Moving = cellCTRLs[potTest.Target.pos.x + 1, potTest.Target.pos.y];

                    }
                    //������
                    else if (potTest.Target.pos.y + 1 < cellCTRLs.GetLength(1) &&
                        CheckObstaclesToMoveUp(cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1], cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y]) &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1] != potTest.cells[0] &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal != null &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal.color == potTest.cells[0].cellInternal.color &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal.type != CellInternalObject.Type.color5 &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal.type != CellInternalObject.Type.blocker &&
                        !(cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1].cellInternal.activateNeed))
                        
                    {

                        potTest.Moving = cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y + 1];

                    }
                    //�����
                    else if (potTest.Target.pos.y - 1 >= 0 &&
                        CheckObstaclesToMoveDown(cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1], cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y]) &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1] != potTest.cells[0] &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal != null &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal.color == potTest.cells[0].cellInternal.color &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal.type != CellInternalObject.Type.color5 &&
                        cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal.type != CellInternalObject.Type.blocker &&
                        !(cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[potTest.Target.pos.x, potTest.Target.pos.y - 1].cellInternal.activateNeed))
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
                if (cell.cellInternal.type == CellInternalObject.Type.color ||
                    cell.cellInternal.type == CellInternalObject.Type.blocker)
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
                    CheckObstaclesToMoveLeft(cellCTRLs[cell.pos.x - 1, cell.pos.y], cellCTRLs[cell.pos.x, cell.pos.y]) &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal != null &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.type != CellInternalObject.Type.color &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.type != CellInternalObject.Type.blocker &&
                    !(cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.activateNeed))
                    
                {

                    left = cellCTRLs[cell.pos.x - 1, cell.pos.y];

                }
                //������
                if (cell.pos.x + 1 < cellCTRLs.GetLength(0) &&
                    CheckObstaclesToMoveRight(cellCTRLs[cell.pos.x + 1, cell.pos.y], cellCTRLs[cell.pos.x, cell.pos.y]) &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal != null &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.type != CellInternalObject.Type.color &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.type != CellInternalObject.Type.blocker &&
                    !(cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.activateNeed))
                    
                {

                    right = cellCTRLs[cell.pos.x + 1, cell.pos.y];

                }
                //������
                if (cell.pos.y + 1 < cellCTRLs.GetLength(1) &&
                    CheckObstaclesToMoveUp(cellCTRLs[cell.pos.x, cell.pos.y + 1], cellCTRLs[cell.pos.x, cell.pos.y]) &&
                    cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal != null &&
                    cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.type != CellInternalObject.Type.color &&
                    cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.type != CellInternalObject.Type.blocker &&
                    !(cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.activateNeed))
                    
                {

                    up = cellCTRLs[cell.pos.x, cell.pos.y + 1];

                }
                //�����
                if (cell.pos.y - 1 >= 0 &&
                    CheckObstaclesToMoveDown(cellCTRLs[cell.pos.x, cell.pos.y - 1], cellCTRLs[cell.pos.x, cell.pos.y]) &&
                    cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal != null &&
                    cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.type != CellInternalObject.Type.color &&
                    cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.type != CellInternalObject.Type.blocker &&
                    !(cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.activateNeed))
                    
                {

                    down = cellCTRLs[cell.pos.x, cell.pos.y - 1];
                }


                //���� ������� ������ ����� ����
                if (cell.cellInternal.type == CellInternalObject.Type.color5)
                {
                    //��������� ������� �� �� ��� ��� ������� ����
                    //�����
                    if (cell.pos.x - 1 >= 0 &&
                        CheckObstaclesToMoveLeft(cellCTRLs[cell.pos.x - 1, cell.pos.y], cellCTRLs[cell.pos.x, cell.pos.y]) &&
                        cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal != null &&
                        (cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.color ||
                        cellCTRLs[cell.pos.x - 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.color5))

                    {

                        left = cellCTRLs[cell.pos.x - 1, cell.pos.y];

                    }
                    //������
                    if (cell.pos.x + 1 < cellCTRLs.GetLength(0) &&
                        CheckObstaclesToMoveRight(cellCTRLs[cell.pos.x + 1, cell.pos.y], cellCTRLs[cell.pos.x, cell.pos.y]) &&
                        cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal != null &&
                        (cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.color ||
                        cellCTRLs[cell.pos.x + 1, cell.pos.y].cellInternal.type == CellInternalObject.Type.color5))

                    {

                        right = cellCTRLs[cell.pos.x + 1, cell.pos.y];

                    }
                    //������
                    if (cell.pos.y + 1 < cellCTRLs.GetLength(1) &&
                        CheckObstaclesToMoveUp(cellCTRLs[cell.pos.x, cell.pos.y + 1], cellCTRLs[cell.pos.x, cell.pos.y]) &&
                        cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal != null &&
                        (cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.type == CellInternalObject.Type.color ||
                        cellCTRLs[cell.pos.x, cell.pos.y + 1].cellInternal.type == CellInternalObject.Type.color5))

                    {

                        up = cellCTRLs[cell.pos.x, cell.pos.y + 1];

                    }
                    //�����
                    if (cell.pos.y - 1 >= 0 &&
                        CheckObstaclesToMoveDown(cellCTRLs[cell.pos.x, cell.pos.y - 1], cellCTRLs[cell.pos.x, cell.pos.y]) &&
                        cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal != null &&
                        (cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.type == CellInternalObject.Type.color ||
                        cellCTRLs[cell.pos.x, cell.pos.y - 1].cellInternal.type == CellInternalObject.Type.color5))
                        
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
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1].cellInternal.type != CellInternalObject.Type.blocker &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1].cellInternal.color == potencialLeft.cells[0].cellInternal.color &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1].wallID != 6 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1].wallID != 11 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1].wallID != 14 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y + 1].wallID != 15 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 8 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 12 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 13 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 15)
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
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1].cellInternal.type != CellInternalObject.Type.blocker &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1].cellInternal.color == potencialRight.cells[0].cellInternal.color &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1].wallID != 7 &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1].wallID != 11 &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1].wallID != 12 &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y + 1].wallID != 15 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 5 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 13 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 14 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 15)
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
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1].cellInternal.type != CellInternalObject.Type.blocker &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1].cellInternal.color == potencialRight.cells[0].cellInternal.color &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1].wallID != 8 &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1].wallID != 12 &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1].wallID != 13 &&
                    cellCTRLs[cell.pos.x + 1, cell.pos.y - 1].wallID != 15 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 6 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 11 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 14 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 15)
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
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1].cellInternal.type != CellInternalObject.Type.blocker &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1].cellInternal.color == potencialLeft.cells[0].cellInternal.color &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1].wallID != 5 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1].wallID != 13 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1].wallID != 14 &&
                    cellCTRLs[cell.pos.x - 1, cell.pos.y - 1].wallID != 15 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 7 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 11 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 12 &&
                    cellCTRLs[cell.pos.x, cell.pos.y].wallID != 15)
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
                        CheckObstaclesToMoveUp(cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1], cellCTRLs[first.Target.pos.x, first.Target.pos.y]) &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1] != first.cells[0] &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1].cellInternal != null &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1].cellInternal.color == first.cells[0].cellInternal.color &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1].cellInternal.type != CellInternalObject.Type.color5 &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1].cellInternal.type != CellInternalObject.Type.blocker)
                        
                    {

                        up = cellCTRLs[first.Target.pos.x, first.Target.pos.y + 1];
                    }
                    if (first.Target.pos.y - 1 >= 0 &&
                        CheckObstaclesToMoveDown(cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1], cellCTRLs[first.Target.pos.x, first.Target.pos.y]) &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1] != first.cells[0] &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1].cellInternal != null &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1].cellInternal.color == first.cells[0].cellInternal.color &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1].cellInternal.type != CellInternalObject.Type.color5 &&
                        cellCTRLs[first.Target.pos.x, first.Target.pos.y - 1].cellInternal.type != CellInternalObject.Type.blocker)                     
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
                        CheckObstaclesToMoveRight(cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y], cellCTRLs[first.Target.pos.x, first.Target.pos.y]) &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y] != first.cells[0] &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y].cellInternal != null &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y].cellInternal.color == first.cells[0].cellInternal.color &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y].cellInternal.type != CellInternalObject.Type.color5 &&
                        cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y].cellInternal.type != CellInternalObject.Type.blocker)
                        
                    {

                        right = cellCTRLs[first.Target.pos.x + 1, first.Target.pos.y];
                    }
                    if (first.Target.pos.x - 1 >= 0 &&
                        CheckObstaclesToMoveLeft(cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y], cellCTRLs[first.Target.pos.x, first.Target.pos.y]) &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y] != first.cells[0] &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y].cellInternal != null &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y].cellInternal.color == first.cells[0].cellInternal.color &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y].cellInternal.type != CellInternalObject.Type.color5 &&
                        cellCTRLs[first.Target.pos.x - 1, first.Target.pos.y].cellInternal.type != CellInternalObject.Type.blocker)
                        
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
    void TestRandomNotPotencial()
    {
        //������� ���� ��� ����� ��� ������������� ��� �������� ��� �� ��������
        if (ListWaitingInternals.Count == 0 || Time.unscaledTime - timeLastMove < 1 || isMovingInternalObj || Time.unscaledTime - timeLastBoom < 0.2f)
            return;


        //������� � ������ � ���� ���-�� �� ������ ������, �������
        if (!isDoneTransformToCenter())
        {
            return;
        }

        //��������� ����������� �������, ���� ���� �������������
        CellSelect = null;
        CellSwap = null;

        //������������� ����� �������
        RandomizadeNewPos();


        bool isDoneTransformToCenter()
        {

            Vector2 pivotNeed = new Vector2((cellCTRLs.GetLength(0) / 2) * -1, (cellCTRLs.GetLength(1) / 2) * -1);

            foreach (CellInternalObject cellInternal in ListWaitingInternals)
            {
                if (cellInternal)
                {
                    cellInternal.rectMy.pivot += (pivotNeed - cellInternal.rectMy.pivot) * Time.deltaTime * 4;
                }
            }


            //��������� ��� ���������� �� �������� � ������
            foreach (CellInternalObject cellInternal in ListWaitingInternals)
            {
                //���� ������������ ���, �� ��������� � ���������
                if (cellInternal == null)
                {
                    continue;
                }
                if (Vector2.Distance(pivotNeed, cellInternal.rectMy.pivot) > 0.1f)
                {
                    return false;
                }
            }

            //���� ��� ������� ���������� ������, ������ ��������
            return true;

        }

        //������������
        void RandomizadeNewPos()
        {
            //���������� ������ ���� �������������
            List<CellCTRL> cellsList = new List<CellCTRL>();
            foreach (CellInternalObject cellInternal in ListWaitingInternals)
            {
                cellsList.Add(cellInternal.myCell);
            }

            //��������� ������

            //����
            int testNow = 0;
            int testMax = 100;
            //������� ���� ������ ���� ������ 100 � ������ �� �������
            while (ListWaitingInternals.Count > 0 && testNow < testMax)
            {
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
                else
                {
                    SetAllCellInternalRandomType(testNow - (testMax / 4) * 3);
                }



                testNow++;
            }

            void SetAllCellNull()
            {
                foreach (CellCTRL cell in cellsList)
                {
                    if (cell != null)

                    cell.cellInternal = null;
                }
            }

            //���������� ���� ������������� �� ������
            void SetAllCellInternal()
            {

                //���������� ����� ������������ � �������
                SetAllCellNull();

                for (int x = 0; x < ListWaitingInternals.Count; x++)
                {
                    CellCTRL selectedCell = null;

                    //������� ������ � ������ � ������������ �������
                    ListWaitingInternals[x].myCell = null;

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

            void SetAllCellInternalRandomColor(int countRandomMax)
            {
                //���������� ����� ������������ � �������
                SetAllCellNull();

                int countRandomNow = 0;
                for (int x = 0; x < ListWaitingInternals.Count; x++)
                {
                    CellCTRL selectedCell = null;

                    //������� ������ � ������ � ������������ �������
                    ListWaitingInternals[x].myCell = null;
                    if (countRandomNow < countRandomMax && ListWaitingInternals[x].type == CellInternalObject.Type.color)
                    {
                        countRandomNow++;

                        ListWaitingInternals[x].randSpawnType(false);
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

                        ListWaitingInternals[x].randSpawnType(false);
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
                        ListWaitingInternals[x].randSpawnType(false);
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
    public struct Buffer
    {
        public MenuGameplay.SuperHitType superHit;
        public int CalculateFrameNow; //������� �������������� ����
        public const int CalculateFrameMax = 5; //�� ����� ����� ���������� ����������, �������� � ����� ������� ������

        //������������� ����������
        public void Ini() {
            CalculateFrameNow = 0;
        }

        //������
        public void Calculate() {
            CalculateFrameNow++;
            if (CalculateFrameNow > CalculateFrameMax) {
                CalculateFrameNow = 0;
            }
        }
    }
    public Buffer buffer;

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

    /// <summary>
    /// ������� ���������� �� ����� ������ ������
    /// </summary>
    public void CreateBlocker(CellCTRL cellLast, CellInternalObject.InternalColor internalColor, int combID)
    {
        cellLast.DeleteInternalNoDamage();

        if (cellLast.myInternalNum == 0 && cellLast.timeAddInternalOld == Time.unscaledTime) return;

        GameObject internalObj = Instantiate(prefabInternal, parentOfInternals);
        CellInternalObject cellInternal = internalObj.GetComponent<CellInternalObject>();
        cellLast.timeAddInternalOld = Time.unscaledTime;

        cellInternal.IniBlockerColor(cellLast, this, internalColor, combID);


    }

    float timeCellSelect = 0;
    //�������� �� �� ��� ����� �������� ������� �������
    public void TestStartSwap()
    {
        //���� ���������� ������ ���� � ������ ������ 5 ������ ������� ���������

        if (CellSelect && Time.unscaledTime - timeCellSelect > 4 && Gameplay.main.playerTurn)
        {
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
        bool wallBlocksMovement = false;
        //��������� ���������
        //�����
        if (CellSelect.pos.x > 0 && CellSwap == cellCTRLs[CellSelect.pos.x - 1, CellSelect.pos.y])
        {
            neibour = true;
            //��� ��������, ��� ����� �� ��������� ������ ����� || ��� ��������, ��� ������ �� ������� ������ �����
            if (!CheckObstaclesToMoveLeft(CellSwap, CellSelect))
            {
                wallBlocksMovement = true;
            }
        }
        //������
        else if (CellSelect.pos.x < cellCTRLs.GetLength(0) - 1 && CellSwap == cellCTRLs[CellSelect.pos.x + 1, CellSelect.pos.y])
        {
            neibour = true;
            //��� ��������, ��� ������ �� ��������� ������ ����� || ��� ��������, ��� ����� �� ������� ������ �����
            if (!CheckObstaclesToMoveRight(CellSwap, CellSelect))
            {
                wallBlocksMovement = true;
            }
        }
        //�����
        else if (CellSelect.pos.y > 0 && CellSwap == cellCTRLs[CellSelect.pos.x, CellSelect.pos.y - 1])
        {
            neibour = true;
            //��� ��������, ��� ����� �� ��������� ������ ����� || ��� ��������, ��� ������ �� ������� ������ �����
            if (!CheckObstaclesToMoveDown(CellSwap, CellSelect))
            {
                wallBlocksMovement = true;
            }
        }
        //������
        else if (CellSelect.pos.y < cellCTRLs.GetLength(1) - 1 && CellSwap == cellCTRLs[CellSelect.pos.x, CellSelect.pos.y + 1])
        {
            neibour = true;
            //��� ��������, ��� ������ �� ��������� ������ ����� || ��� ��������, ��� ����� �� ������� ������ �����
            if (!CheckObstaclesToMoveUp(CellSwap, CellSelect))
            {
                wallBlocksMovement = true;
            }
        }

        //���� �� �������� ������ + ���� �����, ������� 
        if (!neibour || wallBlocksMovement)
        {
            CellSelect = null;
            CellSwap = null;

            SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipMoveReturn, 0.75f, Random.Range(0.9f, 1.1f));

            return;
        }

        //���� ����� �� ��������
        if (!CellSelect.cellInternal || //���� � ����� ����� ��������
            !CellSwap.cellInternal ||
            CellSelect.cellInternal.isMove || //���� � ������ ���������� ��������
            CellSwap.cellInternal.isMove ||
            CellSelect.Box > 0 || //���� � ������ �������
            CellSwap.Box > 0 ||
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
        if (!Gameplay.main.gameStarted)
        {
            PlayerProfile.main.Health.Amount--;
            PlayerProfile.main.Save();
            Gameplay.main.gameStarted = true;
        }
        //Gameplay.main.movingCan--;
    }

    void TestReturnSwap()
    {



        List<Swap> BufferSwapNew = new List<Swap>();

        foreach (Swap swap in BufferSwap)
        {

            if (swap == null)
            {
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

                SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipMoveReturn, 0.75f, Random.Range(0.9f, 1.1f));

                continue;
            }
            BufferSwapNew.Add(swap);

        }
        BufferSwap = BufferSwapNew;
    }

    int lastStepCount = 0;
    float lastTime = 0;
    //���� ���������� ����� ����������
    void TestMold()
    {
        //Debug.Log("MovingMold: " + Gameplay.main.movingMoldCount + " lastStepCount: " + lastStepCount);
        if (Gameplay.main.movingMoldCount <= lastStepCount || //���� ���������� ����� ������ ������ ��� ������� �������
            Gameplay.main.combo > 0 || //���� ���� ����� ���������� � ��� �� ��������
            isMovingInternalObj
            )
        {
            lastTime = Time.unscaledTime; //���������� ��������� ����������� �����
            return;
        }

        //���� �������� ������ ������� �������
        if (Time.unscaledTime - lastTime < 0.5f)
        {
            return;
        }

        StepMold();

        //��� �������
        void StepMold()
        {
            //�������� ��������� ������� �� ����� ������
            int num = Random.Range(0, moldCTRLs.Count);
            //���� ������� ����������, ���� �������� ���������� �������, ���� ����� �������
            if (moldCTRLs.Count > 0 && moldCTRLs[num] && moldCTRLs[num].TestSpawn())
            {
                //���������� ��� �������
                lastStepCount++;
            }
        }
    }

    /// <summary>
    /// C����� ���� ����� � ������� ���������� �� �������� � �������
    /// </summary>
    public CellCTRL[] cellsPriority;
    void TestCalcPriorityCells()
    {
        //���������� �����������
        bool isComplite = false;
        for (int testCount = 0; testCount < 50 && !isComplite; testCount++)
        {

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
            }
        }


    }

    /// <summary>
    /// ������� ������ ���������� ��� ������� ��� �����������
    /// </summary>
    public void SetSelectCell(CellCTRL CellClick)
    {
        if (!CellSelect)
        {
            CellSelect = CellClick;
            timeCellSelect = Time.unscaledTime;
        }
        else
        {
            //��������� ������ �� ���������
            if (isNeibour())
            {
                //��� ����� ������ �������
                CellSwap = CellClick;
            }
            else
            {
                CellSwap = null;
                CellSelect = CellClick;
            }

            //��������� ���������
            bool isNeibour()
            {

                //�����
                if (CellSelect.pos.x > 0 &&
                    cellCTRLs[CellSelect.pos.x - 1, CellSelect.pos.y] &&
                    cellCTRLs[CellSelect.pos.x - 1, CellSelect.pos.y] == CellClick)
                {
                    return true;
                }
                //������
                else if (CellSelect.pos.x < cellCTRLs.GetLength(0) - 1 &&
                    cellCTRLs[CellSelect.pos.x + 1, CellSelect.pos.y] &&
                    cellCTRLs[CellSelect.pos.x + 1, CellSelect.pos.y] == CellClick)
                {
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
    public bool isBlockingBoomDamage(CellCTRL cell)
    {
        bool result = false;

        if (cell == null) return result;


        if (cell.rock > 0 ||
            (cell.cellInternal != null && cell.cellInternal.type == CellInternalObject.Type.blocker))
        {
            result = true;
        }

        return result;
    }



    float timelastMoveTest = 0;

    bool isMovingOld = false;
    bool isMovingInternalObj = false; //��������� �� � �������� ������� ������
    bool isMovingFly = false;

    bool isMoving()
    {
        if (timelastMoveTest != Time.unscaledTime)
        {
            bool isMovingNow = false;

            timelastMoveTest = Time.unscaledTime;
            //���� ����� ��� ����������� �������� ��� ��������� �����
            if (Gameplay.main.isMissionComplite())
            {
                Gameplay.main.colors = 6;
                Gameplay.main.superColorPercent = 20;
                Gameplay.main.typeBlockerPercent = 20;

                CellSelect = null;
            }

            TestMoveInternalObj(); //��������� ������� �������� ��� ������ �����
            TestMoveFly();
            //���� ���������� ��������
            if (isMovingInternalObj || isMovingFly || Time.unscaledTime - movingObjLastTime < 1)
            {
                if (readyToPass)
                {
                    readyToPass = false;
                }
                isMovingNow = true;
            }
            else
            {
                isMovingNow = false;
            }

            //���� ���������� ��������� � ������� ��������
            if (isMovingOld && !isMovingNow ||
                (!isMovingOld && !isMovingNow && Gameplay.main.isMissionComplite()))
            {               
                listPotencial = new List<PotencialComb>();
                potencialBest = null;
                enemyPotencialBest = null;
                //�������� ��������� � ����������
                if (!Gameplay.main.isMissionComplite() && !Gameplay.main.isMissionDefeat())
                {
                    MidleMessageCombo();
                }

                //���� ������ ����� ��������� ������������
                if (Gameplay.main.isMissionComplite())
                {
                    //���������� �� ������� ��� ��������� ����� ����� ������ � ��������� ������������� �����
                    List<CellInternalObject> roskets = new List<CellInternalObject>();
                    List<CellInternalObject> bombs = new List<CellInternalObject>();
                    List<CellInternalObject> flys = new List<CellInternalObject>();
                    List<CellInternalObject> color5 = new List<CellInternalObject>();

                    //������ ����
                    List<CellInternalObject> colors = new List<CellInternalObject>();

                    //��������� ��� ������ �� ������� ������������� ����
                    for (int x = 0; x < cellCTRLs.GetLength(0); x++) {
                        for (int y = 0; y < cellCTRLs.GetLength(1); y++) {
                            //���� ������ ��� ��� ��� ������������ ���� ������
                            if (cellCTRLs[x,y] == null || 
                                cellCTRLs[x,y].cellInternal == null) {
                                continue;
                            }

                            //���� � ������
                            //����
                            if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.color) {
                                colors.Add(cellCTRLs[x, y].cellInternal);
                            }
                            //�����
                            else if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.bomb) {
                                bombs.Add(cellCTRLs[x, y].cellInternal);
                            }
                            //�������
                            else if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.airplane) {
                                flys.Add(cellCTRLs[x, y].cellInternal);
                            }
                            //������
                            else if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.rocketHorizontal ||
                                cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.rocketVertical) {
                                roskets.Add(cellCTRLs[x, y].cellInternal);
                            }
                            //����� ����
                            else if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.color5) {
                                color5.Add(cellCTRLs[x, y].cellInternal);
                            }
                        }
                    }

                    //���������� ��� ����� ����
                    if (color5.Count > 0) {
                        foreach (CellInternalObject cellInternal in color5) {
                            cellInternal.Activate(CellInternalObject.Type.color5, null, null);
                        }
                    }
                    //����� ���������� ��� �����
                    else if (bombs.Count > 0)
                    {
                        foreach (CellInternalObject cellInternal in bombs)
                        {
                            cellInternal.Activate(CellInternalObject.Type.bomb, null, null);
                        }
                    }
                    //����� ���������� ��� ������
                    else if (roskets.Count > 0)
                    {
                        foreach (CellInternalObject cellInternal in roskets)
                        {
                            cellInternal.Activate(cellInternal.type, null, null);
                        }
                    }
                    //���������� ��� �������
                    else if (flys.Count > 0)
                    {
                        foreach (CellInternalObject cellInternal in flys)
                        {
                            cellInternal.Activate(CellInternalObject.Type.airplane, null, null);
                        }
                    }

                    //����� ���� ��� ���� ���� � ������.. 
                    else if (Gameplay.main.movingCan > 0) {

                        //������ ������� �������������� ��� ������������
                        List<int> numRangIsUse = new List<int>();

                        //�������� ��������� ������
                        int count = Gameplay.main.movingCan;
                        for (int num = 0; num < count; num++) {


                            //�������� ���������� ������
                            int numRand = Random.Range(0, colors.Count);

                            bool numIsUsed = false;
                            //��������� ���� ����� �� �� ��� ��� ��� ��� � ������
                            foreach (int numUsed in numRangIsUse) {
                                if (numRand == numUsed) {
                                    numIsUsed = true;
                                    break;
                                }
                            }

                            //���� ���� ������������ ��� ��� ��� �� ����
                            //����������
                            if (numRand >= colors.Count ||
                                colors[numRand] == null || 
                                colors[numRand].type != CellInternalObject.Type.color || //���� ��� ������������ �� ����
                                numIsUsed //���� ����� ��� � �������������
                                ) {
                                continue;
                            }


                            //����������� ���� ���� ��� ���� ����������
                            float shanceRocket = Random.Range(0, 100f);
                            float shanceBomb = Random.Range(0, 100f);
                            float shanceFly = Random.Range(0, 100f);
                            float shanceSupBomb = Random.Range(0, 100f);

                            //�������� ��� ����������� �������
                            CellInternalObject.Type shanceType = CellInternalObject.Type.rocketHorizontal;
                            int shanceInt = Random.Range(0, 3);
                            if (shanceInt == 0) {
                                if (shanceBomb > 60) shanceType = CellInternalObject.Type.bomb;
                            }
                            else if (shanceInt == 1) {
                                if (shanceFly > 60) shanceType = CellInternalObject.Type.airplane;
                            }
                            else if (shanceInt == 2) {
                                if (shanceSupBomb > 1) shanceType = CellInternalObject.Type.color5;
                            }

                            //������� �������
                            if (shanceType == CellInternalObject.Type.rocketHorizontal)
                            {
                                //������ ��� ������������ �� ������ ��������������
                                if (Random.Range(0, 100) < 50)
                                {
                                    colors[numRand].setColorAndType(colors[numRand].color, CellInternalObject.Type.rocketHorizontal);
                                }
                                //������ ��� ������������ �� ������ ������������
                                else
                                {
                                    colors[numRand].setColorAndType(colors[numRand].color, CellInternalObject.Type.rocketVertical);
                                }
                            }
                            else if (shanceType == CellInternalObject.Type.bomb) {
                                colors[numRand].setColorAndType(colors[numRand].color, CellInternalObject.Type.bomb);
                            }
                            else if (shanceType == CellInternalObject.Type.airplane) {
                                colors[numRand].setColorAndType(colors[numRand].color, CellInternalObject.Type.airplane);
                            }
                            else if (shanceType == CellInternalObject.Type.color5) {
                                colors[numRand].setColorAndType(colors[numRand].color, CellInternalObject.Type.color5);
                            }

                            //���������� ������
                            colors[numRand].Activate(colors[numRand].type, null, null);

                            Gameplay.main.movingCan--;

                        }

                        //���� ����� �� �������� ����������� ��������� � ��������� ����
                        if (Gameplay.main.movingCan <= 0) {
                            MenuGameplay.main.CreateMidleMessage(MidleTextGameplay.strLastMove);
                        }
                    }

                    else if (Gameplay.main.isMissionComplite() && !NeedOpenComplite && Time.unscaledTime - timeLastMove > 0.5f)
                    {
                        PlayerProfile.main.Health.Amount++;
                        PlayerProfile.main.Save();
                        NeedOpenComplite = true;
                    }

                }

                else if (Gameplay.main.isMissionDefeat())
                {                                     
                    NeedOpenDefeat = true;
                }





                if (Gameplay.main.isMissionComplite())
                {
                    Gameplay.main.timeScale = 20;
                }

                readyToPass = true;
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

        void MidleMessageCombo()
        {
            if (ComboCount > 10)
            {
                MenuGameplay.main.CreateMidleMessage(MidleTextGameplay.strCombo);
            }
            else if (ComboInternal > 45)
            {
                MenuGameplay.main.CreateMidleMessage(MidleTextGameplay.strCongratulations);
            }
            else if (ComboInternal > 30)
            {
                MenuGameplay.main.CreateMidleMessage(MidleTextGameplay.strIncredible);
            }
            else if (ComboInternal > 15)
            {
                MenuGameplay.main.CreateMidleMessage(MidleTextGameplay.strNotBad);
            }

            ComboCount = 1;
            ComboInternal = 0;
        }
    }



    void TestEndMessage()
    {
        if (MessageCTRL.selected != null)
        {
            return;
        }

        if (NeedOpenComplite)
        {
            Gameplay.main.OpenMessageComplite();
        }
        else if (NeedOpenDefeat)
        {
            Gameplay.main.OpenMessageDefeat();
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
                    cellCTRLs[x, y].Box > 0 || //������������ �������
                    cellCTRLs[x, y].rock > 0 //|| //��� ���� ������
                                             //(cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.bomb && cellCTRLs[x,y].cellInternal.activateNeed) //��� ���� �������������� �����
                    ) continue;

                //���� ����� ���� ���� ���������, �� ���������
                if (y - 1 >= 0 &&
                    CheckObstaclesToMoveDown(cellCTRLs[x, y - 1], cellCTRLs[x, y]) &&
                    !cellCTRLs[x, y - 1].cellInternal)
                {
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

                if (movingNow)
                {
                    movingObjLastTime = Time.unscaledTime;
                }
            }
        }

        isMovingInternalObj = movingNow;
    }
    void TestMoveFly()
    {

        List<FlyCTRL> flyCTRLsNew = new List<FlyCTRL>();
        foreach (FlyCTRL flyCTRL in FlyCTRL.flyCTRLs)
        {
            if (flyCTRL != null)
                flyCTRLsNew.Add(flyCTRL);
        }
        FlyCTRL.flyCTRLs = flyCTRLsNew;

        if (FlyCTRL.flyCTRLs.Count > 0)
        {
            isMovingFly = true;
        }
        else
        {
            isMovingFly = false;
        }
    }

    //���������, ������ �� ����� ��������� ����� \ ���� \ ����� \ ������
    #region WallsCheck
    public bool CheckObstaclesToMoveUp(CellCTRL targetCell, CellCTRL fromCell)
    {
        return (targetCell != null &&
                        targetCell.rock == 0 &&
                        targetCell.Box == 0 &&
                        !targetCell.dispencer &&
                        targetCell.teleport == 0 && //�������� ������ � ��������� ������, �������� ������ ��������
                        targetCell.wallID != 3 &&
                        targetCell.wallID != 6 &&
                        targetCell.wallID != 7 &&
                        targetCell.wallID != 9 &&
                        targetCell.wallID != 11 &&
                        targetCell.wallID != 12 &&
                        targetCell.wallID != 14 &&
                        targetCell.wallID != 15 &&
                        ((fromCell != null &&
                        fromCell.wallID != 1 &&
                        fromCell.wallID != 5 &&
                        fromCell.wallID != 8 &&
                        fromCell.wallID != 9 &&
                        fromCell.wallID != 12 &&
                        fromCell.wallID != 13 &&
                        fromCell.wallID != 14 &&
                        fromCell.wallID != 15) ||
                        (fromCell == null)));
    }

    public bool CheckObstaclesToMoveDown(CellCTRL targetCell, CellCTRL fromCell)
    {
        return (targetCell != null &&
                        targetCell.rock == 0 &&
                        targetCell.Box == 0 &&
                        !targetCell.dispencer &&
                        targetCell.wallID != 1 &&
                        targetCell.wallID != 5 &&
                        targetCell.wallID != 8 &&
                        targetCell.wallID != 9 &&
                        targetCell.wallID != 12 &&
                        targetCell.wallID != 13 &&
                        targetCell.wallID != 14 &&
                        targetCell.wallID != 15 &&
                        ((fromCell != null &&
                        fromCell.teleport == 0 &&
                        fromCell.wallID != 3 &&
                        fromCell.wallID != 6 &&
                        fromCell.wallID != 7 &&
                        fromCell.wallID != 9 &&
                        fromCell.wallID != 11 &&
                        fromCell.wallID != 12 &&
                        fromCell.wallID != 14 &&
                        fromCell.wallID != 15) ||
                        (fromCell == null)));
    }

    public bool CheckObstaclesToMoveLeft(CellCTRL targetCell, CellCTRL fromCell)
    {
        return (targetCell != null && 
                        targetCell.rock == 0 &&
                        targetCell.Box == 0 &&
                        !targetCell.dispencer &&
                        targetCell.wallID != 2 &&
                        targetCell.wallID != 5 &&
                        targetCell.wallID != 6 &&
                        targetCell.wallID != 10 &&
                        targetCell.wallID != 11 &&
                        targetCell.wallID != 13 &&
                        targetCell.wallID != 14 &&
                        targetCell.wallID != 15 &&
                        ((fromCell != null &&
                        fromCell.wallID != 4 &&
                        fromCell.wallID != 7 &&
                        fromCell.wallID != 8 &&
                        fromCell.wallID != 10 &&
                        fromCell.wallID != 11 &&
                        fromCell.wallID != 12 &&
                        fromCell.wallID != 13 &&
                        fromCell.wallID != 15) ||
                        (fromCell == null)));
    }

    public bool CheckObstaclesToMoveRight(CellCTRL targetCell, CellCTRL fromCell)
    {
        return (targetCell != null &&
                        
                        targetCell.rock == 0 &&
                        targetCell.Box == 0 &&
                        !targetCell.dispencer &&
                        targetCell.wallID != 4 &&
                        targetCell.wallID != 7 &&
                        targetCell.wallID != 8 &&
                        targetCell.wallID != 10 &&
                        targetCell.wallID != 11 &&
                        targetCell.wallID != 12 &&
                        targetCell.wallID != 13 &&
                        targetCell.wallID != 15 &&
                        ((fromCell != null &&                        
                        fromCell.wallID != 2 &&
                        fromCell.wallID != 5 &&
                        fromCell.wallID != 6 &&
                        fromCell.wallID != 10 &&
                        fromCell.wallID != 11 &&
                        fromCell.wallID != 13 &&
                        fromCell.wallID != 14 &&
                        fromCell.wallID != 15) || 
                        (fromCell == null)));
    }
    #endregion

    private void TurnPass()
    {
        //���� � ���� ���� ���������
        if (LevelsScript.main.ReturnLevel().PassedWithEnemy &&
            canPassTurn && 
            readyToPass &&
            !Gameplay.main.isMissionComplite() &&
            !Gameplay.main.isMissionDefeat() &&
            Gameplay.main.moveCompleted
            )
        {
            //���� ���������� ������ ��� ���� (������ �����) ���� ���������� �� ����
            if (Gameplay.main.movingCombCount > 1 || Gameplay.main.movingCombCount == 0)
            {
                //�������� ��� ����������
                if (Gameplay.main.playerTurn && !EnemyController.enemyTurn)
                {
                    Gameplay.main.playerTurn = false;
                    EnemyController.enemyTurn = true;
                    MenuGameplay.main.CreateMidleMessage(MidleTextGameplay.strEnemyTurn, Color.blue);

                    Gameplay.main.movingCombCount = 0;
                }
                //�������� ��� ������
                else if (!EnemyController.enemyTurn && !Gameplay.main.playerTurn)
                {
                    Gameplay.main.playerTurn = true;
                    MenuGameplay.main.CreateMidleMessage(MidleTextGameplay.strYourTurn, Color.blue);

                    Gameplay.main.movingCombCount = 0;
                }

                canPassTurn = false;
            }
            //���� ��������� ���������� �� ���������� �������������� ��� ����� ��� ��������
            else {
                bool test = true;
                if(!Gameplay.main.playerTurn)
                EnemyController.enemyTurn = true;
            }
            Gameplay.main.moveCompleted = false;
        }        
    }

    //�������� ������ �� ��������
    void UpdateAddPlayBonus()
    {
        if (Gameplay.main.StartBonus == CellInternalObject.Type.none)
        {
            return;
        }

        //��������� ��� ������ ���� � ��������� �� ����������
        List<CellInternalObject> listInternals = new List<CellInternalObject>();
        List<CellInternalObject> listinternalsClosed = new List<CellInternalObject>();
        List<CellCTRL> listCellNotHaveInternal = new List<CellCTRL>();

        for (int x = 0; x < cellCTRLs.GetLength(0); x++)
        {
            for (int y = 0; y < cellCTRLs.GetLength(1); y++)
            {
                //��������� ������
                if (cellCTRLs[x, y] == null)
                {
                    continue;
                }

                if (cellCTRLs[x, y].cellInternal == null)
                {
                    if (cellCTRLs[x, y].rock == 0 && cellCTRLs[x, y].rockCTRL == null)
                    {
                        listCellNotHaveInternal.Add(cellCTRLs[x, y]);
                    }
                }
                else if (cellCTRLs[x, y].cellInternal.type == CellInternalObject.Type.color)
                {
                    if (cellCTRLs[x, y].rock == 0 && cellCTRLs[x, y].rockCTRL == null)
                    {
                        listInternals.Add(cellCTRLs[x, y].cellInternal);
                    }
                    else
                    {
                        listinternalsClosed.Add(cellCTRLs[x, y].cellInternal);
                    }
                }
            }
        }

        //���� ������������ ��������� ������������ ��� �������� ������
        if (listInternals.Count > 0)
        {
            int rand = Random.Range(0, listInternals.Count);
            listInternals[rand].setColorAndType(listInternals[rand].color, Gameplay.main.StartBonus);
        }
        else if (listinternalsClosed.Count > 0)
        {
            int rand = Random.Range(0, listinternalsClosed.Count);
            listinternalsClosed[rand].setColorAndType(listinternalsClosed[rand].color, Gameplay.main.StartBonus);
        }
        else if (listCellNotHaveInternal.Count > 0)
        {
            //int rand = Random.Range(0, listinternalsClosed.Count);
            //listinternalsClosed[rand].setColorAndType(listinternalsClosed[rand].color, Gameplay.main.StartBonus);
        }

        //�������
        Gameplay.main.StartBonus = CellInternalObject.Type.none;
    }
}
