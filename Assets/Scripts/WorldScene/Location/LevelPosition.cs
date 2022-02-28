using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����
/// <summary>
/// ������ �������������� ������ �� �������
/// </summary>
public class LevelPosition : MonoBehaviour
{

    //������������ �������������� ������
    [SerializeField]
    public GameObject positionVisualizator;

    public WorldGenerateScene.LButton button = new WorldGenerateScene.LButton();
    
    void Start()
    {
        if (positionVisualizator && positionVisualizator.activeSelf) {
            positionVisualizator.SetActive(false);
        }   
    }

    //���������������� �������
    bool inicialized = false;
    public void Inicialize(GameObject but, int level) {
        if (inicialized) return;

        button.NumLevel = level;
        but.transform.SetParent(transform);
        button.obj = but;
        but.transform.rotation = transform.rotation;
        button.obj.transform.localPosition = positionVisualizator.transform.localPosition;
    }
}
