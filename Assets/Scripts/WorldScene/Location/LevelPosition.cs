using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен
/// <summary>
/// Хранит местоположение кнопки на локации
/// </summary>
public class LevelPosition : MonoBehaviour
{

    //Визуализатор местоположения кнопки
    [SerializeField]
    GameObject positionVisualizator;

    GameObject button;
    

    // Start is called before the first frame update
    void Start()
    {
        if (positionVisualizator.activeSelf) {
            positionVisualizator.SetActive(false);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Инициализировать позицию
    bool inicialized = false;
    public void Inicialize(int level) {
        if (inicialized) return;

        button = Instantiate(WorldGenerateScene.main.PrefLevelButton, positionVisualizator.transform.parent);
        button.transform.localPosition = positionVisualizator.transform.localPosition;
    }
}
