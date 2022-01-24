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
    RectTransform PanelDown;
    [SerializeField]
    AnimatorCTRL animatorCTRL;
        


    //����� ����� � ��������� ��� �������� ��� ����
    float timeLastWork;


    // Start is called before the first frame update
    void Start()
    {
        main = this;
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
        GlobalMessage.ComingSoon();
    }

    /// <summary>
    /// ������� ��������� ����
    /// </summary>
    public void clickButtonOpttion()
    {
        GlobalMessage.Settings();
    }

    private void OnEnable()
    {
        OpenPanels();
    }


    void OpenPanels() {
        animatorCTRL.PlayAnimation("restart");
        //PanelDown.pivot = new Vector2(PanelDown.pivot.x,2);
    }

    //
    void UpdatePanels() {
        
        moving();

        void moving() {
            float y = PanelDown.pivot.y;

            y += (0 - y) * Time.unscaledDeltaTime * 4;

            PanelDown.pivot = new Vector2(PanelDown.pivot.x, y);
        }
    }
}
