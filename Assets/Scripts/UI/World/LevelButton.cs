using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//alexandr
/// <summary>
/// ������ ������ �������
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
    /// ����� ������
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
        //������� ������ � ������
        transform.LookAt(MainComponents.MainCamera.transform);
    }

    //������� �� ������
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

    //���������� ������
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

    //��� �� ������
    private void RayCast()
    {
        raycaster = MainComponents.graphicRaycaster;
        eventData.position = Input.mousePosition;
        result = new List<RaycastResult>();
        raycaster.Raycast(eventData, result);
    }

    //���������� �������� ������ � ������
    public void LevelUpdate(int lev)
    {
        NumLevel = lev;
        text.text = lev.ToString();
    }
}
