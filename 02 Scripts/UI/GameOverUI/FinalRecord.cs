using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameOverUI {
    public class FinalRecord : MonoBehaviour
    {
        [Header("UI")]
        [Space(4)]
        [SerializeField]
        TextMeshProUGUI score;
        [SerializeField]
        TextMeshProUGUI stage;

        #region Unity Method
        void Awake() {
            this.score.text = "0";
            this.stage.text = "0";
        }
        #endregion

        public void ChangeRecord(int stage, int score) {
            StartCoroutine(this.Count(
                target: this.stage,
                fps: 30,
                from: 0,
                to: stage
            ));
            StartCoroutine(this.Count(
                target: this.score,
                fps: 60,
                from: 0,
                to: score
            ));
        }

        IEnumerator Count(TextMeshProUGUI target, float fps, int from, int to) {
            target.text = from.ToString();
            int current = from;
            while(to > current) {
                yield return new WaitForSeconds(1 / fps);
                current++;
                target.text = current.ToString();
            }
        }
    }
}
