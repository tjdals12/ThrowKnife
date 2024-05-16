using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GlobalUI {
    using PlayerData = InGame.PlayerData;
    using PlayData = InGame.PlayData;
    using StageData = StageDatabase.Data;
    using SettingsData = Settings.Data;
    using UIData = UI.Data;
    using UIType = UI.UIType;
    using UIAnimationType = UI.UIAnimationType;

    public class UIManager : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        PlayerData playerData;
        [SerializeField]
        PlayData playData;
        [SerializeField]
        SettingsData settingsData;
        [SerializeField]
        UIData uiData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        PointUI pointUI;

        [Header("Sound")]
        [Space(4)]
        [SerializeField]
        AudioSource audioSource;

        #region Unity Method
        void Awake() {
            this.settingsData.OnToggleBGM += this.OnToggleBGM;
            this.playerData.OnChangePoint += this.OnChangePoint;
            this.playData.OnGameStart += this.OnGameStart;
            this.uiData.OnMove += this.OnMove;
        }
        #endregion

        #region Event Listener
        void OnToggleBGM(bool value) {
            this.audioSource.mute = value == false;
        }
        void OnChangePoint(int prev, int current) {
            this.pointUI.ChangePoint(current);
        }
        void OnGameStart(StageData stageData) {
            this.audioSource.DOFade(0, 1f);
        }
        void OnMove(UIType uiType, UIAnimationType uIAnimationType, Action callback) {
            if (uiType == UIType.Lobby) {
                this.audioSource.DOFade(0.25f, 1f);
            }
        }
        #endregion
    }
}
