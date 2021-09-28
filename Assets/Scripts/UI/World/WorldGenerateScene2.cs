using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//alexandr
//�����
/// <summary>
/// ��������� �������� � ���� �������
/// </summary>
public class WorldGenerateScene2 : MonoBehaviour
{
    static public WorldGenerateScene2 main;

    public GameObject PrefLevelButton;
    public WorldLocation[] PrefabsLocation = new WorldLocation[0];

    private int MaxLevel;
    private int MinLevel;
    private const float DistanceFromWorldCenter = 100.5f;
    private const float DistanceSpawnLevelBut = 5f;
    private const float WidthSpawnLevelBut = 8f;

    /// <summary>
    /// ����������� ��� �������� ����
    /// </summary>
    public float rotationNow;
    public static float RealRotation;
    private const float SpeedLerpRotation = 2f;
    private Transform RotatableObj;

    /// <summary>
    /// ����� ������
    /// </summary>
    private class LButton
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

    private void Awake()
    {
        RotatableObj = MainComponents.RotatableObj.transform;
    }

    private void Start()
    {
        main = this;

        RealRotation = -100;

        MaxLevel = 0;
        MinLevel = 1;

    }

    private void Update()
    {
        RotateMainObject();
        //UpdateMainObject();

        UpdateMainObject2();
    }

    //������� �������
    private void RotateMainObject()
    {
        rotationNow = Mathf.Lerp(rotationNow, RealRotation, Time.deltaTime * SpeedLerpRotation);
        RotatableObj.eulerAngles = new Vector3(rotationNow, 0, 0);
    }

    float posStart = -90;
    List<WorldLocation> bufferLocations = new List<WorldLocation>();
    /// <summary>
    /// �������� ������������� �������
    /// </summary>
    private void UpdateMainObject2()
    {


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

        float posNow = posStart;
        //���� �� ������ ������� �������������� �������
        for (int num = 0; num < PrefabsLocation.Length; num++)
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
            if (!found && Mathf.Abs(posNow - rotationNow) < 180)
            {
                GameObject locationObj = Instantiate(PrefabsLocation[num].gameObject, RotatableObj.transform);
                WorldLocation location = locationObj.GetComponent<WorldLocation>();
                location.Inicialize(posNow);

                bufferLocations.Add(location);
            }

            //�������� ������� ��� �������
            posNow -= PrefabsLocation[num].lenghtAngle;
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