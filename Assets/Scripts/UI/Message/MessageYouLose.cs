using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//alexandr
//�����
//������
/// <summary>
/// �������� �� ��������� ���������
/// </summary>
public class MessageYouLose : MonoBehaviour
{
    [SerializeField] Text ScoreText;
    [SerializeField] GameObject showAdButton;
    //[SerializeField] AdMobController adMobController;

    [SerializeField]
    GameObject ButtonBuyMoveOfGold;
    [SerializeField]
    Text PriceMoveOfGoldText;
    int PriceMoveOfGold = 1;

    public void OnEnable()
    {
        //������� ��������� ���� ����������� ����� ��� ����
        if (Gameplay.main.movingCan > 0) {
            Destroy(gameObject);
        }

        ScoreText.text = $"{TranslateManager.main.GetText("Score")}:\n{Gameplay.main.score.ToString()}";
        if (Gameplay.main.adWatchedCount < 3)
        {
            showAdButton.SetActive(true);
        }
        else
        {
            showAdButton.SetActive(false);
        }

        float volume = Settings.main.VolumeMusicFrom0To1;
        if (Settings.main.VolumeSoundFrom0To1 > Settings.main.VolumeMusicFrom0To1) {
            volume = Settings.main.VolumeSoundFrom0To1;
        }
        SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipLVLFailed, volume, 1);

        ReCalcCostMoveOfGold();

    }

    void ReCalcCostMoveOfGold() {
        //��������� ������� ����� ��� ���� ��������
        if (Gameplay.main.buyMoveCount < 1)
        {
            PriceMoveOfGold = 9;
        }
        else if (Gameplay.main.buyMoveCount < 2)
        {
            PriceMoveOfGold = 15;
        }
        else if (Gameplay.main.buyMoveCount < 3)
        {
            PriceMoveOfGold = 25;
        }
        else if (Gameplay.main.buyMoveCount < 4)
        {
            PriceMoveOfGold = 40;
        }
        else if (Gameplay.main.buyMoveCount < 5)
        {
            PriceMoveOfGold = 55;
        }
        else if (Gameplay.main.buyMoveCount < 6)
        {
            PriceMoveOfGold = 70;
        }
        else if (Gameplay.main.buyMoveCount < 7)
        {
            PriceMoveOfGold = 85;
        }
        else if (Gameplay.main.buyMoveCount < 8)
        {
            PriceMoveOfGold = 100;
        }
        else if (Gameplay.main.buyMoveCount < 9)
        {
            PriceMoveOfGold = 115;
        }
        else if (Gameplay.main.buyMoveCount < 10)
        {
            PriceMoveOfGold = 130;
        }
        else {
            ButtonBuyMoveOfGold.SetActive(false);
        }

        PriceMoveOfGoldText.text = PriceMoveOfGold.ToString();
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
    }

    public void ExitGameplay()
    {
        Gameplay.main.GameplayEnd = false;
        Destroy(MenuGameplay.GameField);
        UICTRL.main.OpenWorld();
    }

    public void ClickButtonContinueGold() {
        //���� ������ �� �������.. ��������� ���� � �������� ������
        if (PlayerProfile.main.GoldAmount < PriceMoveOfGold)
        {
            MessageCTRL.selected.ClickButtonClose();

            GlobalMessage.ShopBuyGold();

        }
        //���� ������ ���������� �������� 5 �����
        else {
            PlayerProfile.main.GoldAmount -= PriceMoveOfGold;
            Gameplay.main.movingCan += 5;

            //���������� ���������� �������
            Gameplay.main.buyMoveCount++;

            //��������� ����
            MessageCTRL.selected.ClickButtonClose();
        }
    }

    private void Update()
    {
        //��������� �� ������� ��� ��� � ������ ���� ��� ����
        if (Gameplay.main.movingCan > 0)
        {
            //���� ��������� ����� ������ ��������� ��������
            if (MessageCTRL.selected != null && MessageCTRL.selected.gameObject == gameObject) {
                MessageCTRL.selected.ClickButtonClose();
            }
            //���� ��������� �� ����� ������ ��������� �����
            else {
                MessageCTRL.selected.Destroy();
            }
        }
    }
}
