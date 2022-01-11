using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//�����
/// <summary>
/// ������ ������ ������ ��� 3� ������
/// </summary>
public class LevelButton3D : MonoBehaviour
{
    public Color colorNotOpen;
    public Color colorOpen;
    public Color colorComplite;

    public GameObject parentText;
    public Text[] text;

    [SerializeField]
    MeshRenderer SphereMesh;
    Material SphereMat;

    [SerializeField]
    GameObject osnovanie;
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
    /// ����� ������
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
        UpdateVisualization();

        //����� ������ �������� �� ������
        parentText.transform.LookAt(MainComponents.MainCamera.transform);

    }

    void UpdateVisualization() {
        if (Vector3.Distance(MainCamera.main.gameObject.transform.position, gameObject.transform.position) < 50)
        {
            if (bulka.activeSelf) {
                return;
            }

            bulka.SetActive(true);
            osnovanie.SetActive(true);
            parentText.SetActive(true);
        }
        else {
            if (!bulka.activeSelf) 
                return;

            bulka.SetActive(false);
            osnovanie.SetActive(false);
            parentText.SetActive(false);
        }
    }

    //�������� ���������� ������
    void TestProfileOpen() {
        //��������� ���� ������ � ���������� �� ����������
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

    //������� �� ������
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

        for (int num = 0; num < text.Length; num++)
        {
            text[num].text = lev.ToString();
        }
    }

    //������������� ���������
    public void iniMaterial() {
        SphereMat = SphereMesh.material;
    }
}
