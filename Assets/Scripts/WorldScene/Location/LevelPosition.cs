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
    

    // Start is called before the first frame update
    void Start()
    {
        if (positionVisualizator && positionVisualizator.activeSelf) {
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

        button.NumLevel = level;
        button.obj = Instantiate(WorldGenerateScene.main.PrefLevelButton, positionVisualizator.transform.parent);
        button.obj.transform.localPosition = positionVisualizator.transform.localPosition;
    }
}
