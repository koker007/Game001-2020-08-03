using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

//Семен
/// <summary>
/// Ячейка и ее состояния
/// </summary>
public class CellCTRL : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    static int lastInternalNum = 0; //нужно чтобы ячека знала насколько давно в ней происходило какое либо действие
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
    /// Внутренность ячейки
    /// </summary>
    public CellInternalObject cellInternal;
    public BoxBlockCTRL BoxBlockCTRL;
    public MoldCTRL moldCTRL;
    public PanelSpreadCTRL panelCTRL;

    /// <summary>
    /// Степень запрета на перемещение объекта
    /// </summary>
    public int BlockingMove;
    /// <summary>
    /// Степень плесени
    /// </summary>
    public int mold;
    public bool panel;
    
    public int myInternalNum = 0;

    public float timeBoomOld = 0;     //Время последнего взрыва
    public float timeAddInternalOld = 0;     //Время последнего добавления внутренности

    public void setInternal(CellInternalObject internalObjectNew) {
        cellInternal = internalObjectNew;
        cellInternal.isMove = true;
        myInternalNum = lastInternalNum;
        lastInternalNum++;
    }

    public GameFieldCTRL.Combination BufferCombination;
    public bool BufferNearDamage = true;

    /// <summary>
    /// получить очки и избавиться от внутренности
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

        if (mold > 0) {
            mold--;
        }

        if (cellInternal)
        {


            cellInternal.Activate(cellInternal.type, partner, combination);
            //Избавляемся
            //cellInternal.DestroyObj();
        }

        //Создаем панель если плесени нет и нужно создать панель
        if (combination != null && BlockingMove <= 0 && mold <= 0 && combination.foundPanel && !panel) {
            CreatePanel();
        }

        //наносим соседним ячейкам урон по блокираторам движения
        if (nearDamage)
            DamageNearCells();

        //Самому себе
        DamageNear();


        //Перенасчет приоритета
        CalcMyPriority();


        void DamageNearCells() {

            //Слева
            if (pos.x - 1 >= 0 && myField.cellCTRLs[pos.x - 1, pos.y]) {
                myField.cellCTRLs[pos.x - 1, pos.y].DamageNear();
            }
            //Справа
            if (pos.x + 1 < myField.cellCTRLs.GetLength(0) && myField.cellCTRLs[pos.x + 1, pos.y]) {
                myField.cellCTRLs[pos.x + 1, pos.y].DamageNear();
            }
            //снизу
            if (pos.y - 1 >= 0 && myField.cellCTRLs[pos.x, pos.y - 1]) {
                myField.cellCTRLs[pos.x, pos.y - 1].DamageNear();
            }
            //Сверху
            if (pos.y + 1 < myField.cellCTRLs.GetLength(1) && myField.cellCTRLs[pos.x, pos.y + 1]) {
                myField.cellCTRLs[pos.x, pos.y + 1].DamageNear();
            }
        }

        void CreatePanel() {
            panel = true;

            GameObject panelObj = Instantiate(myField.prefabPanel, myField.parentOfPanels);
            panelCTRL = panelObj.GetComponent<PanelSpreadCTRL>();

            //Инициализация плесени
            panelCTRL.inicialize(this);
        }
    }

    public void DamageInvoke(float time) {
        Invoke("Damage", time);
    }

    public void DamageNear() {
        //Нанести урон ящику
        if (BlockingMove > 0)
        {
            BlockingMove--;
        }

        //Перенасчет приоритета
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

    //Проверка если внутренний обьект не ссылается на эту ячейку, мы забываем про нее
    void TestInternal() {
        if (cellInternal && cellInternal.myCell != this) {
            cellInternal = null;
        }
    }
    void TestBoxBlock() {
        //Если у ячейки нету блокиратора движения, забываем про нее
        if (!BoxBlockCTRL) {
            return;
        }

        
    }


    /// <summary>
    /// Расчет приоритера данной ячейки
    /// </summary>
    public void CalcMyPriority() {

        const int PriorityBox = 10; //Приоритет ящика
        const int PriorityMold = 5; //приоритет плесени
        const int PriorityPanel = 5; //Приоритет отсутствия панели


        MyPriority = GetPriorityNow();


        //Расчитать и получить текущий приоритет
        int GetPriorityNow() {
            int result = 0;

            //расчитываем приоритеты
            //ящика
            if (BlockingMove > 0)
            {
                result += (5 - BlockingMove) * PriorityBox; //Чем меньше жизней, тем желательнее ее сломать
            }
            //Плесени
            if (mold > 0)
            {
                result += (5 - mold) * PriorityMold; //Чем меньше жизней, тем желательнее ее сломать
            }
            //отсутствия панели
            if (!panel)
            {
                result += PriorityPanel;
            }

            return result;
        }

        //Переместить эту ячеку в массиве приоритетов в правильное место
        void CalcPriorityArray() {
            //ищем эту ячейку в массиве
            bool found = false;

            //Ищем свою текущую позицию
            for (int myNum = 0; myNum < myField.cellsPriority.Length && !found; myNum++) {
                //Пропускаем если это не наша ячейка
                if (myField.cellsPriority[myNum] != this) continue;

                //Сравниваем приоритеты ячеек передней стоящих
                for (int minus = 0; minus < myNum; minus++) {
                    //если приоритет текущей ячейки больше то продвигаем ее вперед
                    if (MyPriority > myField.cellsPriority[myNum].MyPriority) {
                        
                    }
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Если клик по ячейке
        MouseCTRL.main.click();

        //Если внутри есть объект и движения нет
        if (cellInternal && !cellInternal.isMove) {

            //если произошел двойной клик и клик по тойже ячейке
            if (MouseCTRL.main.ClickDouble && CellClickOld == this)
            {
                //cellInternal.Activate();
            }

            //Одинарный клик
            else {
                myField.SetSelectCell(this);
            }

            CellClickOld = this;
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {

        if (CellClickOld != this && CellEnterOld != this) {
            myField.SetSelectCell(this);
        }
        CellEnterOld = this;
    }
    public void OnPointerUp( PointerEventData eventData)
    {

    }

}
