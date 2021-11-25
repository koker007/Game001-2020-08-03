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
}
