using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Врутренняя перемещаемая часть ячеек
/// </summary>
public class CellInternalObject : MonoBehaviour
{
    //мое поле
    public GameFieldCTRL myField;
    //Моя ячейка
    public CellCTRL myCell;


    RectTransform rectMy;
    RectTransform rectCell;

    [SerializeField]
    RawImage Image;

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
    Texture2D TextureRed;
    [SerializeField]
    Texture2D TextureGreen;
    [SerializeField]
    Texture2D TextureBlue;
    [SerializeField]
    Texture2D TextureYellow;
    [SerializeField]
    Texture2D TextureViolet;
    [SerializeField]
    Texture2D TextureSuperColor;
    [SerializeField]
    Texture2D TextureBomb;
    [SerializeField]
    Texture2D TextureFly;
    [SerializeField]
    Texture2D TextureRocketHorizontal;
    [SerializeField]
    Texture2D TextureRocketVertical;

    public enum InternalColor {
        Red,
        Green,
        Blue,
        Yellow,
        Violet
    }
    public enum Type {
        color,
        supercolor,
        rocketHorizontal,
        rocketVertical,
        bomb,
        airplane,
        none
    }

    public InternalColor color;
    public Type type;

    public void IniRect() {
        rectMy = GetComponent<RectTransform>();
        rectCell = myCell.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timeCreate = Time.unscaledTime;
        IniRect();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        UpdateActivate();
    }

    public float timeCreate = 0;
    public float timeLastMoving = 0;

    public float MovingSpeed = 0;
    void Moving() {
        

        //Движение к соседу
        if (isMove) {

            //////////////////////////////////////////////////////////
            //Горизонтальное движение
            float posXnew = rectMy.pivot.x;

            MovingSpeed += Time.unscaledDeltaTime;
            float speed = 0.05f + MovingSpeed;

            float correctY = 0;
            //если обьект сверху находится близко и у него скорость падения быстрее
            if (myCell.pos.y < myField.cellCTRLs.GetLength(1) - 1 && 
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

            //Движение влево
            if (rectMy.pivot.x > rectCell.pivot.x) {
                posXnew -= speed;

                //Если слишком
                if (posXnew <= rectCell.pivot.x)
                    posXnew = rectCell.pivot.x;
            }

            //Движение вправо
            if (rectMy.pivot.x < rectCell.pivot.x) {
                posXnew += speed;

                //Если слишком
                if (posXnew >= rectCell.pivot.x)
                    posXnew = rectCell.pivot.x;
            }

            /////////////////////////////////////////////////////////////
            //вертикальное движение
            float posYnew = rectMy.pivot.y;
            //Движение вниз
            if (rectMy.pivot.y > rectCell.pivot.y)
            {
                posYnew -= speed;
                //Если слишком
                if (posYnew <= rectCell.pivot.y)
                {
                    //Иначе останавливаемся
                    posYnew = rectCell.pivot.y;
                }
            }

            //Движение вверх
            if (rectMy.pivot.y < rectCell.pivot.y)
            {
                posYnew += speed + correctY;
                //Если слишком
                if (posYnew >= rectCell.pivot.y)
                    posYnew = rectCell.pivot.y;
            }


            //Если позиция равна точке назначения
            if (rectCell.pivot.x == posXnew &&
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
                }
            }

            //Присваиваем изменения
            rectMy.pivot = new Vector2(posXnew, posYnew);

            //Обновляем время последнего перемещения
            timeLastMoving = Time.unscaledTime;
        }

        //Проверяем снизу наличие свободной ячейки
        else {
            CellCTRL cellMove = GetFreeCellDown();
            if (cellMove)
                StartMove(cellMove);
        }
        
    }

