using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace LobbyUI {
    using PlayerData = InGame.PlayerData;

    public class TopUI : MonoBehaviour
    {
        [SerializeField]
        GameObject topUI;
        [SerializeField]
        TextMeshProUGUI bestStage;
        [SerializeField]
        TextMeshProUGUI bestScore;
        [SerializeField]
        Button settingsButton;

        public event Action OnClickSettings;

        #region Unity Method
        void Awake() {
            this.settingsButton.onClick.AddListener(() => {
                this.OnClickSettings?.Invoke();
            });
        }
        #endregion

        public void ChangeStage(int stage) {
            this.bestStage.text = $"STAGE {stage}";
        }

        public void ChangeScore(int score) {
            this.bestScore.text = $"SCORE {score}";
        }

        public Tweener SlideIn() {
            return ((RectTransform)this.topUI.transform)
                .DOAnchorPos(Vector2.zero, 0.2f)
                .From(fromValue: Vector2.up * 700f, setImmediately: false);
        }

        public Tweener SlideOut() {
            RectTransform rectTransform = this.topUI.GetComponent<RectTransform>();
            return rectTransform
                .DOAnchorPos(Vector2.up * 700f, 0.2f)
                .From(fromValue: Vector2.zero, setImmediately: false)
                .OnComplete(() => {
                    rectTransform.anchoredPosition = Vector2.zero;
                });
        }
    }
}