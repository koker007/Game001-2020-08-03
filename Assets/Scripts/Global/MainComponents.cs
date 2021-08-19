using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//alexandr
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
    public static GameObject Vertical;
    [SerializeField]
    public static GameObject PanelMap;
    [SerializeField]
    public static EventSystem eventSystem;
    [SerializeField]
    public static GraphicRaycaster graphicRaycaster;

    private void Awake()
    {
        MainCamera = GameObject.Find("Main Camera");
        RotatableObj = GameObject.Find("RotatableObj");
        Vertical = GameObject.Find("Vertical");
        PanelMap = GameObject.Find("PanelMap");
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        graphicRaycaster = GameObject.Find("UI").GetComponent<GraphicRaycaster>();
    }

}
