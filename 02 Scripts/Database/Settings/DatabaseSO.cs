using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SettingsDatabase {
    [CreateAssetMenu(fileName = "Database", menuName = "Database/Settings")]
    public class DatabaseSO : ScriptableObject
    {
        public bool SFX { get; private set; }
        public bool BGM { get; private set; }

        public void Load() {
            this.SFX = PlayerPrefs.HasKey("SettingsData_SFX")
                ? (PlayerPrefs.GetInt("SettingsData_SFX") == 1 ? true : false)
                : true;
            this.BGM = PlayerPrefs.HasKey("SettingsData_BGM")
                ? (PlayerPrefs.GetInt("SettingsData_BGM") == 1 ? true : false)
                : true;
        }

        public void ChangeSFX(bool value) {
            PlayerPrefs.SetInt("SettingsData_SFX", value ? 1 : 0);
            PlayerPrefs.Save();
            this.SFX = value;
        }

        public void ChangeBGM(bool value) {
            PlayerPrefs.SetInt("SettingsData_BGM", value ? 1 : 0);
            PlayerPrefs.Save();
            this.BGM = value;
        }
    }
}
