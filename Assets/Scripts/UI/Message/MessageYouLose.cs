using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//alexandr
//Семен
//Андрей
/// <summary>
/// отвечает за сообщение проигрыша
/// </summary>
public class MessageYouLose : MonoBehaviour
{
    [SerializeField] Text ScoreText;
    [SerializeField] GameObject showAdButton;
    //[SerializeField] AdMobController adMobController;

    public void OnEnable()
    {
        ScoreText.text = "Score:\n" + Gameplay.main.score.ToString();
        if (!Gameplay.main.adWatched)
        {
            showAdButton.SetActive(true);
        }
        SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipLVLFailed, Settings.main.VolumeMusicFrom0To1, 1);
    }

    public void Restart()
    {
        if (PlayerProfile.main.Health.Amount > 0)
        {
            Gameplay.main.GameplayEnd = false;
            Destroy(MenuGameplay.GameField);
            UICTRL.main.OpenWorld();
            UICTRL.main.OpenGameplay();
        }
        else
        {
            GlobalMessage.ShopBuyHealth();
        }
    }

    public void ClickButtonSeeRewardedAd() {

        AdMobController.main.ShowRewardedAd();
        Gameplay.main.adWatched = true;
    }

    public void ExitGameplay()
    {
        Gameplay.main.GameplayEnd = false;
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }
}
