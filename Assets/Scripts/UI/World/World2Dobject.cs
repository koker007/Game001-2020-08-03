using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������������ ������ ���� �������� �� �����
/// </summary>
public class World2Dobject : MonoBehaviour
{
    void Update()
    {
        //������� ������� � ������
        transform.LookAt(MainComponents.MainCamera.transform);
    }
}
