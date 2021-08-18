using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// отвечает за сообщение событий
/// </summary>
public class MessageEvens : MonoBehaviour
{
    public void OkClick()
    {
        MenuWorld.main.OpenArenaPanel();
    }
}
