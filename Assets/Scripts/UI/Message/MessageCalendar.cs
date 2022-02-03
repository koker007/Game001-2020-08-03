using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageCalendar : MonoBehaviour
{
    [SerializeField] private Text dayComboText;
    [SerializeField] private DayPanel dayPanel;
    [SerializeField] private RectTransform dayPanelParrent;

    public List<Sprite> itemSprites = new List<Sprite>();

    private Vector2 _panelStartPosition;

    public void Start()
    {
        _panelStartPosition = new Vector2(-dayPanelParrent.sizeDelta.x / 2 + 80, dayPanelParrent.sizeDelta.y / 2 - 80);
        InicializeDays();
    }

    private void InicializeDays()
    {
        dayComboText.text = GiftCalendar.main.DayCombo.ToString();
        for (int i = 0; i < GiftCalendar.main.days.Count; i++)
        {
            DayPanel day = Instantiate(dayPanel, dayPanelParrent);
            day.GetComponent<RectTransform>().anchoredPosition = new Vector2(_panelStartPosition.x + (i % 5) * 130, _panelStartPosition.y - (i / 5) * 130);
            day.Set(itemSprites[(int)GiftCalendar.main.days[i].RetTypeItem()], i + 1, (int)GiftCalendar.main.days[i].RetCount());
        }
    }

    public void TakeButton()
    {
        GiftCalendar.main.GetGift();
    }
}