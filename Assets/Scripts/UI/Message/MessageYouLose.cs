using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageYouLose : MonoBehaviour
{
    public void Restart()
    {
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
        UICTRL.main.OpenGameplay();
    }
}
