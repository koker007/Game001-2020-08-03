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

    //���������������� �������
    bool inicialized = false;
    public void Inicialize(int level) {
        if (inicialized) return;

        button = Instantiate(WorldGenerateScene.main.PrefLevelButton, positionVisualizator.transform.parent);
        button.transform.localPosition = positionVisualizator.transform.localPosition;
    }
}
