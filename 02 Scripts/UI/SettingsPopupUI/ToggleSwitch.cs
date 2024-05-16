using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsPopupUI {
    public class ToggleSwitch : MonoBehaviour
    {
        [SerializeField]
        Button button;
        [SerializeField]
        GameObject enable;
        [SerializeField]
        GameObject disable;

        public event Action<bool> OnClick;

        #region Unity Method
        void Awake() {
            this.button.onClick.AddListener(() => {
                this.OnClick?.Invoke(!this.enable.activeSelf);
            });
        }
        #endregion

        public void Enable() {
            this.enable.SetActive(true);
            this.disable.SetActive(false);
        }

        public void Disable() {
            this.disable.SetActive(true);
            this.enable.SetActive(false);
        }
    }
}
