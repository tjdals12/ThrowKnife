using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace LoadingUI {
    using UIData = UI.Data;
    using UIType = UI.UIType;
    using UIAnimationType = UI.UIAnimationType;

    public class UIManager : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        UIData uiData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject loadingUI;
        [SerializeField]
        Background background;
        [SerializeField]
        Indicator indicator;
        [SerializeField]
        Title title;

        #region Unity Method
        void Awake() {
            this.uiData.OnMove += this.OnMove;
            this.uiData.OnOpen += this.OnOpen;
            this.uiData.OnClose += this.OnClose;
        }
        #endregion

        #region Event Listener
        void OnMove(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType == UIType.Loading) {
                switch(uiAnimationType) {
                    case UIAnimationType.Fade:
                        this.FadeIn(callback);
                        break;
                    default:
                        this.Open(callback);
                        break;
                }
            } else {
                switch(uiAnimationType) {
                    case UIAnimationType.Fade:
                        this.FadeOut(null);
                        break;
                    default:
                        this.Close(null);
                        break;
                }
            }
        }

        void OnOpen(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType != UIType.Loading) return;

            switch(uiAnimationType) {
                case UIAnimationType.Fade:
                    this.FadeIn(callback);
                    break;
                default:
                    this.Open(callback);
                    break;
            }
        }

        void OnClose(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType != UIType.Loading) return;

            switch(uiAnimationType) {
                case UIAnimationType.Fade:
                    this.FadeOut(callback);
                    break;
                default:
                    this.Close(callback);
                    break;
            }
        }
        #endregion

        void Open(Action callback) {
            this.loadingUI.SetActive(true);
            callback?.Invoke();
        }

        void Close(Action callback) {
            this.loadingUI.SetActive(false);
            callback?.Invoke();
        }

        void FadeIn(Action callback) {
            Sequence sequence = DOTween.Sequence();
            sequence
                .OnStart(() => {
                    this.loadingUI.SetActive(true);
                })
                .Join(this.background.FadeIn())
                .Join(this.indicator.FadeIn())
                .Join(this.title.FadeIn())
                .OnComplete(() => {
                    callback?.Invoke();
                });
        }

        void FadeOut(Action callback) {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Join(this.background.FadeOut())
                .Join(this.indicator.FadeOut())
                .Join(this.title.FadeOut())
                .OnComplete(() => {
                    this.loadingUI.SetActive(false);
                    callback?.Invoke();
                });
        }
    }
}
