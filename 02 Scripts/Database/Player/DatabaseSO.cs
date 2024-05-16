using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace PlayerDatabase {
    [CreateAssetMenu(fileName = "Database", menuName = "Database/Player")]

    public class DatabaseSO : ScriptableObject
    {
        public HashSet<int> knives { get; private set; }
        public int knifeID { get; private set; }
        public int point { get; private set; }
        public int bestStage { get; private set; } = 1;
        public int bestScore { get; private set; } = 1;
        public int playCount { get; private set; } = 0;

        public void Load() {
            this.knives = PlayerPrefs.HasKey("PlayerData_Knives")
                ? JsonConvert.DeserializeObject<HashSet<int>>(PlayerPrefs.GetString("PlayerData_Knives"))
                : new HashSet<int>() { 0 };
            this.knifeID = PlayerPrefs.HasKey("PlayerData_KnifeID")
                ? PlayerPrefs.GetInt("PlayerData_KnifeID")
                : 0;
            if (this.knives.Contains(this.knifeID) == false) {
                this.knifeID = 0;
            }
            this.point = PlayerPrefs.GetInt("PlayerData_Point");
            this.bestStage = PlayerPrefs.GetInt("PlayerData_BestStage");
            this.bestScore = PlayerPrefs.GetInt("PlayerData_BestScore");
            this.playCount = PlayerPrefs.GetInt("PlayerData_PlayCount");
        }

        public void ChangeKnifeID(int knifeID) {
            PlayerPrefs.SetInt("PlayerData_KnifeID", knifeID);
            PlayerPrefs.Save();
            this.knifeID = knifeID;
        }

        public void UpdatePoint(int point) {
            PlayerPrefs.SetInt("PlayerData_Point", point);
            PlayerPrefs.Save();
            this.point = point;
        }

        public void UpdateKnives(HashSet<int> knives) {
            PlayerPrefs.SetString("PlayerData_Knives", JsonConvert.SerializeObject(knives));
            PlayerPrefs.Save();
            this.knives = knives;
        }

        public void UpdateBestStage(int stage) {
            PlayerPrefs.SetInt("PlayerData_BestStage", stage);
            PlayerPrefs.Save();
            this.bestStage = stage;
        }

        public void UpdateBestScore(int score) {
            PlayerPrefs.SetInt("PlayerData_BestScore", score);
            PlayerPrefs.Save();
            this.bestScore = score;
        }

        public void UpdatePlayCount(int playCount) {
            PlayerPrefs.SetInt("PlayerData_PlayCount", playCount);
            PlayerPrefs.Save();
            this.playCount = playCount;
        }
    }
}
