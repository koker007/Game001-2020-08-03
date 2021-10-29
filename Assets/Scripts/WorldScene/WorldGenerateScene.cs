using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//alexandr
//�����
//������
/// <summary>
/// ��������� �������� � ���� �������
/// </summary>
public class WorldGenerateScene : MonoBehaviour
{
    static public WorldGenerateScene main;

    public GameObject PrefLevelButton;
    public GameObject PrefGroundLoc10Grad;
    [SerializeField] private WorldLocation[] MainLocations = new WorldLocation[0];

    [SerializeField] private WorldLocation[] AdditionalLocations = new WorldLocation[0];

    [SerializeField]
    public float angleLocationGenerate = 180;

    private float SetRotationForWorldUp;
    private float SetRotationForWorldDown;
    private int MaxLevel;
    private int MinLevel;
    private const float DistanceFromWorldCenter = 100.5f;
    private const float DistanceSpawnLevelBut = 2.5f;  //check
    //int lvlLastCreate = 0; //check
    private int mainLevels = 0;
    private int numOfCreatedAdditionalLocations = 0;
    private const float WidthSpawnLevelBut = 8f;

    public float posStart = -90;
    /// <summary>
    /// ����������� ��� �������� ����
    /// </summary>
    public float rotationNow;
    public float rotationNeed;
    private const float SpeedLerpRotation = 2f;
    private Transform RotatableObj;
    private float levelAngleSum = 0;


    private bool startRotationSet = false;

    /// <summary>
    /// ����� ������
    /// </summary>
    public class LButton
    {
        public int NumLevel;
        public GameObject obj;

        public void DestroyObj()
        {
            Destroy(obj);
        }

        public void Level()
        {
            obj.GetComponent<LevelButton>().LevelUpdate(NumLevel);
        }
    }

    private List<LButton> LevelButtons = new List<LButton>();
    private GameObject[] Locations = new GameObject[2];
    private int locationCounter;

    private void Awake()
    {
        RotatableObj = MainComponents.RotatableObj.transform;
    }

    private void Start()
    {


        main = this;

        rotationNeed = -0;

        SetRotationForWorldUp = 0;
        SetRotationForWorldDown = -180;

        MaxLevel = 0;
        MinLevel = 1;



        locationCounter = 0;
        //Locations[locationCounter] = Instantiate(PrefabsLocation[1].gameObject, MainComponents.RotatableObj.transform.position, Quaternion.identity, MainComponents.RotatableObj.transform);
        locationCounter++;

        levelAngleSum = -90f; // ��������� ������� -90
        for (int num = 0; num < MainLocations.Length; num++)
        {
            levelAngleSum -= MainLocations[num].lenghtAngle;
        }

        for (int i = 0; i < MainLocations.Length; i++)
        {
            mainLevels += (int)(MainLocations[i].lenghtAngle / 2.5f);
        }
    }

    private void Update()
    {

        if (!startRotationSet) //fix
        {
            //rotationNow = rotationNeed + 30;
            startRotationSet = true;
        }


        RotateMainObject();

        //UpdateMainObject();
        UpdateMainObject2();
    }

    //������� �������
    private void RotateMainObject()
    {
        //�������� ������
        if (rotationNeed >= WorldSlider.main.StartRotation)
        {
            rotationNeed = WorldSlider.main.StartRotation;
        }
        else if (rotationNeed <= WorldSlider.main.StartRotation - (PlayerProfile.main.ProfilelevelOpen * 2.5f))
        {
            rotationNeed = WorldSlider.main.StartRotation - (PlayerProfile.main.ProfilelevelOpen * 2.5f);
        }

        rotationNow = Mathf.Lerp(rotationNow, rotationNeed, Time.deltaTime * SpeedLerpRotation);
        RotatableObj.eulerAngles = new Vector3(rotationNow, 0, 0);
    }

