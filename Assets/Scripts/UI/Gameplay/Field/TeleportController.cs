using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������
//�����
public class TeleportController : MonoBehaviour
{
    public Image image;
    public RectTransform rectMigleImage;

    public GameFieldCTRL myField;

    public CellCTRL cellIn; 
    public CellCTRL cellOut;
    public TeleportController secondTeleport;

    public Image ImageUP;
    public Image imageDown;

    //ID �������� ���������
    int ID = 0;

    [SerializeField]
    Color[] colors;

    private void Update()
    {
        
        Teleport();
        UpdateImage();
    }

    private void Teleport()
    {
        //������ ������ �� ��������� �����
        if (myField.buffer.CalculateFrameNow != 0) {
            return;
        }

        //���� �� ����� ���� ��� ���������� � ������ ����� � �����������
        if (cellIn.cellInternal != null &&
            cellIn.rock == 0)
        {
            //���� ����� ������� ��������� �������� � ��������, ���������� ������������
            if (secondTeleport.cellOut != null &&
                secondTeleport.cellOut.BlockingMove == 0 &&
                !secondTeleport.cellOut.cellInternal &&
                secondTeleport.cellOut.rock == 0)
            {
                tele();
            }
        }

        void tele() {
            //������� ���������� ������ ��������
            GameObject dublicateObj = Instantiate(cellIn.myField.prefabInternal, cellIn.myField.parentOfInternals);
            CellInternalObject dublicate = dublicateObj.GetComponent<CellInternalObject>();

            //�������� ������ ��������� ��������� ���������
            dublicate.setColorAndType(cellIn.cellInternal.color, cellIn.cellInternal.type);

            //�������� ���������� �� ����� ���������
            dublicate.rectMy.pivot = cellIn.cellInternal.rectMy.pivot;
            //������� ������ ���������� ����� �� ���� ����������� ����
            dublicate.myMaskNeed.y = 100;

            //������� ��������� ��������� �� ������� ���������������
            dublicate.myDeath = cellOut.pos * -1;
            //�� ���������� �������� ������������
            dublicate.EndMoveAndRemove = true;
            dublicate.isMove = true;
            //������ ������ � ������� ����������
            dublicate.myCell = null;

            //������ �������� ��������� ��� � ���������
            dublicate.MovingSpeed = cellIn.cellInternal.MovingSpeed;

            //�������� ���������� �� ������� ��� ������ ������
            cellIn.cellInternal.rectMy.pivot = secondTeleport.cellIn.pos * -1;
            //�������� ������ ������
            cellIn.cellInternal.SetFullMask2D(CellInternalObject.MaskType.top);    
            //������� ������������ ��������� � ������� �����
            cellIn.cellInternal.StartMove(secondTeleport.cellOut);


            //��������� ����������� ������� � ������ ��������
            ImageUPFull();
            secondTeleport.ImageDownFull();
        }
    }

    //���������� id ��������� � ��������� ��� ����
    public void setIDAndColor(int IDnew) {
        ID = IDnew;

        image.color = colors[ID];
    }

    
    //��������� ����� �������� ����������� �� ������
    void ImageUPFull() {
        Color colorUP = ImageUP.color;
        colorUP.a = 1;
        ImageUP.color = colorUP;
    }

    //��������� ����� ������� ����������� �� ������
    void ImageDownFull() {
        Color colorDown = imageDown.color;
        colorDown.a = 1;
        imageDown.color = colorDown;
    }

    void UpdateImage() {
        //������ ������
        Quaternion rectMidleImage = rectMigleImage.localRotation;
        Vector3 euler = rectMigleImage.eulerAngles;

        euler.z += 90 * Time.unscaledDeltaTime;

        rectMidleImage.eulerAngles = euler;
        rectMigleImage.localRotation = rectMidleImage;

        if (ImageUP.color.a > 0) {
            Color colorUP = ImageUP.color;
            colorUP.a -= Time.unscaledDeltaTime;
            ImageUP.color = colorUP;
        }

        if (imageDown.color.a > 0) {
            Color colorDown = imageDown.color;
            colorDown.a -= Time.unscaledDeltaTime;
            imageDown.color = colorDown;
        }
        
    }

}
