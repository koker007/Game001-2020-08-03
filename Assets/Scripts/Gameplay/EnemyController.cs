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
            //�������� ������������� ����������
            if (GameFieldCTRL.PotencialComb.main.Moving != null && GameFieldCTRL.PotencialComb.main.Target != null)
            {
                //������� GameFieldCTRL.PotencialComb.main.Moving; � GameFieldCTRL.PotencialComb.main.Target;

            }
            Gameplay.main.enemyTurn = false;
        }
    }
}
