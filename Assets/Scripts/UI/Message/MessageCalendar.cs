using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageCalendar : MonoBehaviour
{
    [SerializeField] private Text dayComboText;
    [SerializeField] private DayPanel dayPanel;
    [SerializeField] private RectTransform dayPanelParrent;

    private Vector2 _panelStartPosition;

    public void Start()
    {
        _panelStartPosition = new Vector2(-dayPanelParrent.sizeDelta.x / 2 + 170 / 2 + (dayPanelParrent.sizeDelta.x - 170 * 5) / 2, dayPanelParrent.sizeDelta.y / 2 - 170 / 2 - (dayPanelParrent.sizeDelta.y - 170 * 6) / 2);
        InicializeDays();
    }

    private void InicializeDays()
    {
        dayComboText.text = GiftCalendar.main.DayCombo.ToString();
        for (int i = 0; i < 30; i++)
        {
            DayPanel day = Instantiate(dayPanel, dayPanelParrent);
            day.GetComponent<RectTransform>().anchoredPosition = new Vector2(_panelStartPosition.x + (i % 5) * 170, _panelStartPosition.y - (i / 5) * 170);

            if (i < GiftCalendar.main.days.Count)
            {
                if(i < GiftCalendar.main.DayCombo)
                    day.Set(GiftCalendar.main.itemSprites[(int)GiftCalendar.main.days[i].RetTypeItem()], i + 1, (int)GiftCalendar.main.days[i].RetCount(), true, true);
                else if(i > GiftCalendar.main.days.Count - (GiftCalendar.main.DaySubEnded - GiftCalendar.main.DayCombo))
                    day.Set(GiftCalendar.main.itemSprites[(int)GiftCalendar.main.days[i].RetTypeItem()], i + 1, (int)GiftCalendar.main.days[i].RetCount(), false, false);
                else
                    day.Set(GiftCalendar.main.itemSprites[(int)GiftCalendar.main.days[i].RetTypeItem()], i + 1, (int)GiftCalendar.main.days[i].RetCount(), true, false);
            }
            else
            {
                day.SetNull();
            }
        }
    }

    public void TakeButton()
    {
        GiftCalendar.main.GetGift();
        DailyGifts.main.CheckHaveGift();
    }
}