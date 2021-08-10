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

    void GetRect() {
        rectMy = GetComponent<RectTransform>();
        rectCell = myCell.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetRect();
    }

    // Update is called once per frame
    void Update()
    {
        dropped();
        //Moving();
    }

    public bool isDropped = false;
    float DroppedSpeed = 0;
    void dropped() {
        if (isDropped)
        {
            DroppedSpeed += Time.unscaledDeltaTime * 4;

            float posYnew = rectMy.pivot.y + 0.05f + DroppedSpeed;

            //���� ������� ���� ��� �� ���� ����
            if (posYnew > rectCell.pivot.y)
            {
                CellCTRL cellMove = GetFreeCellDown();
                if (cellMove)
                {
                    dropStart(cellMove);
                }
                else
                {
                    posYnew = rectCell.pivot.y;
                    isDropped = false; //������� ����������
                    myCell.movingInternalNow = false; //������ ������������ ��� �������
                }
            }

            //������������� �������
            rectMy.pivot = new Vector2(rectMy.pivot.x, posYnew);
        }
        //��������� ����� ������� ��������� ������
        else {
            CellCTRL cellMove = GetFreeCellDown();
            if (cellMove)
                dropStart(cellMove);
        }
        
    }

    //�������� ��������� ������ �����
    CellCTRL GetFreeCellDown() {
        CellCTRL returnCell = null;
        for (int minusY = 1; minusY < myField.cellCTRLs.GetLength(1); minusY++) {
            if (myCell.pos.y - minusY >= 0 && //���� �� ����� �� ������
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY] && //���� ���� ������
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].cellInternal && //� ��� ��������
                !myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].movingInternalNow && //���� ����� �� ��������
                myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY].dontMoving == 0 //� ����� ���������
                                                                                  )
            {
                //������ ����� ������ ��� ������
                returnCell = myField.cellCTRLs[myCell.pos.x, myCell.pos.y - minusY];
            }
            //������ ����� ����� ���
            else { 
                //�������
            }
        }
        return returnCell;
    }

    public void dropStart(CellCTRL cellNew) {
        if (myCell)
        {
            myCell.movingInternalNow = false;
            myCell.cellInternal = null;
        }

        myCell = cellNew;

        if (!isDropped)
        {
            DroppedSpeed = 0;
            isDropped = true;
        }
        //������� ������� ������ ��� � ��� ���������� ��������
        myCell.movingInternalNow = true;
        myCell.cellInternal = this;

        GetRect();
    }

    /// <summary>
    /// ������� ������
    /// </summary>
    public void DestroyObj() {
        Destroy(gameObject);
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

        int random = Random.Range(0, 3);
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
