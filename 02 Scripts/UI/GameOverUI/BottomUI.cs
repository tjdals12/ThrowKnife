using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameOverUI {
    public class BottomUI : MonoBehaviour
    {
        [SerializeField]
        Button restartButton;
        [SerializeField]
        Button inventoryButton;

        public event Action OnClickRestart;
        public event Action OnClickInventory;

        #region Unity Method
        void Awake() {
            this.restartButton.onClick.AddListener(() => {
                this.restartButton.enabled = false;
                this.OnClickRestart?.Invoke();
                this.restartButton.enabled = true;
            });
            this.inventoryButton.onClick.AddListener(() => {
                this.OnClickInventory?.Invoke();
            });
        }
        #endregion

        public Sequence ScaleIn() {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(restartButton.transform.DOScale(1, 0.2f).From(0).SetEase(Ease.OutBack));
            sequence.Append(inventoryButton.transform.DOScale(1, 0.2f).From(0).SetEase(Ease.OutBack));
            return sequence;
        }

        public Sequence ScaleOut() {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(restartButton.transform.DOScale(0, 0.2f).From(1));
            sequence.Append(inventoryButton.transform.DOScale(0, 0.2f).From(1));
            return sequence;
        }
    }
}
