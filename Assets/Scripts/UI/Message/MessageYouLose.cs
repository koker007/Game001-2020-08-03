using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//alexandr
//—емен
/// <summary>
/// отвечает за сообщение проигрыша
/// </summary>
public class MessageYouLose : MonoBehaviour
{
    [SerializeField] Text ScoreText;
    //[SerializeField] AdMobController adMobController;

    public void OnEnable()
    {
        ScoreText.text = "Score:\n" + Gameplay.main.score.ToString();

        SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipLVLFailed, Settings.main.VolumeMusicFrom0To1, 1);
    }

    public void Restart()
    {
        Gameplay.main.GameplayEnd = false;
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
        UICTRL.main.OpenGameplay();
    }

    public void ClickButtonSeeRewardedAd() {

        AdMobController.ShowRewardedAd();
    }

    public void ExitGameplay()
    {
        Gameplay.main.GameplayEnd = false;
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }
}
