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
    RectTransform SelectMessanger;

    //����� �� ������� ����
    public bool needClose = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OpenClose();
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

    /// <summary>
    /// ����������� ���� �������
    /// </summary>
    static public void Events() {

    }

    //�������� ��� �������� ��������������� ����
    void OpenClose()
    {

        testFon();

        //��������� �����
        void testFon() {
            float alphaMax = 0.5f;
            if (needClose && Fon.color.a > 0)
            {
                float alpha = Fon.color.a;
                alpha -= Time.unscaledDeltaTime;
                if (alpha < 0)
                {
                    Fon.raycastTarget = false;
                    alpha = 0;
                }

                Fon.color = new Color(Fon.color.r, Fon.color.g, Fon.color.b, alpha);
            }
            else if (!needClose && Fon.color.a < alphaMax) {
                Fon.raycastTarget = true;

                float alpha = Fon.color.a;
                alpha += Time.unscaledDeltaTime;
                if (alpha < 0)
                {
                    alpha = alphaMax;
                }
                Fon.color = new Color(Fon.color.r, Fon.color.g, Fon.color.b, alpha);
            }
        }
    }
}
