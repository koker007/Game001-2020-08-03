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

    [SerializeField]
    bool TestEmotions = false;
    [SerializeField]
    GameObject[] Heads;
    [SerializeField]
    GameObject[] Eyes;

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
