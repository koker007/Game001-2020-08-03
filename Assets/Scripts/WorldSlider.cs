using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// ��������� ������ (��� � ��������)
    /// </summary>
    public Transform RotatableObj;

    private Vector2 StartTouchPosition;

    private const float SpeedRotation = 0.1f;
    /// <summary>
    /// �������� �������� ����
    /// </summary>
    public float rotation;
    private float Srotation;

    private bool isDown;

    //������� ���� �� ������
    public void OnPointerDown(PointerEventData eventData)
    {
        StartTouchPosition = Input.mousePosition;
        Srotation = rotation;
        this.isDown = true;
    }

    //���������� ����
    public void OnPointerUp(PointerEventData eventData)
    {
        this.isDown = false;
    }

    void Update()
    {
        if (isDown)
        {
            //��������� ���������� ��������
            rotation = Srotation - (StartTouchPosition.y - Input.mousePosition.y) * SpeedRotation;
        }
        //������� �������
        RotatableObj.eulerAngles = new Vector3(rotation, 0, 0);
    }
}
