using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePiggyBankV2 : MonoBehaviour
{

    [SerializeField]
    public Text[] CountNow;
    [SerializeField]
    public Text[] CountMax;

    [SerializeField]
    GameObject ObjButtonOpen;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void UpdateTextCount() {
        foreach (Text text in CountNow) text.text = PlayerProfile.main.PiggyBankNow.ToString();
        foreach (Text text in CountMax) text.text = PlayerProfile.main.PiggyBankMax.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTextCount();

        //включить кнопку если золото в копилке есть
        if (PlayerProfile.main.PiggyBankNow > 0) {
            if (!ObjButtonOpen.activeSelf) ObjButtonOpen.SetActive(true);
        }
        //убрать кнопку если золота нет
        else {
            if (ObjButtonOpen.activeSelf) ObjButtonOpen.SetActive(false);
        }
    }


    //Разбить свинью копилку и получить все накопленное золото
    public void ClickOpen() {
        PlayerProfile.main.PiggyBankOpen();
    }

    //Улучшить свинью копилку чтобы вмещалось больше золота
    public void ClickUpdate() {
        PlayerProfile.main.PiggyBankUpgrade();
    }
}
