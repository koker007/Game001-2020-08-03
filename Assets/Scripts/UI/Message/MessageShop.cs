using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageShop : MonoBehaviour
{
    [SerializeField]
    Text CountGold;
    [SerializeField]
    Text CountInternal;
    [SerializeField]
    Text CountRocket;
    [SerializeField]
    Text CountBomb;
    [SerializeField]
    Text CountColor5;
    [SerializeField]
    Text CountMixed;

    [SerializeField] private RectTransform _PacksPanel;
    private float _StartPositionPacksPanelY;
    private const int _VisableHeightPacksPanel = 1080;
    [SerializeField] private Scrollbar _Scrollbar;

    private void Start()
    {
        _StartPositionPacksPanelY = _PacksPanel.anchoredPosition.y;
        if(_PacksPanel.sizeDelta.y <= 1080)
        {
            _Scrollbar.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        UpdateCountItem();
    }

    void UpdateCountItem() {
        CountGold.text = PlayerProfile.main.GoldAmount.ToString();
        CountInternal.text = PlayerProfile.main.ShopInternal.Amount.ToString();
        CountRocket.text = PlayerProfile.main.ShopRocket.Amount.ToString();
        CountBomb.text = PlayerProfile.main.ShopBomb.Amount.ToString();
        CountColor5.text = PlayerProfile.main.ShopColor5.Amount.ToString();
        CountMixed.text = PlayerProfile.main.ShopMixed.Amount.ToString();
    }

    public void Scroll()
    {
        Vector2 position = new Vector2(_PacksPanel.anchoredPosition.x, _StartPositionPacksPanelY + (_PacksPanel.sizeDelta.y - _VisableHeightPacksPanel)  * _Scrollbar.value);
        _PacksPanel.anchoredPosition = position;
    }
}
