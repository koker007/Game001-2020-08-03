using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен

/// <summary>
/// Сообщение с пользовательским соглашением
/// </summary>
public class MessageTermsOfUse : MonoBehaviour
{


    [SerializeField]
    string TermsOfUseUrl = "http://project5092068.tilda.ws";

    public void ButtubClickOpenTermsOfUse() {
        Application.OpenURL(TermsOfUseUrl);
    }

    //Пользователь нажал принять пользовательское соглажение
    public void ButtonClickAccept() {
        //Пользователь принял соглашение
        PlayerProfile.main.ProfileTermsOfUse = 1;

        //Сохраняем эти данные
        PlayerProfile.main.Save();
    }
}
