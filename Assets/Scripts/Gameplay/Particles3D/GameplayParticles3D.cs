using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����
/// <summary>
/// �������� �� ������������ 3D �������� �� 2D ��������
/// </summary>
public class GameplayParticles3D : MonoBehaviour
{
    public GameplayParticles3D()
    {
        main = this;
    }

    static public GameplayParticles3D main;

    [Header("Main")]
    [SerializeField]
    Camera camera;
    [SerializeField]
    RenderTexture texture;

    [Header("Prefabs")]
    [SerializeField]
    public GameObject prefabBoomBomb;
    [SerializeField]
    public GameObject prefabBoomRocket;
    [SerializeField]
    public GameObject prefabBoomCrystal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
