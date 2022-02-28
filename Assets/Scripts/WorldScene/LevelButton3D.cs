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
    public Color colorGold;

    public GameObject parentText;
    public Text[] text;

    [SerializeField]
    MeshRenderer SphereMesh;
    Material SphereMat;

    [SerializeField]
    GameObject osnovanieALL;

    [SerializeField]
    GameObject osnovanie;
    [SerializeField]
    GameObject osnovanieSkirt;

    [SerializeField]
    GameObject bulka;

    [SerializeField]
    Image[] StarsComplite;

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

    //Золотое прохождение, старый результат
    int goldenOld = 0;
    int starsOld = 0;

    private void Awake()
    {
        eventSystem = MainComponents.eventSystem;
        eventData = new PointerEventData(eventSystem);
    }
    public void OnEnable()
    {
        iniMaterial();
        InvokeRepeating("TestProfileOpen", 0, Random.Range(1, 2.5f));
    }

    public void Update()
    {
        UpdateVisualization();

        //Текст должен смотреть на камеру
        parentText.transform.LookAt(MainComponents.MainCamera.transform);

    }

    void UpdateVisualization() {
        if (Vector3.Distance(MainCamera.main.gameObject.transform.position + MainCamera.main.gameObject.transform.forward * 35, gameObject.transform.position) < 40)
        {
            if (bulka.activeSelf) {
                return;
            }

            bulka.SetActive(true);
            osnovanieALL.SetActive(true);
            parentText.SetActive(true);
        }
        else {
            if (!bulka.activeSelf) 
                return;

            bulka.SetActive(false);
            osnovanieALL.SetActive(false);
            parentText.SetActive(false);
        }
    }

    //Проверка активности кнопки
    void TestProfileOpen() {

        TestStars();
        TestGold();

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

            if (goldenOld == 0)
                SphereMat.color = colorOpen;
            else if (goldenOld == 1)
                SphereMat.color = colorGold;
        }

        //Проверка звезд
        void TestStars() {

            if (PlayerProfile.main.LVLStar.Length > NumLevel && starsOld != PlayerProfile.main.LVLStar[NumLevel] && PlayerProfile.main.LVLStar[NumLevel] > 0)
            {
                //1
                if (PlayerProfile.main.LVLStar[NumLevel] == 1)
                {
                    StarsComplite[0].gameObject.SetActive(true);
                    StarsComplite[1].gameObject.SetActive(false);
                    StarsComplite[2].gameObject.SetActive(false);
                }
                //2
                else if (PlayerProfile.main.LVLStar[NumLevel] == 2)
                {
                    StarsComplite[0].gameObject.SetActive(true);
                    StarsComplite[1].gameObject.SetActive(true);
                    StarsComplite[2].gameObject.SetActive(false);
                }
                //3
                else if (PlayerProfile.main.LVLStar[NumLevel] == 3)
                {
                    StarsComplite[0].gameObject.SetActive(true);
                    StarsComplite[1].gameObject.SetActive(true);
                    StarsComplite[2].gameObject.SetActive(true);
                }
            }
            else
            {
                StarsComplite[0].gameObject.SetActive(false);
                StarsComplite[1].gameObject.SetActive(false);
                StarsComplite[2].gameObject.SetActive(false);
            }
        }
        void TestGold() {

            if (PlayerProfile.main.LVLGold.Length > NumLevel && PlayerProfile.main.LVLGold[NumLevel] > 0)
            {
                //Пройдено на золото
                if (PlayerProfile.main.LVLGold[NumLevel] == 1)
                {
                    osnovanie.gameObject.SetActive(false);
                    osnovanieSkirt.gameObject.SetActive(true);
                }

                goldenOld = PlayerProfile.main.LVLGold[NumLevel];
            }
            else
            {
                osnovanie.gameObject.SetActive(true);
                osnovanieSkirt.gameObject.SetActive(false);
            }
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

            }
        }
    }


    public void ClickLevel()
    {
        SoundCTRL.main.PlaySound(SoundCTRL.main.clipPressButtonLVL);
        //LevelGenerator.main.GenerateLevelV2(NumLevel);
        GlobalMessage.LevelInfo(NumLevel);
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
