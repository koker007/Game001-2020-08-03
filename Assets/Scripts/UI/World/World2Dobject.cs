using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// родительский скрипт всех обьектов на карте
/// </summary>
public class World2Dobject : MonoBehaviour
{
    void Update()
    {
        //поворот обьекта к камере
        transform.LookAt(MainComponents.MainCamera.transform);
    }
}
