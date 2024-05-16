using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

namespace Ads.GoogleAdmob {
    public class RewardedAdView : MonoBehaviour
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

        private RewardedAd _rewardedAd;
        private Action OnReward;

        public void Initialize(Action OnReward) {
            this.OnReward = OnReward;
            this.LoadAd();
        }

        public void Show() {
            if (this._rewardedAd != null && this._rewardedAd.CanShowAd()) {
                this._rewardedAd.Show((Reward reward) => {
                    this.OnReward?.Invoke();
                });
            } else {
                this.OnReward?.Invoke();
            }
        }

        void LoadAd() {
            if (this._rewardedAd != null) {
                this._rewardedAd.Destroy();
                this.isInitialized = false;
                this._rewardedAd = null;
            }
            AdRequest adRequest = new AdRequest();
            RewardedAd.Load(this._adUnitId, adRequest, (RewardedAd ad, LoadAdError error) => {
                if (ad == null || error != null) {
                  Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                  return;
                }
                this.RegisterHandler(ad);
                this.isInitialized = true;
                this._rewardedAd = ad;
            });
        }

        void RegisterHandler(RewardedAd ad) {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () => {
                // Reload the ad so that we can show another as soon as possible.
                this.LoadAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) => {
                Debug.LogError("Rewarded ad failed to open full screen content with error : " + error);
                // Reload the ad so that we can show another as soon as possible.
                this.LoadAd();
            };
        }
    }
}
