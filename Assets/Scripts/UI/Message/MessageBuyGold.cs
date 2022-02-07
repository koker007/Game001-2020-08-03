using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBuyGold : MonoBehaviour
{

    [SerializeField]
    Text[] GoldCountText;
    int GoldCountOld = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateGold();
    }


    //Пересчитать количество голды
    void UpdateGold() {
        //Если количество золота не изменилось, пересчитывать не надо
        if (GoldCountOld == PlayerProfile.main.GoldAmount) 
            return;

        GoldCountOld = PlayerProfile.main.GoldAmount;

        foreach (Text text in GoldCountText) {
            text.text = GoldCountOld.ToString();
        }
    }
}
