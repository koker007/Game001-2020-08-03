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
    public GameObject positionVisualizator;

    public WorldGenerateScene.LButton button = new WorldGenerateScene.LButton();
    
    void Start()
    {
        if (positionVisualizator && positionVisualizator.activeSelf) {
            positionVisualizator.SetActive(false);
        }   
    }

    //Инициализировать позицию
    bool inicialized = false;
    public void Inicialize(int level) {
        if (inicialized) return;

        button.NumLevel = level;
        button.obj = Instantiate(WorldGenerateScene.main.PrefLevelButton, positionVisualizator.transform.parent);
        button.obj.transform.localPosition = positionVisualizator.transform.localPosition;
    }
}
