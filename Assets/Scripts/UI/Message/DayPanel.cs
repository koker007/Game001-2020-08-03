using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayPanel : MonoBehaviour
{
    [SerializeField] private Image _GiftImage;
    [SerializeField] private Text _daysCounter;
    [SerializeField] private Text _GiftCounter;

    public void Set(Sprite sprite, int dayNum, int counter)
    {
        _GiftImage.sprite = sprite;
        _daysCounter.text = dayNum.ToString();
        _GiftCounter.text = $"x{counter}";
    }
}