    //Получить свободную ячейку снизу
    CellCTRL GetFreeCellDown() {
        CellCTRL returnCell = null;

        //Получить свободную ячейку снизу
        for (int minusY = 1; minusY < myField.cellCTRLs.GetLength(1); minusY++) {
            if (myCell.pos.y - minusY >= 0 && //если не вышли за массив
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY] && //Если есть ячейка
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].cellInternal && //И она свободна
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].BlockingMove == 0 && //И можно двигаться
                Time.unscaledTime - myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].timeBoomOld > 0.25f) //Совзрыва прошла секунда                                                                 )
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


            bool canTestingRight = true;
            bool canTestingLeft = true;

            //Общая проверка по высоте
            int smeshenie = 1;
            if (myCell.pos.y - smeshenie >= 0 //если не вышли за массив
                ) {

                //Если перемещение четное, то провека справа
                //int internalNum = CellCTRL.GetNowLastInternalNum;
                //Справа
                if (
                    myCell.pos.x + smeshenie < myField.cellCTRLs.GetLength(0) && //если не вышли за массив
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie] && //Если есть ячейка
                    !myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].cellInternal && //И она свободна
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].BlockingMove == 0 && //И можно двигаться
                    Time.unscaledTime - myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].timeBoomOld > 0.25f &&
                    isCanMoveToThisColum(myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie]) //В этом столбце нет потенциального вертикального движения
                    ) {
                    //Ставим такую ячейку как целевую
                    returnCell = myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie];
                }
                //Слева
                else if (
                    myCell.pos.x - smeshenie >= 0 && //если не вышли за массив
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie] && //Если есть ячейка
                    !myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].cellInternal && //И она свободна
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].BlockingMove == 0 && //И можно двигаться
                    Time.unscaledTime - myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].timeBoomOld > 0.25f &&
                    isCanMoveToThisColum(myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie]) //В этом столбце нет потенциального вертикального движения
                    ) {
                    //Ставим такую ячейку как целевую
                    returnCell = myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie];
                }
            }
        }

        return returnCell;

        //Проверка можно ли двигаться боком в эту 
        bool isCanMoveToThisColum(CellCTRL cellFunc) {
            bool result = true; //Изначально двигаться разрешено

            //Проверяем низ на то что никто не движется в ячейки снизу
            for (int minus = 0; minus < myField.cellCTRLs.GetLength(1) && result; minus++) {
                if (cellFunc.pos.y - minus >= 0 &&//Если не вышли за пределы массива
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus] && //есть ячейка
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].BlockingMove <= 0//ячейка находится без блокировки движения
                    ) {
                    //Блокируем если
                    if (
                        (myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].cellInternal && myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].cellInternal.isMove) || // Внутри есть ячейка которая находится в движении
                        Time.unscaledTime - myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].timeBoomOld < 0.25f //Время ожидания после взрыва не вышло
                        ) {
                        result = false;
                    }

                }
                else {
                    break;
                }
            }

            //Проверка сверху
            for (int plus = 0; plus < myField.cellCTRLs.GetLength(1) && result; plus++) {
                //Проверяем верх на то что нету предметов которые могли бы упасть
                if (
                    cellFunc.pos.y + plus < myField.cellCTRLs.GetLength(1) &&//Если не вышли за пределы массива
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus] && //есть ячейка
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].BlockingMove <= 0//ячейка находится без блокировки движения

                    )
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

    }

    /// <summary>
    /// Удалить объект
    /// </summary>
    public void DestroyObj() {
        int score = 100 + 10 * (myField.ComboCount-1);


        SpawnEffects();

        if(gameObject)
            Destroy(gameObject);

        

        void SpawnEffects() {
            myCell.timeBoomOld = Time.unscaledTime; //Ставим время взрыва

            if (type == Type.color) {
                GameObject PrefabDie = Instantiate(myField.PrefabParticleDie, myField.parentOfScore);
                RectTransform rectDie = PrefabDie.GetComponent<RectTransform>();

                GameObject PrefabScore = Instantiate(myField.PrefabParticleScore, myField.parentOfScore);
                PrefabScore.GetComponent<ScoreCTRL>().Inicialize(score, ConvertEnumColor());
                Gameplay.main.ScoreUpdate(score);
                RectTransform rectScore = PrefabScore.GetComponent<RectTransform>();

                //Изменить количество очков

                rectDie.pivot = rectMy.pivot;
                rectScore.pivot = rectMy.pivot;
            }
        }

    }

    public void randColor() {
        color = GetRandomColor();
        if (type == Type.color) {
            if (color == InternalColor.Red) {
                Image.texture = TextureRed;
                Image.color = red;
            }
            else if (color == InternalColor.Green) {
                Image.texture = TextureGreen;
                Image.color = green;
            }
            else if (color == InternalColor.Blue)
            {
                Image.texture = TextureBlue;
                Image.color = blue;
            }
            else if (color == InternalColor.Yellow)
            {
                Image.texture = TextureYellow;
                Image.color = yellow;
            }
            else if (color == InternalColor.Violet)
            {
                Image.texture = TextureViolet;
                Image.color = violet;
            }
        }
    }
    public void setColor(InternalColor internalColor) {
        if (type == Type.color)
        {
            if (internalColor == InternalColor.Red)
            {
                Image.texture = TextureRed;
                Image.color = red;
            }
            else if (internalColor == InternalColor.Green)
            {
                Image.texture = TextureGreen;
                Image.color = green;
            }
            else if (internalColor == InternalColor.Blue)
            {
                Image.texture = TextureBlue;
                Image.color = blue;
            }
            else if (internalColor == InternalColor.Yellow)
            {
                Image.texture = TextureYellow;
                Image.color = yellow;
            }
            else if (internalColor == InternalColor.Violet)
            {
                Image.texture = TextureViolet;
                Image.color = violet;
            }
        }
        else {
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

        if (type == Type.airplane) {
            Image.texture = TextureFly;
        }
        else if (type == Type.bomb)
        {
            Image.texture = TextureBomb;
        }
        else if (type == Type.rocketHorizontal)
        {
            Image.texture = TextureRocketHorizontal;
        }
        else if (type == Type.rocketVertical)
        {
            Image.texture = TextureRocketVertical;
        }
        else if (type == Type.supercolor)
        {
            Image.texture = TextureSuperColor;
        }
    }
    public void setColorAndType(InternalColor internalColor, Type typeNew) {
        type = typeNew;
        setColor(internalColor);
    }

    InternalColor GetRandomColor()
    {
        InternalColor colorReturn = InternalColor.Red;

        int random = Random.Range(0, Gameplay.main.colors);
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

    public Color ConvertEnumColor()
    {
        switch (color)
        {
            case InternalColor.Blue:
                return Color.blue;
                break;
            case InternalColor.Green:
                return Color.green;
                break;
            case InternalColor.Red:
                return Color.red;
                break;
            case InternalColor.Violet:
                return Color.white;
                break;
            case InternalColor.Yellow:
                return Color.yellow;
                break;
            default:
                return Color.black;
                break;
        }
    }

    public void PosToCell() {
        IniRect();

        rectMy.pivot = rectCell.pivot;
    }

    //Активируем врутренность ячейки
    bool activate = false;
    bool activateNeed = false;
    public bool needInstantDamage = true;



    public Type BufferActivateType;
    public CellInternalObject BufferPartner = null;
    public void Activate(Type ActivateType, CellInternalObject partner) {

        activateNeed = true;
        BufferActivateType = ActivateType;
        BufferPartner = partner;

        if (!needInstantDamage)
        {
            if (isMove || Time.unscaledTime - timeLastMoving < 0.25f) return;
        }

        //if (ActivateType == Type.color) return;

        //Если партнера нет
        if (partner == null)
        {
            Debug.Log("Activate");
            if (ActivateType == Type.bomb) ActivateBomb();
            else if (ActivateType == Type.rocketHorizontal) ActivateRocket(true, false);
            else if (ActivateType == Type.rocketVertical) ActivateRocket(false, true);
            else if (ActivateType == Type.supercolor) ActivateSuperColor();
        }
        else {

            //супер колор + что угодно
            if (ActivateType == Type.supercolor) {
                ActivateSuperColor();
            }
            //Бомба + горизонталь
            else if (ActivateType == Type.bomb && partner.type == Type.rocketHorizontal) {
                ActivateBombHorizontal();
            }
            //Бомба + вертикаль
            else if (ActivateType == Type.bomb && partner.type == Type.rocketVertical)
            {
                ActivateBombVertical();
            }
            //ракета + ракета
            else if ((ActivateType == Type.rocketHorizontal || ActivateType == Type.rocketVertical) &&
                (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical)) {
                ActivateRocket(true, true);
            }
        }

        DestroyObj();

        void ActivateBomb()
        {

            Debug.Log("ActivateBomb");

            //Активируем только если бомба еще не активна
            if (activate) return;

            activate = true;

            int sizeMax = 1;
            //проверяем по x
            for (int x = -sizeMax; x <= sizeMax; x++)
            {
                //Если вышли за пределы массива
                if (myCell.pos.x + x < 0 || myCell.pos.x + x >= myField.cellCTRLs.GetLength(0)) continue;

                //проверяем по y
                for (int y = -sizeMax; y <= sizeMax; y++)
                {
                    //Если крайние
                    //if((Mathf.Abs(x)+Mathf.Abs(y)) == sizeMax*2) continue;

                    //Если вышли за пределы массива
                    if (myCell.pos.y + y < 0 || myCell.pos.y + y >= myField.cellCTRLs.GetLength(1)) continue;
                    //Если нету ячейки или внутренности
                    if (!myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] || !myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].cellInternal) continue;

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].Damage(this);

                }
            }
        }

        void ActivateRocket(bool horizontal, bool vertical)
        {
            Debug.Log("ActivateRocket");
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

            //Горизонтальный запуск
            if (horizontal)
            {
                //Номер проверки
                for (int num = 1; num <= myField.cellCTRLs.GetLength(0); num++)
                {
                    float time = num *0.05f;

                    //Слева
                    if (myCell.pos.x - num >= 0 &&
                        myField.cellCTRLs[myCell.pos.x - num, myCell.pos.y])
                    {
                        myField.cellCTRLs[myCell.pos.x - num, myCell.pos.y].DamageInvoke(time);
                    }

                    //Справа
                    if (myCell.pos.x + num < myField.cellCTRLs.GetLength(0) &&
                        myField.cellCTRLs[myCell.pos.x + num, myCell.pos.y])
                    {
                        myField.cellCTRLs[myCell.pos.x + num, myCell.pos.y].DamageInvoke(time);
                    }
                }
            }
            //Вертикальный запуск
            if(vertical) {
                //Номер проверки
                for (int num = 1; num <= myField.cellCTRLs.GetLength(1); num++)
                {
                    float time = num * 0.05f;

                    //Вниз
                    if (myCell.pos.y - num >= 0 && 
                        myField.cellCTRLs[myCell.pos.x, myCell.pos.y - num])
                    {
                        myField.cellCTRLs[myCell.pos.x, myCell.pos.y - num].DamageInvoke(time);
                    }

                    //вверх
                    if (myCell.pos.y + num < myField.cellCTRLs.GetLength(1) && 
                        myField.cellCTRLs[myCell.pos.x, myCell.pos.y + num])
                    {
                        myField.cellCTRLs[myCell.pos.x, myCell.pos.y + num].DamageInvoke(time);
                    }
                }
            }

        }

        void ActivateSuperColor() {
            Debug.Log("ActivateSuperColor");

            //Активируем только если бомба еще не активна
            if (activate) return;

            activate = true;

            int sizeMax = 1;

            //Партнер такая же бомба
            if (partner != null && partner.type == Type.supercolor) {
                DestroyAll();
            }
            //партнер ракета
            else if (partner != null && (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical)) {
                DestroyAllRocket(partner.color);
            }
            //Если партнер просто цвет
            else if (partner != null && partner.type == Type.color)
            {
                DestroyAllColor(partner.color);
            }
            //Если партнер я сам себе
            else if (partner == null || partner == this)
            {
                DestroyAllColor(color);
            }

            void DestroyAllRocket(InternalColor internalColor)
            {

                int destroyNum = 0;
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
                            cellInternalObject.ActivateInvoke(destroyNum);
                            destroyNum++;
                        }
                    }
                }

                myCell.Damage(null);
            }

            void DestroyAllColor(InternalColor internalColor) {
                //Проверяем все ячейки на совпадение цветов
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++) {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        if (!myField.cellCTRLs[x, y] || !myField.cellCTRLs[x, y].cellInternal) {
                            continue;
                        }


                        if (myField.cellCTRLs[x, y].cellInternal.color == internalColor)
                        {
                            myField.cellCTRLs[x, y].Damage(partner);
                        }
                    }
                }

                myCell.Damage(null);
            }

            void DestroyAll()
            {
                //Проверяем все ячейки на совпадение цветов
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
                {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        if (!myField.cellCTRLs[x, y] || !myField.cellCTRLs[x, y].cellInternal)
                        {
                            continue;
                        }

                        //Узнаем растояние от ячейки инициатора
                        float distance = Vector2.Distance(new Vector2(x,y), myCell.pos);

                        myField.cellCTRLs[x, y].DamageInvoke(0.1f * distance);
                    }
                }

            }

        }

        void ActivateBombHorizontal() {
             Debug.Log("ActivateBombHorizontal");    
        }
        void ActivateBombVertical() {
            Debug.Log("ActivateBombVertical");
        }
    }

    public void ActivateInvoke(float timeInvoke) {
        Invoke("Activate", timeInvoke);
    }

    void UpdateActivate() {
        if (activateNeed && !activate) {
            Activate(BufferActivateType, BufferPartner);
        }
    }

    public void IniBomb(CellCTRL myCellNew, GameFieldCTRL gameField ,InternalColor internalColor) {

        myCell = myCellNew;
        myField = gameField;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.bomb;
        color = internalColor;
        Image.texture = TextureBomb;

        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;
    }

    public void IniFly(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor)
    {

        myCell = myCellNew;
        myField = gameField;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.airplane;
        color = internalColor;
        Image.texture = TextureFly;
    }

    public void IniSuperColor(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor)
    {

        myCell = myCellNew;
        myField = gameField;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.supercolor;
        color = internalColor;
        Image.texture = TextureSuperColor;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;
    }

    public void IniRocketVertical(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor)
    {

        myCell = myCellNew;
        myField = gameField;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.rocketVertical;
        color = internalColor;
        Image.texture = TextureRocketVertical;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;
    }
    public void IniRocketHorizontal(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor)
    {

        myCell = myCellNew;
        myField = gameField;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.rocketHorizontal;
        color = internalColor;
        Image.texture = TextureRocketHorizontal;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;
    }
}
