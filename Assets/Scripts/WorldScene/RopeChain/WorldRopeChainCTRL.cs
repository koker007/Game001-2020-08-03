using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����
/// <summary>
/// ���������� ��������� ������� ����������� ��� ������ � ���� �����
/// </summary>
public class WorldRopeChainCTRL : MonoBehaviour
{
    static public WorldRopeChainCTRL main;

    [SerializeField]
    public RopeChain PrefabRopeChain;
    [SerializeField]
    public GameObject PrefabSphere;

    private void Awake()
    {
        //��������� �� ������ ����
        main = this;
    }
}
