using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����
//������ � ���������� ������������ ������� ����� ����� �� �������
public class Calculate : MonoBehaviour
{
    public const float PIinOnegrad = 0.01745329f; //���������� �� ������ � ����� �������

    //�������� ��������� �������� �� ������������ ��������� � �������� ������� ���������, � ���� ������� ���������� � ����������� ��������
    static public float GetValueLinear(float from, float to, float moveValue) {

        float result = from;

        //���������
        if (result > to)
        {
            result -= moveValue;

            //���� ������� �������� ��������
            if (result < to) 
                return to;
        }
        //�����������
        else if (result < to)
        {
            result += moveValue;
            //���� ������� �������� ��������
            if (result > to)
                return to;
        }

        return result;
    }
}
