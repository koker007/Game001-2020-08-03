using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

//Андрей
public class AdMobController : MonoBehaviour
{
    public static AdMobController main;
    private RewardedAd rewardedAd;
    private RewardedAd playBonusAd;
    private BannerView bannerView;

    string keyVideoTest = "ca-app-pub-3940256099942544/5224354917";
    string keyVideoAndroidAddMoving = "ca-app-pub-4685950010415099/1502718587";
    string keyVideoAndroidPlayWithBonus = "ca-app-pub-4685950010415099/7529188422";
    string keyVideoIphone = "";

    [SerializeField]
    private bool showAd = true;

    private void Start()
    {
        main = this;
        MobileAds.Initialize(initStatus => { });

        //Политика использования детской рекламы
        RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
            .SetMaxAdContentRating(MaxAdContentRating.G)
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
            .SetTagForUnderAgeOfConsent(TagForUnderAgeOfConsent.True)
            .build();

        RequestConfiguration requestConfiguration2 = new RequestConfiguration.Builder()
            .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        CreateAndLoadRewardedAd();
        if (showAd)
        {
            RequestBanner();
        }
    }

    //Отключить рекламу (кроме наградной)
    public void DisableAds()
    {
        showAd = false;
        bannerView.Destroy();
    }

    //Создаем наградную рекламу
    private void CreateAndLoadRewardedAd()
    {
        string rewardedAdUnitId;
        #if UNITY_ANDROID
                rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
                reawardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
                reawardedAdUnitId = "unexpected_platform";
        #endif

        if (!Settings.main.DeveloperTesting)
        {
            rewardedAdUnitId = keyVideoAndroidAddMoving;
        }
        else {
            rewardedAdUnitId = keyVideoTest;
        }

        rewardedAd = new RewardedAd(rewardedAdUnitId);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder()
            .Build();
        rewardedAd.LoadAd(request);


        #if UNITY_ANDROID
                rewardedAdUnitId = keyVideoAndroidPlayWithBonus;
        #elif UNITY_IPHONE
                        reawardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
                        reawardedAdUnitId = "unexpected_platform";
        #endif

        playBonusAd = new RewardedAd(rewardedAdUnitId);
        playBonusAd.OnAdLoaded += HandlePlayWithBonusLoaded;
        playBonusAd.OnUserEarnedReward += HandlePlayWithBonusReward;
        playBonusAd.OnAdClosed += HandlePlayWithBonusClosed;

        //Используя настройки для предыдущей рекламмы создаем эту
        playBonusAd.LoadAd(request);
    }

    //Создаем баннер
    private void RequestBanner()
    {
        #if UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
                    string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
                    string adUnitId = "unexpected_platform";
        #endif

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        this.bannerView.OnAdLoaded += this.HandleOnBannerAdLoaded;
        this.bannerView.OnAdFailedToLoad += this.HandleOnBannerAdFailedToLoad;
        this.bannerView.OnAdOpening += this.HandleOnBannerAdOpened;
        this.bannerView.OnAdClosed += this.HandleOnBannerAdClosed;

        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);
    }

    public void ShowRewardedAd()
    {

        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }
    public void ShowPlayBonusAd()
    {

        if (playBonusAd.IsLoaded())
        {
            playBonusAd.Show();
        }
    }

    #region rewardedAdHandlers
    private void HandleUserEarnedReward(object sender, Reward args)
    {
        Gameplay.main.movingCan += 2;
        Gameplay.main.adWatchedCount++;
        //MessageCTRL.selected.ClickButtonClose();


        if (GameFieldCTRL.main != null)
            DataBase.main.typeLevel.SetLevelData(Gameplay.main.levelSelect, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);

        
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        
    }

    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {

    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {

    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        CreateAndLoadRewardedAd();
    }
    #endregion

    #region bannerAdHandlers
    private void HandleOnBannerAdLoaded(object sender, EventArgs args)
    {
        
    }

    private void HandleOnBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        
    }

    private void HandleOnBannerAdOpened(object sender, EventArgs args)
    {
       
    }

    private void HandleOnBannerAdClosed(object sender, EventArgs args)
    {
       
    }

    #endregion


    #region PlayWithBonusHandlers
    private void HandlePlayWithBonusReward(object sender, Reward args)
    {
        UICTRL.main.OpenGameplay();
        Gameplay.main.randomizedPlayBonus();
    }

    private void HandlePlayWithBonusLoaded(object sender, EventArgs args)
    {

    }

    private void HandlePlayWithBonusFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }

    private void HandlePlayWithBonusOpening(object sender, EventArgs args)
    {

    }

    private void HandlePlayWithBonusFailedToShow(object sender, AdErrorEventArgs args)
    {

    }

    private void HandlePlayWithBonusClosed(object sender, EventArgs args)
    {
        CreateAndLoadRewardedAd();
    }
    #endregion
}
