using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//�����
/// <summary>
/// ���������� ��������������� ����
/// </summary>
public class MenuHello : MonoBehaviour
{
    static public MenuHello main;

    [SerializeField]
    Text[] Version;

    [SerializeField]
    GameObject options;


    bool isNeedOpenOptions; //����� �� ������� ��� ������� ������ �����
    


    //����� ����� � ��������� ��� �������� ��� ����
    float timeLastWork;


    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePanelOpttion();
    }

    /// <summary>
    /// ������ ���� ����� � ������� ����
    /// </summary>
    public void clickButtonPlay() {
        UICTRL.main.OpenWorld();
    }

    /// <summary>
    /// ��������� ���� ������� ��������
    /// </summary>
    public void clickButtonSave() {
    
    }

    /// <summary>
    /// ������� ��������� ����
    /// </summary>
    public void clickButtonOpttion()
    {
        isNeedOpenOptions = !isNeedOpenOptions;
    }


    /// <summary>
    /// �������� �� ��������� ������ �����
    /// </summary>
    void UpdatePanelOpttion() {
        if (isNeedOpenOptions && !options.activeSelf)
        {
            options.SetActive(true);
        }
        else if(!isNeedOpenOptions && options.activeSelf) {
            options.SetActive(false);
        }
    }
}
