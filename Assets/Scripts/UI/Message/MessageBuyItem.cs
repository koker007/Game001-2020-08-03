using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBuyItem : MonoBehaviour
{
    [SerializeField]
    Image From;
    [SerializeField]
    Text FromCount;
    [SerializeField]
    Text FromPrice;

    [SerializeField]
    Image To;


    [SerializeField]
    Image Target;
    [SerializeField]
    Text TargetCount;
    [SerializeField]
    Text TargetPrice;



    [SerializeField]
    MenuGameplay.SuperHitType typeBuy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClickBuy() {

        bool NeedBuyAntigen = false;

        //Если хватает антител
        if (typeBuy == MenuGameplay.SuperHitType.internalObj) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopBoom)) {
                NeedBuyAntigen = true;
            }
        }
        else if (typeBuy == MenuGameplay.SuperHitType.rosket2x) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopRocket)) {
                NeedBuyAntigen = true;
            }
        }
        else if (typeBuy == MenuGameplay.SuperHitType.Color5) {
            if (!PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.ShopColor5)) {
                NeedBuyAntigen = true;            
            }
        }

        //Если не хватило голды на покупку, то открываем окно о покупке голды за реал
        if (NeedBuyAntigen) {

        }
    }
}