    //�������� ���������� ��������
    private void UpdateMainObject()
    {
        if (rotationNow <= SetRotationForWorldUp - DistanceSpawnLevelBut)
        {
            GenerateLevelButtonUp();
            SetRotationForWorldUp -= DistanceSpawnLevelBut;
            //UpdateLokations(true);
        }
        else if (rotationNow >= SetRotationForWorldUp + DistanceSpawnLevelBut && rotationNow <= 9)
        {
            DeleteLevelButtonUp();
            SetRotationForWorldUp += DistanceSpawnLevelBut;
            //UpdateLokations(false);
        }
        if (rotationNow <= SetRotationForWorldDown - DistanceSpawnLevelBut)
        {
            DeleteLevelButtonDown();
            SetRotationForWorldDown -= DistanceSpawnLevelBut;
        }
        else if (rotationNow >= SetRotationForWorldDown - DistanceSpawnLevelBut && rotationNow <= -181)
        {
            GenerateLevelButtonDown();
            SetRotationForWorldDown += DistanceSpawnLevelBut;
        }
    }

    List<WorldLocation> bufferLocations = new List<WorldLocation>();
    /// <summary>
    /// �������� ������������� �������
    /// </summary>
    private void UpdateMainObject2()
    {

        int lvlLastCreate = 0;

        //��������� ������ �� ������������� �������� �������
        List<WorldLocation> bufferLocationsNew = new List<WorldLocation>();
        foreach (WorldLocation bufferLocation in bufferLocations)
        {
            //���� ������� ���� ��������� �� �� ��������
            if (bufferLocation != null)
            {
                bufferLocation.TestDelete();

                //���� ��� ��� ���� ��������� � ����� ����
                if (bufferLocation != null)
                {
                    bufferLocationsNew.Add(bufferLocation);
                }
            }
        }
        //�������� ������, �����
        bufferLocations = bufferLocationsNew;

        //��������� ����� �� ������������� �������� ������
        List<LButton> bufferButtonsNew = new List<LButton>();
        foreach (LButton button in LevelButtons)
        {
            //���� ���� ������ ���, ����������
            if (button == null || button.obj == null)
            {
                continue;
            }

            bufferButtonsNew.Add(button);
        }
        LevelButtons = bufferButtonsNew;

        float posNow = posStart;
        int lvlNow = 1;

        //���������, ������� ��������������� ������� ��������
        if (rotationNeed <= levelAngleSum)
        {
            numOfCreatedAdditionalLocations = (int)Mathf.Abs(Mathf.Floor((rotationNeed - levelAngleSum) / 90));
        }
        else
        {
            numOfCreatedAdditionalLocations = 0;
        }

        //���� ��� ������� �� ��������, �� ��������� � ���� �������� �������, ������� ��
        if (numOfCreatedAdditionalLocations == 0)
        {
            //���� �� ������ ������� �������������� �������
            for (int num = 0; num < MainLocations.Length; num++)
            {
                //��������� ���� �� ������� � ������� ����� � �������
                bool found = false;
                foreach (WorldLocation bufferLocation in bufferLocations)
                {
                    if (bufferLocation.myAngle == posNow)
                    {
                        found = true;
                        break;
                    }
                }

                //���� ����, � ��������� ��������� �������
                if (!found && Mathf.Abs(posNow - rotationNow) < angleLocationGenerate)
                {
                    GameObject locationObj = Instantiate(MainLocations[num].gameObject, RotatableObj.transform);
                    WorldLocation location = locationObj.GetComponent<WorldLocation>();
                    location.Inicialize(posNow, lvlNow);

                    //��������� ������� ��������

                    //����������� ����� ���������� ������� ���� ������� �� �������
                    int needCreatelvl = (int)(location.lenghtAngle / 2.5f);
                    foreach (LevelPosition levelPosition in location.LevelPositions)
                    {
                        //��������� ��� ������� ������� ��� �� ������
                        bool foundButton = false;
                        foreach (LButton lButton in LevelButtons)
                        {
                            if (lButton.NumLevel == lvlLastCreate)
                            {
                                foundButton = true;
                                break;
                            }
                        }

                        if (!foundButton)
                        {
                            if (levelPosition != null)
                            {
                                CreateNewButtonPos(levelPosition);
                            }
                            else
                            {
                                CreateNewButtonLocate(location);
                            }
                            needCreatelvl--;
                            lvlLastCreate++;
                        }
                    }

                    for (; needCreatelvl > 0;)
                    {
                        //��������� ��� ������� ������� ��� �� ������
                        bool foundButton = false;
                        foreach (LButton lButton in LevelButtons)
                        {
                            if (lButton.NumLevel == lvlLastCreate)
                            {
                                foundButton = true;
                                break;
                            }
                        }

                        if (!foundButton)
                            CreateNewButtonLocate(location);

                        needCreatelvl--;
                        lvlLastCreate++;
                    }


                    bufferLocations.Add(location);

                }
                //����� ���������� ����� ������� ������� ������ �� ������ ���� ����������
                else
                {
                    lvlLastCreate += (int)(MainLocations[num].lenghtAngle / 2.5f);
                }

                //�������� ������� ���� �������
                posNow -= MainLocations[num].lenghtAngle;

            }
        }

        else 
        {
            Debug.Log(numOfCreatedAdditionalLocations * 90 / 2.5f);
            lvlLastCreate += (int)(mainLevels - 1 + (numOfCreatedAdditionalLocations - 1) * 90 / 2.5f);
            float additionalLocationRotation = levelAngleSum - (90 * (numOfCreatedAdditionalLocations - 1));

            int randomAdditionalLocationID = Random.Range(0, AdditionalLocations.Length);


            //��������� ���� �� ������� � ������� ����� � �������
            bool found = false;
            foreach (WorldLocation bufferLocation in bufferLocations)
            {
                if (bufferLocation.myAngle == additionalLocationRotation)
                {
                    found = true;
                    break;
                }
            }

            //���� ����, �������
            if (!found)
            {
                GameObject locationObj = Instantiate(AdditionalLocations[randomAdditionalLocationID].gameObject, RotatableObj.transform);
                WorldLocation location = locationObj.GetComponent<WorldLocation>();
                location.Inicialize(additionalLocationRotation, lvlNow);

                //��������� ������� ��������

                //����������� ����� ���������� ������� ���� ������� �� �������
                int needCreatelvl = (int)(location.lenghtAngle / 2.5f);
                foreach (LevelPosition levelPosition in location.LevelPositions)
                {
                    //��������� ��� ������� ������� ��� �� ������
                    bool foundButton = false;
                    foreach (LButton lButton in LevelButtons)
                    {
                        if (lButton.NumLevel == lvlLastCreate) //fix
                        {
                            foundButton = true;
                            break;
                        }
                    }

                    if (!foundButton)
                    {
                        if (levelPosition != null)
                        {
                            CreateNewButtonPos(levelPosition);
                        }
                        else
                        {
                            CreateNewButtonLocate(location);
                        }
                        needCreatelvl--;
                        lvlLastCreate++;
                    }
                }

                for (; needCreatelvl > 0;)
                {
                    //��������� ��� ������� ������� ��� �� ������
                    bool foundButton = false;
                    foreach (LButton lButton in LevelButtons)
                    {
                        if (lButton.NumLevel == lvlLastCreate)
                        {
                            foundButton = true;
                            break;
                        }
                    }

                    if (!foundButton)
                        CreateNewButtonLocate(location);

                    needCreatelvl--;
                    lvlLastCreate++;
                }


                bufferLocations.Add(location);

            }
        }

        //������� ������ �� ����� ������������� �������
        void CreateNewButtonPos(LevelPosition levelPos)
        {
            if (levelPos == null || lvlLastCreate <= 0) return;

            levelPos.Inicialize(lvlLastCreate);
            LevelButtons.Add(levelPos.button);
            LevelButtons[LevelButtons.Count - 1].Level();
        }

        //������� ������� �� ��������� �������
        void CreateNewButtonLocate(WorldLocation location)
        {

            if (!location || lvlLastCreate <= 0) return;

            Vector3 position = MainComponents.RotatableObj.transform.position;
            float radian;
            radian = (Mathf.Abs(rotationNow) - lvlLastCreate * DistanceSpawnLevelBut) * Mathf.Deg2Rad;
            position = new Vector3(position.x + AlorithmX(lvlLastCreate), position.y + DistanceFromWorldCenter * Mathf.Sin(radian), position.z + DistanceFromWorldCenter * Mathf.Cos(radian));
            LevelButtons.Add(new LButton()
            {
                NumLevel = lvlLastCreate,
                obj = Instantiate(PrefLevelButton, position, Quaternion.identity, location.transform)
            });
            LevelButtons[LevelButtons.Count - 1].Level();
        }
    }

