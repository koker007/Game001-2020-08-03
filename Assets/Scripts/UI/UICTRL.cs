using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����
/// <summary>
/// ������������ �������� ������ � ��� UI
/// </summary>
public class UICTRL : MonoBehaviour
{
    /// <summary>
    /// �������� UICTRL
    /// </summary>
    static public UICTRL main;

    [SerializeField]
    GameObject UILoading; //���� �������� ����
    [SerializeField]
    GameObject UIHello; //���� �����������
    [SerializeField]
    GameObject UIWorld; //������� ����
    [SerializeField]
    GameObject UIGameplay; //������� �������

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        OpenLoading();
    }

    

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }



    float loadingPlayTime = 0; //��������� ���������� ��� ���������� ��������.


    /// <summary>
    /// ������� ��� ����
    /// </summary>
    void CloseAll() {
        UILoading.SetActive(false);
        UIHello.SetActive(false);
        UIWorld.SetActive(false);
        UIGameplay.SetActive(false);
    }

    /// <summary>
    /// ������� ���� ��������
    /// </summary>
    public void OpenLoading() {
        CloseAll();
        UILoading.SetActive(true);
    }

    /// <summary>
    /// ������� �������������� ����
    /// </summary>
    public void OpenHello() {
        CloseAll();
        UIHello.SetActive(true);
    }
    
    /// <summary>
    /// ������� �������� ���� �����
    /// </summary>
    public void OpenWorld()
    {
        CloseAll();
        UIWorld.SetActive(true);
    }

    /// <summary>
    /// ������� ������� �������
    /// </summary>
    public void OpenGameplay() {
        CloseAll();
        UIGameplay.SetActive(true);
    }

    //������� ������� �������������� ����� �� ���� ��������
    void UpdateUI() {
        if (UILoading.activeSelf) {
            loadingPlayTime += Time.deltaTime;
            if (loadingPlayTime > 3) {
                OpenHello();
            }
        }
        else if (UIHello.activeSelf) {

        }
        else if (UIWorld.activeSelf) {

        }
        else if (UIGameplay.activeSelf) {
        
        }
    }
}
