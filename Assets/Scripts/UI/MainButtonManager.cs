using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// управляет кнопками
/// </summary>
public class MainButtonManager : MonoBehaviour
{
    /// <summary>
    /// содержит парамтеры кнопок
    /// </summary>
    public void PressButtonSound()
    {
        SoundManager.main.PlaySound(SoundManager.main.PressButton);
    }

}