    private void UpdateLokations(bool Up)
    {
        if (SetRotationForWorldUp % 180 == 0)
        {
            if (Up)
            {
                Destroy(Locations[locationCounter]);
                Locations[locationCounter] = Instantiate(MainLocations[Random.Range(0, 3)].gameObject, MainComponents.RotatableObj.transform.position, Quaternion.identity, MainComponents.RotatableObj.transform);
                locationCounter++;
                if (locationCounter > 1)
                {
                    locationCounter = 0;
                }
            }
            else
            {
                locationCounter--;
                if (locationCounter < 0)
                {
                    locationCounter = 1;
                }
                Destroy(Locations[locationCounter]);
                Locations[locationCounter] = Instantiate(MainLocations[Random.Range(0, 3)].gameObject, MainComponents.RotatableObj.transform.position, Quaternion.identity, MainComponents.RotatableObj.transform);
            }
        }
    }

    //��������� � �������� ������ ������ � �����
    private void GenerateLevelButtonUp()
    {
        MaxLevel++;
        Vector3 position = MainComponents.RotatableObj.transform.position;
        float radian;
        radian = (Mathf.Abs(rotationNow) - MaxLevel * DistanceSpawnLevelBut) * Mathf.Deg2Rad;
        position = new Vector3(position.x + AlorithmX(MaxLevel), position.y + DistanceFromWorldCenter * Mathf.Sin(radian), position.z + DistanceFromWorldCenter * Mathf.Cos(radian));
        LevelButtons.Add(new LButton() { NumLevel = MaxLevel, obj = Instantiate(PrefLevelButton, position, Quaternion.identity, MainComponents.RotatableObj.transform) });
        LevelButtons[LevelButtons.Count - 1].Level();
    }

