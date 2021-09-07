using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

//�����
/// <summary>
/// ������ � �� ���������
/// </summary>
public class CellCTRL : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    static int lastInternalNum = 0; //����� ����� ����� ����� ��������� ����� � ��� ����������� ����� ���� ��������
    public int GetNextLastInternalNum {
        get {
            int num = lastInternalNum;
            lastInternalNum++;

            return num;
        }
    }
    static public int GetNowLastInternalNum
    {
        get
        {
            int num = lastInternalNum;

            return num;
        }
    }

    public GameFieldCTRL myField;
    public int MyPriority = 0;

    static CellCTRL CellClickOld;
    static CellCTRL CellEnterOld;

    [SerializeField]
    Image[] ramka;

    public Vector2Int pos = new Vector2Int();
    /// <summary>
    /// ������������ ������
    /// </summary>
    public CellInternalObject cellInternal;
    public BoxBlockCTRL BoxBlockCTRL;
    public MoldCTRL moldCTRL;
    public RockCTRL rockCTRL;
    public PanelSpreadCTRL panelCTRL;

    /// <summary>
    /// ������� ������� �� ����������� �������
    /// </summary>
    public int BlockingMove;
    /// <summary>
    /// ������� �������
    /// </summary>
    public int mold;
    /// <summary>
    /// ������� �����
    /// </summary>
    public int rock;

    public bool panel;
    
    public int myInternalNum = 0;

    public float timeBoomOld = 0;     //����� ���������� ������
    public float timeAddInternalOld = 0;     //����� ���������� ���������� ������������

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //����� ������������������� ������
    public class Explosion
    {
        public bool left = false;
        public bool right = false;
        public bool up = false;
        public bool down = false;

        public GameFieldCTRL.Combination comb;
        public float timer = 0;

        //������� ��������� ������
        public Explosion(bool leftf, bool rightf, bool upf, bool downf, float timerF, GameFieldCTRL.Combination combf)
        {
            left = leftf;
            right = rightf;
            up = upf;
            down = downf;

            timer = timerF;
            comb = combf;
        }

        /// <summary>
        /// ������������ ����� ��� ������� ���� ������� ������ � ������ �����
        /// </summary>
        public void Activate(CellCTRL cell) {
            cell.BufferCombination = comb;
            cell.BufferNearDamage = false;
            cell.Damage();
        }
    }
    //����� ������� ������� ����� ���������
    public Explosion explosion = null;

    void ExplosionBoom() {
        //����� ���� ������ ���
        if (explosion == null) return;

        //���������� ��� �����
        explosion.Activate(this);

        //���������������� �� ����
        if (explosion.left)
        {
            for (int minus = 1; minus < myField.cellCTRLs.GetLength(0); minus++) {
                //���� ������ �����
                if (pos.x - minus >= 0 && myField.cellCTRLs[pos.x - minus, pos.y] != null)
                {

                    //���� �� ������ ���� �����, ��������, ������
                    if (myField.cellCTRLs[pos.x - minus, pos.y].explosion != null) {
                        //����� ������� ��� ����������
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion.Activate(myField.cellCTRLs[pos.x - minus, pos.y]);
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion = null;
                    }

                    //���� � ������ ���� ������� � ������ �������
                    if (myField.isBlockingBoomDamage(myField.cellCTRLs[pos.x - minus, pos.y])) {
                        myField.cellCTRLs[pos.x - minus, pos.y].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, explosion.comb));
                    }
                    //����� ������ �������� � ��������
                    else
                        //������� ����� � �������� � ��������
                        myField.cellCTRLs[pos.x - minus, pos.y].ExplosionBoomInvoke(new Explosion(true, false, false, false, explosion.timer, explosion.comb));

                    break;
                }
            }
        }
        //���������������� �� �����
        if (explosion.right)
        {
            for (int plus = 1; plus < myField.cellCTRLs.GetLength(0); plus++)
            {
                //���� ������ �����
                if (pos.x + plus < myField.cellCTRLs.GetLength(0) && myField.cellCTRLs[pos.x + plus, pos.y] != null)
                {
                    //���� �� ������ ���� �����, ��������, ������
                    if (myField.cellCTRLs[pos.x + plus, pos.y].explosion != null)
                    {
                        //����� ������� ��� ����������
                        myField.cellCTRLs[pos.x + plus, pos.y].explosion.Activate(myField.cellCTRLs[pos.x + plus, pos.y]);
                        myField.cellCTRLs[pos.x + plus, pos.y].explosion = null;
                    }

                    //���� � ������ ���� ������� � ������ �������
                    if (myField.isBlockingBoomDamage(myField.cellCTRLs[pos.x + plus, pos.y]))
                    {
                        myField.cellCTRLs[pos.x + plus, pos.y].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, explosion.comb));
                    }
                    //����� ������ �������� � ��������
                    else
                        //������� ����� � �������� � ��������
                        myField.cellCTRLs[pos.x + plus, pos.y].ExplosionBoomInvoke(new Explosion(false, true, false, false, explosion.timer, explosion.comb));

                    break;
                }
            }
        }
        //���������������� �� ����
        if (explosion.up)
        {
            for (int plus = 1; plus < myField.cellCTRLs.GetLength(0); plus++)
            {
                //���� ������ �����
                if (pos.y + plus < myField.cellCTRLs.GetLength(1) && myField.cellCTRLs[pos.x, pos.y + plus] != null)
                {
                    //���� �� ������ ���� �����, ��������, ������
                    if (myField.cellCTRLs[pos.x, pos.y + plus].explosion != null)
                    {
                        //����� ������� ��� ����������
                        myField.cellCTRLs[pos.x, pos.y + plus].explosion.Activate(myField.cellCTRLs[pos.x, pos.y + plus]);
                        myField.cellCTRLs[pos.x, pos.y + plus].explosion = null;
                    }

                    //���� � ������ ���� ������� � ������ �������
                    if (myField.isBlockingBoomDamage(myField.cellCTRLs[pos.x, pos.y + plus]))
                    {
                        myField.cellCTRLs[pos.x, pos.y + plus].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, explosion.comb));
                    }
                    //����� ������ �������� � ��������
                    else
                        //������� ����� � �������� � ��������
                        myField.cellCTRLs[pos.x, pos.y + plus].ExplosionBoomInvoke(new Explosion(false, false, true, false, explosion.timer, explosion.comb));

                    break;
                }
            }
        }
        //���������������� �� ����
        if (explosion.down)
        {
            for (int minus = 1; minus < myField.cellCTRLs.GetLength(0); minus++)
            {
                //���� ������ �����
                if (pos.y - minus >= 0 && myField.cellCTRLs[pos.x, pos.y - minus] != null)
                {
                    //���� �� ������ ���� �����, ��������, ������
                    if (myField.cellCTRLs[pos.x, pos.y - minus].explosion != null)
                    {
                        //����� ������� ��� ����������
                        myField.cellCTRLs[pos.x, pos.y - minus].explosion.Activate(myField.cellCTRLs[pos.x, pos.y - minus]);
                        myField.cellCTRLs[pos.x, pos.y - minus].explosion = null;
                    }

                    //���� � ������ ���� ������� � ������ �������
                    if (myField.isBlockingBoomDamage(myField.cellCTRLs[pos.x, pos.y - minus]))
                    {
                        myField.cellCTRLs[pos.x, pos.y - minus].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, explosion.comb));
                    }
                    //����� ������ �������� � ��������
                    else
                        //������� ����� � �������� � ��������
                        myField.cellCTRLs[pos.x, pos.y - minus].ExplosionBoomInvoke(new Explosion(false, false, false, true, explosion.timer, explosion.comb));

                    break;
                }
            }
        }

        //����� ��������
        explosion = null;

    }
    public void ExplosionBoomInvoke(Explosion explosionNew) {
        explosion = explosionNew;
        Invoke("ExplosionBoom", explosion.timer);
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void setInternal(CellInternalObject internalObjectNew) {
        cellInternal = internalObjectNew;
        cellInternal.isMove = true;
        myInternalNum = lastInternalNum;
        lastInternalNum++;
    }

    public GameFieldCTRL.Combination BufferCombination;
    public bool BufferNearDamage = true;

    /// <summary>
    /// �������� ���� � ���������� �� ������������
    /// </summary>
    public void Damage()
    {
        Damage(null, BufferCombination, BufferNearDamage);
        BufferCombination = null;
        BufferNearDamage = true;
    }

    public void Damage(CellInternalObject partner, GameFieldCTRL.Combination combination) {
        Damage(partner, combination, true);
    }
    public void Damage(CellInternalObject partner, GameFieldCTRL.Combination combination, bool nearDamage)
    {

        //���� ����� ����
        if (rock <= 0)
        {
            //���� ����� ����
            if (BlockingMove <= 0)
            {

                //���� ��� �������
                if (mold <= 0)
                {
                    if (!panel && combination != null && combination.foundPanel) {
                        CreatePanel();
                    }
                }
                else {
                    mold--;
                }

                //���� ���� ������������
                if (cellInternal)
                {

                    cellInternal.Activate(cellInternal.type, partner, combination);
                }

                //������� �������� ������� ���� �� ������������ ��������
                if (nearDamage)
                    DamageNearCells();
            }
            else {
                //������ ����
                DamageNear();
            }
        }
        else {
            rock--;
        }
        

        //������� ������ ���� ������� ��� � ����� ������� ������
        if (combination != null && BlockingMove <= 0 && mold <= 0 && combination.foundPanel && !panel) {
            CreatePanel();
        }





        //���������� ����������
        CalcMyPriority();


        void DamageNearCells() {

            //�����
            if (pos.x - 1 >= 0 && myField.cellCTRLs[pos.x - 1, pos.y]) {
                myField.cellCTRLs[pos.x - 1, pos.y].DamageNear();
            }
            //������
            if (pos.x + 1 < myField.cellCTRLs.GetLength(0) && myField.cellCTRLs[pos.x + 1, pos.y]) {
                myField.cellCTRLs[pos.x + 1, pos.y].DamageNear();
            }
            //�����
            if (pos.y - 1 >= 0 && myField.cellCTRLs[pos.x, pos.y - 1]) {
                myField.cellCTRLs[pos.x, pos.y - 1].DamageNear();
            }
            //������
            if (pos.y + 1 < myField.cellCTRLs.GetLength(1) && myField.cellCTRLs[pos.x, pos.y + 1]) {
                myField.cellCTRLs[pos.x, pos.y + 1].DamageNear();
            }
        }

        void CreatePanel() {
            panel = true;

            GameObject panelObj = Instantiate(myField.prefabPanel, myField.parentOfPanels);
            panelCTRL = panelObj.GetComponent<PanelSpreadCTRL>();

            //������������� �������
            panelCTRL.inicialize(this);
        }
    }

    public void DamageInvoke(float time) {
        Invoke("Damage", time);
    }

    public void DamageNear() {
        //������� ���� �����
        if (BlockingMove > 0)
        {
            BlockingMove--;
        }

        //���������� ����������
        CalcMyPriority();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TestInternal();
        TestBoxBlock();
    }

    //�������� ���� ���������� ������ �� ��������� �� ��� ������, �� �������� ��� ���
    void TestInternal() {
        if (cellInternal && cellInternal.myCell != this) {
            cellInternal = null;
        }
    }
    void TestBoxBlock() {
        //���� � ������ ���� ����������� ��������, �������� ��� ���
        if (!BoxBlockCTRL) {
            return;
        }

        
    }


    /// <summary>
    /// ������ ���������� ������ ������
    /// </summary>
    public void CalcMyPriority() {

        const int PriorityBox = 10; //��������� �����
        const int PriorityMold = 5; //��������� �������
        const int PriorityPanel = 5; //��������� ���������� ������


        MyPriority = GetPriorityNow();


        //��������� � �������� ������� ���������
        int GetPriorityNow() {
            int result = 0;

            //����������� ����������
            //�����
            if (BlockingMove > 0)
            {
                result += (5 - BlockingMove) * PriorityBox; //��� ������ ������, ��� ����������� �� �������
            }
            //�������
            if (mold > 0)
            {
                result += (5 - mold) * PriorityMold; //��� ������ ������, ��� ����������� �� �������
            }
            //���������� ������
            if (!panel)
            {
                result += PriorityPanel;
            }

            return result;
        }

        //����������� ��� ����� � ������� ����������� � ���������� �����
        void CalcPriorityArray() {
            //���� ��� ������ � �������
            bool found = false;

            //���� ���� ������� �������
            for (int myNum = 0; myNum < myField.cellsPriority.Length && !found; myNum++) {
                //���������� ���� ��� �� ���� ������
                if (myField.cellsPriority[myNum] != this) continue;

                //���������� ���������� ����� �������� �������
                for (int minus = 0; minus < myNum; minus++) {
                    //���� ��������� ������� ������ ������ �� ���������� �� ������
                    if (MyPriority > myField.cellsPriority[myNum].MyPriority) {
                        
                    }
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //���� ���� �� ������
        MouseCTRL.main.click();

        //���� ������ ���� ������ � �������� ���
        if (cellInternal && !cellInternal.isMove) {

            //���� ��������� ������� ���� � ���� �� ����� ������
            if (MouseCTRL.main.ClickDouble && CellClickOld == this)
            {
                //cellInternal.Activate();
            }

            //��������� ����
            else {
                myField.SetSelectCell(this);
            }

            CellClickOld = this;
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {

        if (Input.GetMouseButton(0)) {
            if (CellClickOld != this && CellEnterOld != this) {
                myField.SetSelectCell(this);
            }
            CellEnterOld = this;
        }
    }
    public void OnPointerUp( PointerEventData eventData)
    {

    }

}
