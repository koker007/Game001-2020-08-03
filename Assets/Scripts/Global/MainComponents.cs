using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Содержит основные компоненты и обьекты находщиеся на сцене
/// </summary>
public class MainComponents : MonoBehaviour
{
    [SerializeField]
    public static GameObject MainCamera;
    [SerializeField]
    public static GameObject RotatableObj;
    [SerializeField]
    public static EventSystem eventSystem;
    [SerializeField]
    public static GraphicRaycaster graphicRaycaster;
    [SerializeField]
    public static SoundManager soundManager;

    private void Awake()
    {
        MainCamera = GameObject.Find("Main Camera");
        RotatableObj = GameObject.Find("RotatableObj");
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        graphicRaycaster = GameObject.Find("UI").GetComponent<GraphicRaycaster>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

}
