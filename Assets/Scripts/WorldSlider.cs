using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// вращаемый обьект (мир с уровнями)
    /// </summary>
    public Transform RotatableObj;

    private Vector2 StartTouchPosition;

    private const float SpeedRotation = 0.1f;
    /// <summary>
    /// даьность поворота мира
    /// </summary>
    public float rotation;
    private float Srotation;

    private bool isDown;

    //нажатие мыши на обьект
    public void OnPointerDown(PointerEventData eventData)
    {
        StartTouchPosition = Input.mousePosition;
        Srotation = rotation;
        this.isDown = true;
    }

    //отпускание мыши
    public void OnPointerUp(PointerEventData eventData)
    {
        this.isDown = false;
    }

    void Update()
    {
        if (isDown)
        {
            //изменение переменной поворота
            rotation = Srotation - (StartTouchPosition.y - Input.mousePosition.y) * SpeedRotation;
        }
        //поворот обьекта
        RotatableObj.eulerAngles = new Vector3(rotation, 0, 0);
    }
}
