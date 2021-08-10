using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����
/// <summary>
/// ���������� ������������ ����� �����
/// </summary>
public class CellInternalObject : MonoBehaviour
{
    //��� ������
    public CellCTRL myCell;

    RectTransform rectMy;
    RectTransform rectCell;

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
    }

    public bool isDropped = false;
    float DroppedSpeed = 0;
    void dropped() {
        if (isDropped) {
            DroppedSpeed += Time.unscaledDeltaTime * 4;

            float posYnew = rectMy.pivot.y + 0.05f + DroppedSpeed;

            //���� ������� ���� ��� �� ���� ����, ���������������
            if (posYnew > rectCell.pivot.y) {
                posYnew = rectCell.pivot.y;
                isDropped = false; //������� ����������
                myCell.movingInternalNow = false; //������ ������������ ��� �������
            }

            //������������� �������
            rectMy.pivot = new Vector2(rectMy.pivot.x, posYnew);
        }
    }
    
}
