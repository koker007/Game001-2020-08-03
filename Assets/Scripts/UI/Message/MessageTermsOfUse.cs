using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����

/// <summary>
/// ��������� � ���������������� �����������
/// </summary>
public class MessageTermsOfUse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //������������ ����� ������� ���������������� ����������
    public void ButtonClickAccept() {
        //������������ ������ ����������
        PlayerProfile.main.ProfileTermsOfUse = 1;

        //��������� ��� ������
        PlayerProfile.main.Save();
    }
}
