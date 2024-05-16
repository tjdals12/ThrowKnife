using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsPopupUI {
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
        SettingsData settingsData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject settingsPopupUI;
        [SerializeField]
        Button closeButton;

        [Header("UI - Sound")]
        [Space(4)]
        [SerializeField]
        ToggleSetting SFXSetting;
        [SerializeField]
        ToggleSetting BGMSetting;

        [Header("Sound")]
        [Space(4)]
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip soundOfClose;
        [SerializeField]
        AudioClip soundOfToggle;

        #region Unity Method
        void Awake() {
            this.uiData.OnOpen += this.OnOpen;
            this.uiData.OnClose += this.OnClose;
            this.settingsData.OnToggleSFX += this.OnToggleSFX;
            this.settingsData.OnToggleBGM += this.OnToggleBGM;
            this.SFXSetting.OnClick += (value) => {
                this.audioSource.PlayOneShot(this.soundOfToggle);
                this.settingsData.ToggleSFX(value);
            };
            this.BGMSetting.OnClick += (value) => {
                this.audioSource.PlayOneShot(this.soundOfToggle);
                this.settingsData.ToggleBGM(value);
            };
            this.closeButton.onClick.AddListener(() => {
                this.audioSource.PlayOneShot(this.soundOfClose);
                this.uiData.Close(uiType: UIType.SettingsPopup);
            });
        }
        #endregion

        #region  Event Listener
        void OnOpen(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType != UIType.SettingsPopup) return;
            this.Open();
            callback?.Invoke();
        }

        void OnClose(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType != UIType.SettingsPopup) return;
            this.Close();
            callback?.Invoke();
        }

        void OnToggleSFX(bool value) {
            if (value) {
                this.SFXSetting.On();
                this.audioSource.mute = false;
            } else {
                this.SFXSetting.Off();
                this.audioSource.mute = true;
            }
        }

        void OnToggleBGM(bool value) {
            if (value) {
                this.BGMSetting.On();
            } else {
                this.BGMSetting.Off();
            }
        }
        #endregion

        void Open() {
            this.settingsPopupUI.SetActive(true);
        }

        void Close() {
            this.settingsPopupUI.SetActive(false);
        }
    }
}