using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings {
    using SettingsDatabase = SettingsDatabase.DatabaseSO;

    public class Data : MonoBehaviour
    {
        [SerializeField]
        SettingsDatabase settingsDatabase;

        #region Unity Method
        void Awake() {
            this.settingsDatabase.Load();
        }
        void Start() {
            this.OnToggleSFX(this.settingsDatabase.SFX);
            this.OnToggleBGM(this.settingsDatabase.BGM);
        }
        #endregion

        public event Action<bool> OnToggleSFX;
        public void ToggleSFX(bool value) {
            this.settingsDatabase.ChangeSFX(value);
            this.OnToggleSFX?.Invoke(value);
        }

        public event Action<bool> OnToggleBGM;
        public void ToggleBGM(bool value) {
            this.settingsDatabase.ChangeBGM(value);
            this.OnToggleBGM?.Invoke(value);
        }
    }
}
