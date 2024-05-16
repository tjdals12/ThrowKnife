using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace GameOverUI {
    using PlayData = InGame.PlayData;
    using StageData = StageDatabase.Data;
    using UIData = UI.Data;
    using UIType = UI.UIType;
    using UIAnimationType = UI.UIAnimationType;
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
        SettingsData settingsData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject gameOverUI;
        [SerializeField]
        Mask mask;
        [SerializeField]
        TopUI topUI;
        [SerializeField]
        FinalRecord finalRecord;
        [SerializeField]
        BottomUI bottomUI;

        [Header("Sound")]
        [Space(4)]
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip soundOfGameOver;
        [SerializeField]
        AudioClip soundOfClickHome;
        [SerializeField]
        AudioClip soundOfClickRestart;
        [SerializeField]
        AudioClip soundOfClickButton;

        #region Unity Method
        void Awake() {
            this.settingsData.OnToggleSFX += this.OnToggleSFX;
            this.uiData.OnMove += this.OnMove;
            this.uiData.OnOpen += this.OnOpen;
            this.uiData.OnClose += this.OnClose;
            this.playData.OnGameStart += this.OnGameStart;
            this.playData.OnGameOver += this.OnGameOver;
            this.topUI.OnClickHome += () => {
                this.audioSource.PlayOneShot(this.soundOfClickHome);
                DOTween.KillAll();
                this.uiData.Open(UIType.Loading, UIAnimationType.Fade);
                StartCoroutine(this.CallAfterSeconds(1f, () => this.uiData.Move(UIType.Lobby)));
            };
            this.bottomUI.OnClickRestart += () => {
                this.audioSource.PlayOneShot(this.soundOfClickRestart);
                this.uiData.Open(
                    uiType: UIType.Loading,
                    uiAnimationType: UIAnimationType.Fade,
                    callback: () => {
                        this.playData.GameStart();
                    });
            };
            this.bottomUI.OnClickInventory += () => {
                this.audioSource.PlayOneShot(this.soundOfClickButton);
                uiData.prevUI = UIType.GameOver;
                uiData.currentUI = UIType.GameOver;
                uiData.Move(uiType: UIType.Inventory);
            };
        }
        #endregion

        #region Event Listener
        void OnToggleSFX(bool value) {
            this.audioSource.mute = value == false;
        }

        void OnMove(UIType uiType, UIAnimationType uIAnimationType, Action callback) {
            if (uiType == UIType.GameOver) {
                this.Open(callback);
            } else {
                this.Close(null);
            }
        }

        void OnOpen(UIType uiType, UIAnimationType uIAnimationType, Action callback) {
            if (uiType != UIType.GameOver) return;

            switch (uIAnimationType) {
                default:
                    this.Open(callback);
                    break;
            }
        }

        void OnClose(UIType uiType, UIAnimationType uIAnimationType, Action callback) {
            if (uiType != UIType.GameOver) return;

            switch (uIAnimationType) {
                default:
                    this.Close(callback);
                    break;
            }
        }

        void OnGameStart(StageData stageData) {
            this.uiData.Close(uiType: UIType.Loading, uiAnimationType: UIAnimationType.Fade);
            this.uiData.Close(uiType: UIType.GameOver);
            this.uiData.Open(uiType: UIType.InGame, uiAnimationType: UIAnimationType.Fade);
        }

        void OnGameOver(int currentStageIndex, int currentScore) {
            uiData.Open(uiType: UIType.GameOver);
            Sequence sequence = DOTween.Sequence();
            sequence
                .SetDelay(1)
                .Append(this.mask.FadeIn())
                .Append(
                    this.topUI
                        .SlideIn()
                        .SetDelay(0.3f)
                        .OnStart(() => {
                            this.audioSource.PlayOneShot(this.soundOfGameOver);
                        })
                )
                .AppendCallback(() => {
                    this.finalRecord.ChangeRecord(currentStageIndex + 1, currentScore);
                })
                .Append(this.bottomUI.ScaleIn());
        }
        #endregion

        void Open(Action callback) {
            this.gameOverUI.SetActive(true);
            callback?.Invoke();
        }

        void Close(Action callback) {
            this.gameOverUI.SetActive(false);
            callback?.Invoke();
        }

        IEnumerator CallAfterSeconds(float seconds, Action callback) {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}
