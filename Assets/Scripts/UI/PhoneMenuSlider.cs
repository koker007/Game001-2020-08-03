using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// сенсорный слайдер для меню
/// </summary>
public class PhoneMenuSlider : MonoBehaviour
{
    [SerializeField] private RectTransform _movableObject;

    [SerializeField] private int _visableHeightPanel = 1080;
    [SerializeField] private float _speedMove = 1.5f;

    private const float _speedLerp = 5f;

    private Vector2 _startTouchPosition;
    private Vector2 _movePosition;
    private float _startPositionY;
    private float _minPositionY;
    private float _maxPositionY;

    private bool _isDown;

    //нажатие мыши на обьект
    public void OnDown()
    {
        _startPositionY = _movableObject.anchoredPosition.y;
        _startTouchPosition = Input.mousePosition;
    }

    //отпускание мыши
    public void OnScroll()
    {
        Slide();
    }

    private void Start()
    {
        _movePosition = _movableObject.anchoredPosition;
        _maxPositionY = _movableObject.anchoredPosition.y + (_movableObject.sizeDelta.y - _visableHeightPanel);
        _minPositionY = _movableObject.anchoredPosition.y;
    }

    private void Update()
    {
        _movableObject.anchoredPosition = Vector2.Lerp(_movableObject.anchoredPosition, _movePosition, _speedLerp * Time.deltaTime);
    }

    private void Slide()
    {
        float posY = _startPositionY + (Input.mousePosition.y - _startTouchPosition.y) * _speedMove;

        if (posY > _maxPositionY)
            posY = _maxPositionY;

        if (posY < _minPositionY)
            posY = _minPositionY;

        _movePosition = new Vector2(_movePosition.x, posY);
    }
}
