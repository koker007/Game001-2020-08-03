using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//alexandr
/// <summary>
/// отвечает за сообщение результатов игры при победе
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

        SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipLVLComplite, Settings.main.VolumeMusicFrom0To1, 1);
    }

    public void NextLVL()
    {
        Gameplay.main.levelSelect++;

        GlobalMessage.Close();

        Gameplay.main.GameplayEnd = false;
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
        UICTRL.main.OpenGameplay();
    }

    public void ExitGameplay()
    {
        Gameplay.main.GameplayEnd = false;
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }
}
