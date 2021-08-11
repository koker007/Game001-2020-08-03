using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseCTRL : MonoBehaviour
{
    static public MouseCTRL main;

    [SerializeField]
    public bool ClickDouble = false;
    [SerializeField]
    bool Click = false;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    float timeLastClick = 0;

    public void click() {
        //если клик был быстрым
        if (Time.unscaledTime - timeLastClick < 0.5f)
        {
            ClickDouble = true;
        }
        else {
            ClickDouble = false;
        }

        //«апоминаем врем€ клика
        timeLastClick = Time.unscaledTime;


    }



}
