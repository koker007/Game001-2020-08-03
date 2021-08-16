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
        airplane
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
        IniRect();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }


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
        for (int minusY = 1; minusY < myField.cellCTRLs.GetLength(1); minusY++) {
            if (myCell.pos.y - minusY >= 0 && //если не вышли за массив
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY] && //Если есть ячейка
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].cellInternal && //И она свободна
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].dontMoving == 0 //И можно двигаться
                                                                                  )
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
        return returnCell;
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
            MovingSpeed = 0;
            isMove = true;
        }

        //присваиваем ячейке обьект
        myCell.cellInternal = this;
        myCell.myInternalNum = myCell.LastInternalNum; //Запоминаем действие

        IniRect();
    }

    /// <summary>
    /// Удалить объект
    /// </summary>
    public void DestroyObj() {

        SpawnEffects();
        Destroy(gameObject);

        void SpawnEffects() {
            if (type == Type.color) {
                GameObject PrefabDie = Instantiate(myField.PrefabParticleDie, myField.parentOfScore);
                RectTransform rectDie = PrefabDie.GetComponent<RectTransform>();

                GameObject PrefabScore = Instantiate(myField.PrefabParticleScore, myField.parentOfScore);
                RectTransform rectScore = PrefabScore.GetComponent<RectTransform>();

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

    public void PosToCell() {
        IniRect();

        rectMy.pivot = rectCell.pivot;
    }

    //Активируем врутренность ячейки
    bool activate = false;
    public void Activate() {
        Activate(type);
    }
    public void Activate(Type ActivateType) {

        if (ActivateType == Type.color) return;

        Debug.Log("Activate");
        if (ActivateType == Type.bomb) ActivateBomb();


        void ActivateBomb()
        {
            //Активируем только если бомба еще не активна
            if (activate) return;

            activate = false;

            //проверяем по x
            for (int x = -2; x <= 2; x++)
            {
                //Если вышли за пределы массива
                if (x < 0 || x >= myField.cellCTRLs.GetLength(0)) continue;

                //проверяем по y
                for (int y = -2; y <= 2; y++)
                {
                    //Если вышли за пределы массива
                    if (y < 0 || y >= myField.cellCTRLs.GetLength(1)) continue;
                    //Если нету ячейки или внутренности
                    if (!myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] && !myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].cellInternal) continue;

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].Damage();

                }
            }
        }
    }


    public void IniBomb(CellCTRL myCellNew, GameFieldCTRL gameField ,InternalColor internalColor) {

        myCell = myCellNew;
        myField = gameField;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.bomb;
        Image.texture = TextureBomb;
    }

    public void IniFly(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor)
    {

        myCell = myCellNew;
        myField = gameField;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.airplane;
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
        Image.texture = TextureSuperColor;
        
    }
}
