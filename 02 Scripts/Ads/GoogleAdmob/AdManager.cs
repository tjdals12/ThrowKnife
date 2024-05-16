using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds;
using GoogleMobileAds.Api;

namespace Ads.GoogleAdmob {
    using AdType = InGame.AdType;
    using PlayData = InGame.PlayData;
    using StageData = StageDatabase.Data;
    using UIData = UI.Data;
    using UIType = UI.UIType;
    using UIAnimationType = UI.UIAnimationType;

    public class AdManager : MonoBehaviour
    {
        [SerializeField]
        PlayData playData;
        [SerializeField]
        UIData uiData;

        [SerializeField]
        BannerAdView bannerAdView;
        [SerializeField]
        InterstitialAdView interstitialAdView;
        [SerializeField]
        RewardedAdView rewardedAdView;

        #region Unity Method
        void Awake() {
            this.playData.OnGameStart += this.OnGameStart;
            this.playData.OnGameOver += this.OnGameOver;
            this.playData.OnRequestLoadAd += this.OnRequestLoadAd;
            this.uiData.OnMove += this.OnMove;
            this.uiData.OnOpen += this.OnOpen;
        }
        void Start() {
            MobileAds.Initialize(this.OnInitialize);
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
        }
        #endregion

        #region Event Listener
        void OnGameStart(StageData stageData) {
            this.uiData.Close(UIType.Loading, UIAnimationType.Fade);
            this.bannerAdView.Hide();
        }
        void OnGameOver(int currentStageIndex, int currentScore) {
            StartCoroutine(ShowBanner());
            IEnumerator ShowBanner() {
                yield return new WaitForSeconds(1.5f);
                this.bannerAdView.Show();
            }
        }
        void OnRequestLoadAd(AdType adType) {
            this.uiData.Open(UIType.Loading, UIAnimationType.None);
            switch (adType) {
                case AdType.Interstitial:
                    this.bannerAdView.Hide();
                    this.interstitialAdView.Show();
                    break;
                case AdType.Rewarded:
                    this.bannerAdView.Hide();
                    this.rewardedAdView.Show();
                    break;
                default:
                    this.uiData.Close(UIType.Loading, UIAnimationType.Fade);
                    break;
            }
        }
        void OnMove(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType == UIType.Lobby) {
                this.bannerAdView.Show();
            } 
        }
        void OnOpen(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType == UIType.Loading) {
                this.bannerAdView.Hide();
            }
        }
        #endregion

        void OnInitialize(InitializationStatus initStatus) {
            this.bannerAdView.Initialize();
            this.interstitialAdView.Initialize(OnClosed: () => {
                this.playData.GameStart();
            });
            this.rewardedAdView.Initialize(OnReward: () => {
                this.playData.GameStart();
            });
        }
    }
}
