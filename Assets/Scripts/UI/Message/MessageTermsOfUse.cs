using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTermsOfUse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ѕользователь нажал прин€ть пользовательское соглажение
    public void ButtonClickAccept() {
        //ѕользователь прин€л соглашение
        PlayerProfile.main.ProfileTermsOfUse = 1;

        //—охран€ем эти данные
        PlayerProfile.main.Save();
    }
}
