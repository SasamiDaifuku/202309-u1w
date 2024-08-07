using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdmobBanner : MonoBehaviour
{
    [SerializeField]
    private string AndroidAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField]
    private string IphoneAdUnitId = "ca-app-pub-3940256099942544/2934735716";
    [SerializeField]
    private string ReleaseIphoneAdUnitId = "ca-app-pub-7551946912738729/2851185468";
    
    
    private string _adUnitId = "unused";
    
    BannerView _bannerView;
    
    private void Start()
    {
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    _adUnitId = AndroidAdUnitId;
#elif UNITY_IPHONE
    _adUnitId = IphoneAdUnitId;
#else
    _adUnitId = "unused";
#endif
        //AdmobSDKの初期化
        //Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
        LoadAd();
    }
    
    /// Creates the banner view and loads a banner ad.
    public void LoadAd()
    {
        CreateBannerView();

        var adRequest = new AdRequest();

        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }
    
    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);
    }
    
    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public void OnDestroy()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
}
