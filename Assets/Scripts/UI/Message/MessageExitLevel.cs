using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//alexandr
//—емен
/// <summary>
/// отвечает за сообщение выхода из уровн€
/// </summary>
public class MessageExitLevel : MonoBehaviour
{
    private void Update()
    {
        if(Gameplay.main.GameplayEnd == true)
        {
            gameObject.GetComponent<MessageCTRL>().ClickButtonClose();
        }
    }

    public void Restart()
    {
        Gameplay.main.GameplayEnd = false;
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
        UICTRL.main.OpenGameplay();
    }

    public void ExitLevelYes()
    {
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }
}
