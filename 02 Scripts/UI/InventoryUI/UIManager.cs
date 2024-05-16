using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryUI {
    using UIData = UI.Data;
    using UIType = UI.UIType;
    using UIAnimationType = UI.UIAnimationType;
    using SettingsData = Settings.Data;
    using PlayerData = InGame.PlayerData;

    public class UIManager : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        UIData uiData;
        [SerializeField]
        SettingsData settingsData;
        [SerializeField]
        PlayerData playerData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject inventoryUI;
        [SerializeField]
        Button backButton;

        [Header("Sound")]
        [Space(4)]
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip soundOfButtonClick;

        #region Unity Method
        void Awake() {
            this.settingsData.OnToggleSFX += this.OnToggleSFX;
            this.uiData.OnMove += this.OnMove;
            this.backButton.onClick.AddListener(() => {
                this.audioSource.PlayOneShot(this.soundOfButtonClick);
                this.uiData.Move(uiType: uiData.prevUI);
            });
        }
        #endregion

        #region Event Listener
        void OnToggleSFX(bool value) {
            this.audioSource.mute = value == false;
        }
        void OnMove(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType == UIType.Inventory) {
                this.Open(callback);
            } else {
                this.Close(null);
            }
        }
        #endregion

        void Open(Action callback) {
            this.playerData.ChangeKnife(this.playerData.knife.ID);
            this.inventoryUI.SetActive(true);
            callback?.Invoke();
        }

        void Close(Action callback) {
            this.inventoryUI.SetActive(false);
            callback?.Invoke();
        }
    }
}