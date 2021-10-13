using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//алкександр
/// <summary>
/// Вращение мира прикосновением
/// </summary>
public class WorldSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static WorldSlider main;

    private Vector2 StartTouchPosition;

    private const float SpeedRotation = 0.015f;
    private float Srotation;
    public float StartRotation = -115;

    private bool isDown;

    //нажатие мыши на обьект
    public void OnPointerDown(PointerEventData eventData)
    {
        StartTouchPosition = Input.mousePosition;
        Srotation = WorldGenerateScene.main.rotationNeed;
        this.isDown = true;
    }

    //отпускание мыши
    public void OnPointerUp(PointerEventData eventData)
    {
        this.isDown = false;
    }

    private void Start()
    {
        main = this;
        WorldGenerateScene.main.rotationNeed = StartRotation - (PlayerProfile.main.ProfilelevelOpen * 5);
    }

    private void Update()
    {
        if (isDown)
        {
            //изменение переменной поворота
            WorldGenerateScene.main.rotationNeed = Srotation - (StartTouchPosition.y - Input.mousePosition.y) * SpeedRotation;
        }
    }
}
