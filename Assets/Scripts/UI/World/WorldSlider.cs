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
    private Vector2 StartTouchPosition;

    private const float SpeedRotation = 0.01f;
    private float Srotation;
    private const float StartRotation = -115;

    private bool isDown;

    //нажатие мыши на обьект
    public void OnPointerDown(PointerEventData eventData)
    {
        StartTouchPosition = Input.mousePosition;
        Srotation = WorldGenerateScene.RealRotation;
        this.isDown = true;
    }

    //отпускание мыши
    public void OnPointerUp(PointerEventData eventData)
    {
        this.isDown = false;
    }

    private void Start()
    {
        WorldGenerateScene.RealRotation = StartRotation;
    }

    private void Update()
    {
        if (isDown)
        {
            //изменение переменной поворота
            WorldGenerateScene.RealRotation = Srotation - (StartTouchPosition.y - Input.mousePosition.y) * SpeedRotation;
            if (WorldGenerateScene.RealRotation >= StartRotation)
            {
                WorldGenerateScene.RealRotation = StartRotation;
            }
        }
    }
}
