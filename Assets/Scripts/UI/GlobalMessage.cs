using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// �������� �� ����� ���������� ��������� �� �����
/// </summary>
public class GlobalMessage : MonoBehaviour
{
    static public GlobalMessage main;

    /// <summary>
    /// �����������
    /// </summary>
    public GlobalMessage() {
        main = this;
    }

    [SerializeField]
    Image Fon;
    [SerializeField]
    GameObject PrefabMessanger;
    [SerializeField]
    GameObject SelectMessanger;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ������� ����������� ����
    /// </summary>
    static public void Close() {
    
    }

    /// <summary>
    /// ��������� ��������� � ���������� ����
    /// </summary>
    /// <param name="text"></param>
    static public void Message(string text){

    }
    /// <summary>
    /// ����������� ���� ��������
    /// </summary>
    static public void Health() {

    }
    /// <summary>
    /// ����������� ���� ������
    /// </summary>
    static public void Tickets() {
        
    }
    static public void Events() {

    }


}
