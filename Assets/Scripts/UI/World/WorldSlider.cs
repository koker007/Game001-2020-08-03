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
    /// <summary>
    /// вращаемый обьект (мир с уровнями)
    /// </summary>
    public Transform RotatableObj;

    private Vector2 StartTouchPosition;

    private const float SpeedRotation = 0.01f;
    private const float SpeedLerpRotation = 2f;
    /// <summary>
    /// даьность поворота мира
    /// </summary>
    public static float rotation;
    private float RealRotation;
    private float Srotation;
    private const float StartRotation = -115;

    private bool isDown;

    //нажатие мыши на обьект
    public void OnPointerDown(PointerEventData eventData)
    {
        StartTouchPosition = Input.mousePosition;
        Srotation = RealRotation;
        this.isDown = true;
    }

    //отпускание мыши
    public void OnPointerUp(PointerEventData eventData)
    {
        this.isDown = false;
    }

    private void Start()
    {
        RealRotation = StartRotation;
        //rotation = StartRotation + 100;
    }

    private void Update()
    {
        if (isDown)
        {
            //изменение переменной поворота
            RealRotation = Srotation - (StartTouchPosition.y - Input.mousePosition.y) * SpeedRotation;
            if (RealRotation >= StartRotation)
            {
                RealRotation = StartRotation;
            }
        }
        rotation = Mathf.Lerp(rotation, RealRotation, Time.deltaTime * SpeedLerpRotation);
        //поворот обьекта
        RotatableObj.eulerAngles = new Vector3(rotation, 0, 0);
    }
}
