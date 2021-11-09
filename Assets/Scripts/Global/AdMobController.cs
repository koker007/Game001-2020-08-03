using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AdMobController : MonoBehaviour
{
    public static RewardedAd rewardedAd;
    private BannerView bannerView;

    public void Start()
    {
        MobileAds.Initialize(initStatus => { });
        CreateAndLoadRewardedAd();
        RequestBanner();
    }

    public void CreateAndLoadRewardedAd()
    {
        string reawardedAdUnitId;
#if UNITY_ANDROID
        reawardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            reawardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            reawardedAdUnitId = "unexpected_platform";
#endif

        rewardedAd = new RewardedAd(reawardedAdUnitId);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

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

    public static void ShowRewardedAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Gameplay.main.movingCan += 2;
        GlobalMessage.Close();
    }

    #region rewardedAdHandlers
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        
    }
    
    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {

    }
    
    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {

    }
    
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        CreateAndLoadRewardedAd();
    }
    #endregion

    #region bannerAdHandlers
    public void HandleOnBannerAdLoaded(object sender, EventArgs args)
    {
        
    }

    public void HandleOnBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        
    }

    public void HandleOnBannerAdOpened(object sender, EventArgs args)
    {
       
    }

    public void HandleOnBannerAdClosed(object sender, EventArgs args)
    {
       
    }

    #endregion
}
