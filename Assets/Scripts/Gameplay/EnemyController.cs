using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static bool enemyTurn;
    public static bool canEnemyMove = true;

    private void Update()
    {
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        if (enemyTurn && canEnemyMove)
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
