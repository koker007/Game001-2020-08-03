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

    int CountStar = 0;

    public void OnEnable()
    {
        ScoreText.text = "Score:\n" + Gameplay.main.score.ToString();
        //Gameplay.main.CountStars(ref StarsImage);

        SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipLVLComplite, Settings.main.VolumeMusicFrom0To1, 1);

        iniStar();
    }

    void iniStar() {
        CountStar = Gameplay.main.CountStars(Gameplay.main.score);

        if (CountStar == 0) {
            StarsImage[0].gameObject.SetActive(false);
            StarsImage[1].gameObject.SetActive(false);
            StarsImage[2].gameObject.SetActive(false);
        }
        else if (CountStar == 1) {
            StarsImage[0].gameObject.SetActive(true);
            StarsImage[1].gameObject.SetActive(false);
            StarsImage[2].gameObject.SetActive(false);
        }
        else if (CountStar == 2) {
            StarsImage[0].gameObject.SetActive(true);
            StarsImage[1].gameObject.SetActive(true);
            StarsImage[2].gameObject.SetActive(false);
        }
        else if (CountStar == 3) {
            StarsImage[0].gameObject.SetActive(true);
            StarsImage[1].gameObject.SetActive(true);
            StarsImage[2].gameObject.SetActive(true);
        }
    }

    public void NextLVL()
    {
        Gameplay.main.levelSelect++;

        //«акрыть свое сообщение
        MessageCTRL myMessageCTRL = gameObject.GetComponent<MessageCTRL>();
        myMessageCTRL.ClickButtonClose();

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
