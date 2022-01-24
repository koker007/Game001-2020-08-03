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

    private void Start()
    {
        _panelStartPosY = _panelTransform.anchoredPosition.y;
    }

    public void Scroll()
    {
        Vector2 pos = new Vector2(_panelTransform.anchoredPosition.x, _panelStartPosY - (_panelTransform.sizeDelta.y / 2) + _panelTransform.sizeDelta.y * _scrollbar.value);
        _panelTransform.anchoredPosition = pos;
    }
}
