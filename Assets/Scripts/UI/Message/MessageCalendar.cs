using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCalendar : MonoBehaviour
{
    [SerializeField] private DayPanel dayPanel;
    [SerializeField] private RectTransform dayPanelParrent;
    private Vector2 _panelPosition;

    private int counter;

    public void Start()
    {
        _panelPosition = new Vector2(dayPanelParrent.sizeDelta.x, dayPanelParrent.sizeDelta.y);
    }
    public void AddDay()
    {
    }
}
