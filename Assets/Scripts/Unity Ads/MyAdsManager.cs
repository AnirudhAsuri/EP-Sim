using UnityEngine;
using Unity.Services.LevelPlay;
using System;

public class MyAdsManager : MonoBehaviour
{
    public static MyAdsManager Instance { get; private set; }

    private LevelPlayInterstitialAd interstitialAd;
    private LevelPlayRewardedAd rewardedAd;
    private LevelPlayBannerAd bannerAd;

    // --- Events for your Game Logic ---
    public static event Action OnRewardedAdGranted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 1. STOLEN: Validation (Checks if your Android Manifest/IDs are correct)
        Debug.Log("[Ads] Validating Integration...");
        LevelPlay.ValidateIntegration();

        // 2. Setup Initialization Listeners
        LevelPlay.OnInitSuccess += OnSDKInitialized;
        LevelPlay.OnInitFailed += (error) => Debug.LogError($"[Ads] Init Failed: {error.ErrorMessage}");

        // 3. Initialize
        LevelPlay.Init(AdConfig.AppKey);
    }

    private void OnSDKInitialized(LevelPlayConfiguration config)
    {
        Debug.Log("[Ads] LevelPlay Initialized Successfully.");

        // Create Ad Objects
        interstitialAd = new LevelPlayInterstitialAd(AdConfig.InterstitalAdUnitId);
        rewardedAd = new LevelPlayRewardedAd(AdConfig.RewardedVideoAdUnitId);
        bannerAd = new LevelPlayBannerAd(AdConfig.BannerAdUnitId);

        // 4. STOLEN: Error Listeners (Tells you WHY ads don't show)
        interstitialAd.OnAdLoadFailed += (error) => Debug.LogWarning($"[Ads] Interstitial Load Failed: {error.ErrorMessage}");
        rewardedAd.OnAdLoadFailed += (error) =>
        {
            Debug.LogWarning($"[Ads] Rewarded Load Failed: {error.ErrorMessage}");
            Invoke(nameof(RetryLoadRewarded), 5f);
        };
            bannerAd.OnAdLoadFailed += (error) => Debug.LogWarning($"[Ads] Banner Load Failed: {error.ErrorMessage}");

        // 5. Reward Listener
        rewardedAd.OnAdRewarded += (info, reward) =>
        {
            Debug.Log($"[Ads] Reward Received: {reward.Name} Amount: {reward.Amount}");
            // Trigger your specific game logic (e.g., extra life, coins)
            OnRewardedAdGranted?.Invoke();
        };

        // 6. STOLEN: Auto-Reload Logic & Time Management
        interstitialAd.OnAdDisplayed += (info) => {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        };

        interstitialAd.OnAdClosed += (info) => {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            interstitialAd.LoadAd();
        };

        rewardedAd.OnAdDisplayed += (info) => {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        };

        rewardedAd.OnAdClosed += (info) => {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            rewardedAd.LoadAd();
        };

        // Initial Load
        interstitialAd.LoadAd();
        rewardedAd.LoadAd();
        bannerAd.LoadAd();

        // BANNER AD LOGIC

        ShowBanner();
    }

    private void RetryLoadRewarded()
    {
        if (rewardedAd != null && !rewardedAd.IsAdReady())
        {
            rewardedAd.LoadAd();
        }
    }

    // --- Public Methods ---
    public void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.Log("[Ads] Interstitial not ready yet.");
            Time.timeScale = 1f; // Safety resume if ad fails to even show
        }
    }

    public void ShowRewarded()
    {
        if (rewardedAd != null && rewardedAd.IsAdReady())
        {
            rewardedAd.ShowAd();
        }
        else
        {
            Debug.Log("[Ads] Rewarded Video not ready yet.");
            // Optional: Show a "No Ads Available" UI popup here
        }
    }

    public void ShowBanner()
    {
        if(bannerAd != null)
        {
            bannerAd.ShowAd();
        }

        else
        {
            Debug.Log("[Ads] Banner Ad not ready yet.");
        }
    }

    // 7. STOLEN: Memory Cleanup
    private void OnDisable()
    {
        interstitialAd?.DestroyAd();
        rewardedAd?.DestroyAd();
    }
}