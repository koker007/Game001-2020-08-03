using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAbility : MonoBehaviour
{

    [SerializeField]
    Text Count;

    [SerializeField]
    MenuGameplay.SuperHitType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCount();
    }

    void UpdateCount() {
        if (type == MenuGameplay.SuperHitType.internalObj)
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopInternal.Amount);
        else if (type == MenuGameplay.SuperHitType.rosket2x)
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopRocket.Amount);
        else if (type == MenuGameplay.SuperHitType.bomb)
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopBomb.Amount);
        else if (type == MenuGameplay.SuperHitType.Color5)
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopColor5.Amount);
        else if (type == MenuGameplay.SuperHitType.mixed)
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopMixed.Amount);

                
    }
}
