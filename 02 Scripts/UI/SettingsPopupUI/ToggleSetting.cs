using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsPopupUI {
    public class ToggleSetting : MonoBehaviour
    {
        [SerializeField]
        ToggleIcon toggleIcon;
        [SerializeField]
        ToggleSwitch toggleSwitch;

        public event Action<bool> OnClick;

        #region Unity Method
        void Awake() {
            this.toggleSwitch.OnClick += this.OnClick;
        }
        #endregion

        public void On() {
            this.toggleIcon.Enable();
            this.toggleSwitch.Enable();
        }

        public void Off() {
            this.toggleIcon.Disable();
            this.toggleSwitch.Disable();
        }
    }
}
