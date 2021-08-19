using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//alexandr
/// <summary>
/// отвечает за сообщение выхода из уровня
/// </summary>
public class MessageExitLevel : MonoBehaviour
{
    private void Update()
    {
        if(Gameplay.main.GameplayEnd == true)
        {
            //gameObject.GetComponent<MessageCTRL>().ClickButtonClose();
        }
    }
    public void ExitLevelYes()
    {
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }
}
