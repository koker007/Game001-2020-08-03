using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController main;

    private void Start()
    {
        main = this;
    }

    public void EnmyTurn()
    {
        if (Gameplay.main.enemyTurn)
        {
            //Собираем потенциальную комбинацию
            if (GameFieldCTRL.PotencialComb.main.Moving != null && GameFieldCTRL.PotencialComb.main.Target != null)
            {
                //Двигаем GameFieldCTRL.PotencialComb.main.Moving; в GameFieldCTRL.PotencialComb.main.Target;

            }
            Gameplay.main.enemyTurn = false;
        }
    }
}
