using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// ���������� ������������ ����� �����
/// </summary>
public class CellInternalObject : MonoBehaviour
{
    //��� ����
    public GameFieldCTRL myField;
    //��� ������
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
        timeCreate = Time.unscaledTime;
        IniRect();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    public float timeCreate = 0;
    public float timeLastMoving = 0;

    public float MovingSpeed = 0;
    void Moving() {
        

        //�������� � ������
        if (isMove) {

            //////////////////////////////////////////////////////////
            //�������������� ��������
            float posXnew = rectMy.pivot.x;

            MovingSpeed += Time.unscaledDeltaTime;
            float speed = 0.05f + MovingSpeed;

            float correctY = 0;
            //���� ������ ������ ��������� ������ � � ���� �������� ������� �������
            if (myCell.pos.y < myField.cellCTRLs.GetLength(1) - 1 && 
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y+1] &&
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y+1].cellInternal &&
                Vector2.Distance(
                    new Vector2(myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.x, myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.y), 
                    new Vector2(rectMy.pivot.x, rectMy.pivot.y)) < 1 &&
                MovingSpeed < myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.MovingSpeed) {
                //��������
                MovingSpeed = myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.MovingSpeed;

                //����������
                correctY = 1 - Vector2.Distance(
                    new Vector2(myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.x, myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.y),
                    new Vector2(rectMy.pivot.x, rectMy.pivot.y));
            }

            //�������� �����
            if (rectMy.pivot.x > rectCell.pivot.x) {
                posXnew -= speed;

                //���� �������
                if (posXnew <= rectCell.pivot.x)
                    posXnew = rectCell.pivot.x;
            }

            //�������� ������
            if (rectMy.pivot.x < rectCell.pivot.x) {
                posXnew += speed;

                //���� �������
                if (posXnew >= rectCell.pivot.x)
                    posXnew = rectCell.pivot.x;
            }

            /////////////////////////////////////////////////////////////
            //������������ ��������
            float posYnew = rectMy.pivot.y;
            //�������� ����
            if (rectMy.pivot.y > rectCell.pivot.y)
            {
                posYnew -= speed;
                //���� �������
                if (posYnew <= rectCell.pivot.y)
                {
                    //����� ���������������
                    posYnew = rectCell.pivot.y;
                }
            }

            //�������� �����
            if (rectMy.pivot.y < rectCell.pivot.y)
            {
                posYnew += speed + correctY;
                //���� �������
                if (posYnew >= rectCell.pivot.y)
                    posYnew = rectCell.pivot.y;
            }


            //���� ������� ����� ����� ����������
            if (rectCell.pivot.x == posXnew &&
                rectCell.pivot.y == posYnew) {

                //���� ����� ������ ���
                CellCTRL cellMove = GetFreeCellDown();
                if (cellMove)
                {
                    //���������� ����� ���� ��� ��������
                    StartMove(cellMove);
                }
                else
                {
                    //�������� ��������
                    isMove = false;
                }
            }

            //����������� ���������
            rectMy.pivot = new Vector2(posXnew, posYnew);

