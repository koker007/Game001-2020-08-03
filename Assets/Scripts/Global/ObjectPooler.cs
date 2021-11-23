using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler main;

    [SerializeField] private GameObject[] mainLocations;
    [SerializeField] private GameObject[] additionalLocations;    

    private List<GameObject> listOfMainLocations;
    private List<GameObject> listOfAdditionalLocations;
    private List<int> addLocIDs;

    [SerializeField] private int additionalLocationsCount;

    private void Start()
    {
        main = this;
        listOfMainLocations = new List<GameObject>();
        listOfAdditionalLocations = new List<GameObject>();
        for (int i = 0; i < mainLocations.Length; i++)
        {
            InstantiateGameObjects(mainLocations[i], listOfMainLocations, 1);
        }
        for (int i = 0; i < additionalLocations.Length; i++)
        {
            InstantiateGameObjects(additionalLocations[i], listOfAdditionalLocations, additionalLocationsCount);
        }
    }

    private void InstantiateGameObjects(GameObject obj, List<GameObject> listOfGameObjects, int objCount)
    {
        for (int i = 0; i < objCount; i++)
        {
            GameObject currentObj = Instantiate(obj, MainComponents.RotatableObj.transform.transform);
            listOfGameObjects.Add(currentObj);
            currentObj.SetActive(false);
        }
    }

    public GameObject SpawnMainLocation(int locationID)
    {
        if (!listOfMainLocations[locationID].activeInHierarchy)
        {
            return listOfMainLocations[locationID];
        }
       
        return null;
    }

    public GameObject SpawnAdditionalLocation(int locationID)
    {
        for (int i = 0; i < listOfAdditionalLocations.Count; i++)
        {
            if (!listOfAdditionalLocations[i].activeInHierarchy)
            {
                return listOfAdditionalLocations[i];
            }
        }
        return null;
    }
}
