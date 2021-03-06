using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//?????
//??????
/// <summary>
/// ?????????? ???????????? ????? ?????
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

    //??? ????
    public GameFieldCTRL myField;
    //??? ??????
    public CellCTRL myCell;
    public Vector2Int myDeath = new Vector2Int(999,999); //??????? ??? ???????????????, ??????? ???? ?????????? ?? ???????? ?? ?????????
    public int MyCombID = 0;

    public RectTransform rectMy;
    RectTransform rectCell;

    [SerializeField]
    RawImage Image;
    [SerializeField]
    RawImage LastImage;

    [SerializeField]
    Color red = Color.red;
    [SerializeField]
    Color green = Color.green;
    [SerializeField]
    Color blue = Color.blue;
    [SerializeField]
    Color yellow = Color.yellow;
    [SerializeField]
    Color orange = Color.white;

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
    Texture2D TextureOrange;
    [SerializeField]
    Texture2D TextureOrengeCore;
    [SerializeField]
    Texture2D TextureColor5;
    [SerializeField]
    Texture2D TextureUltimative;
    [SerializeField]
    Texture2D TextureBomb;
    [SerializeField]
    Texture2D TextureBombLast;
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
        blocker //?????? ??????? ?????? ? ???????????? ??????????????? ??????, ?? ???????? ??? ????
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
            result = orange;
        }

        return result;
    }


    //????????????? ?????????? ?????? ?? ????, ?? ???? ?? ????? ?????????
    public void SetFullMask2D(MaskType maskType) {
        if (myMask2D == null) myMask2D = gameObject.AddComponent<RectMask2D>();

        Vector4 mask2D = myMask2D.padding;

        //????????? ??????
        if (maskType == MaskType.top) {
            mask2D.w = 100;
        }
        //????????? ?????
        else if (maskType == MaskType.left) {
            mask2D.x = 100;
        }
        //????????? ??????
        else if (maskType == MaskType.right) {
            mask2D.z = 100;
        }
        //????????? ?????
        else if (maskType == MaskType.bot) {
            mask2D.y = 100;
        }
        myMask2D.padding = mask2D;
    }

    public void IniRect() {
        if (this == null) return; //???????? ?? ????????????

        rectMy = gameObject.GetComponent<RectTransform>();

        if (myCell)
            rectCell = myCell.GetComponent<RectTransform>();
        else {
            Destroy(gameObject);
        }
        
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

    static public float timeLastPlayDestroy = 0; //??? ????? ??? ?????????????? ???? ???????????

    public float MovingSpeed = 0;

    public bool EndMoveAndRemove = false; //??? ?????? ???????? ??????? ?????????? ?????? ???? ???????????????. ??? ????????? ???? ???? ?????
    void Moving() {
        //???????? ? ??????

        //???? ? ??????????? ??????? ??? ?????? ? ??????? ??? ?????????? ??? ??? ?? ??????
        //???????? ??? ???????? ? ????? ???????????????

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


            MovingSpeed += Time.unscaledDeltaTime * 1.35f;

            float speed = 0.09f + MovingSpeed;
            speed *= Gameplay.main.timeScale;

            float correctY = 0;
            //???? ?????? ?????? ????????? ?????? ? ? ???? ???????? ??????? ???????
            if (myCell != null &&
                myCell.pos.y < myField.cellCTRLs.GetLength(1) - 1 && 
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y+1] &&
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y+1].cellInternal &&
                Vector2.Distance(
                    new Vector2(myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.x, myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.y), 
                    new Vector2(rectMy.pivot.x, rectMy.pivot.y)) < 1 &&
                MovingSpeed < myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.MovingSpeed) {
                //????????
                MovingSpeed = myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.MovingSpeed;

                //??????????
                correctY = 1 - Vector2.Distance(
                    new Vector2(myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.x, myField.cellCTRLs[myCell.pos.x, myCell.pos.y + 1].cellInternal.rectMy.pivot.y),
                    new Vector2(rectMy.pivot.x, rectMy.pivot.y));
            }


            float posXnew = rectMy.pivot.x;
            float posYnew = rectMy.pivot.y;
            //???? ???? ??????? ??????
            if (myDeath.x < 100 || myDeath.y < 100)
            {
                posXnew = Calculate.GetValueLinear(rectMy.pivot.x, myDeath.x, speed);
                posYnew = Calculate.GetValueLinear(rectMy.pivot.y, myDeath.y, speed);
            }
            //????? ??????? ???????? ? ??????????? ??????
            else
            {
                posXnew = Calculate.GetValueLinear(rectMy.pivot.x, rectCell.pivot.x, speed);
                posYnew = Calculate.GetValueLinear(rectMy.pivot.y, rectCell.pivot.y, speed);
            }

            if (myDeath.y < 100 || myDeath.x < 100) {
                float test = 0;
            }


            if (myMask2D != null && myMask2D.padding != myMaskNeed ||
                myMaskNow != myMaskNeed || myMaskNow.x != 0 || myMaskNow.y != 0 || myMaskNow.z != 0 || myMaskNow.w != 0) {
                if (myMask2D == null) myMask2D = gameObject.AddComponent<RectMask2D>();

                //???????? ?????
                float maskCoof = 100;

                Vector4 mask2D = myMask2D.padding;

                mask2D.x = Calculate.GetValueLinear(mask2D.x, myMaskNeed.x, mask2D.x/100 + speed * maskCoof);
                mask2D.y = Calculate.GetValueLinear(mask2D.y, myMaskNeed.y, mask2D.y/100 + speed * maskCoof);
                mask2D.z = Calculate.GetValueLinear(mask2D.z, myMaskNeed.z, mask2D.z/100 + speed * maskCoof);
                mask2D.w = Calculate.GetValueLinear(mask2D.w, myMaskNeed.w, mask2D.w/100 + speed * maskCoof);

                //?????????? ????? ?????
                myMask2D.padding = mask2D;
                myMaskNow = myMask2D.padding;

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

            //???? ??????? ????? ????? ??????????
            else if (rectCell.pivot.x == posXnew &&
                rectCell.pivot.y == posYnew) {

                //???? ????? ?????? ???
                CellCTRL cellMove = GetFreeCellDown();
                if (cellMove)
                {
                    //?????????? ????? ???? ??? ????????
                    StartMove(cellMove);
                }
                else
                {
                    //???????? ????????
                    isMove = false;
                    myCell.CalcMyPriority();
                    //????????? ???????? ?????????

                    animatorCTRL.PlayAnimation("DroppedDown");
                }
            }

            //??????????? ?????????
            rectMy.pivot = new Vector2(posXnew, posYnew);

            //????????? ????? ?????????? ???????????
            timeLastMoving = Time.unscaledTime;
        }

        //????????? ????? ??????? ????????? ?????? //?????? ?????????? ?????? ? ????? ??? ???????
        else if(myField.buffer.CalculateFrameNow == 0) {
            
            trailSpawned = false;
            CellCTRL cellMove = GetFreeCellDown();
            if (cellMove)
                StartMove(cellMove);
        }
        
    }

    /// <summary>
    /// ???????? ????????? ?????? ?????
    /// </summary>
    CellCTRL GetFreeCellDown() {
        CellCTRL returnCell = null;

        //??????? ???? ??????? ? ???? ?????? ????????? ? ?????
        if (myField.cellCTRLs[myCell.pos.x, myCell.pos.y].rock > 0 || myField.cellCTRLs[myCell.pos.x, myCell.pos.y].wallID == 15)
            return null;


        //???????? ????????? ?????? ?????
        for (int minusY = 1; minusY < myField.cellCTRLs.GetLength(1); minusY++) {
            if (myCell.pos.y - minusY >= 0 && //???? ?? ????? ?? ??????
                myField.CheckObstaclesToMoveDown(myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY], myField.cellCTRLs[myCell.pos.x, myCell.pos.y - (minusY - 1)]) &&
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].cellInternal && //? ??? ????????
                Time.unscaledTime - myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].timeBoomOld > 0.35f && //c ??????????? ????? ?????
                Time.unscaledTime - myField.timeLastBoom > 0)
                
            {
                //?????? ????? ?????? ??? ??????
                returnCell = myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY];
            }
            //?????? ????? ????? ???
            else {
                //???????
                break;
            }
        }

        //???? ????? ????? ??? ?????????
        //???????? ????????? ?????? ?????? ??? ????? ????
        if (returnCell == null) {
            //????????? ???????????? ??????

            /*
            bool canTestingRight = true;
            bool canTestingLeft = true;
            */

            //????? ???????? ?? ??????
            int smeshenie = 1;
            if (myCell.pos.y - smeshenie >= 0) //???? ?? ????? ?? ??????
            {
                if (myCell.pos.x == 3 && myCell.pos.y == 5 ) {
                    bool test = false;
                }
                //???? ??????????? ??????, ?? ??????? ?? ???????? ??????
                //int internalNum = CellCTRL.GetNowLastInternalNum;
                //??????
                if (myCell.pos.x + smeshenie < myField.cellCTRLs.GetLength(0) && //???? ?? ????? ?? ??????
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie] != null &&
                    //GameFieldCTRL.main.CheckObstaclesToMoveUp(myField.cellCTRLs[myCell.pos.x, myCell.pos.y], myField.cellCTRLs[myCell.pos.x, myCell.pos.y - smeshenie]) &&
                    //GameFieldCTRL.main.CheckObstaclesToMoveDown(myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie], myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y]) &&
                    !myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].cellInternal && //? ??? ????????
                    Time.unscaledTime - myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].timeBoomOld > 0.35f &&
                    Time.unscaledTime - myField.timeLastBoom > 0 &&
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].rock == 0 &&
                    myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie].Box == 0 &&
                    isCanMoveToThisColum(myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie]) &&//? ???? ??????? ??? ?????????????? ????????????? ????????

                    //??? ???????????? ???????
                    notBlockMoveFromToRight(myCell, myField.cellCTRLs[myCell.pos.x+1, myCell.pos.y -1], myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1], myField.cellCTRLs[myCell.pos.x + 1, myCell.pos.y])

                    //???????? ??? ?????? ? ?????? ?????? ?????? ?? ???????? ?????
                    //canMoveRightDown(myField.cellCTRLs[myCell.pos.x + 1, myCell.pos.y], myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1])
                    )
                {
                    //?????? ????? ?????? ??? ???????
                    returnCell = myField.cellCTRLs[myCell.pos.x + smeshenie, myCell.pos.y - smeshenie];
                }
                //?????
                else if (myCell.pos.x - smeshenie >= 0 && //???? ?? ????? ?? ??????
                                                          //GameFieldCTRL.main.CheckObstaclesToMoveUp(myField.cellCTRLs[myCell.pos.x, myCell.pos.y], myField.cellCTRLs[myCell.pos.x, myCell.pos.y - smeshenie]) &&
                                                          //GameFieldCTRL.main.CheckObstaclesToMoveDown(myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie], myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y]) &&
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie] != null &&
                    !myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].cellInternal && //? ??? ????????
                    Time.unscaledTime - myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].timeBoomOld > 0.35f &&
                    Time.unscaledTime - myField.timeLastBoom > 0 &&
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].rock == 0 &&
                    myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie].Box == 0 &&
                    isCanMoveToThisColum(myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie]) && //? ???? ??????? ??? ?????????????? ????????????? ????????
                    //??? ???????????? ???????
                    notBlockMoveFromToLeft(myCell, myField.cellCTRLs[myCell.pos.x- 1, myCell.pos.y - 1], myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1], myField.cellCTRLs[myCell.pos.x - 1, myCell.pos.y])
                    //???????? ??? ????? ? ?????? ?????? ?????? ?? ???????? ?????
                    //canMoveLeftDown(myField.cellCTRLs[myCell.pos.x-1, myCell.pos.y], myField.cellCTRLs[myCell.pos.x, myCell.pos.y-1])
                    )
                {
                    //?????? ????? ?????? ??? ???????
                    returnCell = myField.cellCTRLs[myCell.pos.x - smeshenie, myCell.pos.y - smeshenie];
                }
            }
        }

        return returnCell;
        //test
        //???????? ????? ?? ????????? ????? ? ??? 
        bool isCanMoveToThisColum(CellCTRL cellFunc) {
            bool result = true; //?????????? ????????? ?????????

            //????????? ??? ?? ?? ??? ????? ?? ???????? ? ?????? ?????
            for (int minus = 1; minus < myField.cellCTRLs.GetLength(1) && result; minus++) {
                if (cellFunc.pos.y - minus >= 0 &&//???? ?? ????? ?? ??????? ???????
                    GameFieldCTRL.main.CheckObstaclesToMoveDown(myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus], myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - (minus - 1)]) &&
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus] && //???? ??????
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].Box <= 0 &&//?????? ????????? ??? ?????????? ????????
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].rock <= 0)
                {
                    //????????? ????
                    if (
                        (myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].cellInternal && myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].cellInternal.isMove && minus <= 1) || // ?????? ???? ?????? ??????? ????????? ? ????????
                        Time.unscaledTime - myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y - minus].timeBoomOld < 0.25f//????? ???????? ????? ?????? ?? ?????
                        ) 
                    {
                        result = false;
                    }

                }
                else {
                    break;
                }
            }

            //???????? ??????
            for (int plus = 1; plus < myField.cellCTRLs.GetLength(1) && result; plus++) {
                //????????? ???? ?? ?? ??? ???? ????????? ??????? ????? ?? ??????
                if (cellFunc.pos.y + plus < myField.cellCTRLs.GetLength(1) &&//???? ?? ????? ?? ??????? ???????
                    GameFieldCTRL.main.CheckObstaclesToMoveUp(myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus], myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + (plus - 1)]) &&
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus] && //???? ??????
                    myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].Box <= 0)//?????? ????????? ??? ?????????? ????????
                    
                {
                    //????????? ????
                    if (
                        (myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].cellInternal) || // ?????? ???? ?????? ??????? ???????? ????? ?????? ????????
                        Time.unscaledTime - myField.cellCTRLs[cellFunc.pos.x, cellFunc.pos.y + plus].timeBoomOld < 0.25f //????? ???????? ????? ?????? ?? ?????
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

        bool canMoveRightDown(CellCTRL right, CellCTRL down) {
            
            //?????????? ????? ??????
            bool result = true;

            //???? ?????? ?? ????????? ???????? ? ?????? ???? ??????

            bool freeRight = true;
            //???????? ??????
            if (right == null ||
                right != null &&
                right.wallID != 0 && (
                right.wallID == 4 ||
                right.wallID == 6 ||
                right.wallID == 7 ||
                right.wallID == 8 ||
                right.wallID == 9 ||
                right.wallID == 10 ||
                right.wallID == 11 ||
                right.wallID == 12 ||
                right.wallID == 13 ||
                right.wallID == 14 ||
                right.wallID == 15
                )) {
                freeRight = false;
            }


            //???????? ?????
            bool freeDown = true;
            if (down == null ||
                down != null &&
                down.wallID != 0 && (
                down.wallID == 1 ||
                down.wallID == 2 ||
                down.wallID == 5 ||
                down.wallID == 6 ||
                down.wallID == 8 ||
                down.wallID == 9 ||
                down.wallID == 10 ||
                down.wallID == 11 ||
                down.wallID == 12 ||
                down.wallID == 13 ||
                down.wallID == 14 ||
                down.wallID == 15 ||
                myCell.teleport != 0 //???? ????? ????????
                )) {
                freeDown = false;
            }

            if (!freeDown && !freeRight) {
                result = false;
            }

            return result;
        }
        bool canMoveLeftDown(CellCTRL left, CellCTRL down)
        {

            //?????????? ????? ??????
            bool result = true;

            //???? ?????? ?? ????????? ???????? ? ?????? ???? ??????

            bool freeLeft = true;
            //???????? ?????
            if (left == null ||
                left != null &&
                left.wallID != 0 && (
                left.wallID == 2 ||
                left.wallID == 3 ||
                left.wallID == 5 ||
                left.wallID == 6 ||
                left.wallID == 7 ||
                left.wallID == 9 ||
                left.wallID == 10 ||
                left.wallID == 11 ||
                left.wallID == 12 ||
                left.wallID == 13 ||
                left.wallID == 14 ||
                left.wallID == 15
                ))
            {
                freeLeft = false;
            }


            //???????? ?????
            bool freeDown = true;
            if (down == null ||
                down != null &&
                down.wallID != 0 && (
                down.wallID == 1 ||
                down.wallID == 4 ||
                down.wallID == 5 ||
                down.wallID == 7 ||
                down.wallID == 8 ||
                down.wallID == 9 ||
                down.wallID == 10 ||
                down.wallID == 11 ||
                down.wallID == 12 ||
                down.wallID == 13 ||
                down.wallID == 14 ||
                down.wallID == 15 ||
                myCell.teleport != 0 //???? ????? ????????
                ))
            {
                freeDown = false;
            }

            if (!freeDown && !freeLeft)
            {
                result = false;
            }

            return result;
        }

        //???????? ???? ?? ???????? ? ???
        bool notBlockMoveFromToRight(CellCTRL from, CellCTRL target, CellCTRL DownC, CellCTRL RightC)
        {
            //?????????? ????????? ?????
            bool result = true;

            if (myCell.pos.x == 2 && myCell.pos.y == 2)
            {
                bool test = true;
            }

            //??????????? 4 ????? ?????????? ???????????? ????? 4 ???????? ??????? ???? ??????????? ? ??? ???????
            bool Up = false;
            bool Down = false;
            bool Left = false;
            bool Right = false;

            //????????? ????
            if (from.wallID == 2 || from.wallID == 5 || from.wallID == 6 || from.wallID == 10 || from.wallID == 11 || from.wallID == 13 || from.wallID == 14 || from.wallID == 15 ||
                RightC != null && (RightC.wallID == 4 || RightC.wallID == 7 || RightC.wallID == 8 || RightC.wallID == 10 || RightC.wallID == 11 || RightC.wallID == 12 || RightC.wallID == 13 || RightC.wallID == 15)) {
                Up = true;
            }
            //???????? ????
            if (from.wallID == 3 || from.wallID == 6 || from.wallID == 7 || from.wallID == 9 || from.wallID == 11 || from.wallID == 12 || from.wallID == 14 || from.wallID == 15 || from.teleport != 0 ||
                DownC != null && (DownC.wallID == 1 || DownC.wallID == 5 || DownC.wallID == 8 || DownC.wallID == 9 || DownC.wallID == 12 || DownC.wallID == 13 || DownC.wallID == 14 || DownC.wallID == 15)) {
                Left = true;
            }
            //???????? ?????
            if (target.wallID == 1 || target.wallID == 5 || target.wallID == 8 || target.wallID == 9 || target.wallID == 12 || target.wallID == 13 || target.wallID == 14 || target.wallID == 15 ||
                RightC != null && (RightC.wallID == 3 || RightC.wallID == 6 || RightC.wallID == 7 || RightC.wallID == 9 || RightC.wallID == 11 || RightC.wallID == 12 || RightC.wallID == 14 || RightC.wallID == 15 || RightC.teleport != 0)) {
                Right = true;
            }
            //???????? ???
            if (target.wallID == 4 || target.wallID == 7 || target.wallID == 8 || target.wallID == 10 || target.wallID == 11 || target.wallID == 12 || target.wallID == 13 || target.wallID == 15 ||
                DownC != null && (DownC.wallID == 2 || DownC.wallID == 5 || DownC.wallID == 6 || DownC.wallID == 10 || DownC.wallID == 11 || DownC.wallID == 13 || DownC.wallID == 14 || DownC.wallID == 15)) {
                Down = true;
            }

            //???????? ??????????? ?????????? ????
            if (Up && Left || 
                Down && Right || 
                Up && Down ||
                Left && Right) {

                //???????? ?? ????????
                result = false;
                return result;
            }


            return result;
        }
        bool notBlockMoveFromToLeft(CellCTRL from, CellCTRL target, CellCTRL DownC, CellCTRL LeftC)
        {
            //?????????? ????????? ?????
            bool result = true;

            if (myCell.pos.x == 2 && myCell.pos.y == 2)
            {
                bool test = true;
            }

            //??????????? 4 ????? ?????????? ???????????? ????? 4 ???????? ??????? ???? ??????????? ? ??? ???????
            bool Up = false;
            bool Down = false;
            bool Left = false;
            bool Right = false;

            //????????? ????
            if (from.wallID == 4 || from.wallID == 7 || from.wallID == 8 || from.wallID == 10 || from.wallID == 11 || from.wallID == 12 || from.wallID == 13 || from.wallID == 15 ||
                LeftC != null && (LeftC.wallID == 2 || LeftC.wallID == 5 || LeftC.wallID == 6 || LeftC.wallID == 10 || LeftC.wallID == 11 || LeftC.wallID == 13 || LeftC.wallID == 14 || LeftC.wallID == 15))
            {
                Up = true;
            }
            //???????? ?????
            if (from.wallID == 3 || from.wallID == 6 || from.wallID == 7 || from.wallID == 9 || from.wallID == 11 || from.wallID == 12 || from.wallID == 14 || from.wallID == 15 || from.teleport != 0 ||
                DownC != null && (DownC.wallID == 1 || DownC.wallID == 5 || DownC.wallID == 8 || DownC.wallID == 9 || DownC.wallID == 12 || DownC.wallID == 13 || DownC.wallID == 14 || DownC.wallID == 15))
            {
                Left = true;
            }
            //???????? ????
            if (target.wallID == 1 || target.wallID == 5 || target.wallID == 8 || target.wallID == 9 || target.wallID == 12 || target.wallID == 13 || target.wallID == 14 || target.wallID == 15 ||
                LeftC != null && (LeftC.wallID == 3 || LeftC.wallID == 6 || LeftC.wallID == 7 || LeftC.wallID == 9 || LeftC.wallID == 11 || LeftC.wallID == 12 || LeftC.wallID == 14 || LeftC.wallID == 15 || LeftC.teleport != 0))
            {
                Right = true;
            }
            //???????? ???
            if (target.wallID == 2 || target.wallID == 5 || target.wallID == 6 || target.wallID == 10 || target.wallID == 11 || target.wallID == 13 || target.wallID == 14 || target.wallID == 15 ||
                DownC != null && (DownC.wallID == 4 || DownC.wallID == 7 || DownC.wallID == 8 || DownC.wallID == 10 || DownC.wallID == 11 || DownC.wallID == 12 || DownC.wallID == 13 || DownC.wallID == 15))
            {
                Down = true;
            }

            //???????? ??????????? ?????????? ????
            if (Up && Left ||
                Down && Right ||
                Up && Down ||
                Left && Right)
            {

                //???????? ?? ????????
                result = false;
                return result;
            }


            return result;
        }

    }

    /// <summary>
    /// ????????? ?? ? ???????? ?????? ??????
    /// </summary>
    public bool isMove = false;
    /// <summary>
    /// ???????? ? ????????? ??????
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

        //??????????? ?????? ??????
        myCell.cellInternal = this;
        myCell.myInternalNum = myCell.GetNextLastInternalNum; //?????????? ????????

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
    /// ??????? ??????
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
            myCell.timeBoomOld = Time.unscaledTime; //?????? ????? ??????

            int scoreNow = score;
            if (!Gameplay.main.playerTurn) {
                //???? ???????? ????? ?????? ????? ? ?????? ???? 
                if (EnemyController.MoveCount <= EnemyController.MoveCountForPlayer)
                {
                    scoreNow = scoreNow * 2;
                }
                else {
                    scoreNow = (int)(scoreNow * 0.62f);
                }
            }

            if (type == Type.color) {
                //GameObject PrefabDie = Instantiate(myField.PrefabParticleDie, myField.parentOfScore);
                //RectTransform rectDie = PrefabDie.GetComponent<RectTransform>();
                //rectDie.pivot = rectMy.pivot;

                GameObject PrefabScore = Instantiate(myField.PrefabParticleScore, myField.parentOfScore);
                PrefabScore.GetComponent<ScoreCTRL>().Inicialize(scoreNow, ConvertEnumColor());
                Gameplay.main.ScoreUpdate(scoreNow);
                RectTransform rectScore = PrefabScore.GetComponent<RectTransform>();

                //?????????? ???????
                Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateCellDamage(myField.transform, myCell);
                Color color1 = GetColor(color);
                Vector3 colorVec = new Vector3(color1.r, color1.g, color1.b);
                colorVec.Normalize();
                color1 = new Color(color1.r, color1.g, color1.b);
                particle3DCTRL.SetColor(color1);

                //???????? ?????????? ?????
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
        //??????? ???????????
        else {
            DestroyObj();
        }

        //??????????? ?? ?????? ??? ????????? ????? ? ???????????
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
            animatorCTRL.SetFloat("StayType", 0);
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
                LastImage.texture = null;
                //Image.color = red;
            }
            else if (internalColor == InternalColor.Green)
            {
                Image.texture = TextureGreen;
                LastImage.texture = TextureGreenCore;
                //Image.color = green;
            }
            else if (internalColor == InternalColor.Blue)
            {
                Image.texture = TextureBlue;
                LastImage.texture = TextureBlueCore;
                //Image.color = blue;
            }
            else if (internalColor == InternalColor.Yellow)
            {
                Image.texture = TextureYellow;
                LastImage.texture = TextureYellowCore;
                //Image.color = yellow;
            }
            else if (internalColor == InternalColor.Violet)
            {
                Image.texture = TextureOrange;
                LastImage.texture = TextureOrengeCore;
                //Image.color = violet;
            }
            else if (internalColor == InternalColor.Ultimate) {
                Image.texture = TextureUltimative;
                LastImage.texture = null;
                //Image.color = Ultimative;
            }

        }

        if (type == Type.airplane) {
            Image.texture = TextureFly;
            setInternalColor();
            LastImage.texture = TextureFlyLast;
        }
        else if (type == Type.bomb)
        {
            Image.texture = TextureBomb;
            setInternalColor();
            LastImage.texture = TextureBombLast;
        }
        else if (type == Type.rocketHorizontal)
        {
            Image.texture = TextureRocketHorizontal;
            setInternalColor();
            LastImage.texture = null;
        }
        else if (type == Type.rocketVertical)
        {
            Image.texture = TextureRocketVertical;
            setInternalColor();

            LastImage.texture = null;
        }
        else if (type == Type.color5)
        {
            animatorCTRL.SetFloat("StayType", 2);
            Image.texture = TextureColor5;
            Image.color = new Color(1, 1, 1, 1);
            LastImage.texture = null;
        }
        else if (type == Type.blocker) {
            Image.texture = TextureBlocker;
            Image.color = new Color(1, 1, 1, 1);

            LastImage.texture = null;
        }

        color = internalColor;

        if (LastImage.texture == null) {
            LastImage.gameObject.SetActive(false);
        }
        else {
            LastImage.gameObject.SetActive(true);
            LastImage.color = new Color(1,1,1,1);
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
                Image.color = orange;
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

        //???? ????? ?????
        if (random == 0)
        {
            //???? ????? ???? ?????????? ????????????? ???? ?? ???????
            if (Random.Range(0, 100) < Gameplay.main.superColorPercent)
            {
                Result.color = InternalColor.Ultimate;
            }
            //????? ???????
            else
            {
                Result.color = GetColorBasic();
            }
        }
        //???? ?????????? ??????????
        else if (random == 1 && isSpawn) {
            //???? ????? ???? ?????????? ??????????
            if (Random.Range(0, 100) < Gameplay.main.typeBlockerPercent) {
                Result.color = InternalColor.Red;
                Result.type = Type.blocker;
            }
            //????? ???????
            else {
                Result.color = GetColorBasic();
            }
        }

        //????? ??????? ????
        else {
            Result.color = GetColorBasic();
        }


        

        return Result;


        //???????? ????? ??????? ????
        InternalColor GetColorBasic() {
            InternalColor colorReturn = InternalColor.Red;

            int random = Random.Range(0, Gameplay.main.colors);
            //????
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

    //?????????? ???????????? ??????
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

        //????????? ??????????? ?????? ???? ??? ?? ???? ????? ?????????? ??????? ??? ??????? ???? ??????
        if (combination != null &&
            combination.ID == MyCombID &&
            !isMove //???? ?????? ?? ? ????????
            ) return;

        //????? ?????? ?????????????? ?????? ????? ?????, ? ???? ?? ?????? ???? ????????? ?????
        if (activateNeed && type == Type.bomb && myCell.rock == 0 &&
            myCell.pos.y > 0 && 
            myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1] != null &&
            myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1].cellInternal == null &&
            myField.cellCTRLs[myCell.pos.x, myCell.pos.y - 1].Box == 0 &&
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

        //???? ?????????? ???, ?? ???????
        if (BufferCombination == null)
        {
            BufferCombination = new GameFieldCTRL.Combination();
            //????????? ? ?????? ??????????
            BufferCombination.cells.Add(myCell);
            //????????? ???????? ???? ?? ????
            if (partner != null)
                BufferCombination.cells.Add(partner.myCell);

            BufferCombination.TestCells();
        }

        if (!needInstantDamage)
        {
            if (isMove || Time.unscaledTime - timeLastMoving < 1f) return;
        }
        //????????? ??????????? ????
        needInstantDamage = false;
        timeLastMoving = Time.unscaledTime;


        //???? ???????? ???
        if (partner == null)
        {
            if (ActivateType == Type.bomb) ActivateBomb(BoombRadius);
            else if (ActivateType == Type.rocketHorizontal) ActivateRocket(true, false);
            else if (ActivateType == Type.rocketVertical) ActivateRocket(false, true);
            else if (ActivateType == Type.color5) ActivateSuperColor();
            else if (ActivateType == Type.airplane) ActivateFly();
        }
        else {
            //????? ????? + ??? ??????
            if (ActivateType == Type.color5)
            {
                ActivateSuperColor();
            }
            //????? + ??????
            else if ((ActivateType == Type.bomb && partner.type == Type.rocketHorizontal) ||
                (ActivateType == Type.rocketHorizontal && partner.type == Type.bomb) ||
                (ActivateType == Type.bomb && partner.type == Type.rocketVertical) ||
                (ActivateType == Type.rocketVertical && partner.type == Type.bomb))
            {
                ActivateBombAndRocket();
            }
            //????? + ?????
            else if (ActivateType == Type.bomb && partner.type == Type.bomb)
            {
                ActivateBomb(2);
            }
            //?????? + ??????
            else if ((ActivateType == Type.rocketHorizontal || ActivateType == Type.rocketVertical) &&
                (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical))
            {
                ActivateRocket(true, true);
            }

            //??????? + ???
            else if (ActivateType == Type.airplane)
            {
                ActivateFly();
            }

            //????? + ???????
            else if (ActivateType == Type.bomb && partner.type == Type.airplane)
            {
                partner.type = Type.bomb;
                ActivateFly();
            }

            //???? ??????? ????? ????
            //??? ????????? ? ???????? ????? ?????? ????????
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
            //?????????? ?????? ???? ????? ??? ?? ???????
            if (activate) return;
            
            //???? ????????? ? ?????? ???????
            if (partner && partner != this && (partner.type == Type.rocketHorizontal || partner.type == Type.rocketVertical)) {
                horizontal = true;
                vertical = true;

                //??????? ????????
                partner.activate = false;
                partner.DestroyObj();
            }

            //?? ??? ???????
            if (horizontal && vertical) {
                if (myCell.explosion == null)
                {
                    myCell.explosion = new CellCTRL.Explosion(true, true, true, true, 0.05f, BufferCombination, GetColor(color));
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
            //??????????????
            else if (horizontal) {

                if (myCell.explosion == null)
                {
                    myCell.explosion = new CellCTRL.Explosion(true, true, false, false, 0.05f, BufferCombination, GetColor(color));
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
            //????????????
            else if (vertical) {

                if (myCell.explosion == null)
                {
                    myCell.explosion = new CellCTRL.Explosion(false, false, true, true, 0.05f, BufferCombination, GetColor(color));
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
            //?????????? ?????? ???? ????? ??? ?? ???????
            if (activate) return;

            activate = true;

            Type typePartner = Type.none;
            if (partner != null) {
                typePartner = partner.type;
            }

            //????, ?? ??????? ????????
            InternalColor replaceColor;
            //???? ???????????? ??? ?? ????? 5 ?? ??????? ? ?????5 ? ?????? ??? ???? ????? 5 ??? ???? ???????
            //(???????? ????? ????? ???????? ?????? ????? ??? ?????? ?? ???????? ? ????????? ?? ?? ????? ????)
            //???? ????????? ?? ??????????
            if (ActivateType != Type.color5 && partner.type == Type.color5 || partner == null && ActivateType != Type.color5)
            {
                typePartner = ActivateType;
                replaceColor = CalculateColorPriority();
            }
            //???? ???? ??????? ? ?? ?? ???????????
            else if (partner != null && ActivateType != Type.color5)
            {
                //????? ???? ????????
                replaceColor = partner.color;
            }
            //???? ??????? ????, ?? ?? ???????????, ?? ?? ???????????
            else if (partner != null && ActivateType == Type.color5 && partner.type != Type.color5)
            {
                //?? ????? ???? ????????
                replaceColor = partner.color;
            }
            //????? ????? ???? ??????????? ????
            else
            {
                replaceColor = color;
            }

            //???????? ??? ?????????
            //???? ??????? ?????? ????
            if (typePartner == Type.color)
            {
                DestroyAllColor(partner.color);
            }
            //???? ??????? ?????, ?????? ??? ???????
            else if (typePartner == Type.bomb ||
                typePartner == Type.airplane ||
                typePartner == Type.rocketHorizontal ||
                typePartner == Type.rocketVertical
                )
            {
                replacementColorAndActivate();
            }
            //??????? ????? ?? ?????
            else if (typePartner == Type.color5) {
                DestroyAll();
            }
            else {
                DestroyAllColor(color);
            }

            SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipExploseColor5);

            /*
            void DestroyAllRocket(InternalColor internalColor)
            {

                float destroyNum = 0;
                //????????? ??? ?????? ?? ?????????? ??????
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
                            //??????? ?????? ??????
                            Destroy(myField.cellCTRLs[x, y].cellInternal.gameObject);

                            //?????? ??????? ????? ?????? ??????
                            GameObject internalObj = Instantiate(myField.prefabInternal, myField.parentOfInternals);
                            CellInternalObject cellInternalObject = internalObj.GetComponent<CellInternalObject>();
                            cellInternalObject.myField = myField;

                            if (Random.Range(0, 100) < 50) {
                                cellInternalObject.setColorAndType(internalColor, Type.rocketVertical);
                            }
                            else {
                                cellInternalObject.setColorAndType(internalColor, Type.rocketHorizontal);
                            }
                            //?????????? ?????? ?? ????? ???????
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
                //????????? ??? ???? ?? ?????
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
                {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {

                        //????????? ??? ?????? ????
                        if (!myField.cellCTRLs[x, y] || //???? ?????? ???
                            !myField.cellCTRLs[x, y].cellInternal || //???? ??? ????????????
                            myField.cellCTRLs[x, y].cellInternal.color != replaceColor || //???? ???? ?? ????????? ? ?????? ????????
                            myField.cellCTRLs[x, y].cellInternal.type != Type.color //???? ??? ??????? ?????????
                                                                                    //myField.cellCTRLs[x,y].cellInternal.type == Type.color5
                            )
                        {
                            continue;
                        }
                        float dist = Vector2.Distance(myField.cellCTRLs[x, y].pos, myCell.pos);

                        //??????? ?????? ?? ????????? ??????? ? ?????????? ?????????
                        myField.cellCTRLs[x, y].cellInternal.InvokeReplacementColorAndActivate(dist * speedPerCell, typePartner, combination);

                        /*
                        CellInternalObject cellInternalObject = myField.cellCTRLs[x, y].cellInternal;

                        //??????? ?????? ??????
                        if (myField.cellCTRLs[x, y].cellInternal && myField.cellCTRLs[x, y].rock <= 0)
                        {
                            Destroy(myField.cellCTRLs[x, y].cellInternal.gameObject);

                            //??????? ????? ??????
                            GameObject internalObj = Instantiate(myField.prefabInternal, myField.parentOfInternals);
                            cellInternalObject = internalObj.GetComponent<CellInternalObject>();
                            cellInternalObject.myField = myField;
                        }

                        //???????? ???? ??? ?????
                        if (myField.cellCTRLs[x, y].rock <= 0) {

                            //???? ??????? ??????
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
                            //???? ?????
                            else if (typePartner == Type.bomb)
                            {
                                cellInternalObject.setColorAndType(replaceColor, Type.bomb);
                            }
                            //???? ???????
                            else if (typePartner == Type.airplane)
                            {
                                cellInternalObject.setColorAndType(partner.color, Type.airplane);

                            }
                            else
                            {

                            }
                        }
                        */

                        Particle3dCTRL particle3DCTRL = Particle3dCTRL.CreateBoomSuperColor(myField.transform, myCell);
                        particle3DCTRL.SetTransformTarget(new Vector2(x + 0.5f, y + 0.5f));
                        particle3DCTRL.SetTransformSpeed(1 / speedPerCell);

                        /*
                        //?????????? ?????? ?? ????? ???????
                        cellInternalObject.StartMove(myField.cellCTRLs[x, y]);
                        cellInternalObject.EndMove();

                        //needInstantDamage = false;
                        cellInternalObject.BufferActivateType = cellInternalObject.type;
                        cellInternalObject.BufferPartner = null;
                        cellInternalObject.BufferCombination = combination;
                        //cellInternalObject.Activate(cellInternalObject.type, null, combination);
                        cellInternalObject.ActivateInvoke(dist * speedPerCell);
                        */
                    }
                }

                //??????? ?????? ????????
                if (partner)
                    Destroy(partner.gameObject);
            }

            void DestroyAllColor(InternalColor internalColor) {
                float speedPerCell = 0.1f;
                //????????? ??? ?????? ?? ?????????? ??????
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++) {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        //??????? ???? ??? ??????, ??? ??????????? ??????? ??? ??? ??? ?????? ??? ?????? ????????
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

                        //??????? ?????? ????????
                        if (partner)
                            Destroy(partner.gameObject);

                    }
                }

                myCell.Damage(null, combination);
            }

            void DestroyAll()
            {
                //????????? ??? ?????? ?? ?????????? ??????
                for (int x = 0; x < myField.cellCTRLs.GetLength(0); x++)
                {
                    for (int y = 0; y < myField.cellCTRLs.GetLength(1); y++)
                    {
                        if (!myField.cellCTRLs[x, y])
                        {
                            continue;
                        }

                        //?????? ????????? ?? ?????? ??????????
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

                //??????? ?????? ????????
                if (partner)
                    Destroy(partner.gameObject);

            }


        }

        void ActivateBombAndRocket() {
            Debug.Log("ActivateBombVertical");

            Color ResultColor = GetColor(color);

            if (partner) {
                ResultColor = GetColor(partner.color);
                //??????? ????????
                partner.activate = false;
                partner.DestroyObj();
            }

            ResultColor = Color.Lerp(GetColor(color), ResultColor, 0.5f);

            //????? ??????? ?????
            Vector2Int pos = myCell.pos;


            bool[,] activated = new bool[myField.cellCTRLs.GetLength(0), myField.cellCTRLs.GetLength(1)];

            //???????????? ?????
            //?????????? 9 ?????
            for (int x = -1; x <= 1; x++) {
                if (myCell.pos.x + x < 0 || myCell.pos.x > myField.cellCTRLs.GetLength(0) - 1)
                    continue;

                for (int y = -1; y <= 1; y++) {
                    //???? ????? ?? ??????? ??????? ??????
                    if (myCell.pos.y + y < 0 || myCell.pos.y > myField.cellCTRLs.GetLength(1) - 1)
                        continue;

                    //???? ???? ?????? ???, ??????? ??????
                    if (myField.cellCTRLs.GetLength(0) >= myCell.pos.x + x ||
                        myField.cellCTRLs.GetLength(1) >= myCell.pos.y + y ||
                        myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] == null) {
                        continue;
                    }


                }
            }

            List<CellCTRL> expCells = new List<CellCTRL>();

            //??????? ???????? ?????
            //??????????????
            for (int y = -1; y <= 1; y++) {
                //???? ????? ?? ??????? ??????? //?? ?
                if (myCell.pos.y + y < 0 || myCell.pos.y > myField.cellCTRLs.GetLength(1) - 1)
                    continue;

                //?????
                for (int x = -1; x > myField.cellCTRLs.GetLength(0) * -1; x--) {
                    //???? ????? ?? ??????? ??????? //?? x
                    if (myCell.pos.x + x < 0 || myCell.pos.x + x > myField.cellCTRLs.GetLength(0) - 1)
                        continue;

                    //???? ?????? ???
                    if (myCell.myField.cellCTRLs[myCell.pos.x + x,myCell.pos.y + y] == null) {
                        continue;
                    }

                    //??????? ????? ???? ?? ???? ?????? ??? ??????
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion == null)
                    {
                        myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, BufferCombination, ResultColor);
                    }
                    else {
                    
                    }

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion.left = true;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferCombination = combination;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferNearDamage = false;

                    //????????? ? ??????
                    AddExplosionToList(myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y]);

                    break;
                }

                //??????
                for (int x = 1; x < myField.cellCTRLs.GetLength(0); x++) {
                    //???? ????? ?? ??????? ??????? //?? x
                    if (myCell.pos.x + x < 0 || myCell.pos.x + x > myField.cellCTRLs.GetLength(0) - 1)
                        continue;

                    //???? ?????? ???
                    if (myCell.myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] == null)
                    {
                        continue;
                    }

                    //??????? ????? ???? ?? ???? ?????? ??? ??????
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion == null)
                    {
                        myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, BufferCombination, ResultColor);
                    }
                    else {
                    
                    }

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion.right = true;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferCombination = combination;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferNearDamage = false;

                    //????????? ? ??????
                    AddExplosionToList(myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y]);

                    break;
                }
            }

            //????????????
            for (int x = -1; x <= 1; x++)
            {
                //???? ????? ?? ??????? ??????? //?? x
                if (myCell.pos.x + x < 0 || myCell.pos.x + x > myField.cellCTRLs.GetLength(0) - 1)
                    continue;

                //????
                for (int y = -1; y > myField.cellCTRLs.GetLength(1) * -1; y--)
                {
                    //???? ????? ?? ??????? ??????? //?? y
                    if (myCell.pos.y + y < 0 || myCell.pos.y + y > myField.cellCTRLs.GetLength(1) - 1)
                        continue;

                    //???? ?????? ???
                    if (myCell.myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] == null)
                    {
                        continue;
                    }

                    //??????? ????? ???? ?? ???? ?????? ??? ??????
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion == null)
                    {
                        myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, BufferCombination, ResultColor);
                    }
                    else {
                    
                    }

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion.down = true;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferCombination = combination;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferNearDamage = false;

                    //????????? ? ??????
                    AddExplosionToList(myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y]);

                    break;
                }

                //?????
                for (int y = 1; y < myField.cellCTRLs.GetLength(1); y++)
                {
                    //???? ????? ?? ??????? ??????? //?? y
                    if (myCell.pos.y + y < 0 || myCell.pos.y + y > myField.cellCTRLs.GetLength(1) - 1)
                        continue;

                    //???? ?????? ???
                    if (myCell.myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y] == null)
                    {
                        continue;
                    }

                    //??????? ????? ???? ?? ???? ?????? ??? ??????
                    if (myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion == null)
                    {
                        myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, BufferCombination, ResultColor);
                    }
                    else {
                    
                    }

                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].explosion.up = true;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferCombination = combination;
                    myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y].BufferNearDamage = false;

                    //????????? ? ??????
                    AddExplosionToList(myField.cellCTRLs[myCell.pos.x + x, myCell.pos.y + y]);

                    break;
                }
            }

            //????????
            foreach (CellCTRL expCell in expCells) {
                expCell.ExplosionBoomInvoke(expCell.explosion, 0);
            }

            float radius = 1;
            //??????? ??????? ??????
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

                //???? ????? ???? ????? ? ??????, ?? ???????
                foreach (CellCTRL explosion in expCells) {
                    if (explosion == explosionNew) {
                        return;
                    }
                }

                //????????? ?????
                expCells.Add(explosionNew);


            }
        }

        void ActivateBomb(int radius) {

            animatorCTRL.PlayAnimation("BombActivated");

            BoombRadius = radius;

            bool partnerBomb = false;
            //??????? ????????
            if (partner)
            {
                if (partner.type == Type.bomb)
                    partnerBomb = true;

                //??????? ????????
                partner.activate = false;
                partner.DestroyObj();

            }



            //?????????? ???? 5 ?? 5
            for (int x = -radius * 3; x <= radius * 3; x++) {
                for (int y = -radius * 3; y <= radius * 3; y++) {
                    int fieldPosX = myCell.pos.x + x;
                    int fieldPosY = myCell.pos.y + y;
                    //???? ????? ?? ??????? ????? ??? ???? ?????? ????
                    if (fieldPosX < 0 || fieldPosX >= myField.cellCTRLs.GetLength(0) ||
                        fieldPosY < 0 || fieldPosY >= myField.cellCTRLs.GetLength(1) ||
                        !myField.cellCTRLs[fieldPosX, fieldPosY]
                        )
                    {
                        continue;
                    }

                    //??????? ????? ???????? ?????? ???? ??????
                    float time = Vector2.Distance(new Vector2(), new Vector2(x, y)) * 0.2f;

                    //????????? ???????? ???????? ???? ???? ????????????
                    if (myField.cellCTRLs[fieldPosX, fieldPosY].cellInternal)
                    {
                        myField.cellCTRLs[fieldPosX, fieldPosY].cellInternal.PlayBoomAnimationInvoke(myCell.pos, radius * 3f, time/1.5f);
                    }


                    //???? ???? ???? ?????? ?? ????? ???????
                    if (Mathf.Abs(x) > radius || Mathf.Abs(y) > radius) {
                        continue;
                    }

                    myField.cellCTRLs[fieldPosX, fieldPosY].BufferCombination = BufferCombination;
                    myField.cellCTRLs[fieldPosX, fieldPosY].BufferNearDamage = false;
                    myField.cellCTRLs[fieldPosX, fieldPosY].DamageInvoke(time);

                }
            }

            //??????? ??????? ??????
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
                //??????? ?????
                GameObject flyObj = Instantiate(FlyPrefab, myField.parentOfFly);
                FlyCTRL flyCTRL = flyObj.GetComponent<FlyCTRL>();

                //???? ?????? ? ??????? ??? ????? ?? ?????
                foreach (CellCTRL cellPriority in myField.cellsPriority)
                {
                    if (cellPriority == myCell) { 
                        continue; 
                    }

                    //???? ????? ??? ?????? ? ?????? ????? ?? ????????? ??????? ? ????????????? ?????
                    bool found = false;
                    foreach (FlyCTRL fly in FlyCTRL.flyCTRLs)
                    {
                        if (fly.CellTarget == cellPriority)
                        {
                            found = true;
                            break;
                        }
                    }

                    //???? ????????? ??????? ? ?? ????? ?????? ? ??????, ?????? ??? ?? ??? ????? ??????? ? ???????? ????? ????
                    if (!found)
                    {
                        flyCTRL.inicialize(myCell, cellPriority, partner, combination, GetColor(color));
                        break;
                    }


                }
            }

            void CreatePartner() {
                //??????? ?????
                GameObject flyObj = Instantiate(FlyPrefab, myField.parentOfFly);
                FlyCTRL flyCTRL = flyObj.GetComponent<FlyCTRL>();


                //???? ?????? ? ??????? ??? ????? ?? ?????
                foreach (CellCTRL cellPriority in myField.cellsPriority)
                {
                    //???? ????? ??? ?????? ? ?????? ????? ?? ????????? ??????? ? ????????????? ?????
                    bool found = false;
                    foreach (FlyCTRL fly in FlyCTRL.flyCTRLs)
                    {
                        if (fly.CellTarget == cellPriority)
                        {
                            found = true;
                            break;
                        }
                    }

                    //???? ????????? ??????? ? ?? ????? ?????? ? ??????, ?????? ??? ?? ??? ????? ??????? ? ???????? ????? ????
                    if (!found)
                    {
                        flyCTRL.inicialize(partner.myCell, cellPriority, null, combination, GetColor(partner.color));

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
        //??????? ???? ?? ???? ???????
        if (myField == null || myField.buffer.CalculateFrameNow != 0)
            return;

        if (activateNeed && !activate) {
            Activate(BufferActivateType, BufferPartner, BufferCombination);
        }
    }

    /// <summary>
    /// ????????? ???????? ???????? ?? ??????
    /// </summary>
    Vector2 BoomFrom = new Vector2();
    float BoomDistanceMax = 0;

    public void PlayBoomAnimation() {

        //??????? ????????? ??????
        float distance = Vector2.Distance(BoomFrom, myCell.pos);

        if (distance == 0) {
            return;
        }

        //???? ???? ????????
        float strange =  1 - (distance / BoomDistanceMax);

        //???? ???? ?????? ?????? ???? ???????
        if (strange < 0) {
            return;
        }

        //??????? ?????? ??????
        Vector2 vectorBoom = new Vector2(myCell.pos.x, myCell.pos.y) - BoomFrom;

        //??????????????? ??????
        vectorBoom.Normalize();

        //????? ?????????? ????????? ???????? ???????? ? ?????????? ?????? ????????
        animatorCTRL.SetFloat("DroppedSizeX", vectorBoom.x * strange * -1);
        animatorCTRL.SetFloat("DroppedSizeY", vectorBoom.y * strange * -1);

        //?????? ???????????? ??????????????? ???????? ??? ????????????
        //????????
        float horizontal = Mathf.Acos(Mathf.Abs(vectorBoom.x));
        horizontal = 1-(horizontal / (Mathf.PI / 2));

        animatorCTRL.SetFloat("DroppedIsHorizontal", horizontal);

        //????????? ????????
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

    //?????? ????????????? ?????
    private InternalColor CalculateColorPriority()
    {
        //?????? ??????????? (?????????? ????????? ??????? ?????)
        int[] colorPriority = new int[10]; //fix

        //?????????? ?? ???? ???????, ??????? ?????
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

        //???????????? ????
        InternalColor priorityColor = 0;
        //????? ??????? ????????? (??? ?????????)
        int largestPriority = 0;
        for (int i = 0; i < colorPriority.Length; i++)
        {
            //???????? ?????????, ???? ?? ??????
            if (colorPriority[i] > largestPriority)
            {
                largestPriority = colorPriority[i];
                priorityColor = (InternalColor)i;
            }
        }
        return priorityColor;
    }


    Type replacementType = Type.color;
    GameFieldCTRL.Combination replacementCombination = null;


    //??????????? ?????? ? ??????????
    public void InvokeReplacementColorAndActivate(float time, Type type, GameFieldCTRL.Combination comb) {
        replacementType = type;
        replacementCombination = comb;
        Invoke("ReplacementColorAndActivate", time);
    }
    //???????? ? ????????????
    void ReplacementColorAndActivate()
    {
        
        //??????? ????? ??????
        GameObject internalObj = Instantiate(myField.prefabInternal, myField.parentOfInternals);

        //??????? ??????
        CellInternalObject cellInternalObject = internalObj.GetComponent<CellInternalObject>();

        cellInternalObject.myField = myField;

        //???????? ???? ??? ?????
        if (myCell.rock <= 0)
        {

            //???? ??????? ??????
            if (replacementType == Type.rocketHorizontal || replacementType == Type.rocketVertical)
            {
                if (Random.Range(0, 100) < 50)
                {
                    cellInternalObject.setColorAndType(color, Type.rocketVertical);
                }
                else
                {
                    cellInternalObject.setColorAndType(color, Type.rocketHorizontal);
                }
            }
            //???? ?????
            else if (replacementType == Type.bomb)
            {
                cellInternalObject.setColorAndType(color, Type.bomb);
            }
            //???? ???????
            else if (replacementType == Type.airplane)
            {
                cellInternalObject.setColorAndType(color, Type.airplane);

            }
            else
            {

            }
        }

        //?????????? ????? ?????? ?? ??? ?????
        cellInternalObject.StartMove(myCell);
        cellInternalObject.EndMove();

        //needInstantDamage = false;
        cellInternalObject.BufferActivateType = cellInternalObject.type;
        cellInternalObject.BufferPartner = null;
        cellInternalObject.BufferCombination = replacementCombination;
        //cellInternalObject.Activate(cellInternalObject.type, null, combination);
        cellInternalObject.ActivateInvoke(1.0f);
        cellInternalObject.animatorCTRL.PlayAnimation("BombActivated");


        //??????? ?????? ?? ?????? ???????
        DestroyObj(true);
    }

}
