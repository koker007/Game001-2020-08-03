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
    public Text[] text;

    public GraphicRaycaster raycaster;
    private PointerEventData eventData;
    private EventSystem eventSystem;
    [SerializeField]
    private SpriteRenderer sprite;
    List<RaycastResult> result;

    private Collider2D butCollider;
    /// <summary>
    /// номер уровня
    /// </summary>
    public int NumLevel = 0;

    public bool isActive = true;

    private void Awake()
    {
        butCollider = gameObject.GetComponent<Collider2D>();
        eventSystem = MainComponents.eventSystem;
        eventData = new PointerEventData(eventSystem);
    }

    public void Update()
    {
        transform.LookAt(MainComponents.MainCamera.transform);

        if (NumLevel > PlayerProfile.main.ProfilelevelOpen)
        {
            isActive = false;
            sprite.color = Color.gray;
        }
        else
        {
            sprite.color = Color.white;
            isActive = true;
        }
    }

    //нажатие на кнопку
    private void OnMouseDown()
    {
        if(isActive == false)
        {
            return;
        }
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

    public void AnimStartPress() {
        LevelAnim.SetBool("Touch", true);
    }
    public void AnimEndPress() {
        LevelAnim.SetBool("Touch", false);
    }
    public void ClickLevel() {
        SoundCTRL.main.PlaySound(SoundCTRL.main.clipPressButton);
        //LevelGenerator.main.GenerateLevelV2(NumLevel);
        GlobalMessage.LevelInfo(NumLevel);

        AnimEndPress();
    }

    private void OnMouseExit()
    {
        LevelAnim.SetBool("Touch", false);
    }

    //отпускание конпки
    private void OnMouseUpAsButton()
    {
        if(isActive == false)
        {
            return;
        }
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
                    SoundCTRL.main.PlaySound(SoundCTRL.main.clipPressButton);
                    //LevelGenerator.main.GenerateLevelV2(NumLevel);
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

        for(int num = 0; num < text.Length; num++) {
            text[num].text = lev.ToString();
        }
    }
}
