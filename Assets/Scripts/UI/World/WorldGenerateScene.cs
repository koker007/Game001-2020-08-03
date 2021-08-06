using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerateScene : MonoBehaviour
{
    public GameObject PrefLevelButton;
    public float DistanceFromWorldCenter;
    private float SetRotationForWorldUp;
    private float SetRotationForWorldDown;
    private int MaxLevel;
    private int MinLevel;
    private const float DistanceSpawnLevelBut = 10f;

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

    public void Start()
    {
        SetRotationForWorldUp = 0;
        SetRotationForWorldDown = -180;

        MaxLevel = 0;
        MinLevel = 1;
    }

    private void Update()
    {
        if (WorldSlider.rotation <= SetRotationForWorldUp - DistanceSpawnLevelBut)
        {
            GenerateLevelButtonUp();
            SetRotationForWorldUp -= DistanceSpawnLevelBut;
        }
        else if (WorldSlider.rotation >= SetRotationForWorldUp + DistanceSpawnLevelBut && WorldSlider.rotation <= 9)
        {
            DeleteLevelButtonUp();
            SetRotationForWorldUp += DistanceSpawnLevelBut;
        }
        if (WorldSlider.rotation <= SetRotationForWorldDown - DistanceSpawnLevelBut)
        {
            DeleteLevelButtonDown();
            SetRotationForWorldDown -= DistanceSpawnLevelBut;
        }
        else if (WorldSlider.rotation >= SetRotationForWorldDown - DistanceSpawnLevelBut && WorldSlider.rotation <= -189)
        {
            GenerateLevelButtonDown();
            SetRotationForWorldDown += DistanceSpawnLevelBut;
        }
    }

    public void GenerateLevelButtonUp()
    {
        Vector3 position = MainComponents.RotatableObj.transform.position;
        position = new Vector3(position.x, position.y, position.z + DistanceFromWorldCenter);
        MaxLevel++;
        LevelButtons.Add(new LButton() { NumLevel = MaxLevel, obj = Instantiate(PrefLevelButton, position, Quaternion.identity, MainComponents.RotatableObj.transform) });
        LevelButtons[LevelButtons.Count - 1].Level();
    }

    public void GenerateLevelButtonDown()
    {
        Vector3 position = MainComponents.RotatableObj.transform.position;
        position = new Vector3(position.x, position.y, position.z - DistanceFromWorldCenter);
        MinLevel--;
        LevelButtons.Insert(0, new LButton() { NumLevel = MinLevel, obj = Instantiate(PrefLevelButton, position, Quaternion.identity, MainComponents.RotatableObj.transform) });
        LevelButtons[0].Level();
    }

    public void DeleteLevelButtonUp()
    {
        LevelButtons[LevelButtons.Count - 1].DestroyObj();
        LevelButtons.RemoveAt(LevelButtons.Count - 1);
        MaxLevel--;
    }

    public void DeleteLevelButtonDown()
    {
        LevelButtons[0].DestroyObj();
        LevelButtons.RemoveAt(0);
        MinLevel++;
    }
}
