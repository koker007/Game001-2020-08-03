using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float angleForvardSpawn = 35;


    private int MaxLevel;
    private int MinLevel;
    private const float DistanceFromWorldCenter = 100.5f;
    private const float DistanceSpawnLevelBut = 2.5f; //���������� ����� ��������
    //int lvlLastCreate = 0;
    private int mainLevels = 0;
    private int numOfCreatedAdditionalLocations = 0;
    private const float WidthSpawnLevelBut = 8f;

    public float posStart = -90;
    /// <summary>
    /// ����������� ��� �������� ����
    /// </summary>
    public float rotationNow;
    public float rotationNeed;
    private const float SpeedLerpRotation = 5f;
    public Transform RotatableObj;
    private float levelAngleSum = 0;

    [SerializeField] private float additionalLocationSize;

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
            obj.GetComponent<LevelButton3D>().LevelUpdate(NumLevel);
        }
    }

    [SerializeField] private List<GameObject> LevelButtonsObjects = new List<GameObject>(new GameObject[50]);
    private List<LButton> LevelButtons = new List<LButton>();
    private GameObject[] Locations = new GameObject[2];
    private int locationCounter;

    private void Awake()
    {
        RotatableObj = MainComponents.RotatableObj.transform;
        main = this;
    }

    private void Start()
    {
        main = this;

        //��������� ���� ���������� �����������
        Invoke("InvokeFonImage", 0);

        rotationNeed = -0;


        MaxLevel = 0;
        MinLevel = 1;

        InicializeButtonBufer();

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

        rotationNeed = -(PlayerProfile.main.ProfilelevelOpen * 2.5f) - 115;

        
    }

    private void Update()
    {
        RotateMainObject();

        //UpdateMainObject();
        UpdateMainObject2();


    }
    void InvokeFonImage() {
        //���� ��������� � ����������� ���������
        float distFromAngleOld = 99999999999;

        if (MenuGameplay.main == null)
        {
            Invoke("InvokeFonImage", Random.Range(0.5f, 1f));
            return;
        }

        Texture nearestFon = MenuGameplay.main.ImageFon.texture;

        //���������� ��� ������� � ������ ���� ���������
        foreach (WorldLocation location in bufferLocations) {
            //�������� ������ ������ ���� � ������� ���� �� ������ ���
            if (location.fonGameplay == null)
                continue;

            //������ ������� 2 ���� ������� ��������� � ��������
            float start = location.myAngle - 25;
            float end = location.myAngle - location.lenghtAngle + 1 - 25;

            //��������� �� ������
            float distStart = Mathf.Abs(rotationNow - start);
            //��������� �� �����
            float distEnd = Mathf.Abs(rotationNow - end);
            
            //���� ����� ����� ��� ���������� ������
            if (distStart < distFromAngleOld) {
                distFromAngleOld = distStart;
                nearestFon = location.fonGameplay;
            }
            //���� ����� ����� ��� ���������� ������
            else if(distEnd < distFromAngleOld) {
                distFromAngleOld = distEnd;
                nearestFon = location.fonGameplay;
            }
        }

        //���� �� ������ �������� �������� ���������� �� ������� ������ ��
        if (MenuGameplay.main.ImageFon.texture != nearestFon) {
            MenuGameplay.main.ImageFon.texture = nearestFon;
        }

        //��������� �������� �� ������ ����, ����� ������� ��������
        Invoke("InvokeFonImage", Random.Range(0.5f, 1f));
    }

    private void InicializeButtonBufer()
    {
        for (int i = 0; i < LevelButtonsObjects.Count; i++)
        {
            LevelButtonsObjects[i] = Instantiate(PrefLevelButton, transform.position, Quaternion.identity, RotatableObj);
            LevelButtonsObjects[i].SetActive(false);
        }
    }

    //������� �������
    private void RotateMainObject()
    {
        if (rotationNeed == rotationNow)
            return;

        //������� ���� ��� �� ��������������������
        if (WorldSlider.main == null) return;

        //�������� ������
        if (rotationNeed >= WorldSlider.main.StartRotation)
        {
            rotationNeed = WorldSlider.main.StartRotation;
        }
        else if (rotationNeed <= WorldSlider.main.StartRotation - ((PlayerProfile.main.ProfilelevelOpen + 10) * 2.5f))
        {
            rotationNeed = WorldSlider.main.StartRotation - ((PlayerProfile.main.ProfilelevelOpen + 10) * 2.5f);
        }

        float timeOffSet = Time.deltaTime * SpeedLerpRotation;
        if (timeOffSet > 1) timeOffSet = 1;
        rotationNow = Mathf.Lerp(rotationNow, rotationNeed, timeOffSet);
        if (rotationNow > -110) rotationNow = -110;

        if (Mathf.Abs(rotationNeed - rotationNow) < 0.01f) {
            rotationNow = rotationNeed;
        }
        
        RotatableObj.eulerAngles = new Vector3(rotationNow, 0, 0);
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
        if (Mathf.Abs(rotationNow - levelAngleSum) <= additionalLocationSize && rotationNow > levelAngleSum)
        {
            numOfCreatedAdditionalLocations = 1;
        }
        else if (Mathf.Abs(rotationNow - levelAngleSum) <= additionalLocationSize && rotationNow <= levelAngleSum)
        {
            numOfCreatedAdditionalLocations = 2;
        }
        else if (rotationNow - levelAngleSum <= -additionalLocationSize)
        {
            numOfCreatedAdditionalLocations = (int)Mathf.Abs(Mathf.Floor((rotationNow - levelAngleSum) / additionalLocationSize)) + 1;

        }
        else
        {
            numOfCreatedAdditionalLocations = 0;
        }

        //���� ��� ������� ������ 3, ������� �������� �������
        if (numOfCreatedAdditionalLocations <= 2)
        {
            //���� �� ������ ������� �������������� �������
            for (int num = 0; num < MainLocations.Length; num++)
            {
                //��������� ���� �� ������� � ������� ����� � ������� ��� ��������� �������
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
                // ������ Mathf.Abs(rotationNow + 45 - posNow) <= MainLocations[num].lenghtAngle
                if (!found
                    && rotationNow - posNow < angleForvardSpawn
                    && rotationNow - posNow > (MainLocations[num].lenghtAngle + 45) * -1
                    )
                {
                    GameObject locationObj = Instantiate(MainLocations[num].gameObject, RotatableObj.transform);
                    //GameObject locationObj = ObjectPooler.main.SpawnMainLocation(num);
                    locationObj.SetActive(true);
                    WorldLocation location = locationObj.GetComponent<WorldLocation>();
                    location.Inicialize(posNow, lvlNow);

                    //��������� ������� ��������

                    //����������� ����� ���������� ������� ���� ������� �� �������
                    int needCreatelvl = (int)(location.lenghtAngle / 2.5f);
                    foreach (LevelPosition levelPosition in location.LevelPositions)
                    {
                        //��������� ��� ������� ������� ��� �� ������
                        if (!CheckButtons(lvlLastCreate))
                        {
                            if (levelPosition != null)
                            {
                                CreateNewButtonPos(location, levelPosition);
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
                        if (!CheckButtons(lvlLastCreate))
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

        //���� �������� ��� �������, ������� ��
        if (numOfCreatedAdditionalLocations > 0)
        {
            
            //������� �� 3 �������: ���� ��������, ����� �������, ������ - �����, ������ - �������
            for (int locationsToSpawn = 0; locationsToSpawn < 3; locationsToSpawn++)
            {


                //���� ������ ��� �������
                float additionalLocationRotation = levelAngleSum - 30 * (numOfCreatedAdditionalLocations - locationsToSpawn - 1);

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

                //���� ���, �������
                //���� ���� ������ ��� ������� ������ (������) ��� ����� ���� ����� ��������� �������� �������, ������� �� (��� �������)
                if (!found && additionalLocationRotation <= levelAngleSum && Mathf.Abs(WorldGenerateScene.main.rotationNow - additionalLocationRotation) <= 135)
                {

                    //������������ ��������������� ID ������� �� ������ ���������� ���������� ��� �������

                    //����� ����� �� ������� �� 10
                    int integer = AdditionalLocations.Length;

                    //����� ��������
                    int remCounter = 1;

                    //�������, ������� ��� ����� �������� �� 10
                    while (integer >= 9)
                    {
                        remCounter++;
                        integer = Mathf.FloorToInt(integer / 10);
                    }

                    //������ ��������
                    float[] rem = new float[remCounter];

                    integer = AdditionalLocations.Length;

                    //��������� ������ ��������
                    for (int i = remCounter; i > 0; i--)
                    {
                        rem[rem.Length - i] = integer % 10;

                        if (i > 1)
                        {
                            integer = Mathf.FloorToInt(integer / 10);
                        }
                        else
                        {
                            rem[rem.Length - i]--;
                           
                        }                       
                    }                    

                    //������� ID ��� �������
                    int randomAdditionalLocationID = 0;

                    for (int i = remCounter; i > 0; i--)
                    {
                        //���������� �� ������ ����
                        //int perVar = (int)Mathf.Floor(Mathf.PerlinNoise(Mathf.Sin(numOfCreatedAdditionalLocations * 10 * i) * 77.77f, 0) * 10000 % 9) ;
                        int perVar = (int)Mathf.Floor(Mathf.PerlinNoise(numOfCreatedAdditionalLocations + i + 0.777f, 0) * 10000 % 9);
                        //��������� �������
                        int randRem = Mathf.RoundToInt(rem[i - 1] / 8 * perVar);

                        if (i > 1)    
                        {                                                    
                            randomAdditionalLocationID += integer * perVar + randRem;
                        }
                        else
                        {
                            randomAdditionalLocationID += randRem;
                        }
                    }

                    GameObject locationObj = Instantiate(AdditionalLocations[randomAdditionalLocationID].gameObject, RotatableObj.transform);
                    //GameObject locationObj = ObjectPooler.main.SpawnAdditionalLocation(0); //fix
                    locationObj.SetActive(true);
                    WorldLocation location = locationObj.GetComponent<WorldLocation>();
                    location.Inicialize(additionalLocationRotation, lvlNow);
                    //��������� ������� ��������

                    //�������, ����� ����� ������ ������ ������ �� ������� (����������� �� ����)
                    lvlLastCreate = (int)(mainLevels + (numOfCreatedAdditionalLocations - locationsToSpawn - 1) * additionalLocationSize / 2.5f);
                    //����������� ����� ���������� ������� ���� ������� �� �������
                    int needCreatelvl = (int)(location.lenghtAngle / 2.5f);

                    foreach (LevelPosition levelPosition in location.LevelPositions)
                    {
                        //��������� ��� ������� ������� ��� �� ������
                        if (!CheckButtons(lvlLastCreate))
                        {

                            if (levelPosition != null)
                            {
                                CreateNewButtonPos(location, levelPosition);
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
                        if (!CheckButtons(lvlLastCreate))
                            CreateNewButtonLocate(location);

                        needCreatelvl--;
                        lvlLastCreate++;
                    }

                    bufferLocations.Add(location);

                }


            }
        }

        //������� ������ �� ����� ������������� �������
        void CreateNewButtonPos(WorldLocation location, LevelPosition levelPos)
        {
            if (levelPos == null || lvlLastCreate <= 0) return;
            GameObject but = FindButtonObject();
            levelPos.Inicialize(but, lvlLastCreate);
            LevelButtons.Add(levelPos.button);
            LevelButtons[LevelButtons.Count - 1].Level();
            location.AddLevelButton(but);
        }

        //������� ������� �� ��������� �������
        void CreateNewButtonLocate(WorldLocation location)
        {
            if (!location || lvlLastCreate <= 0) return;

            Vector3 position = MainComponents.RotatableObj.transform.position;
            float radian;
            radian = (Mathf.Abs(rotationNow) - lvlLastCreate * DistanceSpawnLevelBut) * Mathf.Deg2Rad;
            position = new Vector3(position.x + AlorithmX(lvlLastCreate), position.y + DistanceFromWorldCenter * Mathf.Sin(radian), position.z + DistanceFromWorldCenter * Mathf.Cos(radian));

            GameObject but = FindButtonObject();
            but.transform.SetParent(location.transform);
            location.AddLevelButton(but);
            but.transform.position = position;
            but.transform.rotation = Quaternion.identity;

            LevelButtons.Add(new LButton()
            {
                NumLevel = lvlLastCreate,
                obj = but
            });
            LevelButtons[LevelButtons.Count - 1].Level();
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

    private GameObject FindButtonObject()
    {
        GameObject but = null;
        foreach (GameObject button in LevelButtonsObjects)
        {
            if(button.activeSelf == false)
            {
                but = button;
                break;
            }
        }

        if(but == null)
        {
            LevelButtonsObjects.Add(Instantiate(PrefLevelButton, transform.position, Quaternion.identity, RotatableObj));
            but = LevelButtonsObjects[LevelButtonsObjects.Count - 1];
        }

        but.SetActive(true);
        return but;
    }

    private bool CheckButtons(int lvlLastCreate)
    {
        bool foundButton = false;
        foreach (LButton lButton in LevelButtons)
        {
            if (lButton.NumLevel == lvlLastCreate)
            {
                foundButton = true;
                break;
            }
        }
        return false;
    }

}
