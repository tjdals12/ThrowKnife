using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

namespace Ads.GoogleAdmob {
    public class InterstitialAdView : MonoBehaviour
    {
        public bool isInitialized { get; private set; }

        // These ad units are configured to always serve test ads.
        #if UNITY_ANDROID
            // Test ID
            private string _adUnitId = "unused";
        #elif UNITY_IPHONE
            // Test ID
            private string _adUnitId = "unused";
        #else
            // Test ID
            private string _adUnitId = "unused";
        #endif

        private InterstitialAd _interstitialAd;
        public event Action OnClosed;

        public void Initialize(Action OnClosed) {
            this.OnClosed = OnClosed;
            this.LoadAd();
        }

        public void Show() {
            if (this._interstitialAd != null && this._interstitialAd.CanShowAd()) {
                this._interstitialAd.Show();
            } else {
                this.OnClosed?.Invoke();
            }
        }

        void LoadAd() {
            if (this._interstitialAd != null) {
                this._interstitialAd.Destroy();
                this.isInitialized = false;
                this._interstitialAd = null;
            }
            AdRequest adRequest = new AdRequest();
            InterstitialAd.Load(this._adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) => {
                if (ad == null || error != null) {
                    Debug.LogError("interstitial ad failed to load an ad with error : " + error);
                    return;
                }
                this.RegisterHandler(ad);
                this.isInitialized = true;
                this._interstitialAd = ad;
            });
        }

        void RegisterHandler(InterstitialAd ad) {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                // Reload the ad so that we can show another as soon as possible.
                this.LoadAd();
                this.OnClosed?.Invoke();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                            "with error : " + error);
                // Reload the ad so that we can show another as soon as possible.
                this.LoadAd();
            };
        }

    }
}
