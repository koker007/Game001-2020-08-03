using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен

//Скрипт контролирует рендер камеры в текстуру
public class MainCamera : MonoBehaviour
{
    static public MainCamera main;


    public Camera myCamera;


    /// <summary>
    /// Качество изображения от 0 - плохое до 1 - полное
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

    void iniCamera() {
        if (!myCamera)
        {
            myCamera = GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
