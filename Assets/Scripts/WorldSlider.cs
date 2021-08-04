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
    private Vector3 StartObjRotation;

    private const float SpeedRotation = 0.1f;

    private bool isDown;

    //нажатие мышина обьект
    public void OnPointerDown(PointerEventData eventData)
    {
        StartTouchPosition = Input.mousePosition;
        StartObjRotation = RotatableObj.eulerAngles;
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
            RotatableObj.eulerAngles = new Vector3(StartObjRotation.x - (StartTouchPosition.y - Input.mousePosition.y) * SpeedRotation, RotatableObj.eulerAngles.y, RotatableObj.eulerAngles.z);
            Debug.Log((StartTouchPosition.y - Input.mousePosition.y) * SpeedRotation);
        }
    }
}
