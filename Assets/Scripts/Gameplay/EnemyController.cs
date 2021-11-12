using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static bool enemyTurn;

    private void Update()
    {
        EnmyTurn();
    }

    public void EnmyTurn()
    {
        if (enemyTurn)
        {
            if (GameFieldCTRL.enemyPotencialBest != null)
            {
                CellCTRL cellSelect = GameFieldCTRL.enemyPotencialBest.Moving;
                CellCTRL cellSwap = GameFieldCTRL.enemyPotencialBest.Target;
                //Собираем потенциальную комбинацию
                if (cellSelect != null && cellSwap != null)
                {
                    GameFieldCTRL.main.CellSelect = cellSelect;
                    GameFieldCTRL.main.CellSwap = cellSwap;
                    Debug.Log("ET");
                    enemyTurn = false;
                }
            }
        }
    }
}
