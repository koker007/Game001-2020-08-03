using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������
//�����
//���������� ����� � ��� �����������
public class EnemyController : MonoBehaviour
{
    public static bool enemyTurn;
    public static bool canEnemyMove = true;
    public static int MoveCount = 0;
    public const int MoveCountForPlayer = 5; //���������� ����� ���������� ����� ������� ���������� ���� ������, ���� ���� ����� ����������� � ������� � ����� ����� �������� 

    [SerializeField]
    Slider SliderCoof;

    [SerializeField]
    Text scoreText;

    private void Update()
    {
        TestTurn();
        UpdateIndicators();
    }

    void TestTurn()
    {
        if (enemyTurn && canEnemyMove)
        {
            if (GameFieldCTRL.enemyPotencialBest != null)
            {
                CellCTRL cellSelect = GameFieldCTRL.enemyPotencialBest.Moving;
                CellCTRL cellSwap = GameFieldCTRL.enemyPotencialBest.Target;
                //�������� ������������� ����������
                if (cellSelect != null && cellSwap != null)
                {
                    GameFieldCTRL.main.CellSelect = cellSelect;
                    GameFieldCTRL.main.CellSwap = cellSwap;
                }
                enemyTurn = false;
            }
        }
    }

    void UpdateIndicators() {
        if (Gameplay.main.enemyScore != 0)
        {
            SliderCoof.value += ((float)(Gameplay.main.score) / (float)(Gameplay.main.enemyScore) - SliderCoof.value) * Time.unscaledDeltaTime;
        }
        else SliderCoof.value = 0;

        scoreText.text = Gameplay.main.enemyScore.ToString();
    }
}
