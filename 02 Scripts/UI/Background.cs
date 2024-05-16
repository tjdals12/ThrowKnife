using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    using PlayData = InGame.PlayData;
    using StageData = StageDatabase.Data;

    public class Background : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        PlayData playData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject homeBackground;
        [SerializeField]
        GameObject inGameBackground;

        void Awake() {
            this.playData.OnGameStart += this.OnGameStart;
        }

        void OnGameStart(StageData stageData) {
            this.inGameBackground.SetActive(true);
            this.homeBackground.SetActive(false);
        }
    }
}
