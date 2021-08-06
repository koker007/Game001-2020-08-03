using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Содержит основные компоненты и обьекты находщиеся на сцене
/// </summary>
public class MainComponents : MonoBehaviour
{
    public static GameObject MainCamera;
    public static GameObject RotatableObj;

    private void Awake()
    {
        MainCamera = GameObject.Find("Main Camera");
        RotatableObj = GameObject.Find("RotatableObj");
    }

}
