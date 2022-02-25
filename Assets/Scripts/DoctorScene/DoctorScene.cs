using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorScene : MonoBehaviour
{
    public static DoctorScene main;


    [SerializeField]
    GameObject objBottle;
    [SerializeField]
    GameObject objDocument;
    [SerializeField]
    GameObject objMask;

    [SerializeField]
    RenderTexture render;
    [SerializeField]
    Camera camera;
    [SerializeField]
    Animator anim;

    public enum Emotions {
        Happy,
        Normal,
        Sad
    }
    public Emotions emotion;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        GetRender(100, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //��������� ����� ������ � �������
    public static void SetEmotion(Emotions emotionNew) {
        if (main == null) return;

        main.emotion = emotionNew;

        if (main.emotion == Emotions.Happy) {
            if(main.objBottle != null) main.objBottle.SetActive(true);
            if (main.objDocument != null) main.objDocument.SetActive(false);
            if(main.objMask) main.objMask.SetActive(false);

            main.anim.SetInteger("emotion", 3);
        }
        else if (main.emotion == Emotions.Normal) {

            if (main.objBottle != null) main.objBottle.SetActive(true);
            if (main.objDocument != null) main.objDocument.SetActive(true);
            if (main.objMask) main.objMask.SetActive(true);
            main.anim.SetInteger("emotion", 1);
        }
        else if (main.emotion == Emotions.Sad) {
            if (main.objBottle != null) main.objBottle.SetActive(false);
            if (main.objDocument != null) main.objDocument.SetActive(false);
            if (main.objMask) main.objMask.SetActive(true);
            main.anim.SetInteger("emotion", 2);
        }
    }

    //���������� ����� ���������� � �������� �������� � ���� �����������
    public static RenderTexture GetRender(int wight, int height) {

        //������� ����� ������ ��� ��������� ����������
        RenderTexture renderNew = new RenderTexture(wight, height, 1);

        renderNew.anisoLevel = 0;
        renderNew.antiAliasing = 1;
        renderNew.filterMode = FilterMode.Trilinear;

        renderNew.depth = 24;
        renderNew.autoGenerateMips = true;
        renderNew.useMipMap = true;

        //� ������ ����� �� ����
        if (main != null)
        {
            main.camera.targetTexture = renderNew;
            main.render = main.camera.targetTexture;
        }

        return renderNew;
    }

    //���������� ��� ������
    public static void SetViewAngle(float angle) {

        float angleNew = angle;

        if (angleNew < 1) angleNew = 1;
        else if (angleNew > 45) angleNew = 45;

        //���� �� �������������������� �������
        if (main == null) return;

        main.camera.fieldOfView = angleNew;

    }

    //���������� ������� ������
    public static void SetCamOffset(Vector2 offset) {
        if (main == null) return;

        main.camera.gameObject.transform.localPosition = new Vector3(offset.x, offset.y, 0);
    }

    public void CameraSetActive(bool value)
    {
        camera.gameObject.SetActive(value);
    }
}
