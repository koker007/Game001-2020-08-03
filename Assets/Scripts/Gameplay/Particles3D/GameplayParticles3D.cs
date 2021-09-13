using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public RenderTexture texture;

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
        iniRenderTexture();
    }

    //������������� ������ ��������
    Vector2 sizeOld = new Vector2();
    void iniRenderTexture() {

        if (sizeOld.x != Screen.width || sizeOld.y != Screen.height) {
            sizeOld = new Vector2(Screen.width, Screen.height);

            //������� ����� �������� ��� ��������� �����������
            texture = new RenderTexture(Screen.width, Screen.height, 1);
            camera.targetTexture = texture;
        }
        
    }

    public void SetCameraPos(float scaleX, float scaleY) {
        camera.orthographicSize = scaleX;
        camera.transform.localPosition = new Vector3(scaleX / 2, scaleY / 2, camera.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
