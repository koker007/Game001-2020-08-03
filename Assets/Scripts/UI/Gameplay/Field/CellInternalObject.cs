using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
//Андрей
/// <summary>
/// Врутренняя перемещаемая часть ячеек
/// </summary>
public class CellInternalObject : MonoBehaviour
{
    [SerializeField]
    public RectMask2D myMask2D;
    [SerializeField]
    public Vector4 myMaskNeed = new Vector4();
    public Vector4 myMaskNow = new Vector4();
    [SerializeField]
    public AnimatorCTRL animatorCTRL;
    private Particle3dCTRL trail;
    private bool trailSpawned = false;

    //мое поле
    public GameFieldCTRL myField;
    //Моя ячейка
    public CellCTRL myCell;
    public Vector2Int myDeath = new Vector2Int(999,999); //позиция для самоуничтожения, активно если отличается от значений по умолчанию
    public int MyCombID = 0;

    public RectTransform rectMy;
    RectTransform rectCell;

    [SerializeField]
    RawImage Image;
    [SerializeField]
    RawImage LastImage;
    [SerializeField]
    RawImage CoreImage;

    [SerializeField]
    Color red = Color.red;
    [SerializeField]
    Color green = Color.green;
    [SerializeField]
    Color blue = Color.blue;
    [SerializeField]
    Color yellow = Color.yellow;
    [SerializeField]
    Color violet = Color.white;

    [SerializeField]
    Color Ultimative = Color.white;


    [SerializeField]
    Texture2D TextureRed;
    [SerializeField]
    Texture2D TextureRedCore;
    [SerializeField]
    Texture2D TextureGreen;
    [SerializeField]
    Texture2D TextureGreenCore;
    [SerializeField]
    Texture2D TextureBlue;
    [SerializeField]
    Texture2D TextureBlueCore;
    [SerializeField]
    Texture2D TextureYellow;
    [SerializeField]
    Texture2D TextureYellowCore;
    [SerializeField]
    Texture2D TextureViolet;
    [SerializeField]
    Texture2D TextureVioletCore;
    [SerializeField]
    Texture2D TextureColor5;
    [SerializeField]
    Texture2D TextureColor5Core;
    [SerializeField]
    Texture2D TextureUltimative;
    [SerializeField]
    Texture2D TextureUltimativeCore;
    [SerializeField]
    Texture2D TextureBomb;
    [SerializeField]
    Texture2D TextureBombCore;
    [SerializeField]
    Texture2D TextureFly;
    [SerializeField]
    Texture2D TextureFlyLast;
    [SerializeField]
    Texture2D TextureBlocker;

    [SerializeField]
    Texture2D TextureRocketHorizontal;
    [SerializeField]
    Texture2D TextureRocketVertical;

    [Header("Prefabs")]
    [SerializeField]
    GameObject FlyPrefab;

    public enum InternalColor {
        Red,
        Green,
        Blue,
        Yellow,
        Violet,
        Ultimate
    }
    public enum Type {
        none,
        color,
        color5,
        rocketHorizontal,
        rocketVertical,
        bomb,
        airplane,
        blocker //Объект который падает и препятствует распространению ракеты, но ломается как ящик
    }

    
    public enum MaskType{
        top,
        bot,
        left,
        right
    }

    [Header("Other")]
    public InternalColor color;
    public Type type;



    public Color GetColor(InternalColor internalColor) {
        Color result = new Color(1,1,1);

        if (internalColor == InternalColor.Red) {
            result = red;
        }
        else if (internalColor == InternalColor.Green) {
            result = green;
        }
        else if (internalColor == InternalColor.Blue) {
            result = blue;
        }
        else if (internalColor == InternalColor.Yellow) {
            result = yellow;
        }
        else if (internalColor == InternalColor.Violet) {
            result = violet;
        }

        return result;
    }


    //Замаскировать внутренний объект от туда, от куда он будет двигаться
    public void SetFullMask2D(MaskType maskType) {
        if (myMask2D == null) myMask2D = gameObject.AddComponent<RectMask2D>();

        Vector4 mask2D = myMask2D.padding;

        //Пропадает сверху
        if (maskType == MaskType.top) {
            mask2D.w = 100;
        }
        //Пропадает слева
        else if (maskType == MaskType.left) {
            mask2D.x = 100;
        }
        //Пропадает справа
        else if (maskType == MaskType.right) {
            mask2D.z = 100;
        }
        //пропадает снизу
        else if (maskType == MaskType.bot) {
            mask2D.y = 100;
        }
        myMask2D.padding = mask2D;
    }

    public void IniRect() {
        rectMy = GetComponent<RectTransform>();

        if (myCell)
            rectCell = myCell.GetComponent<RectTransform>();
    }

    void Start()
    {
        timeCreate = Time.unscaledTime;
        IniRect();
    }

    void Update()
    {
        Moving();
        UpdateActivate();
    }


    public float timeCreate = 0;
    public float timeLastMoving = 0;

    static public float timeLastPlayDestroy = 0; //Как давно был воиспроизведен звук уничтожения

    public float MovingSpeed = 0;

    public bool EndMoveAndRemove = false; //Как только движение объекта закончится объект тихо самоуничтожится. без нанесения чему либо урона
    void Moving() {
        //Движение к соседу

        //если у внутреннего объекта нет ячейки к которой она привязанна это еще не ошибка
        //возможно она движется в точку самоуничтожения

        if (isMove) {
            if ((trail == null || !trailSpawned) && myCell != null && (GameFieldCTRL.main.CellSelect == myCell || GameFieldCTRL.main.CellSwap == myCell))
            {
                trail = Particle3dCTRL.CreateTrail(myCell.myField.transform, myCell);
                trailSpawned = true;
            }

            if (trail != null)
            {
                trail.gameObject.transform.localPosition = new Vector3(
                (myCell.myField.transform.GetComponent<RectTransform>().pivot.x - 0.5f) - rectMy.pivot.x + 0.5f,
                (myCell.myField.transform.GetComponent<RectTransform>().pivot.y - 0.5f) - rectMy.pivot.y + 0.5f, 0);
            }


            MovingSpeed += Time.unscaledDeltaTime * 1.5f;

            float speed = 0.1f + MovingSpeed;
            speed *= Gameplay.main.timeScale;

            float correctY = 0;
            //если обьект сверху находится близко и у него скорость падения быстрее
            if (myCell != null &&
                myCell.pos.y < myField.cellCTRLs.GetLength(1) - 1 && 
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y+1] &&
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y+1].cellInternal &&
                Vector2.Distance(
                    new Vector2(myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.x, myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.y), 
                    new Vector2(rectMy.pivot.x, rectMy.pivot.y)) < 1 &&
                MovingSpeed < myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.MovingSpeed) {
                //Ускоряем
                MovingSpeed = myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.MovingSpeed;

                //перемещаем
                correctY = 1 - Vector2.Distance(
                    new Vector2(myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.x, myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.y),
                    new Vector2(rectMy.pivot.x, rectMy.pivot.y));
            }


