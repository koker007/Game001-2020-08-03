using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����

/// <summary>
/// ��������� � ���������������� �����������
/// </summary>
public class MessageTermsOfUse : MonoBehaviour
{
    //������������ ����� ������� ���������������� ����������
    public void ButtonClickAccept() {
        //������������ ������ ����������
        PlayerProfile.main.ProfileTermsOfUse = 1;

        //��������� ��� ������
        PlayerProfile.main.Save();
    }
}
