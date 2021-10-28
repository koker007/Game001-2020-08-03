using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен

/// <summary>
/// Сообщение с пользовательским соглашением
/// </summary>
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

    //Пользователь нажал принять пользовательское соглажение
    public void ButtonClickAccept() {
        //Пользователь принял соглашение
        PlayerProfile.main.ProfileTermsOfUse = 1;

        //Сохраняем эти данные
        PlayerProfile.main.Save();
    }
}
