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
        ScoreText.text = $"{TranslateManager.main.GetText("Score")}:\n{Gameplay.main.score.ToString()}";
        //Gameplay.main.CountStars(ref StarsImage);

        float volume = Settings.main.VolumeMusicFrom0To1;
        if (Settings.main.VolumeSoundFrom0To1 > Settings.main.VolumeMusicFrom0To1)
        {
            volume = Settings.main.VolumeSoundFrom0To1;
        }
        SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipLVLComplite, volume, 1);

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

        //Узнать количество звезд ранее
        int CountStarOld = PlayerProfile.main.GetLVLStar(Gameplay.main.levelSelect);

        //если ранее звед было меньше чем стало сейчас что прибавляем разницу в виде золота в свинью копилку
        if (CountStarOld < CountStar) {
            PlayerProfile.main.PiggyBankAdd(CountStar - CountStarOld);
        }

        //Сохранить прогресс звезд
        PlayerProfile.main.SetLVLStar(Gameplay.main.levelSelect, CountStar);

        //Если текущий уровень предпоследний и собранно 3 звезды
        //то пройдено на золото
        if (Gameplay.main.levelSelect == PlayerProfile.main.ProfilelevelOpen - 1 && CountStar == 3)
        {
            PlayerProfile.main.SetLVLGold(Gameplay.main.levelSelect, 1);
        }
    }

    public void NextLVL()
    {
        Gameplay.main.levelSelect++;

        //Закрыть свое сообщение
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
