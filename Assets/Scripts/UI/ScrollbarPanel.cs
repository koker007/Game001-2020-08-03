using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//alexandr
/// <summary>
/// перемещение UI обьекта по Y с помощью скроллбара
/// </summary>
public class ScrollbarPanel : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private RectTransform _panelTransform;
    private float _panelStartPosY;
    private float _panelEndPosY;

    private void Start()
    {
        Scroll();
    }

    public void Scroll()
    {
        _panelStartPosY = 0 - _panelTransform.sizeDelta.y / 2;
        _panelEndPosY = _panelTransform.sizeDelta.y - 600 < 0 ? 0 : _panelTransform.sizeDelta.y - 600;
        Vector2 pos = new Vector2(_panelTransform.anchoredPosition.x, _panelStartPosY + _panelEndPosY * _scrollbar.value);
        _panelTransform.anchoredPosition = pos;
    }
}
