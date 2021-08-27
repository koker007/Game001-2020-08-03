using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// ������������ ����� �������, �������� ��������
/// </summary>
public class FlyCTRL : MonoBehaviour
{
    [SerializeField]
    Image image;

    Vector2 PivotTarget;

    float SpeedRotate = 0;
    float SpeedMove = 0;

    RectTransform myRect;
    Vector2 PivotStart;

    // Start is called before the first frame update
    void Start()
    {
        inicialize();
    }

    void inicialize() {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = PivotStart;
    }

    // Update is called once per frame
    void Update()
    {
        CalcTransform();
    }

    void CalcTransform() {

        Rotate();
        Move();

        void Rotate() {
            //���� ����� ��� ����� ����� ����������� � ����
            float angleTarget = 0;

            //����������� � ���� ������������ �������
            Vector2 vectorTarget = PivotTarget - myRect.pivot;
            Vector2 vectorTargetNormalized = vectorTarget.normalized;

            //���� ����������
            //float radianSin = Mathf.Asin(vectorTargetNormalized.x);
            //�� ������ ��� �������� ������� ��� �� ������� ���������� ���������
            float radianCos = Mathf.Acos(vectorTargetNormalized.y);
            //�� ������ ���� ������������� ��� ������������� X ������ ������������� ��� ������������� �������

            //����� ���� � ��������
            //����� ������ �� ����
            //������ ����� ��� ��� ��������
            



        }
        void Move() {
        
        }
    }
}
