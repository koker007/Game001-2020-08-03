using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//alexandr
/// <summary>
/// отвечает за сообщение проигрыша
/// </summary>
public class MessageYouLose : MonoBehaviour
{
    public void Restart()
    {
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
        UICTRL.main.OpenGameplay();
    }

    public void ExitGameplay()
    {
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }
}
