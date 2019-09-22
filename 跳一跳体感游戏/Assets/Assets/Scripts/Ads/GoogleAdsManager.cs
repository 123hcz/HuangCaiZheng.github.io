using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System.Collections.Generic;

public class GoogleAdsManager : MonoBehaviour
{
    public static GoogleAdsManager Instance;

    [Header("Google AdMob")]
    public string adKeyword = "Game";

    public bool displayBanner = true;
    public AdPosition adBannerPosition = AdPosition.Bottom;

    public string adBannerAndroidID = "INSERT_ANDROID_BANNER_AD_UNIT_ID_HERE";
    public string adBannerIosID = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";

    public bool displayInterstial = true;
    public int toShowInterstial = 3;
    public string adInterstialAndroidID = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
    public string adInterstialIosID = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";

    public delegate void RewardCompleteDelegate();
    public RewardCompleteDelegate RewardCompleteCallback;
    public RewardCompleteDelegate InterstialDoneCallback;

    internal List<double> rewardStack = new List<double>();

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;
    private bool bannerReady = false;
    private float deltaTime = 0.0f;
    private static string outputMessage = "";
    private int countPlay = 0;

    public static string OutputMessage
    {
        set { outputMessage = value; }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        if (displayBanner)
            RequestBanner();

        if (displayInterstial)
            RequestInterstitial();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest createAdRequest()
    {
        AdRequest.Builder adReqBuilder = new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddKeyword(adKeyword)
            ;

        string deviceID = AdsHelper.getDeviceAdMobID();
        Debug.Log("Adding Test Device ID: " + deviceID);
        if (!string.IsNullOrEmpty(deviceID))
            adReqBuilder.AddTestDevice(deviceID);

        return adReqBuilder.Build();
    }

    // ----------------------------------------
    // -------------- BANNER ------------------
    // ----------------------------------------
    public void RequestBanner()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = adBannerAndroidID;
#elif UNITY_IPHONE
            string adUnitId = adBannerIosID;  
#else
            string adUnitId = "unexpected_platform";
#endif

        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, adBannerPosition);

        bannerView.OnAdLoaded += HandleAdLoaded;
        bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
        bannerView.OnAdLoaded += HandleAdOpened;
        bannerView.OnAdClosed += HandleAdClosed;
        bannerView.OnAdLeavingApplication += HandleAdLeftApplication;

        bannerView.LoadAd(createAdRequest());

        Debug.Log("Banner request: " + adUnitId);
    }

    public void ShowBanner()
    {
        if (bannerReady)
            bannerView.Show();
    }

    public void HideBanner()
    {
        bannerView.Hide();
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received.");
        bannerReady = true;
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
        bannerReady = false;
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLeftApplication event received");
    }

    #endregion


    // --------------------------------------------
    // -------------- INTERSTIAL ------------------
    // --------------------------------------------

    public void RequestInterstitial()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = adInterstialAndroidID;
#elif UNITY_IPHONE
            string adUnitId = adInterstialIosID;
#else
            string adUnitId = "unexpected_platform";
#endif

        interstitial = new InterstitialAd(adUnitId);

        interstitial.OnAdLoaded += HandleInterstitialLoaded;
        interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.OnAdOpening += HandleInterstitialOpened;
        interstitial.OnAdClosed += HandleInterstitialClosed;
        interstitial.OnAdLeavingApplication += HandleInterstitialLeftApplication;

        interstitial.LoadAd(createAdRequest());

        Debug.Log("Interstial request: " + adUnitId);
    }

    public void ShowInterstitial()
    {
        countPlay++;
        if (interstitial.IsLoaded())
        {
             if (countPlay >= toShowInterstial)
             {
                 countPlay = 0;
                 interstitial.Show();
             }       
        }
        else
        {
            Debug.Log("Interstitial is not ready yet.");
        }
    }

    public void HideInterstitial()
    {
        if (interstitial.IsLoaded())
            interstitial.Destroy();
    }

    private void InterstialCompleted()
    {
        if (InterstialDoneCallback != null)
            InterstialDoneCallback();
    }

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialClosed event received");
        InterstialCompleted();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialLeftApplication event received");
        InterstialCompleted();
    }

    #endregion
}