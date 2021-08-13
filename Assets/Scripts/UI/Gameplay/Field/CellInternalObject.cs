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
    Texture2D TextureRed;
    [SerializeField]
    Texture2D TextureGreen;
    [SerializeField]
    Texture2D TextureBlue;
    [SerializeField]
    Texture2D TextureYellow;
    [SerializeField]
    Texture2D textureViolet;

    public enum InternalColor {
        Red,
        Green,
        Blue,
        Yellow,
        Violet
    }
    public enum Type {
        color,
        rocketHorizontal,
        rocketVertical,
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
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].dontMoving == 0 //� ����� ���������
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

        SpawnEffects();

        Destroy(gameObject);

        void SpawnEffects() {
            GameObject PrefabDie = Instantiate(myField.PrefabParticleDie, myField.parentOfScore);
            RectTransform rectDie = PrefabDie.GetComponent<RectTransform>();

            GameObject PrefabScore = Instantiate(myField.PrefabParticleScore, myField.parentOfScore);
            RectTransform rectScore = PrefabScore.GetComponent<RectTransform>();

            rectDie.pivot = rectMy.pivot;
            rectScore.pivot = rectMy.pivot;
        }
    }

    public void randColor() {
        color = GetRandomColor();
        if (type == Type.color) {
            if (color == InternalColor.Red) {
                Image.texture = TextureRed;
            }
            else if (color == InternalColor.Green) {
                Image.texture = TextureGreen;
            }
            else if (color == InternalColor.Blue)
            {
                Image.texture = TextureBlue;
            }
            else if (color == InternalColor.Yellow)
            {
                Image.texture = TextureYellow;
            }
            else if (color == InternalColor.Violet)
            {
                Image.texture = textureViolet;
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



    public void ActivateObj() {
        Debug.Log("Activate");
    }
}