            float posXnew = rectMy.pivot.x;
            float posYnew = rectMy.pivot.y;
            //Если есть позиция смерти
            if (myDeath.x < 100 || myDeath.y < 100)
            {
                posXnew = Calculate.GetValueLinear(rectMy.pivot.x, myDeath.x, speed);
                posYnew = Calculate.GetValueLinear(rectMy.pivot.y, myDeath.y, speed);
            }
            //Самое обычное движение к привязанной ячейке
            else
            {
                posXnew = Calculate.GetValueLinear(rectMy.pivot.x, rectCell.pivot.x, speed);
                posYnew = Calculate.GetValueLinear(rectMy.pivot.y, rectCell.pivot.y, speed);
            }

            if (myDeath.y < 100 || myDeath.x < 100) {
                float test = 0;
            }


            if (myMaskNow != myMaskNeed || myMaskNow.x != 0 || myMaskNow.y != 0 || myMaskNow.z != 0 || myMaskNow.w != 0) {
                if (myMask2D == null) myMask2D = gameObject.AddComponent<RectMask2D>();

                //Смещение маски
                float maskCoof = 100;

                Vector4 mask2D = myMask2D.padding;

                mask2D.x = Calculate.GetValueLinear(mask2D.x, myMaskNeed.x, speed * maskCoof);
                mask2D.y = Calculate.GetValueLinear(mask2D.y, myMaskNeed.y, speed * maskCoof);
                mask2D.z = Calculate.GetValueLinear(mask2D.z, myMaskNeed.z, speed * maskCoof);
                mask2D.w = Calculate.GetValueLinear(mask2D.w, myMaskNeed.w, speed * maskCoof);

                //Применение новой маски
                myMask2D.padding = mask2D;
                myMaskNow = mask2D;

                if (myMask2D.padding.x == 0 && myMask2D.padding.y == 0 && myMask2D.padding.z == 0 && myMask2D.padding.w == 0) {
                    Destroy(myMask2D);
                }
            }

            if (EndMoveAndRemove) {
                if (myDeath.x == posXnew &&
                    myDeath.y == posYnew) {

                        DestroyObj(true);
                        return;

                }
            }

            //Если позиция равна точке назначения
            else if (rectCell.pivot.x == posXnew &&
                rectCell.pivot.y == posYnew) {

                //Если снизу ничего нет
                CellCTRL cellMove = GetFreeCellDown();
                if (cellMove)
                {
                    //Установить новую цель для движения
                    StartMove(cellMove);
                }
                else
                {
                    //Движение окончено
                    isMove = false;
                    myCell.CalcMyPriority();
                    //Запустить анимацию остановки
                    animatorCTRL.PlayAnimation("DroppedDown");
                }
            }

            //Присваиваем изменения
            rectMy.pivot = new Vector2(posXnew, posYnew);

