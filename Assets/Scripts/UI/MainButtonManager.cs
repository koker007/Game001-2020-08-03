using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonManager : MonoBehaviour
{
    /// <summary>
    /// содержит парамтеры кнопок
    /// </summary>
    public void PressButtonSound()
    {
        MainComponents.soundManager.PlaySound(MainComponents.soundManager.PressButton);
    }

}
