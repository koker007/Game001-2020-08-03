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
    public IceCTRL iceCTRL;
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
    /// степень льда
    /// </summary>
    public int ice;
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
            GameFieldCTRL.Combination comb = new GameFieldCTRL.Combination(explosion.comb);

            bool found = false;
            for (int minus = 1; minus < myField.cellCTRLs.GetLength(0); minus++) {
                //Если ячейку нашли
                if (pos.x - minus >= 0 && myField.cellCTRLs[pos.x - minus, pos.y] != null)
                {
                    //Если в ячейке есть панель
                    if (myField.cellCTRLs[pos.x - minus, pos.y].panel) comb.foundPanel = true;
                    if (myField.cellCTRLs[pos.x - minus, pos.y].mold > 0) comb.foundBenefit = true;

                    //Если на ячейке есть взрыв, взрываем, удалем
                    if (myField.cellCTRLs[pos.x - minus, pos.y].explosion != null) {
                        //Сразу говорим ему взорваться
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion.Activate(myField.cellCTRLs[pos.x - minus, pos.y]);
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion = null;
                    }

                    //Если в ячейке есть стойкий к взрыву предмет
                    if (myField.isBlockingBoomDamage(myField.cellCTRLs[pos.x - minus, pos.y]))
                    {
                        myField.cellCTRLs[pos.x - minus, pos.y].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, comb), explosion.timer * minus);
                    }
                    //Иначе просто взрываем с таймером
                    else
                    {  

                        //Создаем взрыв и взрываем с таймером
                        myField.cellCTRLs[pos.x - minus, pos.y].ExplosionBoomInvoke(new Explosion(true, false, false, false, explosion.timer, comb), explosion.timer * minus);

                        //Если нету частиц взрыва то создаем
                        if (explosion.LeftParticleEffect == null)
                        {
                            explosion.LeftParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                            explosion.LeftParticleEffect.SetTransformSpeed(1 / explosion.timer);

                            //Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                        }
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion.LeftParticleEffect = explosion.LeftParticleEffect;
                        myField.cellCTRLs[pos.x - minus, pos.y].explosion.LeftParticleEffect.SetTransformTarget(new Vector2(pos.x - minus + 0.5f, pos.y + 0.5f));
                    }

                    found = true;
                    break;
                }
            }


            if (!found)
            {
                //Если нету частиц взрыва то создаем
                if (explosion.LeftParticleEffect == null)
                {
                    explosion.LeftParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                    explosion.LeftParticleEffect.SetTransformSpeed(1 / explosion.timer);
                    //Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                }

                explosion.LeftParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f - 15, pos.y + 0.5f));
            }
        }
        //распространяемся на право
        if (explosion.right)
        {

            GameFieldCTRL.Combination comb = new GameFieldCTRL.Combination(explosion.comb);

            bool found = false;
            for (int plus = 1; plus < myField.cellCTRLs.GetLength(0); plus++)
            {
                //Если ячейку нашли
                if (pos.x + plus < myField.cellCTRLs.GetLength(0) && myField.cellCTRLs[pos.x + plus, pos.y] != null)
                {

                    //Если в ячейке есть панель
                    if (myField.cellCTRLs[pos.x + plus, pos.y].panel) comb.foundPanel = true;
                    if (myField.cellCTRLs[pos.x + plus, pos.y].mold > 0) comb.foundBenefit = true;

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
                        myField.cellCTRLs[pos.x + plus, pos.y].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, comb), explosion.timer * plus);
                    }
                    //Иначе просто взрываем с таймером
                    else
                    {
                        //Создаем взрыв и взрываем с таймером
                        myField.cellCTRLs[pos.x + plus, pos.y].ExplosionBoomInvoke(new Explosion(false, true, false, false, explosion.timer, comb), explosion.timer * plus);

                        //Если нету частиц взрыва то создаем
                        if (explosion.RightParticleEffect == null)
                        {
                            explosion.RightParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                            explosion.RightParticleEffect.SetTransformSpeed(1 / explosion.timer);
                            //Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                        }
                        myField.cellCTRLs[pos.x + plus, pos.y].explosion.RightParticleEffect = explosion.RightParticleEffect;
                        myField.cellCTRLs[pos.x + plus, pos.y].explosion.RightParticleEffect.SetTransformTarget(new Vector2(pos.x + plus + 0.5f, pos.y + 0.5f));
                    }

                    found = true;
                    break;
                }
            }


            if (!found)
            {
                //Если нету частиц взрыва то создаем
                if (explosion.RightParticleEffect == null)
                {
                    explosion.RightParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                    explosion.RightParticleEffect.SetTransformSpeed(1 / explosion.timer);
                    //Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                }

                explosion.RightParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f + 15, pos.y + 0.5f));
            }

        }
        //Распространяемся на верх
        if (explosion.up)
        {

            GameFieldCTRL.Combination comb = new GameFieldCTRL.Combination(explosion.comb);

            bool found = false;
            for (int plus = 1; plus < myField.cellCTRLs.GetLength(1); plus++)
            {
                //Если ячейку нашли
                if (pos.y + plus < myField.cellCTRLs.GetLength(1) && myField.cellCTRLs[pos.x, pos.y + plus] != null)
                {

                    //Если в ячейке есть панель
                    if (myField.cellCTRLs[pos.x, pos.y + plus].panel) comb.foundPanel = true;
                    if (myField.cellCTRLs[pos.x, pos.y + plus].mold > 0) comb.foundBenefit = true;

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
                        myField.cellCTRLs[pos.x, pos.y + plus].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, comb), explosion.timer * plus);
                    }
                    //Иначе просто взрываем с таймером
                    else
                    {

                        //Создаем взрыв и взрываем с таймером
                        myField.cellCTRLs[pos.x, pos.y + plus].ExplosionBoomInvoke(new Explosion(false, false, true, false, explosion.timer, comb), explosion.timer * plus);

                        //Если нету частиц взрыва то создаем
                        if (explosion.UpParticleEffect == null)
                        {
                            explosion.UpParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                            explosion.UpParticleEffect.SetTransformSpeed(1 / explosion.timer);
                            //Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                        }
                        myField.cellCTRLs[pos.x, pos.y + plus].explosion.UpParticleEffect = explosion.UpParticleEffect;
                        myField.cellCTRLs[pos.x, pos.y + plus].explosion.UpParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f, pos.y + plus + 0.5f));
                    }

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                //Если нету частиц взрыва то создаем
                if (explosion.UpParticleEffect == null)
                {
                    explosion.UpParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                    explosion.UpParticleEffect.SetTransformSpeed(1 / explosion.timer);
                    //Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                }

                explosion.UpParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f, pos.y + 0.5f + 15));
            }


        }
        //Распространяемся на вниз
        if (explosion.down)
        {
            GameFieldCTRL.Combination comb = new GameFieldCTRL.Combination(explosion.comb);

            bool found = false;
            for (int minus = 1; minus < myField.cellCTRLs.GetLength(1); minus++)
            {
                //Если ячейку нашли
                if (pos.y - minus >= 0 && myField.cellCTRLs[pos.x, pos.y - minus] != null)
                {

                    //Если в ячейке есть панель
                    if (myField.cellCTRLs[pos.x, pos.y - minus].panel) comb.foundPanel = true;
                    if (myField.cellCTRLs[pos.x, pos.y - minus].mold > 0) comb.foundBenefit = true;

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
                        myField.cellCTRLs[pos.x, pos.y - minus].ExplosionBoomInvoke(new Explosion(false, false, false, false, explosion.timer, comb), explosion.timer * minus);
                    }
                    //Иначе просто взрываем с таймером
                    else
                    {

                        //Создаем взрыв и взрываем с таймером
                        myField.cellCTRLs[pos.x, pos.y - minus].ExplosionBoomInvoke(new Explosion(false, false, false, true, explosion.timer, comb), explosion.timer * minus);

                        //Если нету частиц взрыва то создаем
                        if (explosion.DownParticleEffect == null)
                        {
                            explosion.DownParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                            explosion.DownParticleEffect.SetTransformSpeed(1 / explosion.timer);
                            //Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                        }
                        myField.cellCTRLs[pos.x, pos.y - minus].explosion.DownParticleEffect = explosion.DownParticleEffect;
                        myField.cellCTRLs[pos.x, pos.y - minus].explosion.DownParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f, pos.y - minus + 0.5f));
                    }


                    found = true;
                    break;
                }
            }

            if (!found)
            {
                //Если нету частиц взрыва то создаем
                if (explosion.DownParticleEffect == null)
                {
                    explosion.DownParticleEffect = Particle3dCTRL.CreateBoomRocket(myField.transform, this);
                    explosion.DownParticleEffect.SetTransformSpeed(1 / explosion.timer);
                    //Particle3dCTRL.CreateBoomBomb(myField.gameObject, this, 1);
                }

                explosion.DownParticleEffect.SetTransformTarget(new Vector2(pos.x + 0.5f, pos.y + 0.5f - 15));
            }
        }

        //Взрыв выполнен
        explosion = null;

    }
    public void ExplosionBoomInvoke(Explosion explosionNew, float invokeTime) {
        explosion = explosionNew;
        Invoke("ExplosionBoom", invokeTime);
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
        bool test = false;
        if (myField.testCell.x == pos.x && myField.testCell.y == pos.y) {
            test = true;
        }


        if (Time.unscaledTime - myField.timeLastBoom > 0.2f)
        {
            myField.timeLastBoom = Time.unscaledTime;
        }
        else {
            //Добавляем время взрыва если его мало
            if(Time.unscaledTime - myField.timeLastBoom < 2.5f)
                myField.timeLastBoom += 0.025f;
        }


        bool CanInternal = false;
        bool canNear = false;
        bool CanBenefit = false;

        //Если есть камень урон камню
        if (rock > 0) {
            rock--;
            CanBenefit = true;
        }
        //иначе если есть ящик урон ящику
        else if (BlockingMove > 0) {
            BlockingMove--;
            CanBenefit = true;
        }
        //иначе если есть лед урон льду и объектам внутри и рядом
        else if (ice > 0) {
            ice--;
            CanBenefit = true;
            canNear = true;
            CanInternal = true;
        }
        //Иначе если плесень урон плесени и объектам внутри и рядом
        else if (mold > 0) {
            mold--;
            CanBenefit = true;
            canNear = true;
            CanInternal = true;
        }
        //иначе если панель ставим панель и урон объектам рядом и снаружи
        else if (!panel && combination != null && combination.foundPanel) {
            CreatePanel();

            CanBenefit = true;
            canNear = true;
            CanInternal = true;
        }
        else {
            canNear = true;
            CanInternal = true;
        }



        //////////////////////////
        //Урон по внутренней
        if (CanInternal && cellInternal) {
            cellInternal.Activate(cellInternal.type, partner, combination);
        }

        //////////////////////////
        //Урон по ближайшим
        if (canNear && nearDamage) {
            bool nearBenefit = DamageNearCells();
            if (!CanBenefit) 
                CanBenefit = nearBenefit;
        }

        //////////////////////////
        //Ставим бользу
        if (CanBenefit && combination != null) {
            combination.foundBenefit = true;
        }

        /*
        //Если камня нету
        if (rock <= 0)
        {
            //Если ящика нету
            if (BlockingMove <= 0)
            {

                //Если нет плесени
                if (mold <= 0)
                {
                    if (!panel && combination != null && combination.foundPanel)
                    {
                        CreatePanel();
                        combination.foundBenefit = true;
                    }
                }
                else
                {
                    mold--;
                }

                //Если есть внутренность
                if (cellInternal)
                {

                    cellInternal.Activate(cellInternal.type, partner, combination);
                }

                //наносим соседним ячейкам урон по блокираторам движения
                if (nearDamage)
                {
                    bool benefit = DamageNearCells();
                    if (combination != null && benefit) {
                        combination.foundBenefit = benefit;
                    }
                }
            }
            else
            {
                //Самому себе
                bool benefit = DamageNear();
                if (combination != null && benefit)
                {
                    combination.foundBenefit = benefit;
                }
            }
        }
        else {
            rock--;
        }
        */

        //Создаем панель если плесени нет и нужно создать панель
        if (combination != null && BlockingMove <= 0 && mold <= 0 && combination.foundPanel && !panel) {
            CreatePanel();
        }

        //Обновляем время последнего действия комбинации
        if (combination != null) {
            combination.timeLastAction = Time.unscaledTime;
        }


        //Перенасчет приоритета
        CalcMyPriority();


        bool DamageNearCells() {

            bool benefit = false;
            void setBenefit(bool benefitNew) {
                if (!benefit && benefitNew)
                    benefit = benefitNew;
            }

            //Слева
            if (pos.x - 1 >= 0 && 
                myField.cellCTRLs[pos.x - 1, pos.y]) {
                setBenefit(myField.cellCTRLs[pos.x - 1, pos.y].DamageNear());
            }
            //Справа
            if (pos.x + 1 < myField.cellCTRLs.GetLength(0) && 
                myField.cellCTRLs[pos.x + 1, pos.y]) {
                setBenefit(myField.cellCTRLs[pos.x + 1, pos.y].DamageNear());
            }
            //снизу
            if (pos.y - 1 >= 0 &&
                myField.cellCTRLs[pos.x, pos.y - 1]) {
                setBenefit(myField.cellCTRLs[pos.x, pos.y - 1].DamageNear());
            }
            //Сверху
            if (pos.y + 1 < myField.cellCTRLs.GetLength(1) && 
                myField.cellCTRLs[pos.x, pos.y + 1]) {
                setBenefit(myField.cellCTRLs[pos.x, pos.y + 1].DamageNear());
            }

            return benefit;
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

    public bool DamageNear() {

        bool benefit = false;

        bool test = false;
        if (myField.testCell.x == pos.x && myField.testCell.y == pos.y) {
            test = true;
        }

        //Нанести урон ящику
        if (BlockingMove > 0)
        {
            BlockingMove--;
            benefit = true;
        }
        else if (cellInternal != null && cellInternal.type == CellInternalObject.Type.blocker) {
            if (rock > 0)
            {
                rock--;
            }
            else {
                //Урон по внутреннему объекту блокиратору
                cellInternal.Activate(CellInternalObject.Type.blocker, null, null);
            }
        }
        

        //Перенасчет приоритета
        CalcMyPriority();

        return benefit;
    }


    //Удалить внутренность ячейки без спецэфетов, урона и подсчета очков
    public void DeleteInternalNoDamage() {
        if (cellInternal != null) {
            Destroy(cellInternal.gameObject);
        }
        cellInternal = null;
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

        const int PriorityBox = 20; //Приоритет ящика
        const int PriorityRock = 10;
        const int PriorityMold = 5; //приоритет плесени
        const int PriorityIce = 5;
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
            //Камня
            if (rock > 0) {
                result += (5 - rock) * PriorityRock; //Чем меньше жизней, тем желательнее ее сломать

                //проверка соседей
                //Слева
                if (pos.x - 1 >= 0 &&
                    myField.cellCTRLs[pos.x - 1, pos.y] != null &&
                    myField.cellCTRLs[pos.x - 1, pos.y].rock == 0)
                {
                    result += 5;
                }
                //справа
                if (pos.x + 1 < myField.cellCTRLs.GetLength(0) &&
                    myField.cellCTRLs[pos.x + 1, pos.y] != null &&
                    myField.cellCTRLs[pos.x + 1, pos.y].rock == 0)
                {
                    result += 5;
                }
                //сверху
                if (pos.y + 1 < myField.cellCTRLs.GetLength(1) &&
                    myField.cellCTRLs[pos.x, pos.y + 1] != null &&
                    myField.cellCTRLs[pos.x, pos.y + 1].rock == 0)
                {
                    result += 5;
                }
                //снизу
                if (pos.y - 1 >= 0 &&
                    myField.cellCTRLs[pos.x, pos.y - 1] != null &&
                    myField.cellCTRLs[pos.x, pos.y - 1].rock == 0)
                {
                    result += 5;
                }
            }
            //Плесени
            if (mold > 0)
            {
                result += (5 - mold) * PriorityMold; //Чем меньше жизней, тем желательнее ее сломать
            }
            //Лед
            if (ice > 0) {
                result += ice * PriorityIce; //Чем больше жизней тем желательнее сломать
            }
            //отсутствия панели
            if (!panel)
            {
                result += PriorityPanel;
            }

            /////////////////////////////////////////////
            //Умножаем за цель миссии на 2.5
            //ящики
            if (BlockingMove > 0 && LevelsScript.main.ReturnLevel().PassedWithBox)
            {
                result *= 3;
            }
            //панель
            if (rock > 0 && LevelsScript.main.ReturnLevel().PassedWithRock)
            {
                result *= 3;
            }
            //mold
            if (mold > 0 && LevelsScript.main.ReturnLevel().PassedWithMold)
            {
                result *= 3;
            }
            //панель
            if (!panel && LevelsScript.main.ReturnLevel().PassedWithPanel) { 
                result *= 3;            
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
        //if (cellInternal && !cellInternal.isMove) {

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
        //}
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
