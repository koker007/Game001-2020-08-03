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

    void Start()
    {
        main = this;
    }

    float timeLastClick = 0;

    public void click() {
        //???? ???? ??? ???????
        if (Time.unscaledTime - timeLastClick < 0.5f)
        {
            ClickDouble = true;
        }
        else {
            ClickDouble = false;
        }

        //?????????? ????? ?????
        timeLastClick = Time.unscaledTime;


    }



}
