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

    //Поставить новую эмоцию у доктора
    public void SetEmotion(Emotions emotion) {
        if (emotion == Emotions.Happy) {
            anim.SetInteger("Emotion", 0);
        }
        else if (emotion == Emotions.Normal) {
            anim.SetInteger("Emotion", 1);
        }
        else if (emotion == Emotions.Sad) {
            anim.SetInteger("emotion", 2);
        }
    }

    //Установить новое разрешение и получить текстуру с этим разрешением
    public static RenderTexture GetRender(int wight, int height) {

        //Создаем новый рендер под указанное разрешение
        RenderTexture renderNew = new RenderTexture(wight, height, 1);

        renderNew.anisoLevel = 0;
        renderNew.antiAliasing = 0;
        renderNew.filterMode = FilterMode.Trilinear;

        main.camera.targetTexture = renderNew;
        main.render = main.camera.targetTexture;

        return renderNew;
    }
}