            //Обновляем время последнего перемещения
            timeLastMoving = Time.unscaledTime;
        }

        //Проверяем снизу наличие свободной ячейки //Расчет происходит только в кадре для расчета
        else if(myField.buffer.CalculateFrameNow == 0) {
            
            trailSpawned = false;
            CellCTRL cellMove = GetFreeCellDown();
            if (cellMove)
                StartMove(cellMove);
        }
        
    }

    /// <summary>
    /// Получить свободную ячейку снизу
    /// </summary>
    CellCTRL GetFreeCellDown() {
        CellCTRL returnCell = null;

        //выходим если предмет в этой ячейке находится в камне
        if (myField.cellCTRLs[myCell.pos.x, myCell.pos.y].rock > 0 || myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID == 15)
            return null;


        //Получить свободную ячейку снизу
        for (int minusY = 1; minusY < myField.cellCTRLs.GetLength(1); minusY++) {
            if (myCell.pos.y - minusY >= 0 && //если не вышли за массив
                myField.CheckObstaclesToMoveDown(myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY], myField.cellCTRLs[myCell.pos.x, myCell.pos.y - (minusY - 1)]) &&
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].cellInternal && //И она свободна
                Time.unscaledTime - myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].timeBoomOld > 0.35f && //c уничтожения ячеек снизу
                Time.unscaledTime - myField.timeLastBoom > 0)
                
            {
                //Ставим такую ячейку как нижнюю
                returnCell = myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY];
            }
            //Дальше таких ячеек нет
            else {
                //Выходим
                break;
            }
        }

        //Если ячеки снизу нет свободных
        //Получить свободную ячейку справа или слева ниже
        if (returnCell == null) {
            //Проверяем относительно высоты

            /*
            bool canTestingRight = true;
            bool canTestingLeft = true;
            */

            //Общая проверка по высоте
            int smeshenie = 1;
            if (myCell.pos.y - smeshenie >= 0) //если не вышли за массив
            {

                //Если перемещение четное, то провека справа
                //int internalNum = CellCTRL.GetNowLastInternalNum;
                //Справа
                if (myCell.pos.x + smeshenie < myField.cellCTRLs.GetLength(0) && //если не вышли за массив
                    GameFieldCTRL.main.CheckObstaclesToMoveUp(myField.cellCTRLs[myCell.pos.x, myCell.pos.y], myField.cellCTRLs[myCell.pos.x, myCell.pos.y - smeshenie]) &&
                    GameFieldCTRL.main.CheckObstaclesToMoveDown(myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie], myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y]) &&
                    !myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].cellInternal && //И она свободна
                    Time.unscaledTime - myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].timeBoomOld > 0.35f &&
                    Time.unscaledTime - myField.timeLastBoom > 0 &&
                    isCanMoveToThisColum(myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie]) &&//В этом столбце нет потенциального вертикального движения
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].wallID != 8 &&
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].wallID != 12 &&
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].wallID != 13 &&
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].wallID != 15 &&
                    myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID != 6 &&
                    myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID != 11 &&
                    myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID != 14 &&
                    myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID != 15)
                {
                    //Ставим такую ячейку как целевую
                    returnCell = myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie];
                }
                //Слева
                else if (myCell.pos.x - smeshenie >= 0 && //если не вышли за массив
                    GameFieldCTRL.main.CheckObstaclesToMoveUp(myField.cellCTRLs[myCell.pos.x, myCell.pos.y], myField.cellCTRLs[myCell.pos.x, myCell.pos.y - smeshenie]) &&
                    GameFieldCTRL.main.CheckObstaclesToMoveDown(myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie], myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y]) &&
                    !myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].cellInternal && //И она свободна
                    Time.unscaledTime - myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].timeBoomOld > 0.35f &&
                    Time.unscaledTime - myField.timeLastBoom > 0 &&
                    isCanMoveToThisColum(myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie]) && //В этом столбце нет потенциального вертикального движения
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].wallID != 5 &&
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].wallID != 13 &&
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].wallID != 14 &&
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].wallID != 15 &&
                    myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID != 7 &&
                    myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID != 11 &&
                    myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID != 12 &&
                    myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID != 15)
                {
                    //Ставим такую ячейку как целевую
                    returnCell = myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie];
                }
            }
        }

        return returnCell;
        //test
        //Проверка можно ли двигаться боком в эту 
        bool isCanMoveToThisColum(CellCTRL cellFunc) {
            bool result = true; //Изначально двигаться разрешено

            //Проверяем низ на то что никто не движется в ячейки снизу
            for (int minus = 1; minus < myField.cellCTRLs.GetLength(1) && result; minus++) {
                if (cellFunc.pos.y - minus >= 0 &&//Если не вышли за пределы массива
                    GameFieldCTRL.main.CheckObstaclesToMoveDown(myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus], myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - (minus - 1)]) &&
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus] && //есть ячейка
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].BlockingMove <= 0 &&//ячейка находится без блокировки движения
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].rock <= 0)
                {
                    //Блокируем если
                    if (
                        (myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].cellInternal && myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].cellInternal.isMove && minus <= 1) || // Внутри есть ячейка которая находится в движении
                        Time.unscaledTime - myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].timeBoomOld < 0.25f//Время ожидания после взрыва не вышло
                        ) 
                    {
                        result = false;
                    }

                }
                else {
                    break;
                }
            }

            //Проверка сверху
            for (int plus = 1; plus < myField.cellCTRLs.GetLength(1) && result; plus++) {
                //Проверяем верх на то что нету предметов которые могли бы упасть
                if (cellFunc.pos.y + plus < myField.cellCTRLs.GetLength(1) &&//Если не вышли за пределы массива
                    GameFieldCTRL.main.CheckObstaclesToMoveUp(myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus], myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + (plus - 1)]) &&
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus] && //есть ячейка
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].BlockingMove <= 0)//ячейка находится без блокировки движения
                    
                {
                    //Блокируем если
                    if (
                        (myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].cellInternal) || // Внутри есть ячейка Которая вероятно скоро начнет движение
                        Time.unscaledTime - myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].timeBoomOld < 0.25f //Время ожидания после взрыва не вышло
                        )
                    {
                        result = false;
                    }
                }
                else {
                    break;
                }
            }


            return result;
        }
    }

    /// <summary>
    /// Находится ли в движении обьект сейчас
    /// </summary>
    public bool isMove = false;
    /// <summary>
    /// Движение к выбранной ячейке
    /// </summary>
    public void StartMove(CellCTRL cellNew) {
        if (myCell)
        {
            myCell.cellInternal = null;
        }

        myCell = cellNew;

        if (!isMove)
        {
            cellNew.myInternalNum = cellNew.GetNextLastInternalNum;
            MovingSpeed = 0;
            isMove = true;
        }

        //присваиваем ячейке обьект
        myCell.cellInternal = this;
        myCell.myInternalNum = myCell.GetNextLastInternalNum; //Запоминаем действие

        IniRect();
    }
    public void EndMove() {
        if (!myCell) {
            return;
        }

        IniRect();

        rectMy.pivot = rectCell.pivot;
        isMove = false;
    }

    /// <summary>
    /// Удалить объект
    /// </summary>
    public void DestroyObj() {
        int score = 100 + 10 * (myField.ComboCount-1);

        myField.ComboInternal++;

        AddCountColor();
        SpawnEffects();

        if (gameObject)
            Destroy(gameObject);

        SoundCTRL.main.PlaySoundDestroyInvoke();
        
        

        void SpawnEffects() {
            myCell.timeBoomOld = Time.unscaledTime; //Ставим время взрыва

            int scoreNow = score;
            if (!Gameplay.main.playerTurn) {
                //Враг получает вдвое больше очков в начале игры 
                if (EnemyController.MoveCount <= EnemyController.MoveCountForPlayer)
                {
                    scoreNow = scoreNow * 2;
                }
                else {
                    scoreNow = (int)(scoreNow * 0.62f);
                }
            }

            if (type == Type.color) {
                GameObject PrefabDie = Instantiate(myField.PrefabParticleDie, myField.parentOfScore);
                RectTransform rectDie = PrefabDie.GetComponent<RectTransform>();

                GameObject PrefabScore = Instantiate(myField.PrefabParticleScore, myField.parentOfScore);
                PrefabScore.GetComponent<ScoreCTRL>().Inicialize(scoreNow, ConvertEnumColor());
                Gameplay.main.ScoreUpdate(scoreNow);
                RectTransform rectScore = PrefabScore.GetComponent<RectTransform>();

                //Нарисовать частицы
                Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateCellDamage(myField.transform, myCell);
                Color color1 = GetColor(color);
                Vector3 colorVec = new Vector3(color1.r, color1.g, color1.b);
                colorVec.Normalize();
                color1 = new Color(color1.r, color1.g, color1.b);
                particle3DCTRL.SetColor(color1);

                //Изменить количество очков
                rectDie.pivot = rectMy.pivot;
                rectScore.pivot = rectMy.pivot;
            }
            
            if (type == Type.blocker) {
                Particle3dCTRL.CreateDestroyBlocker(myField.transform, myCell);
            }
        }

        void AddCountColor() {
            Gameplay.main.colorsCount[(int)color]++;
        }

    }
    public void DestroyObj(bool silent)
    {
        if (silent) {
            SilendRemove();
        }
        //Обычное уничтожение
        else {
            DestroyObj();
        }

        //Уничтожение по тихому без нанесения урона и последствий
        void SilendRemove() {
            Destroy(gameObject);
        }
    }

    public void randSpawnType(bool isSpawn, bool isRandType = true) {
        CellInternalObject cellInternal = GetRandomColor(isSpawn);
        color = cellInternal.color;

        if (isRandType) {
            type = cellInternal.type;
            setColorAndType(color, type);
        }
        else {
            setColor(color);
        }
    }
    public void randType() {
        int rand = Random.Range(1, 6+1);

        if (rand == 1)
        {
            type = Type.color;
        }
        else if (rand == 2)
        {
            type = Type.color5;
        }
        else if (rand == 3)
        {
            type = Type.rocketHorizontal;
        }
        else if (rand == 4) {
            type = Type.rocketVertical;
        }
        else if (rand == 5) {
            type = Type.bomb;
        }
        else if (rand == 6) {
            type = Type.airplane;
        } 
    }
    public void setColor(InternalColor internalColor) {
        if (animatorCTRL != null) {
            animatorCTRL.SetFloat("InternalColor", (float)color);
        }

        Image.color = new Color(1, 1, 1, 1);

        if (type == Type.color)
        {
            animatorCTRL.SetFloat("StayType", 1);
            LastImage.texture = null;
            LastImage.color = new Color(0,0,0,0);

            if (internalColor == InternalColor.Red)
            {
                Image.texture = TextureRed;
                CoreImage.texture =TextureRedCore;
                LastImage.texture = null;
                //Image.color = red;
            }
            else if (internalColor == InternalColor.Green)
            {
                Image.texture = TextureGreen;
                CoreImage.texture = TextureGreenCore;
                LastImage.texture = null;
                //Image.color = green;
            }
            else if (internalColor == InternalColor.Blue)
            {
                Image.texture = TextureBlue;
                CoreImage.texture = TextureBlueCore;
                LastImage.texture = null;
                //Image.color = blue;
            }
            else if (internalColor == InternalColor.Yellow)
            {
                Image.texture = TextureYellow;
                CoreImage.texture = TextureYellowCore;
                LastImage.texture = null;
                //Image.color = yellow;
            }
            else if (internalColor == InternalColor.Violet)
            {
                Image.texture = TextureViolet;
                CoreImage.texture = TextureVioletCore;
                LastImage.texture = null;
                //Image.color = violet;
            }
            else if (internalColor == InternalColor.Ultimate) {
                Image.texture = TextureUltimative;
                CoreImage.texture = TextureUltimativeCore;
                LastImage.texture = null;
                //Image.color = Ultimative;
            }

        }

        if (type == Type.airplane) {
            Image.texture = TextureFly;
            setInternalColor();
            LastImage.texture = TextureFlyLast;

            CoreImage.texture = null;
        }
        else if (type == Type.bomb)
        {
            Image.texture = TextureBomb;
            setInternalColor();
            LastImage.texture = null;

            CoreImage.texture = null;
        }
        else if (type == Type.rocketHorizontal)
        {
            Image.texture = TextureRocketHorizontal;
            setInternalColor();
            LastImage.texture = null;

            CoreImage.texture = null;
        }
        else if (type == Type.rocketVertical)
        {
            Image.texture = TextureRocketVertical;
            setInternalColor();

            LastImage.texture = null;

            CoreImage.texture = null;
        }
        else if (type == Type.color5)
        {
            animatorCTRL.SetFloat("StayType", 2);
            Image.texture = TextureColor5;
            Image.color = new Color(1, 1, 1, 1);
            LastImage.texture = null;

            CoreImage.texture = TextureColor5Core;
        }
        else if (type == Type.blocker) {
            Image.texture = TextureBlocker;
            Image.color = new Color(1, 1, 1, 1);

            LastImage.texture = null;
            CoreImage.texture = null;
        }

        color = internalColor;

        if (CoreImage.texture == null)
        {
            CoreImage.gameObject.SetActive(false);
        }
        else {
            CoreImage.gameObject.SetActive(true);
        }

        if (LastImage.texture == null) {
            LastImage.gameObject.SetActive(false);
        }
        else {
            LastImage.gameObject.SetActive(true);
        }

        void setInternalColor() {
            if (internalColor == InternalColor.Red)
            {
                Image.color = red;
            }
            else if (internalColor == InternalColor.Green)
            {
                Image.color = green;
            }
            else if (internalColor == InternalColor.Blue)
            {
                Image.color = blue;
            }
            else if (internalColor == InternalColor.Yellow)
            {
                Image.color = yellow;
            }
            else if (internalColor == InternalColor.Violet)
            {
                Image.color = violet;
            }
        }
    }
    public void setColorAndType(InternalColor internalColor, Type typeNew) {
        type = typeNew;
        setColor(internalColor);
    }

    public CellInternalObject GetRandomColor(bool isSpawn)
    {
        CellInternalObject Result = new CellInternalObject();

        Result.color = InternalColor.Red;
        Result.type = Type.color;


        int random = Random.Range(0, Gameplay.main.colors + 2);

        //Шанс супер цвета
        if (random == 0)
        {
            //Если выпал шанс заспавнить ультимативный цвет то спавним
            if (Random.Range(0, 100) < Gameplay.main.superColorPercent)
            {
                Result.color = InternalColor.Ultimate;
            }
            //Иначе обычный
            else
            {
                Result.color = GetColorBasic();
            }
        }
        //Шанс завпавнить блокиратор
        else if (random == 1 && isSpawn) {
            //Если выпал шанс заспавнить блокиратор
            if (Random.Range(0, 100) < Gameplay.main.typeBlockerPercent) {
                Result.color = InternalColor.Red;
                Result.type = Type.blocker;
            }
            //Иначе обычный
            else {
                Result.color = GetColorBasic();
            }
        }

        //Иначе обычный цвет
        else {
            Result.color = GetColorBasic();
        }


        

        return Result;


        //Получить любой базовый цвет
        InternalColor GetColorBasic() {
            InternalColor colorReturn = InternalColor.Red;

            int random = Random.Range(0, Gameplay.main.colors);
            //Супе
            if (random == 0)
            {
                colorReturn = InternalColor.Red;
            }
            else if (random == 1)
            {
                colorReturn = InternalColor.Green;
            }
            else if (random == 2)
            {
                colorReturn = InternalColor.Blue;
            }
            else if (random == 3)
            {
                colorReturn = InternalColor.Yellow;
            }
            else if (random == 4)
            {
                colorReturn = InternalColor.Violet;
            }




            return colorReturn;

        }
    }

    public Color ConvertEnumColor()
    {
        switch (color)
        {
            case InternalColor.Blue:
                return Color.blue;
            case InternalColor.Green:
                return Color.green;
            case InternalColor.Red:
                return Color.red;
            case InternalColor.Violet:
                return Color.white;
            case InternalColor.Yellow:
                return Color.yellow;
            default:
                return Color.black;
        }
    }

    public void PosToCell() {
        IniRect();

        rectMy.pivot = rectCell.pivot;
    }

    //Активируем врутренность ячейки
    bool activate = false;
    public int activateNum = 1;
    public int activateCount = 0;
    public bool activateNeed = false;

    public bool needInstantDamage = true;
    public int BoombRadius = 1;


    public Type BufferActivateType;
    public CellInternalObject BufferPartner = null;
    public GameFieldCTRL.Combination BufferCombination = null;
    public void Activate(Type ActivateType, CellInternalObject partner, GameFieldCTRL.Combination combination) {

        //Активация срабатывает только если это не таже самая комбинация которой был созданн этот объект
        if (combination != null &&
            combination.ID == MyCombID &&
            !isMove //если объект не в движении
            ) return;

        //Бомба должна активироваться только когда упала, с низу не должно быть свободных ячеек
        if (activateNeed && type == Type.bomb && myCell.rock == 0 &&
            myCell.pos.y > 0 && 
            myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1] != null &&
            myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1].cellInternal == null &&
            myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1].BlockingMove == 0 &&
            myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1].rock == 0 &&
            myField.CheckObstaclesToMoveDown(myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1], myField.cellCTRLs[myCell.pos.x, myCell.pos.y])
            ) {
            return;
        }

        activateNeed = true;
        activateCount++;
        if (activateCount == 1 && ActivateType == Type.bomb &&
            (partner == null || (partner.type != Type.rocketHorizontal && partner.type != Type.rocketVertical))) {
            activateNum = 2;
        }

        BufferActivateType = ActivateType;
        BufferPartner = partner;
        BufferCombination = combination;

        //Если комбинации нет, то создаем
        if (BufferCombination == null)
        {
            BufferCombination = new GameFieldCTRL.Combination();
            //добавляем в ячейку комбинацию
            BufferCombination.cells.Add(myCell);
            //добавляем партнера если он есть
            if (partner != null)
                BufferCombination.cells.Add(partner.myCell);

            BufferCombination.TestCells();
        }

        if (!needInstantDamage)
        {
            if (isMove || Time.unscaledTime - timeLastMoving < 1f) return;
        }
        //Отключаем мнгновенный урон
        needInstantDamage = false;
        timeLastMoving = Time.unscaledTime;


        //if (ActivateType == Type.color) return;

        //Если партнера нет
        if (partner == null)
        {
            if (ActivateType == Type.bomb) ActivateBomb(BoombRadius);
            else if (ActivateType == Type.rocketHorizontal) ActivateRocket(true, false);
            else if (ActivateType == Type.rocketVertical) ActivateRocket(false, true);
            else if (ActivateType == Type.color5) ActivateSuperColor();
            else if (ActivateType == Type.airplane) ActivateFly();
        }
        else {
            //супер колор + что угодно
            if (ActivateType == Type.color5)
            {
                ActivateSuperColor();
            }
            //Бомба + ракета
            else if ((ActivateType == Type.bomb && partner.type == Type.rocketHorizontal) ||
                (ActivateType == Type.rocketHorizontal && partner.type == Type.bomb) ||
                (ActivateType == Type.bomb && partner.type == Type.rocketVertical) ||
                (ActivateType == Type.rocketVertical && partner.type == Type.bomb))
            {
                ActivateBombAndRocket();
            }
            //Бомба + бомба
            else if (ActivateType == Type.bomb && partner.type == Type.bomb)
            {
                ActivateBomb(2);
            }
            //ракета + ракета
            else if ((ActivateType == Type.rocketHorizontal || ActivateType == Type.rocketVertical) &&
                (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical))
            {
                ActivateRocket(true, true);
            }

            //Самолет + ???
            else if (ActivateType == Type.airplane)
            {
                ActivateFly();
            }

            //Бомба + самолет
            else if (ActivateType == Type.bomb && partner.type == Type.airplane)
            {
                partner.type = Type.bomb;
                ActivateFly();
            }

            //Если партнер супер цвет
            //Эта активация в основном через кнопки магазина
            else if (partner.type == Type.color5) {
                if (ActivateType == Type.rocketHorizontal || ActivateType == Type.rocketVertical ||
                    ActivateType == Type.bomb) {

                    ActivateSuperColor();
                }
            }
        }

        activateNum--;
        if (activateNum <= 0) {
            
            DestroyObj();
        }



        void ActivateRocket(bool horizontal, bool vertical)
        {
            //Debug.Log("ActivateRocket");
            //Активируем только если бомба еще не активна
            if (activate) return;
            
            //Если совмещаем с другой ракетой
            if (partner && partner != this && (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical)) {
                horizontal = true;
                vertical = true;

                //Удаляем партнера
                partner.activate = false;
                partner.DestroyObj();
            }

            //во все стороны
            if (horizontal && vertical) {
                if (myCell.explosion == null)
                {
                    myCell.explosion = new CellCTRL.Explosion(true, true, true, true, 0.05f, BufferCombination);
                }
                else {
                    myCell.explosion.left = true;
                    myCell.explosion.right = true;
                    myCell.explosion.up = true;
                    myCell.explosion.down = true;
                }

                myCell.BufferCombination = combination;
                myCell.BufferNearDamage = false;
                myCell.ExplosionBoomInvoke(myCell.explosion, 0);
            }
            //горизонтальный
            else if (horizontal) {

                if (myCell.explosion == null)
                {
                    myCell.explosion = new CellCTRL.Explosion(true, true, false, false, 0.05f, BufferCombination);
                }
                else
                {
                    myCell.explosion.left = true;
                    myCell.explosion.right = true;
                }

                myCell.BufferCombination = combination;
                myCell.BufferNearDamage = false;
                myCell.ExplosionBoomInvoke(myCell.explosion, 0);
            }
            //вертикальный
            else if (vertical) {

                if (myCell.explosion == null)
                {
                    myCell.explosion = new CellCTRL.Explosion(false, false, true, true, 0.05f, BufferCombination);
                }
                else
                {
                    myCell.explosion.up = true;
                    myCell.explosion.down = true;
                }

                myCell.BufferCombination = combination;
                myCell.BufferNearDamage = false;
                myCell.ExplosionBoomInvoke(myCell.explosion, 0);
            }

        }

        void ActivateSuperColor() {
            //Активируем только если бомба еще не активна
            if (activate) return;

            activate = true;

            Type typePartner = Type.none;
            if (partner != null) {
                typePartner = partner.type;
            }

            //Цвет, на который заменяем
            InternalColor replaceColor;
            //Если активируемый тип не колор 5 но партнер с колор5 и вообще я сам себе партнер
            //(ситуация когда игрок нажимает кнопку бомбы или ракеты из магазина и применяет ее на супер цвет)
            //Цвет считается по приоритету
            if (ActivateType != Type.color5 && partner.type == Type.color5 || partner == null) 
            {
                typePartner = ActivateType;
                replaceColor = CalculateColorPriority();
            }
            //Иначе берем цвет партнера
            else
            {               
                replaceColor = partner.color;
            }

            //Партнер такая же бомба
            if (typePartner == Type.color5) {
                DestroyAll();
            }
            //Если партнер бомба, ракета или самолет
            else if (typePartner == Type.bomb ||
                typePartner == Type.airplane ||
                typePartner == Type.rocketHorizontal || 
                typePartner == Type.rocketVertical
                ) {
                replacementColorAndActivate();
            }
            //Если партнер просто цвет
            else if (typePartner == Type.color)
            {
                DestroyAllColor(partner.color);
            }
            //Если партнер я сам себе
            else if (partner == null || partner == this)
            {
                DestroyAllColor(color);
            }

            SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipExploseColor5);

            /*
            void DestroyAllRocket(InternalColor internalColor)
            {

                float destroyNum = 0;
                //Проверяем все ячейки на совпадение цветов
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
                {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        if (!myField.cellCTRLs[x, y] || !myField.cellCTRLs[x, y].cellInternal)
                        {
                            continue;
                        }


                        if (myField.cellCTRLs[x, y].cellInternal.color == internalColor)
                        {
                            //Удаляем старый объект
                            Destroy(myField.cellCTRLs[x, y].cellInternal.gameObject);

                            //Сперва создаем новый обьект ракету
                            GameObject internalObj = Instantiate(myField.prefabInternal, myField.parentOfInternals);
                            CellInternalObject cellInternalObject = internalObj.GetComponent<CellInternalObject>();
                            cellInternalObject.myField = myField;

                            if (Random.Range(0, 100) < 50) {
                                cellInternalObject.setColorAndType(internalColor, Type.rocketVertical);
                            }
                            else {
                                cellInternalObject.setColorAndType(internalColor, Type.rocketHorizontal);
                            }
                            //Перемещаем объект на место старого
                            cellInternalObject.StartMove(myField.cellCTRLs[x, y]);
                            cellInternalObject.EndMove();

                            needInstantDamage = false;
                            cellInternalObject.BufferActivateType = cellInternalObject.type;
                            cellInternalObject.ActivateInvoke(destroyNum);
                            destroyNum++;
                        }
                    }
                }

                myCell.Damage(null, combination);
            }
            */

            void replacementColorAndActivate() {

                float speedPerCell = 0.1f;
                //Проверяем все поле на цвета
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
                {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {

                        //Проверяем что ячейка есть
                        if (!myField.cellCTRLs[x, y] || //Если ячейки нет
                            !myField.cellCTRLs[x, y].cellInternal || //Если нет внутренности
                            myField.cellCTRLs[x, y].cellInternal.color != replaceColor || //Если цвет не совпадает с цветом партнера
                            myField.cellCTRLs[x, y].cellInternal.type != Type.color //Если тип объекта особенный
                                                                                    //myField.cellCTRLs[x,y].cellInternal.type == Type.color5
                            )
                        {
                            continue;
                        }
                        float dist = Vector2.Distance(myField.cellCTRLs[x, y].pos, myCell.pos);


                        CellInternalObject cellInternalObject = myField.cellCTRLs[x, y].cellInternal;

                        //Удаляем старый объект
                        if (myField.cellCTRLs[x, y].cellInternal && myField.cellCTRLs[x, y].rock <= 0)
                        {
                            Destroy(myField.cellCTRLs[x, y].cellInternal.gameObject);

                            //создаем новый обьект
                            GameObject internalObj = Instantiate(myField.prefabInternal, myField.parentOfInternals);
                            cellInternalObject = internalObj.GetComponent<CellInternalObject>();
                            cellInternalObject.myField = myField;
                        }

                        //Заменяем если нет камня
                        if (myField.cellCTRLs[x, y].rock <= 0) {

                            //Если партнер ракета
                            if (typePartner == Type.rocketHorizontal || typePartner == Type.rocketVertical)
                            {
                                if (Random.Range(0, 100) < 50)
                                {
                                    cellInternalObject.setColorAndType(partner.color, Type.rocketVertical);
                                }
                                else
                                {
                                    cellInternalObject.setColorAndType(partner.color, Type.rocketHorizontal);
                                }
                            }
                            //Если бомба
                            else if (typePartner == Type.bomb)
                            {
                                cellInternalObject.setColorAndType(replaceColor, Type.bomb);
                            }
                            //Если самолет
                            else if (typePartner == Type.airplane)
                            {
                                cellInternalObject.setColorAndType(partner.color, Type.airplane);

                            }
                            else
                            {

                            }
                        }

                        Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateBoomSuperColor(myField.transform, myCell);
                        particle3DCTRL.SetTransformTarget(new Vector2(x + 0.5f, y + 0.5f));
                        particle3DCTRL.SetTransformSpeed(1 / speedPerCell);

                        //Перемещаем объект на место старого
                        cellInternalObject.StartMove(myField.cellCTRLs[x, y]);
                        cellInternalObject.EndMove();

                        //needInstantDamage = false;
                        cellInternalObject.BufferActivateType = cellInternalObject.type;
                        cellInternalObject.BufferPartner = null;
                        cellInternalObject.BufferCombination = combination;
                        //cellInternalObject.Activate(cellInternalObject.type, null, combination);
                        cellInternalObject.ActivateInvoke(dist * speedPerCell);

                    }
                }

                //Удаляем обьект партнера
                if (partner)
                    Destroy(partner.gameObject);
            }

            void DestroyAllColor(InternalColor internalColor) {
                float speedPerCell = 0.1f;
                //Проверяем все ячейки на совпадение цветов
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++) {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        //Выходим если нет ячейки, нет внутреннего объекта или это моя ячейка или ячейка партнера
                        if (!myField.cellCTRLs[x, y] || !myField.cellCTRLs[x, y].cellInternal || 
                            myField.cellCTRLs[x,y].cellInternal == this || 
                            (partner != null && myField.cellCTRLs[x,y].cellInternal == partner) || 
                            myField.cellCTRLs[x,y].cellInternal.type == Type.color5 ||
                            myField.cellCTRLs[x, y].cellInternal.type == Type.blocker) {
                            continue;
                        }

                        float dist = Vector2.Distance(myField.cellCTRLs[x, y].pos, myCell.pos);


                        if (myField.cellCTRLs[x, y].cellInternal.color == internalColor)
                        {
                            myField.cellCTRLs[x, y].cellInternal.BufferPartner = partner;
                            myField.cellCTRLs[x, y].cellInternal.BufferCombination = combination;
                            //myField.cellCTRLs[x, y].DamageInvoke(dist * speedPerCell + 0.5f);

                            

                            Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateBoomSuperColor(myField.transform, myCell);
                            particle3DCTRL.SetTransformTarget(myField.cellCTRLs[x,y].cellInternal, combination);
                            particle3DCTRL.SetTransformSpeed(1 / speedPerCell);
                        }

                        //Удаляем обьект партнера
                        if (partner)
                            Destroy(partner.gameObject);

                    }
                }

                myCell.Damage(null, combination);
            }

            void DestroyAll()
            {
                //Проверяем все ячейки на совпадение цветов
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
                {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        if (!myField.cellCTRLs[x, y])
                        {
                            continue;
                        }

                        //Узнаем растояние от ячейки инициатора
                        float distance = Vector2.Distance(new Vector2(x,y), myCell.pos);

                        //myField.cellCTRLs[x, y].cellInternal.BufferPartner = partner;
                        myField.cellCTRLs[x, y].BufferCombination = combination;
                        myField.cellCTRLs[x, y].BufferNearDamage = false;
                        myField.cellCTRLs[x, y].DamageInvoke(0.1f * distance);
                    }
                }

                Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateBoomAll(myField.gameObject.transform, myCell);
                //particle3DCTRL.SetSpeed(1);
                //particle3DCTRL.SetSize(1);

                //Удаляем обьект партнера
                if (partner)
                    Destroy(partner.gameObject);

            }


        }

        void ActivateBombAndRocket() {
            Debug.Log("ActivateBombVertical");

            if (partner) {
                //Удаляем партнера
                partner.activate = false;
                partner.DestroyObj();
            }
            //Берем позицию бомбы
            Vector2Int pos = myCell.pos;


            bool[,] activated = new bool[myField.cellCTRLs.GetLength(0), myField.cellCTRLs.GetLength(1)];

            //Моментальный взрыв
            //перебираем 9 ячеек
            for (int x = -1; x <= 1; x++) {
                if (myCell.pos.x + x < 0 || myCell.pos.x > myField.cellCTRLs.GetLength(0) - 1)
                    continue;

                for (int y = -1; y <= 1; y++) {
                    //Если вышли за пределы смотрим дальше
                    if (myCell.pos.y + y < 0 || myCell.pos.y > myField.cellCTRLs.GetLength(1) - 1)
                        continue;

                    //Если этой ячейки нет, смотрим дальше
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] == null) {
                        continue;
                    }


                }
            }

            List<CellCTRL> expCells = new List<CellCTRL>();

            //Создаем взрывные волны
            //Горизонтальные
            for (int y = -1; y <= 1; y++) {
                //Если вышли за пределы массива //По у
                if (myCell.pos.y + y < 0 || myCell.pos.y > myField.cellCTRLs.GetLength(1) - 1)
                    continue;

                //влево
                for (int x = -1; x > myField.cellCTRLs.GetLength(0) * -1; x--) {
                    //Если вышли за пределы массива //По x
                    if (myCell.pos.x + x < 0 || myCell.pos.x + x > myField.cellCTRLs.GetLength(0) - 1)
                        continue;

                    //Если ячейки нет
                    if (myCell.myField.cellCTRLs[myCell.pos.x + x,myCell.pos.y + y] == null) {
                        continue;
                    }

                    //создаем взрыв если на этой ячейке нет взрыва
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion == null)
                    {
                        myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, BufferCombination);
                    }
                    else {
                    
                    }

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion.left = true;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferCombination = combination;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferNearDamage = false;

                    //Добавляем в список
                    AddExplosionToList(myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y]);

                    break;
                }

                //вправо
                for (int x = 1; x < myField.cellCTRLs.GetLength(0); x++) {
                    //Если вышли за пределы массива //По x
                    if (myCell.pos.x + x < 0 || myCell.pos.x + x > myField.cellCTRLs.GetLength(0) - 1)
                        continue;

                    //Если ячейки нет
                    if (myCell.myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] == null)
                    {
                        continue;
                    }

                    //создаем взрыв если на этой ячейке нет взрыва
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion == null)
                    {
                        myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, BufferCombination);
                    }
                    else {
                    
                    }

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion.right = true;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferCombination = combination;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferNearDamage = false;

                    //Добавляем в список
                    AddExplosionToList(myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y]);

                    break;
                }
            }

            //вертикальные
            for (int x = -1; x <= 1; x++)
            {
                //Если вышли за пределы массива //По x
                if (myCell.pos.x + x < 0 || myCell.pos.x > myField.cellCTRLs.GetLength(0) - 1)
                    continue;

                //вниз
                for (int y = -1; y > myField.cellCTRLs.GetLength(1) * -1; y--)
                {
                    //Если вышли за пределы массива //По y
                    if (myCell.pos.y + y < 0 || myCell.pos.y + y > myField.cellCTRLs.GetLength(1) - 1)
                        continue;

                    //Если ячейки нет
                    if (myCell.myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] == null)
                    {
                        continue;
                    }

                    //создаем взрыв если на этой ячейке нет взрыва
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion == null)
                    {
                        myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, BufferCombination);
                    }
                    else {
                    
                    }

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion.down = true;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferCombination = combination;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferNearDamage = false;

                    //Добавляем в список
                    AddExplosionToList(myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y]);

                    break;
                }

                //вверх
                for (int y = 1; y < myField.cellCTRLs.GetLength(1); y++)
                {
                    //Если вышли за пределы массива //По y
                    if (myCell.pos.y + y < 0 || myCell.pos.y + y > myField.cellCTRLs.GetLength(1) - 1)
                        continue;

                    //Если ячейки нет
                    if (myCell.myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] == null)
                    {
                        continue;
                    }

                    //создаем взрыв если на этой ячейке нет взрыва
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion == null)
                    {
                        myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, BufferCombination);
                    }
                    else {
                    
                    }

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion.up = true;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferCombination = combination;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferNearDamage = false;

                    //Добавляем в список
                    AddExplosionToList(myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y]);

                    break;
                }
            }

            //Взрываем
            foreach (CellCTRL expCell in expCells) {
                expCell.ExplosionBoomInvoke(expCell.explosion, 0);
            }

            float radius = 1;
            //Создаем частицы взрыва
            Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateBoomBomb(myField.gameObject.transform, myCell);
            particle3DCTRL.SetSpeed(radius);
            particle3DCTRL.SetSize(radius);

            Color color1 = GetColor(color);
            Vector3 colorVec = new Vector3(color1.r, color1.g, color1.b);
            colorVec.Normalize();
            color1 = new Color(color1.r, color1.g, color1.b);

            particle3DCTRL.SetColor(color1);


            Destroy(gameObject);

            void AddExplosionToList(CellCTRL explosionNew) {

                //если нашли этот взрыв в списке, то выходим
                foreach (CellCTRL explosion in expCells) {
                    if (explosion == explosionNew) {
                        return;
                    }
                }

                //Добавляем взрыв
                expCells.Add(explosionNew);


            }
        }

        void ActivateBomb(int radius) {

            animatorCTRL.PlayAnimation("BombActivated");

            BoombRadius = radius;

            bool partnerBomb = false;
            //Удаляем партнера
            if (partner)
            {
                if (partner.type == Type.bomb)
                    partnerBomb = true;

                //Удаляем партнера
                partner.activate = false;
                partner.DestroyObj();

            }



            //Перебираем поле 5 на 5
            for (int x = -radius * 3; x <= radius * 3; x++) {
                for (int y = -radius * 3; y <= radius * 3; y++) {
                    int fieldPosX = myCell.pos.x + x;
                    int fieldPosY = myCell.pos.y + y;
                    //Если вышли за пределы карты или этой ячейки нету
                    if (fieldPosX < 0 || fieldPosX >= myField.cellCTRLs.GetLength(0) ||
                        fieldPosY < 0 || fieldPosY >= myField.cellCTRLs.GetLength(1) ||
                        !myField.cellCTRLs[fieldPosX, fieldPosY]
                        )
                    {
                        continue;
                    }

                    //Считаем время задержки взрыва этой ячейки
                    float time = Vector2.Distance(new Vector2(), new Vector2(x, y)) * 0.2f;

                    //Запускаем анимацию смещения если есть внутренность
                    if (myField.cellCTRLs[fieldPosX, fieldPosY].cellInternal)
                    {
                        myField.cellCTRLs[fieldPosX, fieldPosY].cellInternal.PlayBoomAnimationInvoke(myCell.pos, radius * 3f, time/1.5f);
                    }


                    //Если урон этой ячейке не нужен выходим
                    if (Mathf.Abs(x) > radius || Mathf.Abs(y) > radius) {
                        continue;
                    }

                    myField.cellCTRLs[fieldPosX, fieldPosY].BufferCombination = BufferCombination;
                    myField.cellCTRLs[fieldPosX, fieldPosY].BufferNearDamage = false;
                    myField.cellCTRLs[fieldPosX, fieldPosY].DamageInvoke(time);

                }
            }

            //Создаем частицы взрыва
            Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateBoomBomb(myField.gameObject.transform, myCell);
            particle3DCTRL.SetSpeed(radius);
            particle3DCTRL.SetSize(radius);

            Color color1 = GetColor(color);
            Vector3 colorVec = new Vector3(color1.r, color1.g, color1.b);
            colorVec.Normalize();
            color1 = new Color(color1.r, color1.g, color1.b);

            particle3DCTRL.SetColor(color1);

            if (partnerBomb) {
                activateNum = 1;
            }
        }

        void ActivateFly() {

            CreateThis();

            if (partner != null && partner.type == Type.airplane) {
                CreatePartner();
                CreatePartner();
            }

            
            if(partner != null)
                Destroy(partner.gameObject);

            void CreateThis() {
                //Создаем объкт
                GameObject flyObj = Instantiate(FlyPrefab, myField.parentOfFly);
                FlyCTRL flyCTRL = flyObj.GetComponent<FlyCTRL>();


                //ищем ячейку к которой еще никто не летит
                foreach (CellCTRL cellPriority in myField.cellsPriority)
                {
                    if (cellPriority == myCell) { 
                        continue; 
                    }

                    //Если нашли эту ячейку в списке целей то завершаем перебор и переключаемся далее
                    bool found = false;
                    foreach (FlyCTRL fly in FlyCTRL.flyCTRLs)
                    {
                        if (fly.CellTarget == cellPriority)
                        {
                            found = true;
                            break;
                        }
                    }

                    //Если закончили перебор и не нашли ячейку в списке, значит это то что нужно выбрать в качестве новой цели
                    if (!found)
                    {
                        flyCTRL.inicialize(myCell, cellPriority, partner, combination, Image.color);

                        break;
                    }


                }
            }

            void CreatePartner() {
                //Создаем объкт
                GameObject flyObj = Instantiate(FlyPrefab, myField.parentOfFly);
                FlyCTRL flyCTRL = flyObj.GetComponent<FlyCTRL>();


                //ищем ячейку к которой еще никто не летит
                foreach (CellCTRL cellPriority in myField.cellsPriority)
                {
                    //Если нашли эту ячейку в списке целей то завершаем перебор и переключаемся далее
                    bool found = false;
                    foreach (FlyCTRL fly in FlyCTRL.flyCTRLs)
                    {
                        if (fly.CellTarget == cellPriority)
                        {
                            found = true;
                            break;
                        }
                    }

                    //Если закончили перебор и не нашли ячейку в списке, значит это то что нужно выбрать в качестве новой цели
                    if (!found)
                    {
                        flyCTRL.inicialize(partner.myCell, cellPriority, null, combination, Image.color);

                        break;
                    }


                }
            }
        }

    }
    void Activate() {
        Activate(BufferActivateType, BufferPartner, BufferCombination);
    }

    public void ActivateInvoke(float timeInvoke) {
        //Invoke("Activate", timeInvoke);
        Invoke("DamageMyCell", timeInvoke);
    }
    public void DamageMyCell() {
        myCell.Damage(BufferPartner, BufferCombination);
    }

    void UpdateActivate() {
        //Выходим если не кадр расчета
        if (myField == null || myField.buffer.CalculateFrameNow != 0)
            return;

        if (activateNeed && !activate) {
            Activate(BufferActivateType, BufferPartner, BufferCombination);
        }
    }

    /// <summary>
    /// Запустить анимацию смещения от взрыва
    /// </summary>
    Vector2 BoomFrom = new Vector2();
    float BoomDistanceMax = 0;

    public void PlayBoomAnimation() {

        //считаем растояние взрыва
        float distance = Vector2.Distance(BoomFrom, myCell.pos);

        if (distance == 0) {
            return;
        }

        //Ищем силу смещения
        float strange =  1 - (distance / BoomDistanceMax);

        //если сила взрыва меньше нуля выходим
        if (strange < 0) {
            return;
        }

        //находим вектор взрыва
        Vector2 vectorBoom = new Vector2(myCell.pos.x, myCell.pos.y) - BoomFrom;

        //Нормализовываем вектор
        vectorBoom.Normalize();

        //Иначе отправляем запускаем анимацию смещения и отправляем вектор смещения
        animatorCTRL.SetFloat("DroppedSizeX", vectorBoom.x * strange * -1);
        animatorCTRL.SetFloat("DroppedSizeY", vectorBoom.y * strange * -1);

        //Узнаем преобладание горизонтального смещения над вертикальным
        //получаем
        float horizontal = Mathf.Acos(Mathf.Abs(vectorBoom.x));
        horizontal = 1-(horizontal / (Mathf.PI / 2));

        animatorCTRL.SetFloat("DroppedIsHorizontal", horizontal);

        //Запускаем анимацию
        animatorCTRL.PlayAnimation("DroppedDown");
    }
    public void PlayBoomAnimationInvoke(Vector2 from, float distanceMax, float invokeTime) {
        BoomFrom = from;
        BoomDistanceMax = distanceMax;
        Invoke("PlayBoomAnimation", invokeTime);
    }

    public void IniBomb(CellCTRL myCellNew, GameFieldCTRL gameField ,InternalColor internalColor, int myCombIDFunc) {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.bomb;
        color = internalColor;
        Image.texture = TextureBomb;

        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;

        setColor(internalColor);
    }

    public void IniFly(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor, int myCombIDFunc)
    {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;

        type = Type.airplane;

        myCell.cellInternal = this;
        PosToCell();

        type = Type.airplane;
        color = internalColor;
        Image.texture = TextureFly;

        setColor(internalColor);
    }

    public void IniSuperColor(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor, int myCombIDFunc)
    {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;

        myCell.cellInternal = this;
        PosToCell();

        type = Type.color5;
        color = internalColor;
        Image.texture = TextureColor5;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;

        setColor(internalColor);
    }

    public void IniRocketVertical(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor, int myCombIDFunc)
    {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;

        myCell.cellInternal = this;
        PosToCell();

        type = Type.rocketVertical;
        color = internalColor;
        Image.texture = TextureRocketVertical;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;

        setColor(internalColor);
    }
    public void IniRocketHorizontal(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor, int myCombIDFunc)
    {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;

        myCell.cellInternal = this;
        PosToCell();

        type = Type.rocketHorizontal;
        color = internalColor;
        Image.texture = TextureRocketHorizontal;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;

        setColor(internalColor);
    }


    public void IniBlockerColor(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor, int myCombIDFunc)
    {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;

        myCell.cellInternal = this;
        PosToCell();

        type = Type.blocker;
        color = internalColor;
        Image.texture = TextureBlocker;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;

        setColor(internalColor);
    }

    //Расчет приоритетного цвета
    private InternalColor CalculateColorPriority()
    {
        //Массив приоритетов (количество элементов каждого цвета)
        int[] colorPriority = new int[10]; //fix

        //Проходимся по всем клеткам, считаем цвета
        for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
        {
            for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
            {
                if (myField.cellCTRLs[x, y] != null && myField.cellCTRLs[x, y].cellInternal != null)
                {
                    colorPriority[(int)myField.cellCTRLs[x, y].cellInternal.color]++;
                }
            }
        }

        //Приоритетный цвет
        InternalColor priorityColor = 0;
        //Самый большой приоритет (для сравнения)
        int largestPriority = 0;
        for (int i = 0; i < colorPriority.Length; i++)
        {
            //Заменяем приоритет, если он больше
            if (colorPriority[i] > largestPriority)
            {
                largestPriority = colorPriority[i];
                priorityColor = (InternalColor)i;
            }
        }
        return priorityColor;
    }

}
