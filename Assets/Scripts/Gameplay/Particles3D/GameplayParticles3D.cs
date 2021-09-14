using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Отвечает за визуализацию 3D эффектов на 2D текстуру
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
    public GameObject prefabBoomSuperColor;

    // Start is called before the first frame update
    void Start()
    {

    }

    //инициализация рендер текстуры
    Vector2 sizeOld = new Vector2();
    void iniRenderTexture() {

        if (sizeOld.x != Screen.width || sizeOld.y != Screen.height) {
            sizeOld = new Vector2(Screen.width, Screen.height);

            //Создаем новую текстуру для рендеинга изображений
            texture = new RenderTexture(Screen.width, Screen.height, 1);
            camera.targetTexture = texture;

            MenuGameplay.main.Particles3D.texture = camera.targetTexture;
        }
        
    }

    public void SetCameraPos(float scaleX, float scaleY) {
        camera.orthographicSize = scaleX-0.5f;
        camera.transform.localPosition = new Vector3(scaleX / 2, scaleY / 2, camera.transform.localPosition.z);

    }

    // Update is called once per frame
    void Update()
    {
        iniRenderTexture();
    }

}
