using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//alexandr
/// <summary>
/// генерация обьектов в меню уровней
/// </summary>
public class WorldGenerateScene : MonoBehaviour
{
    public GameObject PrefLevelButton;
    public GameObject[] Location = new GameObject[0];

    private float SetRotationForWorldUp;
    private float SetRotationForWorldDown;
    private int MaxLevel;
    private int MinLevel;
    private const float DistanceFromWorldCenter = 100.5f;
    private const float DistanceSpawnLevelBut = 5f;
    private const float WidthSpawnLevelBut = 8f;

    private float rotation;
    public static float RealRotation;
    private const float SpeedLerpRotation = 2f;
    private Transform RotatableObj;

    /// <summary>
    /// класс кнопки
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
    private GameObject[] Locations = new GameObject[2];
    private int locationCounter;

    private void Awake()
    {
        RotatableObj = MainComponents.RotatableObj.transform;
    }

    private void Start()
    {
        RealRotation = -100;

        SetRotationForWorldUp = 0;
        SetRotationForWorldDown = -180;

        MaxLevel = 0;
        MinLevel = 1;

        locationCounter = 0;
        Locations[locationCounter] = Instantiate(Location[1], MainComponents.RotatableObj.transform.position, Quaternion.identity, MainComponents.RotatableObj.transform);
        locationCounter++;
    }

    private void Update()
    {
        RotateMainObject();
        UpdateMainObject();
    }

    //поворот обьекта
    private void RotateMainObject()
    {
        rotation = Mathf.Lerp(rotation, RealRotation, Time.deltaTime * SpeedLerpRotation);
        RotatableObj.eulerAngles = new Vector3(rotation, 0, 0);
    }

    //проверка обновлений цилиндра
    private void UpdateMainObject()
    {
        if (rotation <= SetRotationForWorldUp - DistanceSpawnLevelBut)
        {
            GenerateLevelButtonUp();
            SetRotationForWorldUp -= DistanceSpawnLevelBut;
            UpdateLokations(true);
        }
        else if (rotation >= SetRotationForWorldUp + DistanceSpawnLevelBut && rotation <= 9)
        {
            DeleteLevelButtonUp();
            SetRotationForWorldUp += DistanceSpawnLevelBut;
            UpdateLokations(false);
        }
        if (rotation <= SetRotationForWorldDown - DistanceSpawnLevelBut)
        {
            DeleteLevelButtonDown();
            SetRotationForWorldDown -= DistanceSpawnLevelBut;
        }
        else if (rotation >= SetRotationForWorldDown - DistanceSpawnLevelBut && rotation <= -181)
        {
            GenerateLevelButtonDown();
            SetRotationForWorldDown += DistanceSpawnLevelBut;
        }
    }

    private void UpdateLokations(bool Up)
    {
        if (SetRotationForWorldUp % 180 == 0)
        {
            if (Up)
            {
                Destroy(Locations[locationCounter]);
                Locations[locationCounter] = Instantiate(Location[Random.Range(0, 3)], MainComponents.RotatableObj.transform.position, Quaternion.identity, MainComponents.RotatableObj.transform);
                locationCounter++;
                if(locationCounter > 1)
                {
                    locationCounter = 0;
                }
            }
            else
            {
                Destroy(Locations[locationCounter]);
                Locations[locationCounter] = Instantiate(Location[Random.Range(0, 3)], MainComponents.RotatableObj.transform.position, Quaternion.identity, MainComponents.RotatableObj.transform);
                locationCounter--;
                if (locationCounter < 0)
                {
                    locationCounter = 1;
                }
            }
        }
    }

    //генерация и удаление кнопок сверху и снизу
    private void GenerateLevelButtonUp()
    {
        MaxLevel++;
        Vector3 position = MainComponents.RotatableObj.transform.position;
        float radian;
        radian = (Mathf.Abs(rotation) - MaxLevel * DistanceSpawnLevelBut) * Mathf.Deg2Rad;
        position = new Vector3(position.x + AlorithmX(MaxLevel), position.y + DistanceFromWorldCenter * Mathf.Sin(radian), position.z + DistanceFromWorldCenter * Mathf.Cos(radian));
        LevelButtons.Add(new LButton() { NumLevel = MaxLevel, obj = Instantiate(PrefLevelButton, position, Quaternion.identity, MainComponents.RotatableObj.transform) });
        LevelButtons[LevelButtons.Count - 1].Level();
    }

    private void GenerateLevelButtonDown()
    {
        MinLevel--;
        Vector3 position = MainComponents.RotatableObj.transform.position;
        float radian;
        radian = (Mathf.Abs(rotation) - MinLevel * DistanceSpawnLevelBut) * Mathf.Deg2Rad;
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

    //алгоритм спавна по X
    private float AlorithmX(int Level)
    {
        float res;
        res = Mathf.Cos(Mathf.Sin(Level)) * 1000;
        res = res % WidthSpawnLevelBut - WidthSpawnLevelBut / 2;
        return res;
    }

}
