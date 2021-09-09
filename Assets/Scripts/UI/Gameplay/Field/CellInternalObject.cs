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
    [SerializeField]
    Animator animatorMove;
    [SerializeField]
    Animator animatorExplose;

    //��� ����
    public GameFieldCTRL myField;
    //��� ������
    public CellCTRL myCell;
    public int MyCombID = 0;


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

    [Header("Prefabs")]
    [SerializeField]
    GameObject FlyPrefab;

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


    [Header("Other")]
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
                    myCell.CalcMyPriority();
                    //��������� �������� ���������
                    AnimationDroppedDown(true);
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


        //�������� ��������� ������ �����
        for (int minusY = 1; minusY < myField.cellCTRLs.GetLength(1); minusY++) {
            if (myCell.pos.y - minusY >= 0 && //���� �� ����� �� ������
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY] && //���� ���� ������
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].cellInternal && //� ��� ��������
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].BlockingMove == 0 && //� ��� �����
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].rock == 0 && //� ��� �����
                Time.unscaledTime - myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].timeBoomOld > 0.35f && //c ����������� ����� �����
                Time.unscaledTime - myField.timeLastBoom > 0
                ) 
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

        //���� ����� ����� ��� ���������
        //�������� ��������� ������ ������ ��� ����� ����
        if (returnCell == null) {
            //��������� ������������ ������


            bool canTestingRight = true;
            bool canTestingLeft = true;

            //����� �������� �� ������
            int smeshenie = 1;
            if (myCell.pos.y - smeshenie >= 0 //���� �� ����� �� ������
                ) {

                //���� ����������� ������, �� ������� ������
                //int internalNum = CellCTRL.GetNowLastInternalNum;
                //������
                if (
                    myCell.pos.x + smeshenie < myField.cellCTRLs.GetLength(0) && //���� �� ����� �� ������
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie] && //���� ���� ������
                    !myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].cellInternal && //� ��� ��������
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].BlockingMove == 0 && //� ��� �����
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].rock == 0 && //� ��� �����
                    Time.unscaledTime - myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].timeBoomOld > 0.35f &&
                    Time.unscaledTime - myField.timeLastBoom > 0 &&
                    isCanMoveToThisColum(myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie]) //� ���� ������� ��� �������������� ������������� ��������
                    ) {
                    //������ ����� ������ ��� �������
                    returnCell = myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie];
                }
                //�����
                else if (
                    myCell.pos.x - smeshenie >= 0 && //���� �� ����� �� ������
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie] && //���� ���� ������
                    !myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].cellInternal && //� ��� ��������
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].BlockingMove == 0 && //� ����� ���������
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].rock == 0 && //� ��� �����
                    Time.unscaledTime - myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].timeBoomOld > 0.35f &&
                    Time.unscaledTime - myField.timeLastBoom > 0 &&
                    isCanMoveToThisColum(myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie]) //� ���� ������� ��� �������������� ������������� ��������
                    ) {
                    //������ ����� ������ ��� �������
                    returnCell = myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie];
                }
            }
        }

        return returnCell;

        //�������� ����� �� ��������� ����� � ��� 
        bool isCanMoveToThisColum(CellCTRL cellFunc) {
            bool result = true; //���������� ��������� ���������

            //��������� ��� �� �� ��� ����� �� �������� � ������ �����
            for (int minus = 0; minus < myField.cellCTRLs.GetLength(1) && result; minus++) {
                if (cellFunc.pos.y - minus >= 0 &&//���� �� ����� �� ������� �������
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus] && //���� ������
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].BlockingMove <= 0 &&//������ ��������� ��� ���������� ��������
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].rock <= 0
                    ) {
                    //��������� ����
                    if (
                        (myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].cellInternal && myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].cellInternal.isMove) || // ������ ���� ������ ������� ��������� � ��������
                        Time.unscaledTime - myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].timeBoomOld < 0.25f //����� �������� ����� ������ �� �����
                        ) {
                        result = false;
                    }

                }
                else {
                    break;
                }
            }

            //�������� ������
            for (int plus = 0; plus < myField.cellCTRLs.GetLength(1) && result; plus++) {
                //��������� ���� �� �� ��� ���� ��������� ������� ����� �� ������
                if (
                    cellFunc.pos.y + plus < myField.cellCTRLs.GetLength(1) &&//���� �� ����� �� ������� �������
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus] && //���� ������
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].BlockingMove <= 0 &&//������ ��������� ��� ���������� ��������
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].rock <= 0
                    )
                {
                    //��������� ����
                    if (
                        (myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].cellInternal) || // ������ ���� ������ ������� �������� ����� ������ ��������
                        Time.unscaledTime - myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].timeBoomOld < 0.25f //����� �������� ����� ������ �� �����
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
            cellNew.myInternalNum = cellNew.GetNextLastInternalNum;
            MovingSpeed = 0;
            isMove = true;
        }

        //����������� ������ ������
        myCell.cellInternal = this;
        myCell.myInternalNum = myCell.GetNextLastInternalNum; //���������� ��������

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
    /// ������� ������
    /// </summary>
    public void DestroyObj() {
        int score = 100 + 10 * (myField.ComboCount-1);


        SpawnEffects();

        if(gameObject)
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
        else {
            if (color == InternalColor.Red)
            {
                Image.color = red;
            }
            else if (color == InternalColor.Green)
            {
                Image.color = green;
            }
            else if (color == InternalColor.Blue)
            {
                Image.color = blue;
            }
            else if (color == InternalColor.Yellow)
            {
                Image.color = yellow;
            }
            else if (color == InternalColor.Violet)
            {
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

        color = internalColor;
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

    //���������� ������������ ������
    bool activate = false;
    public int activateNum = 1;
    public int activateCount = 0;
    bool activateNeed = false;

    public bool needInstantDamage = true;
    public int BoombRadius = 1;



    public Type BufferActivateType;
    public CellInternalObject BufferPartner = null;
    public GameFieldCTRL.Combination BufferCombination = null;
    public void Activate(Type ActivateType, CellInternalObject partner, GameFieldCTRL.Combination combination) {

        //��������� ����������� ������ ���� ��� �� ���� ����� ���������� ������� ��� ������� ���� ������
        if (combination != null &&
            combination.ID == MyCombID &&
            !isMove //���� ������ �� � ��������
            ) return;


        activateNeed = true;
        activateCount++;
        if (activateCount == 1 && ActivateType == Type.bomb &&
            (partner == null || (partner.type != Type.rocketHorizontal && partner.type != Type.rocketVertical))) {
            activateNum = 2;
        }

        BufferActivateType = ActivateType;
        BufferPartner = partner;
        BufferCombination = combination;

        if (!needInstantDamage)
        {
            if (isMove || Time.unscaledTime - timeLastMoving < 1f) return;
        }
        //��������� ����������� ����
        needInstantDamage = false;
        timeLastMoving = Time.unscaledTime;


        //if (ActivateType == Type.color) return;

        //���� �������� ���
        if (partner == null)
        {
            Debug.Log("Activate");
            if (ActivateType == Type.bomb) ActivateBomb(BoombRadius);
            else if (ActivateType == Type.rocketHorizontal) ActivateRocket(true, false);
            else if (ActivateType == Type.rocketVertical) ActivateRocket(false, true);
            else if (ActivateType == Type.supercolor) ActivateSuperColor();
            else if (ActivateType == Type.airplane) ActivateFly();
        }
        else {

            //����� ����� + ��� ������
            if (ActivateType == Type.supercolor) {
                ActivateSuperColor();
            }
            //����� + ������
            else if ((ActivateType == Type.bomb && partner.type == Type.rocketHorizontal) ||
                (ActivateType == Type.rocketHorizontal && partner.type == Type.bomb) ||
                (ActivateType == Type.bomb && partner.type == Type.rocketVertical) ||
                (ActivateType == Type.rocketVertical && partner.type == Type.bomb)) {
                ActivateBombAndRocket();
            }
            //����� + �����
            else if (ActivateType == Type.bomb && partner.type == Type.bomb) {
                ActivateBomb(2);
            }
            //������ + ������
            else if ((ActivateType == Type.rocketHorizontal || ActivateType == Type.rocketVertical) &&
                (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical)) {
                ActivateRocket(true, true);
            }

            //������� + �������
            else if (ActivateType == Type.airplane) {
                ActivateFly();
            }

        }

        activateNum--;
        if (activateNum <= 0) {
            DestroyObj();
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

            //�� ��� �������
            if (horizontal && vertical) {
                myCell.explosion = new CellCTRL.Explosion(true, true, true, true, 0.05f, BufferCombination);
                myCell.BufferCombination = combination;
                myCell.BufferNearDamage = false;
                myCell.ExplosionBoomInvoke(myCell.explosion);
            }
            //��������������
            else if (horizontal) {
                myCell.explosion = new CellCTRL.Explosion(true, true, false, false, 0.05f, BufferCombination);
                myCell.BufferCombination = combination;
                myCell.BufferNearDamage = false;
                myCell.ExplosionBoomInvoke(myCell.explosion);
            }
            //������������
            else if (vertical) {
                myCell.explosion = new CellCTRL.Explosion(false, false, true, true, 0.05f, BufferCombination);
                myCell.BufferCombination = combination;
                myCell.BufferNearDamage = false;
                myCell.ExplosionBoomInvoke(myCell.explosion);
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
            //���� ������� �����, ������ ��� �������
            else if (partner != null && 
                (partner.type == Type.bomb ||
                partner.type == Type.airplane)
                ) {
                replacementColorAndActivate();
            }
            //������� ������
            else if (partner != null && (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical)) {
                replacementColorAndActivate();
                //DestroyAllRocket(partner.color);
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

            void DestroyAllRocket(InternalColor internalColor)
            {

                float destroyNum = 0;
                //��������� ��� ������ �� ���������� ������
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
                            //������� ������ ������
                            Destroy(myField.cellCTRLs[x, y].cellInternal.gameObject);

                            //������ ������� ����� ������ ������
                            GameObject internalObj = Instantiate(myField.prefabInternal, myField.parentOfInternals);
                            CellInternalObject cellInternalObject = internalObj.GetComponent<CellInternalObject>();
                            cellInternalObject.myField = myField;

                            if (Random.Range(0, 100) < 50) {
                                cellInternalObject.setColorAndType(internalColor, Type.rocketVertical);
                            }
                            else {
                                cellInternalObject.setColorAndType(internalColor, Type.rocketHorizontal);
                            }
                            //���������� ������ �� ����� �������
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

            void replacementColorAndActivate() {
                //��������� ��� ���� �� �����
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
                {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        //��������� ��� ������ ����
                        if (!myField.cellCTRLs[x, y] || //���� ������ ���
                            !myField.cellCTRLs[x,y].cellInternal || //���� ��� ������������
                            myField.cellCTRLs[x,y].cellInternal.color != partner.color || //���� ���� �� ��������� � ������ ��������
                            myField.cellCTRLs[x,y].cellInternal.type == Type.supercolor
                            )
                        {
                            continue;
                        }

                        float dist = Vector2.Distance(myField.cellCTRLs[x, y].pos, myCell.pos);

                        //������� ������ ������
                        if (myField.cellCTRLs[x, y].cellInternal)
                        {
                            Destroy(myField.cellCTRLs[x, y].cellInternal.gameObject);
                        }


                        //������� ����� ������
                        GameObject internalObj = Instantiate(myField.prefabInternal, myField.parentOfInternals);
                        CellInternalObject cellInternalObject = internalObj.GetComponent<CellInternalObject>();
                        cellInternalObject.myField = myField;

                        //���� ������� ������
                        if (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical)
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
                        //���� �����
                        else if (partner.type == Type.bomb)
                        {
                            cellInternalObject.setColorAndType(partner.color, Type.bomb);
                        }
                        //���� �������
                        else if (partner.type == Type.airplane)
                        {
                            cellInternalObject.setColorAndType(partner.color, Type.airplane);

                        }
                        else
                        {

                        }

                        //���������� ������ �� ����� �������
                        cellInternalObject.StartMove(myField.cellCTRLs[x, y]);
                        cellInternalObject.EndMove();

                        //needInstantDamage = false;
                        cellInternalObject.BufferActivateType = cellInternalObject.type;
                        cellInternalObject.BufferPartner = null;
                        cellInternalObject.BufferCombination = combination;
                        //cellInternalObject.Activate(cellInternalObject.type, null, combination);
                        cellInternalObject.ActivateInvoke(dist * 0.1f);

                    }
                }
            }

            void DestroyAllColor(InternalColor internalColor) {
                //��������� ��� ������ �� ���������� ������
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++) {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        if (!myField.cellCTRLs[x, y] || !myField.cellCTRLs[x, y].cellInternal) {
                            continue;
                        }

                        float dist = Vector2.Distance(myField.cellCTRLs[x, y].pos, myCell.pos);


                        if (myField.cellCTRLs[x, y].cellInternal.color == internalColor)
                        {
                            myField.cellCTRLs[x, y].cellInternal.BufferPartner = partner;
                            myField.cellCTRLs[x, y].cellInternal.BufferCombination = combination;
                            myField.cellCTRLs[x, y].DamageInvoke(dist * 0.1f);
                        }

                        //������� ������ ��������
                        if(partner)
                            Destroy(partner.gameObject);

                    }
                }

                myCell.Damage(null, combination);
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

                        myField.cellCTRLs[x, y].cellInternal.BufferPartner = partner;
                        myField.cellCTRLs[x, y].cellInternal.BufferCombination = combination;
                        myField.cellCTRLs[x, y].DamageInvoke(0.1f * distance);
                    }
                }

            }

        }

        void ActivateBombAndRocket() {
            Debug.Log("ActivateBombVertical");

            if (partner) {
                //������� ��������
                partner.activate = false;
                partner.DestroyObj();
            }
            //����� ������� �����
            Vector2Int pos = myCell.pos;


            bool[,] activated = new bool[myField.cellCTRLs.GetLength(0), myField.cellCTRLs.GetLength(1)];

            //���������� 9 �����
            for (int x = -1; x <= 1; x++) {
                if (myCell.pos.x + x < 0 || myCell.pos.x > myField.cellCTRLs.GetLength(0) - 1)
                    continue;

                for (int y = -1; y <= 1; y++) {
                    //���� ����� �� ������� ������� ������
                    if (myCell.pos.y + y < 0 || myCell.pos.y > myField.cellCTRLs.GetLength(1) - 1)
                        continue;

                    //���� ���� ������ ���, ������� ������
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] == null) {
                        continue;
                    }

                    //������ ����, ������� �����
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion = new CellCTRL.Explosion(true, true, true, true, 0.05f, BufferCombination);
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferCombination = combination;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferNearDamage = false;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].ExplosionBoomInvoke(myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion);
                }
            }

           


            /*
            //���������� ��� ������ �� �����
            for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
            {
                for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                {
                    int absX = Mathf.Abs(x - pos.x);
                    int absY = Mathf.Abs(y - pos.y);

                    

                    //���� ������, ����
                    if (!myField.cellCTRLs[x, y] ||
                        activated[x,y] || // ������ ���
                        (absX > 1 && //����� �� 3 ������ �� �
                        absY > 1) //����� �� 3 ������ �� y
                        ) {
                        continue;
                    }

                    activated[x, y] = true;

                    //������� ����� �������� ������ ���� ������
                    float time = Vector2.Distance(pos, new Vector2(x, y)) * 0.1f;
                    //
                    myField.cellCTRLs[x, y].BufferCombination = combination;
                    myField.cellCTRLs[x, y].BufferNearDamage = false;
                    myField.cellCTRLs[x, y].DamageInvoke(time);
                }
            }
            */

            Destroy(gameObject);
        }

        void ActivateBomb(int radius) {

            AnimationBombActivated();

            BoombRadius = radius;

            //������� ��������
            if (partner)
            {
                //������� ��������
                partner.activate = false;
                partner.DestroyObj();
            }

            //���������� ���� 5 �� 5
            for (int x = -radius; x <= radius; x++) {
                for (int y = -radius; y <= radius; y++) {
                    int fieldPosX = myCell.pos.x + x;
                    int fieldPosY = myCell.pos.y + y;
                    //���� ����� �� ������� ����� ��� ���� ������ ����
                    if (fieldPosX < 0 || fieldPosX >= myField.cellCTRLs.GetLength(0) ||
                        fieldPosY < 0 || fieldPosY >= myField.cellCTRLs.GetLength(1) ||
                        !myField.cellCTRLs[fieldPosX, fieldPosY]
                        )
                    {
                        continue;
                    }

                    //������� ����� �������� ������ ���� ������
                    float time = Vector2.Distance(new Vector2(), new Vector2(x, y)) * 0.05f;
                    //
                    myField.cellCTRLs[fieldPosX, fieldPosY].BufferCombination = combination;
                    myField.cellCTRLs[fieldPosX, fieldPosY].BufferNearDamage = false;
                    myField.cellCTRLs[fieldPosX, fieldPosY].DamageInvoke(time);
                }
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
                //������� �����
                GameObject flyObj = Instantiate(FlyPrefab, myField.parentOfFly);
                FlyCTRL flyCTRL = flyObj.GetComponent<FlyCTRL>();


                //���� ������ � ������� ��� ����� �� �����
                foreach (CellCTRL cellPriority in myField.cellsPriority)
                {
                    if (cellPriority == myCell) { 
                        continue; 
                    }

                    //���� ����� ��� ������ � ������ ����� �� ��������� ������� � ������������� �����
                    bool found = false;
                    foreach (FlyCTRL fly in FlyCTRL.flyCTRLs)
                    {
                        if (fly.CellTarget == cellPriority)
                        {
                            found = true;
                            break;
                        }
                    }

                    //���� ��������� ������� � �� ����� ������ � ������, ������ ��� �� ��� ����� ������� � �������� ����� ����
                    if (!found)
                    {
                        flyCTRL.inicialize(myCell, cellPriority, partner, combination);

                        break;
                    }


                }
            }

            void CreatePartner() {
                //������� �����
                GameObject flyObj = Instantiate(FlyPrefab, myField.parentOfFly);
                FlyCTRL flyCTRL = flyObj.GetComponent<FlyCTRL>();


                //���� ������ � ������� ��� ����� �� �����
                foreach (CellCTRL cellPriority in myField.cellsPriority)
                {
                    //���� ����� ��� ������ � ������ ����� �� ��������� ������� � ������������� �����
                    bool found = false;
                    foreach (FlyCTRL fly in FlyCTRL.flyCTRLs)
                    {
                        if (fly.CellTarget == cellPriority)
                        {
                            found = true;
                            break;
                        }
                    }

                    //���� ��������� ������� � �� ����� ������ � ������, ������ ��� �� ��� ����� ������� � �������� ����� ����
                    if (!found)
                    {
                        flyCTRL.inicialize(partner.myCell, cellPriority, null, combination);

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

        if (activateNeed && !activate) {
            Activate(BufferActivateType, BufferPartner, BufferCombination);
        }
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
    }

    public void IniFly(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor, int myCombIDFunc)
    {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.airplane;
        color = internalColor;
        Image.texture = TextureFly;
    }

    public void IniSuperColor(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor, int myCombIDFunc)
    {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.supercolor;
        color = internalColor;
        Image.texture = TextureSuperColor;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;
    }

    public void IniRocketVertical(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor, int myCombIDFunc)
    {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.rocketVertical;
        color = internalColor;
        Image.texture = TextureRocketVertical;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;
    }
    public void IniRocketHorizontal(CellCTRL myCellNew, GameFieldCTRL gameField, InternalColor internalColor, int myCombIDFunc)
    {

        myCell = myCellNew;
        myField = gameField;
        MyCombID = myCombIDFunc;
        setColor(internalColor);

        myCell.cellInternal = this;
        PosToCell();

        type = Type.rocketHorizontal;
        color = internalColor;
        Image.texture = TextureRocketHorizontal;
        myCellNew.myInternalNum = myCellNew.GetNextLastInternalNum;
    }


    void AnimationDroppedDown(bool dropped) {
        animatorMove.SetBool("DroppedDown" , dropped);
    }
    public void StopAnimationDroppedDown() {
        AnimationDroppedDown(false);
    }

    void AnimationDroppedLeft(bool dropped) {
        animatorMove.SetBool("DroppedLeft", dropped);
    }
    public void StopAnimationDroppedLeft()
    {
        AnimationDroppedLeft(false);
    }

    void AnimationDroppedRight(bool dropped) {
        animatorMove.SetBool("DroppedRight", dropped);
    }
    public void StopAnimationDroppedRight()
    {
        AnimationDroppedRight(false);
    }


    public void AnimationBombActivated() {
        animatorExplose.SetBool("ActiveBomb", true);
    }
}
