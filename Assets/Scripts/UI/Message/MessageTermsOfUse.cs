using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����

/// <summary>
/// ��������� � ���������������� �����������
/// </summary>
public class MessageTermsOfUse : MonoBehaviour
{


    [SerializeField]
    string TermsOfUseUrl = "http://project5092068.tilda.ws";

    public void ButtubClickOpenTermsOfUse() {
        Application.OpenURL(TermsOfUseUrl);
    }

    //������������ ����� ������� ���������������� ����������
    public void ButtonClickAccept() {
        //������������ ������ ����������
        PlayerProfile.main.ProfileTermsOfUse = (float)System.Convert.ToDouble(Application.version);

        //��������� ��� ������
        PlayerProfile.main.Save();
    }
}
