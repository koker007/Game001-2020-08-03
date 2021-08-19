using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//alexandr
/// <summary>
/// отвечает за сообщение выхода из уровня
/// </summary>
public class MessageExitLevel : MonoBehaviour
{
    public void ExitLevelYes()
    {
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }
}
