using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//alexandr
/// <summary>
/// �������� �� ��������� ������ �� ������
/// </summary>
public class MessageExitLevel : MonoBehaviour
{
    public void ExitLevelYes()
    {
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }
}
