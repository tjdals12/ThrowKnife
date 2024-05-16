using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace LobbyUI {
    using UIData = UI.Data;
    using UIType = UI.UIType;
    using UIAnimationType = UI.UIAnimationType;
    using PlayData = InGame.PlayData;
    using StageData = StageDatabase.Data;
    using PlayerData = InGame.PlayerData;
    using SettingsData = Settings.Data;

    public class UIManager : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        UIData uiData;
        [SerializeField]
        PlayData playData;
        [SerializeField]
        PlayerData playerData;
        [SerializeField]
        SettingsData settingsData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject lobbyUI;
        [SerializeField]
        TopUI topUI;
        [SerializeField]
        BottomUI bottomUI;

        [Header("Sound")]
        [Space(4)]
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip soundOfClickPlay;
        [SerializeField]
        AudioClip soundOfClickButton;
        [SerializeField]
        AudioClip soundOfOpenPopup;

        #region Unity Method
       void Awake() {
            this.settingsData.OnToggleSFX += this.OnToggleSFX;
            this.uiData.OnMove += this.OnMove;
            this.uiData.OnOpen += this.OnOpen;
            this.uiData.OnClose += this.OnClose;
            this.playerData.OnChangeBestStage += this.OnChangeBestStage;
            this.playerData.OnChangeBestScore += this.OnChangeBestScore;
            this.topUI.OnClickSettings += () => {
                this.audioSource.PlayOneShot(this.soundOfOpenPopup);
                this.uiData.Open(UIType.SettingsPopup);
            };
            this.bottomUI.OnClickPlay += () => {
                this.audioSource.PlayOneShot(this.soundOfClickPlay);
                this.playData.GameStart();
            };
            this.bottomUI.OnClickInventory += () => {
                this.audioSource.PlayOneShot(this.soundOfClickButton);
                this.uiData.prevUI = UIType.Lobby;
                this.uiData.currentUI = UIType.Inventory;
                this.uiData.Move(uiType: UIType.Inventory);
            };
            this.playData.OnGameStart += this.OnGameStart;
        }
        #endregion

        #region Event Listener
        void OnToggleSFX(bool value) {
            this.audioSource.mute = value == false;
        }
        void OnMove(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType == UIType.Lobby) {
                switch (uiAnimationType) {
                    case UIAnimationType.Slide:
                        this.SlideIn(callback);
                        break;
                    default:
                        this.Open(callback);
                        break;
                }
            } else {
                switch (uiAnimationType) {
                    case UIAnimationType.Slide:
                        this.SlideOut(null);
                        break;
                    default:
                        this.Close(null);
                        break;
                }
            }
        }

        void OnOpen(UIType uIType, UIAnimationType uiAnimationType, Action callback) {
            if (uIType != UIType.Lobby) return;

            switch (uiAnimationType) {
                case UIAnimationType.Slide:
                    this.SlideIn(callback);
                    break;
                default:
                    this.Open(callback);
                    break;
            }
        }

        void OnClose(UIType uIType, UIAnimationType uiAnimationType, Action callback) {
            if (uIType != UIType.Lobby) return;

            switch (uiAnimationType) {
                case UIAnimationType.Slide:
                    this.SlideOut(callback);
                    break;
                default:
                    this.Close(callback);
                    break;
            }
        }

        void OnChangeBestStage(int prev, int current) {
            this.topUI.ChangeStage(current);
        }

        void OnChangeBestScore(int prev, int current) {
            this.topUI.ChangeScore(current);
        }

        void OnGameStart(StageData stageData) {
            this.uiData.Open(uiType: UIType.InGame);
            this.uiData.Close(uiType: UIType.Lobby, uiAnimationType: UIAnimationType.Slide);
        }
        #endregion

        void Open(Action callback) {
            this.lobbyUI.SetActive(true);
            callback?.Invoke();
        }

        void Close(Action callback) {
            this.lobbyUI.SetActive(false);
            callback?.Invoke();
        }

        void SlideIn(Action callback) {
            Sequence sequence = DOTween.Sequence();
            sequence
                .OnStart(() => {
                    this.lobbyUI.SetActive(true);
                })
                .Join(this.topUI.SlideIn())
                .Join(this.bottomUI.SlideIn())
                .OnComplete(() => {
                    callback?.Invoke();
                });
        }
        void SlideOut(Action callback) {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Join(this.topUI.SlideOut())
                .Join(this.bottomUI.SlideOut())
                .OnComplete(() => {
                    this.lobbyUI.SetActive(false);
                    callback?.Invoke();
                });
        }

        IEnumerator CallAfterSeconds(float seconds, Action callback) {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}