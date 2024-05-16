using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameOverUI {
    public class TopUI : MonoBehaviour
    {
        [SerializeField]
        GameObject topUI;
        [SerializeField]
        Button homeButton;

        public event Action OnClickHome;

        #region Unity Method
        void Awake() {
            this.homeButton.onClick.AddListener(() => {
                this.OnClickHome?.Invoke();
            });
        }
        #endregion

        public Tweener SlideIn() {
            return ((RectTransform)this.topUI.transform)
                .DOAnchorPos(Vector2.zero, 0.2f)
                .From(fromValue: Vector2.up * 900);
        }

        public Tweener SlideOut() {
            return ((RectTransform)this.topUI.transform)
                .DOAnchorMax(Vector2.up * 900, 0.2f)
                .From(fromValue: Vector2.zero, setImmediately: false);
        }
    }
}
