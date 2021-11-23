using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����

//������ ������������ ������ ������ � ��������
public class MainCamera : MonoBehaviour
{
    static public MainCamera main;

    [SerializeField]
    public RenderTexture renderTexture;

    public Camera myCamera;


    /// <summary>
    /// �������� ����������� �� 0 - ������ �� 1 - ������
    /// </summary>
    [SerializeField]
    public float percent = 1;

    // Start is called before the first frame update
    void Start()
    {
        main = this;

        iniTexture();
        iniCamera();
    }

    void iniTexture() {
        if (!renderTexture)
        {

            if (percent > 1)
                percent = 1;
            else if (percent < 0.01f)
                percent = 0.01f;

            //���� �������� ���, ������� ��
            renderTexture = new RenderTexture((int)(Screen.width * percent), (int)(Screen.height * percent), 0);
        }
    }
    void iniCamera() {
        if (!myCamera)
        {
            myCamera = GetComponent<Camera>();
            myCamera.targetTexture = renderTexture;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TestTextureSize();
    }

    //������� ����� ������� � ����� �����������
    void TestTextureSize() {
        if (percent > 1)
            percent = 1;
        else if (percent < 0.1f)
            percent = 0.1f;

        //������� ���������� �������� �� ������
        int x = (int)(Screen.width * percent);
        int y = (int)(Screen.height * percent);

        //������� ���� ������� ��������� � �� ����������
        if (x == renderTexture.width || y == renderTexture.height) return;

        //������� ����� ��������
        renderTexture = new RenderTexture(x, y, 8);
        renderTexture.filterMode = FilterMode.Bilinear;
        renderTexture.antiAliasing = 1;
        renderTexture.anisoLevel = 1;

        renderTexture.Create();
        myCamera.targetTexture = renderTexture;
    }
}
