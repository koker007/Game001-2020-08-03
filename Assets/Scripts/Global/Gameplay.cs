using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����
/// <summary>
/// ���������� ������� ���������, ������, ����� ���������� ����������� ������ � ������.
/// </summary>
public class Gameplay : MonoBehaviour
{

    static public Gameplay main;

    /// <summary>
    /// ��������� �������� ��������� �������
    /// </summary>
    public int levelOpen = 0;
    /// <summary>
    /// ������� ��������� �������
    /// </summary>
    public int levelSelect = 0;
    /// <summary>
    /// ������
    /// </summary>
    public int tickets = 0;

    [Header("Level parameters")]
    public int score = 0;
    public int scoreMax = 0;
    public int movingCan = 0;
    public int movingCount = 0;
    public int colors = 5;
    

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameplay() {
        //���� ������� ������
        score = 0;
        movingCan = 30;
    }
}
