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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Emotion emotion = Emotion.Normal;
    Emotion emotionOld;

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
}
