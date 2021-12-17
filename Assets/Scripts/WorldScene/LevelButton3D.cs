using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//Семен
/// <summary>
/// Скрипт кнопок уроней для 3Д кнопок
/// </summary>
public class LevelButton3D : MonoBehaviour
{
    public Color colorNotOpen;
    public Color colorOpen;
    public Color colorComplite;

    public Animator LevelAnim;
    public GameObject parentText;
    public Text[] text;

    [SerializeField]
    MeshRenderer SphereMesh;
    Material SphereMat;

    public GraphicRaycaster raycaster;
    private PointerEventData eventData;
    private EventSystem eventSystem;
    [SerializeField]
    private SpriteRenderer sprite;
    List<RaycastResult> result;
    /// <summary>
    /// номер уровня
    /// </summary>
    public int NumLevel = 0;

    public bool isActive = true;

    private void Awake()
    {
        eventSystem = MainComponents.eventSystem;
        eventData = new PointerEventData(eventSystem);
    }

    public void Update()
    {
        //Текст должен смотреть на камеру
        parentText.transform.LookAt(MainComponents.MainCamera.transform);

    }

    private void FixedUpdate()
    {
        //Invoke("TestAnimation", Random.Range(0, 0.25f));
    }

    //Проверить на необходимость воспроизведения анимации для этой кнопки
    void TestAnimation() {

        if (Vector3.Distance(MainCamera.main.animatePos, gameObject.transform.position) < MainCamera.animateDist)
        {
            if (LevelAnim.enabled)
                return;

            LevelAnim.enabled = true;
            LevelAnim.Rebind();
            LevelAnim.Update(0f);
        }
        else
        {
            if (!LevelAnim.enabled) 
                return;
                
            LevelAnim.enabled = false;
            
        }
    }

    //Проверка активности кнопки
    void TestProfileOpen() {
        //Поставить цвет кнопки и установить ее активность
        if (NumLevel > PlayerProfile.main.ProfilelevelOpen)
        {
            isActive = false;
            SphereMat.color = colorNotOpen;

            //sprite.color = Color.gray;
        }
        else
        {
            //sprite.color = Color.white;
            isActive = true;
            SphereMat.color = colorOpen;

        }
    }

    //нажатие на кнопку
    private void OnMouseDown()
    {
        if (isActive == false)
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

    public void AnimStartPress()
    {
        LevelAnim.SetBool("Press", true);
    }
    public void AnimEndPress()
    {
        LevelAnim.SetBool("Press", false);
    }

    public void Start()
    {
        iniMaterial();
        InvokeRepeating("TestProfileOpen", 0, Random.Range(1, 2.5f));
    }
    public void ClickLevel()
    {
        SoundCTRL.main.PlaySound(SoundCTRL.main.clipPressButtonLVL);
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
        if (isActive == false)
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
                    SoundCTRL.main.PlaySound(SoundCTRL.main.clipPressButtonLVL);
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

        for (int num = 0; num < text.Length; num++)
        {
            text[num].text = lev.ToString();
        }
    }

    //Инициализация материала
    public void iniMaterial() {
        SphereMat = SphereMesh.material;
    }
}
