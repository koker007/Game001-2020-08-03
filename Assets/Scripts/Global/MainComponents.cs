using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �������� ���������� � ������� ���������� �� �����
/// </summary>
public class MainComponents : MonoBehaviour
{
    public static GameObject MainCamera;

    private void Awake()
    {
        MainCamera = GameObject.Find("Main Camera");
    }

}
