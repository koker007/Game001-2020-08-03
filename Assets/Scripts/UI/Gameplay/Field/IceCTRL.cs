using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// ������������ ������ ����
/// </summary>
public class IceCTRL : MonoBehaviour
{

    [SerializeField]
    Image image;

    CellCTRL myCell;
    RectTransform myRect;

    int HealthOld = -1;

    bool isInicialize = false;
    public void inicialize(CellCTRL cellIni)
    {
        if (isInicialize) return;

        myCell = cellIni;

        //��������� � ������ ��� �������
        myCell.myField.iceCTRLs[cellIni.pos.x, cellIni.pos.y] = this;

        IniRect();

        ReCalcIceCount();

        isInicialize = true;
    }

    void IniRect()
    {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-myCell.pos.x, -myCell.pos.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAlpha();
    }
    private void FixedUpdate()
    {
        UpdateLife2();
    }

    /*
    void UpdateLife()
    {
        //���� �������� �� ��������
        if (HealthOld == myCell.ice)
            return;


        //���� �������� ���� ������������� ������� ��������
        if (HealthOld > myCell.ice) {
            Particle3dCTRL.CreateDestroyIce(myCell.myField.transform, myCell);
        }

        HealthOld = myCell.ice;


        if (myCell.ice > 0)
        {
            return;
        }

    }
    */

    float alphaNeed = 1;
    float alphaSpeed = 0.2f;
    bool alphaCalc = false;
    void UpdateLife2() {
        //���� �������� �� ��������
        if (HealthOld == myCell.ice)
            return;

        //������������� ������� ��������
        if (HealthOld > myCell.ice)
        {
            Particle3dCTRL.CreateDestroyIce(myCell.myField.transform, myCell);
        }

        HealthOld = myCell.ice;

        //���������� ����� ���� ������������
        calcColor();

        //������� ���� �������� ���� 0
        if (myCell.ice > 0)
        {
            return;
        }

        destroy();

        void destroy()
        {
            //���� ����������
            SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipDestroyRock, 0.5f, Random.Range(0.9f, 1.1f));

            myCell.myField.iceCTRLs[myCell.pos.x, myCell.pos.y] = null;
            ReCalcIceCount();
            Destroy(gameObject);
        }

        void calcColor() {

            alphaCalc = true;

            if (myCell.ice == 1)
            {
                alphaNeed = 0.7f;
            }
            else if (myCell.ice == 2)
            {
                alphaNeed = 0.8f;
            }
            else if (myCell.ice == 3)
            {
                alphaNeed = 0.9f;
            }
            else if (myCell.ice == 4)
            {
                alphaNeed = 0.95f;
            }
            else if (myCell.ice == 5) {
                alphaNeed = 1;
            }
        }
    }

    void UpdateAlpha() {
        if (alphaCalc) {
            //����������� ����
            Color color = image.color;

            //�������� ���������� ����������
            if (color.a == alphaNeed) {
                //�������� ���� �������
                alphaCalc = false;
                return;
            }


            //����� ���������� ����� ���������� ����
            bool plusNeed = false;
            if (alphaNeed - color.a > 0) {
                plusNeed = true;
            }

            //����������
            if (plusNeed)
            {
                color.a += alphaSpeed * Time.deltaTime;
            }
            //��������
            else {
                color.a -= alphaSpeed * Time.deltaTime;
            }

            //��������� �� ���������� ����
            bool plusNow = false;
            if (alphaNeed - color.a > 0)
            {
                plusNow = true;
            }

            //���� �����������
            if (plusNeed != plusNow) {
                color.a = alphaNeed;
                alphaCalc = false;
            }

            image.color = color;
        }
    }

    void ReCalcIceCount()
    {

        int count = 0;

        //���������� ��� ������� ����
        for (int x = 0; x < myCell.myField.iceCTRLs.GetLength(0); x++)
        {
            for (int y = 0; y < myCell.myField.iceCTRLs.GetLength(1); y++)
            {
                if (myCell.myField.iceCTRLs[x, y])
                {
                    count++;
                }
            }
        }

        myCell.myField.CountIce = count;
    }
}
