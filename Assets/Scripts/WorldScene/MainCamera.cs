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

    /*
    public Vector2 DefaultResolution = new Vector2(720, 1280);
    [Range(0f, 1f)] public float WidthOrHeight = 0;

    private Camera componentCamera;

    private float initialSize;
    private float targetAspect;

    private float initialFov;
    private float horizontalFov = 120f;
    */

    // Start is called before the first frame update
    void Start()
    {
        main = this;

        iniTexture();
        iniCamera();
        //iniScaler();
        //InvokeRepeating("TestTextureSize", 1, 1);

        //InvokeRepeating("CameraScaler", 0, Random.Range(1f, 1.5f));
    }

    /*
    void iniScaler() {
        componentCamera = GetComponent<Camera>();
        initialSize = componentCamera.orthographicSize;

        targetAspect = DefaultResolution.x / DefaultResolution.y;

        initialFov = componentCamera.fieldOfView;
        horizontalFov = CalcVerticalFov(initialFov, 1 / targetAspect);
    }
    */

    void iniTexture() {
        if (!renderTexture)
        {

            if (percent > 1)
                percent = 1;
            else if (percent < 0.01f)
                percent = 0.01f;

            //���� �������� ���, ������� ��
            renderTexture = new RenderTexture((int)(Screen.width * percent), (int)(Screen.height * percent), 0);
            renderTexture.filterMode = FilterMode.Bilinear;
            renderTexture.antiAliasing = 2;
            renderTexture.anisoLevel = 2;
            renderTexture.Create();
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
        TestNewQuality();
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //�������� ���������� ������
    /*
    void CameraScaler() {
        if (componentCamera.orthographic)
        {
            float constantWidthSize = initialSize * (targetAspect / componentCamera.aspect);
            componentCamera.orthographicSize = Mathf.Lerp(constantWidthSize, initialSize, WidthOrHeight);
        }
        else
        {
            float constantWidthFov = CalcVerticalFov(horizontalFov, componentCamera.aspect);
            componentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, initialFov, WidthOrHeight);
        }

    }
    float CalcVerticalFov(float hFovInDeg, float aspectRatio)
    {
        float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

        float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

        return vFovInRads * Mathf.Rad2Deg;
    }
    /
    */

    ///����� ���������� ������
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    float fpsAverageOld = 60;
    struct FPS {
        public float frame;
        public float timeTest;
    }
    List<FPS> fpsList = new List<FPS>();

    //��������� ������� ���������� ��� ��������
    void TestNewQuality() {

        float testSecond = 3;

        float FpsTarget = 60;

        //��������� ������� ���
        FPS fpsNow = new FPS();
        fpsNow.frame = 1 / Time.deltaTime; //���
        fpsNow.timeTest = Time.unscaledTime; //����� ������ ���������

        //��������� � ������ ������� ���
        fpsList.Add(fpsNow);

        //����� ������ ���
        List<FPS> fpsListNew = new List<FPS>();
        float fpsAverage = 1;
        int fpsAverageCount = 0;
        //������� ���� ���
        foreach (FPS fps in fpsList) {
            //������� ���� ���� ���� �������
            if (Time.unscaledTime - fps.timeTest > testSecond) {
                continue;
            }

            fpsAverage += fps.frame;
            fpsAverageCount++;

            //��������� ���� ��� �� ��������� ����
            fpsListNew.Add(fps);
        }
        //��������� ���
        fpsList = fpsListNew;

        //������� ���� ���
        fpsAverage /= fpsAverageCount;

        //������ ����� ��� ��� � ��������� ���
        float correct = fpsAverage / fpsAverageOld;
        //Debug.Log("correct = " + correct);

        //���� ��� � �������� ��������� ������ ���������� �� ������
        if (correct > 1.1 || correct < 0.9) {
            if (fpsAverage >= FpsTarget)
            {
                percent = 1;
            }
            else {
                percent = fpsAverage / FpsTarget;
            }
            fpsAverageOld = fpsAverage;
            TestTextureSize();
        }
    }

    //������� ����� ������� � ����� �����������
    void TestTextureSize() {
        if (percent > 1)
            percent = 1;
        else if (percent < 0.7f)
            percent = 0.7f;

        //������� ���������� �������� �� ������
        int x = (int)(Screen.width * percent);
        int y = (int)(Screen.height * percent);

        //������� ���� ������� ��������� � �� ����������
        if (x == renderTexture.width || y == renderTexture.height || x < 1 || y < 1) return;

        //������� ����� ��������
        renderTexture = new RenderTexture(x, y, 0);
        renderTexture.filterMode = FilterMode.Bilinear;
        renderTexture.antiAliasing = 2;
        renderTexture.anisoLevel = 2;

        renderTexture.Create();
        myCamera.targetTexture = renderTexture;
    }
}