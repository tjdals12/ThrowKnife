using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InGameUI {
    using UIData = UI.Data;
    using UIType = UI.UIType;
    using UIAnimationType = UI.UIAnimationType;
    using PlayData = InGame.PlayData;
    using StageData = StageDatabase.Data;

    public class UIManager : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        UIData uiData;
        [SerializeField]
        PlayData playData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject inGameUI;
        [SerializeField]
        ScoreUI scoreUI;
        [SerializeField]
        StageUI stageUI;
        [SerializeField]
        KnivesUI knivesUI;

        #region Unity Method
        void Awake() {
            this.uiData.OnMove += this.OnMove;
            this.uiData.OnOpen += this.OnOpen;
            this.uiData.OnClose += this.OnClose;
            this.playData.OnGameStart += this.OnGameStart;
            this.playData.OnKnifeStuck += this.OnKnifeStuck;
            this.playData.OnChangeStage += this.OnChangeStage;
            this.playData.OnChangeScore += this.OnChangeScore;
            this.playData.OnGameClear += this.OnGameClear;
            this.playData.OnGameOver += this.OnGameOver;
        }
        #endregion

        #region Event Listener
        void OnMove(UIType uIType, UIAnimationType uIAnimationType, Action callback) {
            if (uIType == UIType.InGame) {
                this.Open(callback);
            } else {
                this.Close(null);
            }
        }
        void OnOpen(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType != UIType.InGame) return;

            switch (uiAnimationType) {
                case UIAnimationType.Fade:
                    this.FadeIn(callback);
                    break;
                default:
                    this.Open(callback);
                    break;
            }
        }
        void OnClose(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType != UIType.InGame) return;

            switch (uiAnimationType) {
                case UIAnimationType.Fade:
                    this.FadeOut(callback);
                    break;
                default:
                    this.Close(callback);
                    break;
            }
        }
        void OnGameStart(StageData stageData) {
            this.knivesUI.GameStart(stageData.throwableKnifeCount);
            this.scoreUI.GameStart();
            this.stageUI.GameStart();
        }
        void OnKnifeStuck(GameObject[] remainKnives) {
            this.knivesUI.KnifeStuck(remainKnives.Length);
        }
        void OnChangeStage(int currentStageIndex, StageData stageData) {
            this.knivesUI.ChangeStage(stageData.throwableKnifeCount);
            this.stageUI.ChangeStage(currentStageIndex);
        }
        void OnChangeScore(int prev, int current) {
            this.scoreUI.ChangeScore(current);
        }
        void OnGameClear() {
            this.knivesUI.GameClear();
            this.stageUI.GameClear();
        }
        void OnGameOver(int currentStageIndex, int currentScore) {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Join(this.knivesUI.FadeOut().SetDelay(0.5f))
                .Join(this.scoreUI.FadeOut().SetDelay(0.5f))
                .Join(this.stageUI.FadeOut().SetDelay(0.5f))
                .OnComplete(() => {
                    this.knivesUI.GameOver();
                });
        }
        #endregion

        void Open(Action callback) {
            this.inGameUI.SetActive(true);
            callback?.Invoke();
        }

        void Close(Action callback) {
            this.inGameUI.SetActive(false);
            callback?.Invoke();
        }

        void FadeIn(Action callback) {
            Sequence sequence = DOTween.Sequence();
            sequence
                .OnStart(() => {
                    this.inGameUI.SetActive(true);
                })
                .Join(this.scoreUI.FadeIn())
                .Join(this.stageUI.FadeIn())
                .Join(this.knivesUI.FadeIn())
                .OnComplete(() => {
                    callback?.Invoke();
                });
        }

        void FadeOut(Action callback) {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Join(this.scoreUI.FadeOut())
                .Join(this.stageUI.FadeOut())
                .Join(this.knivesUI.FadeOut())
                .OnComplete(() => {
                    this.inGameUI.SetActive(false);
                    callback?.Invoke();
                });
        }
    }
}