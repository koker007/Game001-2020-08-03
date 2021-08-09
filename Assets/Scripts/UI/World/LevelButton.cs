using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//alexandr
/// <summary>
/// Скрипт кнопок уровней
/// </summary>
public class LevelButton : MonoBehaviour
{

    public Animator LevelAnim;
    public Text text;

    public GraphicRaycaster raycaster;
    private PointerEventData eventData;
    private EventSystem eventSystem;
    List<RaycastResult> result;

    private Collider2D butCollider;
    /// <summary>
    /// номер уровня
    /// </summary>
    public int NumLevel = 0;

    private void Awake()
    {
        butCollider = gameObject.GetComponent<Collider2D>();
        eventSystem = MainComponents.eventSystem;
        eventData = new PointerEventData(eventSystem);
    }

    private void Update()
    {
        //поворот кнопки к камере
        transform.LookAt(MainComponents.MainCamera.transform);
    }

    //нажатие на кнопку
    private void OnMouseDown()
    {
        RayCast();
        if (result.Count <= 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform.tag == "LevelButton")
                {
                    LevelAnim.SetBool("Touch", true);
                }
            }
        }
    }

    private void OnMouseExit()
    {
        LevelAnim.SetBool("Touch", false);
    }

    //отпускание конпки
    private void OnMouseUpAsButton()
    {
        LevelAnim.SetBool("Touch", false);

        RayCast();
        if (result.Count <= 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform.tag == "LevelButton")
                {
                    GlobalMessage.LevelInfo(NumLevel);
                }
            }
        }
    }

    //луч на кнопку
    private void RayCast()
    {
        raycaster = MainComponents.graphicRaycaster;
        eventData.position = Input.mousePosition;
        result = new List<RaycastResult>();
        raycaster.Raycast(eventData, result);
    }

    //обновление значения уровня у кнопки
    public void LevelUpdate(int lev)
    {
        NumLevel = lev;
        text.text = lev.ToString();
    }
}