    private void GenerateLevelButtonDown()
    {
        MinLevel--;
        Vector3 position = MainComponents.RotatableObj.transform.position;
        float radian;
        radian = (Mathf.Abs(rotationNow) - MinLevel * DistanceSpawnLevelBut) * Mathf.Deg2Rad;
        position = new Vector3(position.x + AlorithmX(MinLevel), position.y + DistanceFromWorldCenter * Mathf.Sin(radian), position.z + DistanceFromWorldCenter * Mathf.Cos(radian));
        LevelButtons.Insert(0, new LButton() { NumLevel = MinLevel, obj = Instantiate(PrefLevelButton, position, Quaternion.identity, MainComponents.RotatableObj.transform) });
        LevelButtons[0].Level();
    }

    private void DeleteLevelButtonUp()
    {
        LevelButtons[LevelButtons.Count - 1].DestroyObj();
        LevelButtons.RemoveAt(LevelButtons.Count - 1);
        MaxLevel--;
    }

    private void DeleteLevelButtonDown()
    {
        LevelButtons[0].DestroyObj();
        LevelButtons.RemoveAt(0);
        MinLevel++;
    }

    //�������� ������ �� X
    private float AlorithmX(int Level)
    {
        float res;
        res = Mathf.Cos(Mathf.Sin(Level)) * 1000;
        res = res % WidthSpawnLevelBut - WidthSpawnLevelBut / 2;
        return res;
    }

}
