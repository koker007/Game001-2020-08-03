using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorScene : MonoBehaviour
{
    public static DoctorScene main;

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
    public void SetEmotion(Emotions emotion) {
        if (emotion == Emotions.Happy) {
            anim.SetInteger("Emotion", 0);
        }
        else if (emotion == Emotions.Normal) {
            anim.SetInteger("Emotion", 1);
        }
        else if (emotion == Emotions.Sad) {
            anim.SetInteger("Emotion", 2);
        }
    }

    //���������� ����� ���������� � �������� �������� � ���� �����������
    public static RenderTexture GetRender(int wight, int height) {

        //������� ����� ������ ��� ��������� ����������
        RenderTexture renderNew = new RenderTexture(wight, height, 1);

        renderNew.anisoLevel = 0;
        renderNew.antiAliasing = 0;
        renderNew.filterMode = FilterMode.Trilinear;

        main.camera.targetTexture = renderNew;
        main.render = main.camera.targetTexture;

        return renderNew;
    }

    //���������� ��� ������
    public static void SetViewAngle(float angle) {

        float angleNew = angle;

        if (angleNew < 1) angleNew = 1;
        else if (angleNew > 45) angleNew = 45;

        main.camera.fieldOfView = angleNew;

    }

    //���������� ������� ������
    public static void SetCamOffset(Vector2 offset) {
        main.camera.gameObject.transform.localPosition = new Vector3(offset.x, offset.y, 0);
    }
}
