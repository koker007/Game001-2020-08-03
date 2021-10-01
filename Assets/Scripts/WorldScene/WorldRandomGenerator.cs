using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// рандомная генерация обьектов на цилиндре
/// </summary>
public class WorldRandomGenerator : MonoBehaviour
{
    public GameObject Prefab;
    public int amountObj;
    private float rotation;
    private float width;
    public float WidthSpawnLevelObj;
    private const float DistanceFromWorldCenter = 100.5f;


    private void Start()
    {
        rotation = 0;
        for(int i = 0; i < amountObj; i++)
        {
            rotation = Random.Range(0, 180);
            width = Random.Range(-WidthSpawnLevelObj, WidthSpawnLevelObj);
            
            Vector3 position = MainComponents.RotatableObj.transform.position;
            float radian;
            radian = rotation * Mathf.Deg2Rad;
            position = new Vector3(position.x + width, position.y + DistanceFromWorldCenter * Mathf.Sin(radian), position.z + DistanceFromWorldCenter * Mathf.Cos(radian));
            Instantiate(Prefab, position, Quaternion.identity, MainComponents.RotatableObj.transform);
        }
    }
}