            //��������� ����� ���������� �����������
            timeLastMoving = Time.unscaledTime;
        }

        //��������� ����� ������� ��������� ������
        else {
            CellCTRL cellMove = GetFreeCellDown();
            if (cellMove)
                StartMove(cellMove);
        }
        
    }

    //�������� ��������� ������ �����
    CellCTRL GetFreeCellDown() {
        CellCTRL returnCell = null;
        for (int minusY = 1; minusY < myField.cellCTRLs.GetLength(1); minusY++) {
            if (myCell.pos.y - minusY >= 0 && //���� �� ����� �� ������
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY] && //���� ���� ������
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].cellInternal && //� ��� ��������
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].BlockingMove == 0 && //� ����� ���������
                Time.unscaledTime - myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].timeBoomOld > 0.25f) //�������� ������ �������                                                                 )
            {
                //������ ����� ������ ��� ������
                returnCell = myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY];
            }
            //������ ����� ����� ���
            else {
                //�������
                break;
            }
        }
        return returnCell;
    }

    /// <summary>
    /// ��������� �� � �������� ������ ������
    /// </summary>
    public bool isMove = false;
    /// <summary>
    /// �������� � ��������� ������
    /// </summary>
    public void StartMove(CellCTRL cellNew) {
        if (myCell)
        {
            myCell.cellInternal = null;
        }

        myCell = cellNew;

        if (!isMove)
        {
            cellNew.myInternalNum = cellNew.LastInternalNum;
            MovingSpeed = 0;
            isMove = true;
        }

        //����������� ������ ������
        myCell.cellInternal = this;
        myCell.myInternalNum = myCell.LastInternalNum; //���������� ��������

        IniRect();
    }

    /// <summary>
    /// ������� ������
    /// </summary>
    public void DestroyObj() {
        int score = 100 + 10 * (myField.ComboCount-1);


        SpawnEffects();

        Destroy(gameObject);

        

        void SpawnEffects() {
            myCell.timeBoomOld = Time.unscaledTime; //������ ����� ������

            if (type == Type.color) {
                GameObject PrefabDie = Instantiate(myField.PrefabParticleDie, myField.parentOfScore);
                RectTransform rectDie = PrefabDie.GetComponent<RectTransform>();

                GameObject PrefabScore = Instantiate(myField.PrefabParticleScore, myField.parentOfScore);
                PrefabScore.GetComponent<ScoreCTRL>().Inicialize(score, ConvertEnumColor());
                Gameplay.main.ScoreUpdate(score);
                RectTransform rectScore = PrefabScore.GetComponent<RectTransform>();

                //�������� ���������� �����

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

    //���������� ������������ ������
    bool activate = false;

    public void Activate() {
        Activate(type, null);
    }
    public void Activate(Type ActivateType, CellInternalObject partner) {

        if (ActivateType == Type.color) return;

        //���� �������� ���
        if (partner == null)
        {
            Debug.Log("Activate");
            if (ActivateType == Type.bomb) ActivateBomb();
            else if (ActivateType == Type.rocketHorizontal) ActivateRocket(true, false);
            else if (ActivateType == Type.rocketVertical) ActivateRocket(false, true);
            else if (ActivateType == Type.supercolor) ActivateSuperColor();
        }
        else {

            //����� ����� + ��� ������
            if (ActivateType == Type.supercolor) {
                ActivateSuperColor();
            }
            //����� + �����������
            else if (ActivateType == Type.bomb && partner.type == Type.rocketHorizontal) {
                ActivateBombHorizontal();
            }
            //����� + ���������
            else if (ActivateType == Type.bomb && partner.type == Type.rocketVertical)
            {
                ActivateBombVertical();
            }
            //������ + ������
            else if ((ActivateType == Type.rocketHorizontal || ActivateType == Type.rocketVertical) &&
                (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical)) {
                ActivateRocket(true, true);
            }
        }

        DestroyObj();

        void ActivateBomb()
        {

            Debug.Log("ActivateBomb");

            //���������� ������ ���� ����� ��� �� �������
            if (activate) return;

            activate = true;

            int sizeMax = 1;
            //��������� �� x
            for (int x = -sizeMax; x <= sizeMax; x++)
            {
                //���� ����� �� ������� �������
                if (myCell.pos.x + x < 0 || myCell.pos.x + x >= myField.cellCTRLs.GetLength(0)) continue;

                //��������� �� y
                for (int y = -sizeMax; y <= sizeMax; y++)
                {
                    //���� �������
                    //if((Mathf.Abs(x)+Mathf.Abs(y)) == sizeMax*2) continue;

                    //���� ����� �� ������� �������
                    if (myCell.pos.y + y < 0 || myCell.pos.y + y >= myField.cellCTRLs.GetLength(1)) continue;
                    //���� ���� ������ ��� ������������
                    if (!myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] && !myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].cellInternal) continue;

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].Damage(this);

                }
            }
        }

        void ActivateRocket(bool horizontal, bool vertical)
        {
            Debug.Log("ActivateRocket");
            //���������� ������ ���� ����� ��� �� �������
            if (activate) return;
            
            //���� ��������� � ������ �������
            if (partner && partner != this && (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical)) {
                horizontal = true;
                vertical = true;

                //������� ��������
                partner.activate = false;
                partner.DestroyObj();
            }

            //�������������� ������
            if (horizontal)
            {
                //����� ��������
                for (int num = 1; num <= myField.cellCTRLs.GetLength(0); num++)
                {
                    float time = num *0.05f;

                    //�����
                    if (myCell.pos.x - num >= 0)
                    {
                        myField.cellCTRLs[myCell.pos.x - num, myCell.pos.y].DamageInvoke(time);
                    }

                    //������
                    if (myCell.pos.x + num < myField.cellCTRLs.GetLength(0))
                    {
                        myField.cellCTRLs[myCell.pos.x + num, myCell.pos.y].DamageInvoke(time);
                    }
                }
            }
            //������������ ������
            if(vertical) {
                //����� ��������
                for (int num = 1; num <= myField.cellCTRLs.GetLength(1); num++)
                {
                    float time = num * 0.05f;

                    //����
                    if (myCell.pos.y - num >= 0)
                    {
                        myField.cellCTRLs[myCell.pos.x, myCell.pos.y - num].DamageInvoke(time);
                    }

                    //�����
                    if (myCell.pos.y + num < myField.cellCTRLs.GetLength(1))
                    {
                        myField.cellCTRLs[myCell.pos.x, myCell.pos.y + num].DamageInvoke(time);
                    }
                }
            }

        }

        void ActivateSuperColor() {
            Debug.Log("ActivateSuperColor");

            //���������� ������ ���� ����� ��� �� �������
            if (activate) return;

            activate = true;

            int sizeMax = 1;

            //������� ����� �� �����
            if (partner != null && partner.type == Type.supercolor) {
                DestroyAll();
            }
            //������� ������ ��������������
            else if (partner != null && partner.type == Type.rocketHorizontal) {
                
            }
            //���� ������� ������ ����
            else if (partner != null && partner.type == Type.color)
            {
                DestroyAllColor(partner.color);
            }
            //���� ������� � ��� ����
            else if (partner == null || partner == this)
            {
                DestroyAllColor(color);
            }


            void DestroyAllColor(InternalColor internalColor) {
                //��������� ��� ������ �� ���������� ������
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
                //��������� ��� ������ �� ���������� ������
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
                {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        if (!myField.cellCTRLs[x, y] || !myField.cellCTRLs[x, y].cellInternal)
                        {
                            continue;
                        }

                        //������ ��������� �� ������ ����������
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


    public void IniBomb(CellCTRL myCellNew, GameFieldCTRL gameField ,InternalColor internalColor) {

        myCell = myCellNew;
        myField = gameField;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.bomb;
        color = internalColor;
        Image.texture = TextureBomb;

        myCellNew.myInternalNum = myCellNew.LastInternalNum;
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
        myCellNew.myInternalNum = myCellNew.LastInternalNum;
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
        myCellNew.myInternalNum = myCellNew.LastInternalNum;
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
        myCellNew.myInternalNum = myCellNew.LastInternalNum;
    }
}
