using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

namespace Ads.GoogleAdmob {
    public class BannerAdView : MonoBehaviour
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

        private BannerView _bannerView;

        BannerView CreateBannerView() {
            if (this._bannerView != null) {
                this._bannerView.Destroy();
                this._bannerView = null;
            }
            AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            BannerView bannerView = new BannerView(this._adUnitId, adaptiveSize, AdPosition.Bottom);
            return bannerView;
        }

        public void Initialize() {
            this.isInitialized = false;
            if (this._bannerView == null) {
                this._bannerView = CreateBannerView();
            }
            AdRequest adRequest = new AdRequest();
            this._bannerView.LoadAd(adRequest);
            this.isInitialized = true;
        }

        public void Show() {
            this._bannerView?.Show();
        }

        public void Hide() {
            this._bannerView?.Hide();
        }
    }
}
