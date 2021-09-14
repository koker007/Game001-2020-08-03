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
    public RockCTRL rockCTRL;
    public PanelSpreadCTRL panelCTRL;

    /// <summary>
    /// Степень запрета на перемещение объекта
    /// </summary>
    public int BlockingMove;
    /// <summary>
    /// Степень плесени
    /// </summary>
    public int mold;
    /// <summary>
    /// Степень камня
    /// </summary>
    public int rock;

    public bool panel;
    
    public int myInternalNum = 0;

    public float timeBoomOld = 0;     //Время последнего взрыва
    public float timeAddInternalOld = 0;     //Время последнего добавления внутренности

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Класс распространяющегося взрыва
    public class Explosion
    {
        public bool left = false;
        public bool right = false;
        public bool up = false;
        public bool down = false;

        public GameFieldCTRL.Combination comb;
        public float timer = 0;

        public Particle3dCTRL LeftParticleEffect;
        public Particle3dCTRL RightParticleEffect;
        public Particle3dCTRL UpParticleEffect;
        public Particle3dCTRL DownParticleEffect;

        //Создаем параметры взрыва
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
        /// Активировать взрыв это нанесет урон текущей ячейке и удалит взрыв
        /// </summary>
        public void Activate(CellCTRL cell) {
            //Применить взрыв к ячейке
            cell.BufferCombination = comb;
            cell.BufferNearDamage = false;
            cell.Damage();
        }
    }
    //Взрыв который ожидает своей активации
    public Explosion explosion = null;

    void ExplosionBoom() {
        //ВЫйти если взрыва нет
        if (explosion == null) return;

        //активируем сам взрыв
        explosion.Activate(this);

        //Распространяемся на лево
        if (explosion.left)
        {
            bool found = false;
            for (int minus = 1; minus < myField.cellCTRLs.GetLength(0); minus++) {
                //Если ячейку нашли
                if (pos.x - minus >= 0 && myField.cellCTRLs[pos.x - minus, pos.y] != null)
                {

                    //Если на ячейке есть взрыв, взрываем, удалем
                    if (myField.cellCTRLs[pos.x - minus, pos.y].explosion != null) {
                        //Сразу говорим ему взорваться
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion.Activate(myField.cellCTRLs[pos.x - minus, pos.y]);
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion = null;
                    }

                    //Если в ячейке есть стойкий к взрыву предмет
                    if (myField.isBlockingBoomDamage(myField.cellCTRLs[pos.x - minus, pos.y]))
                    {
                        myField.cellCTRLs[pos.x - minus, pos.y].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, explosion.comb));
                    }
                    //Иначе просто взрываем с таймером
                    else
                    {  

                        //Создаем взрыв и взрываем с таймером
                        myField.cellCTRLs[pos.x - minus, pos.y].ExplosionBoomInvoke(new Explosion(true, false, false, false, explosion.timer, explosion.comb));

                        //Если нету частиц взрыва то создаем
                        if (explosion.LeftParticleEffect == null)
                        {
                            explosion.LeftParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                            explosion.LeftParticleEffect.SetTransformSpeed(1 / explosion.timer);

                            Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                        }
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion.LeftParticleEffect = explosion.LeftParticleEffect;
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion.LeftParticleEffect.SetTransformTarget(new Vector2(pos.x - minus + 0.5f, pos.y + 0.5f));
                    }

                    found = true;
                    break;
                }
            }

            if (!found && explosion.LeftParticleEffect) {
                explosion.LeftParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f - 5, pos.y + 0.5f));
            }
        }
        //распространяемся на право
        if (explosion.right)
        {
            bool found = false;
            for (int plus = 1; plus < myField.cellCTRLs.GetLength(0); plus++)
            {
                //Если ячейку нашли
                if (pos.x + plus < myField.cellCTRLs.GetLength(0) && myField.cellCTRLs[pos.x + plus, pos.y] != null)
                {
                    //Если на ячейке есть взрыв, взрываем, удалем
                    if (myField.cellCTRLs[pos.x + plus, pos.y].explosion != null)
                    {
                        //Сразу говорим ему взорваться
                        myField.cellCTRLs[pos.x + plus, pos.y].explosion.Activate(myField.cellCTRLs[pos.x + plus, pos.y]);
                        myField.cellCTRLs[pos.x + plus, pos.y].explosion = null;
                    }

                    //Если в ячейке есть стойкий к взрыву предмет
                    if (myField.isBlockingBoomDamage(myField.cellCTRLs[pos.x + plus, pos.y]))
                    {
                        myField.cellCTRLs[pos.x + plus, pos.y].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, explosion.comb));
                    }
                    //Иначе просто взрываем с таймером
                    else
                    {
                        //Создаем взрыв и взрываем с таймером
                        myField.cellCTRLs[pos.x + plus, pos.y].ExplosionBoomInvoke(new Explosion(false, true, false, false, explosion.timer, explosion.comb));

                        //Если нету частиц взрыва то создаем
                        if (explosion.RightParticleEffect == null)
                        {
                            explosion.RightParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                            explosion.RightParticleEffect.SetTransformSpeed(1 / explosion.timer);
                            Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                        }
                        myField.cellCTRLs[pos.x + plus, pos.y].explosion.RightParticleEffect = explosion.RightParticleEffect;
                        myField.cellCTRLs[pos.x + plus, pos.y].explosion.RightParticleEffect.SetTransformTarget(new Vector2(pos.x + plus + 0.5f, pos.y + 0.5f));
                    }

                    found = true;
                    break;
                }
            }

            if (!found && explosion.RightParticleEffect)
            {
                explosion.RightParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f + 5, pos.y + 0.5f));
            }
        }
        //Распространяемся на верх
        if (explosion.up)
        {
            bool found = false;
            for (int plus = 1; plus < myField.cellCTRLs.GetLength(0); plus++)
            {
                //Если ячейку нашли
                if (pos.y + plus < myField.cellCTRLs.GetLength(1) && myField.cellCTRLs[pos.x, pos.y + plus] != null)
                {
                    //Если на ячейке есть взрыв, взрываем, удалем
                    if (myField.cellCTRLs[pos.x, pos.y + plus].explosion != null)
                    {
                        //Сразу говорим ему взорваться
                        myField.cellCTRLs[pos.x, pos.y + plus].explosion.Activate(myField.cellCTRLs[pos.x, pos.y + plus]);
                        myField.cellCTRLs[pos.x, pos.y + plus].explosion = null;
                    }

                    //Если в ячейке есть стойкий к взрыву предмет
                    if (myField.isBlockingBoomDamage(myField.cellCTRLs[pos.x, pos.y + plus]))
                    {
                        myField.cellCTRLs[pos.x, pos.y + plus].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, explosion.comb));
                    }
                    //Иначе просто взрываем с таймером
                    else
                    {

                        //Создаем взрыв и взрываем с таймером
                        myField.cellCTRLs[pos.x, pos.y + plus].ExplosionBoomInvoke(new Explosion(false, false, true, false, explosion.timer, explosion.comb));

                        //Если нету частиц взрыва то создаем
                        if (explosion.UpParticleEffect == null)
                        {
                            explosion.UpParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                            explosion.UpParticleEffect.SetTransformSpeed(1 / explosion.timer);
                            Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                        }
                        myField.cellCTRLs[pos.x, pos.y + plus].explosion.UpParticleEffect = explosion.UpParticleEffect;
                        myField.cellCTRLs[pos.x, pos.y + plus].explosion.UpParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f, pos.y + plus + 0.5f));
                    }

                    found = true;
                    break;
                }
            }

            if (!found && explosion.UpParticleEffect)
            {
                explosion.UpParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f, pos.y + 0.5f + 5));
            }


        }
        //Распространяемся на вниз
        if (explosion.down)
        {
            bool found = false;
            for (int minus = 1; minus < myField.cellCTRLs.GetLength(0); minus++)
            {
                //Если ячейку нашли
                if (pos.y - minus >= 0 && myField.cellCTRLs[pos.x, pos.y - minus] != null)
                {
                    //Если на ячейке есть взрыв, взрываем, удалем
                    if (myField.cellCTRLs[pos.x, pos.y - minus].explosion != null)
                    {
                        //Сразу говорим ему взорваться
                        myField.cellCTRLs[pos.x, pos.y - minus].explosion.Activate(myField.cellCTRLs[pos.x, pos.y - minus]);
                        myField.cellCTRLs[pos.x, pos.y - minus].explosion = null;
                    }

                    //Если в ячейке есть стойкий к взрыву предмет
                    if (myField.isBlockingBoomDamage(myField.cellCTRLs[pos.x, pos.y - minus]))
                    {
                        myField.cellCTRLs[pos.x, pos.y - minus].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, explosion.comb));
                    }
                    //Иначе просто взрываем с таймером
                    else
                    {

                        //Создаем взрыв и взрываем с таймером
                        myField.cellCTRLs[pos.x, pos.y - minus].ExplosionBoomInvoke(new Explosion(false, false, false, true, explosion.timer, explosion.comb));

                        //Если нету частиц взрыва то создаем
                        if (explosion.DownParticleEffect == null)
                        {
                            explosion.DownParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                            explosion.DownParticleEffect.SetTransformSpeed(1 / explosion.timer);
                            Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                        }
                        myField.cellCTRLs[pos.x, pos.y - minus].explosion.DownParticleEffect = explosion.DownParticleEffect;
                        myField.cellCTRLs[pos.x, pos.y - minus].explosion.DownParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f, pos.y - minus + 0.5f));
                    }


                    found = true;
                    break;
                }
            }

            if (!found && explosion.DownParticleEffect)
            {
                explosion.DownParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f, pos.y + 0.5f - 5));
            }
        }

        //Взрыв выполнен
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

        if (Time.unscaledTime - myField.timeLastBoom > 0.2f)
        {
            myField.timeLastBoom = Time.unscaledTime;
        }
        else {
            myField.timeLastBoom += 0.025f;
        }

        //Если камня нету
        if (rock <= 0)
        {
            //Если ящика нету
            if (BlockingMove <= 0)
            {

                //Если нет плесени
                if (mold <= 0)
                {
                    if (!panel && combination != null && combination.foundPanel) {
                        CreatePanel();
                    }
                }
                else {
                    mold--;
                }

                //Если есть внутренность
                if (cellInternal)
                {

                    cellInternal.Activate(cellInternal.type, partner, combination);
                }

                //наносим соседним ячейкам урон по блокираторам движения
                if (nearDamage)
                    DamageNearCells();
            }
            else {
                //Самому себе
                DamageNear();
            }
        }
        else {
            rock--;
        }
        

        //Создаем панель если плесени нет и нужно создать панель
        if (combination != null && BlockingMove <= 0 && mold <= 0 && combination.foundPanel && !panel) {
            CreatePanel();
        }





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
