using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(TestEmotions)
            InvokeRepeating("RandomEmotion", 1, 0.5f);

        invokeTypeEye();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEmotions();
    }

    public Emotion emotion = Emotion.Normal;
    Emotion emotionOld = Emotion.Normal;

    [SerializeField]
    bool TestEmotions = false;
    [SerializeField]
    GameObject[] Heads;
    [SerializeField]
    GameObject[] Eyes;

    [Header("Head")]
    [SerializeField]
    GameObject HeadNormal;
    [SerializeField]
    GameObject HeadAgress;
    [SerializeField]
    GameObject HeadHappy;
    [SerializeField]
    GameObject HeadSad;
    [SerializeField]
    GameObject HeadDisgust;

    [Header("Eyes")]
    [SerializeField]
    GameObject EyeNormalClose;
    [SerializeField]
    GameObject EyeNormalUp;
    [SerializeField]
    GameObject EyeNormalLeft;
    [SerializeField]
    GameObject EyeNormalCamera;

    [SerializeField]
    GameObject EyeAgressClose;
    [SerializeField]
    GameObject EyeAgressUp;
    [SerializeField]
    GameObject EyeAgressLeft;
    [SerializeField]
    GameObject EyeAgressCamera;

    [SerializeField]
    GameObject eyeHappyClose;
    [SerializeField]
    GameObject eyeHappyUp;
    [SerializeField]
    GameObject eyeHappyLeft;
    [SerializeField]
    GameObject eyeHappyCamera;

    [SerializeField]
    GameObject EyeSadClose;
    [SerializeField]
    GameObject EyeSadUp;
    [SerializeField]
    GameObject EyeSadLeft;
    [SerializeField]
    GameObject EyeSadCamera;

    [SerializeField]
    GameObject eyeDisgustClose;
    [SerializeField]
    GameObject eyeDisgustUp;
    [SerializeField]
    GameObject eyeDisgustLeft;
    [SerializeField]
    GameObject eyeDisgustCamera;

    public enum Emotion {
        Normal,
        Agress,
        Happy,
        Sad,
        Disgust
    }

    void RandomEmotion() {
        //Сперва выключаем все эмоции
        foreach (GameObject obj in Heads) {
            obj.SetActive(false);
        }
        foreach (GameObject obj in Eyes)
        {
            obj.SetActive(false);
        }

        //Теперь включаем рандомные
        Heads[Random.Range(0, Heads.Length)].SetActive(true);
        Eyes[Random.Range(0, Eyes.Length)].SetActive(true);
    }


    float timeEyeClose = 0.5f;
    int typeEye = 0;


    //Обновить эмоцию
    void UpdateEmotions()
    {
        timeEyeClose -= Time.unscaledDeltaTime;
        //Сброс моргания
        if (timeEyeClose < 0)
            timeEyeClose = Random.Range(0.5f, 5.0f);

        //Выбираем эмоцию



        //Согласно выбранной эмоции выбираем голову
        if (emotion != emotionOld)
        {
            closeAllHead();

            GetHead().SetActive(true);

            emotionOld = emotion;
        }


        closeAllEyes();

        //Согласно выбранной эмоции выбираем глаза
        GameObject selectEye;
        if (emotion == Emotion.Normal)
            selectEye = GetNormal();
        else if (emotion == Emotion.Agress)
            selectEye = GetAgress();
        else if (emotion == Emotion.Happy)
            selectEye = GetHappy();
        else if (emotion == Emotion.Sad)
            selectEye = GetSad();
        else selectEye = GetDisgust();

        //Активируем глаза
        selectEye.SetActive(true);



        void closeAllEyes()
        {

            EyeNormalClose.SetActive(false);
            EyeNormalUp.SetActive(false);
            EyeNormalLeft.SetActive(false);
            EyeNormalCamera.SetActive(false);

            EyeAgressClose.SetActive(false);
            EyeAgressUp.SetActive(false);
            EyeAgressLeft.SetActive(false);
            EyeAgressCamera.SetActive(false);

            eyeHappyClose.SetActive(false);
            eyeHappyUp.SetActive(false);
            eyeHappyLeft.SetActive(false);
            eyeHappyCamera.SetActive(false);

            EyeSadClose.SetActive(false);
            EyeSadUp.SetActive(false);
            EyeSadLeft.SetActive(false);
            EyeSadCamera.SetActive(false);

            eyeDisgustClose.SetActive(false);
            eyeDisgustUp.SetActive(false);
            eyeDisgustLeft.SetActive(false);
            eyeDisgustCamera.SetActive(false);
        }
        void closeAllHead()
        {
            HeadNormal.SetActive(false);
            HeadAgress.SetActive(false);
            HeadHappy.SetActive(false);
            HeadSad.SetActive(false);
            HeadDisgust.SetActive(false);
        }

        GameObject GetNormal()
        {
            if (timeEyeClose < 0.1f)
                return EyeNormalClose;

            if (typeEye == 0)
                return EyeNormalUp;
            else if (typeEye == 1)
                return EyeNormalLeft;
            else
                return EyeNormalCamera;
        }
        GameObject GetAgress()
        {
            if (timeEyeClose < 0.1f)
                return EyeAgressClose;

            if (typeEye == 0)
                return EyeAgressUp;
            else if (typeEye == 1)
                return EyeAgressLeft;
            else
                return EyeAgressCamera;
        }
        GameObject GetHappy()
        {
            if (timeEyeClose < 0.1f)
                return eyeHappyClose;

            if (typeEye == 0)
                return eyeHappyUp;
            else if (typeEye == 1)
                return eyeHappyLeft;
            else
                return eyeHappyCamera;
        }
        GameObject GetSad()
        {
            if (timeEyeClose < 0.1f)
                return EyeSadClose;

            if (typeEye == 0)
                return EyeSadUp;
            else if (typeEye == 1)
                return EyeSadLeft;
            else
                return EyeSadCamera;
        }
        GameObject GetDisgust()
        {
            if (timeEyeClose < 0.1f)
                return eyeDisgustClose;

            if (typeEye == 0)
                return eyeDisgustUp;
            else if (typeEye == 1)
                return eyeDisgustLeft;
            else
                return eyeDisgustCamera;
        }

        GameObject GetHead()
        {
            if (emotion == Emotion.Normal)
                return HeadNormal;
            else if (emotion == Emotion.Agress)
                return HeadAgress;
            else if (emotion == Emotion.Happy)
                return HeadHappy;
            else if (emotion == Emotion.Sad)
                return HeadSad;
            else return HeadDisgust;
        }
    }

    void invokeTypeEye() {
        Invoke("invokeTypeEye", Random.Range(0.1f, 1f));

        typeEye = Random.Range(0,3);
    }
}
