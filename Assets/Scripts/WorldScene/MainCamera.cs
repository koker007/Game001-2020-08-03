using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен

//Скрипт контролирует рендер камеры в текстуру
public class MainCamera : MonoBehaviour
{
    static public MainCamera main;

    [SerializeField]
    public RenderTexture renderTexture;

    public Camera myCamera;


    /// <summary>
    /// Качество изображения от 0 - плохое до 1 - полное
    /// </summary>
    [SerializeField]
    public float percent = 1;

    // Start is called before the first frame update
    void Start()
    {
        main = this;

        iniTexture();
        iniCamera();

        //InvokeRepeating("TestTextureSize", 1, 1);
    }

    void iniTexture() {
        if (!renderTexture)
        {

            if (percent > 1)
                percent = 1;
            else if (percent < 0.01f)
                percent = 0.01f;

            //Если текстуры нет, создаем ее
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
        //TestTextureSize();
    }

    float fpsAverageOld = 60;
    struct FPS {
        public float frame;
        public float timeTest;
    }
    List<FPS> fpsList = new List<FPS>();

    //выявление нужного разрешения
    void TestNewQuality() {

        float testSecond = 3;

        float FpsTarget = 60;

        //проверяем текущий фпс
        FPS fpsNow = new FPS();
        fpsNow.frame = 1 / Time.deltaTime; //фпс
        fpsNow.timeTest = Time.unscaledTime; //время работы программы

        //Добавляем в список текущий фпс
        fpsList.Add(fpsNow);

        //новый список фпс
        List<FPS> fpsListNew = new List<FPS>();
        float fpsAverage = 1;
        int fpsAverageCount = 0;
        //Перебор всех фпс
        foreach (FPS fps in fpsList) {
            //Выходим если этот кадр устарел
            if (Time.unscaledTime - fps.timeTest > testSecond) {
                continue;
            }

            fpsAverage += fps.frame;
            fpsAverageCount++;

            //Добавляем этот кдр на следующий лист
            fpsListNew.Add(fps);
        }
        //Сохраняем фпс
        fpsList = fpsListNew;

        //Перебор всех фпс
        fpsAverage /= fpsAverageCount;

        //узнаем какой был фпс в последний раз
        float correct = fpsAverage / fpsAverageOld;
        //Debug.Log("correct = " + correct);

        //если фпс с прошлого изменения сильно отличаются то меняем
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

    //создать новую тестуру с новым разрешением
    void TestTextureSize() {
        if (percent > 1)
            percent = 1;
        else if (percent < 0.7f)
            percent = 0.7f;

        //Считаем количество пикселей на экране
        int x = (int)(Screen.width * percent);
        int y = (int)(Screen.height * percent);

        //выходим если размеры совпадают и не изменились
        if (x == renderTexture.width || y == renderTexture.height || x < 1 || y < 1) return;

        //создаем новую текстуру
        renderTexture = new RenderTexture(x, y, 0);
        renderTexture.filterMode = FilterMode.Bilinear;
        renderTexture.antiAliasing = 2;
        renderTexture.anisoLevel = 2;

        renderTexture.Create();
        myCamera.targetTexture = renderTexture;
    }
}
