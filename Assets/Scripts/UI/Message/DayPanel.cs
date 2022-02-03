using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayPanel : MonoBehaviour
{
    [SerializeField] private Image _GiftImage;
    [SerializeField] private Image _BackgroundImage;
    [SerializeField] private Image _TakenImage;
    [SerializeField] private Text _daysCounter;
    [SerializeField] private Text _GiftCounter;

    public void Set(Sprite sprite, int dayNum, int counter, bool isActive, bool taken)
    {
        _GiftImage.sprite = sprite;
        _daysCounter.text = $"{dayNum.ToString()}";
        _GiftCounter.text = $"x{counter}";
        _BackgroundImage.color = isActive ? Color.white : Color.grey * Color.white;
        _TakenImage.gameObject.SetActive(taken);
    }
    public void SetNull()
    {
        _GiftImage.gameObject.SetActive(false);
        _daysCounter.gameObject.SetActive(false);
        _BackgroundImage.color = Color.grey * Color.white;
        _TakenImage.gameObject.SetActive(false);
    }
}
