using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// скрипт сообщения выхода из игры
/// </summary>
public class MessageExitGame : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit();
    }
}
