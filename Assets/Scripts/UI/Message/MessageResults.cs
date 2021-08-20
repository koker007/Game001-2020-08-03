using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//alexandr
/// <summary>
/// �������� �� ��������� ����������� ���� ��� ������
/// </summary>

public class MessageResults : MonoBehaviour
{
    [SerializeField]
    Text ScoreText;
    [SerializeField]
    Image[] StarsImage = new Image[3];

    public void OnEnable()
    {
        ScoreText.text = "Score:\n" + Gameplay.main.score.ToString();
        Gameplay.main.CountStars(ref StarsImage);
    }

    public void ExitGameplay()
    {
        Gameplay.main.GameplayEnd = false;
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }
}
