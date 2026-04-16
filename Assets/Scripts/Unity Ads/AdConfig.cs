public static class AdConfig
{
    public static string AppKey => GetAppKey();
    public static string BannerAdUnitId => GetBannerAdUnitId();
    public static string InterstitalAdUnitId => GetInterstitialAdUnitId();
    public static string RewardedVideoAdUnitId => GetRewardedVideoAdUnitId();

    static string GetAppKey()
    {
#if UNITY_ANDROID
            return "258f42e4d";
#elif UNITY_IPHONE
            return "8545d445";
#else
        return "unexpected_platform";
#endif
    }

    static string GetBannerAdUnitId()
    {
#if UNITY_ANDROID
            return "v3qhin8qd9ukahf2";
#elif UNITY_IPHONE
            return "iep3rxsyp9na3rw8";
#else
        return "unexpected_platform";
#endif
    }
    static string GetInterstitialAdUnitId()
    {
#if UNITY_ANDROID
            return "u8vlye4wxggj8qyw";
#elif UNITY_IPHONE
            return "wmgt0712uuux8ju4";
#else
        return "unexpected_platform";
#endif
    }

    static string GetRewardedVideoAdUnitId()
    {
#if UNITY_ANDROID
            return "48dqd24m043to9v3";
#elif UNITY_IPHONE
            return "qwouvdrkuwivay5q";
#else
        return "unexpected_platform";
#endif
    }
}