using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace LobbyUI {
    public class BottomUI : MonoBehaviour
    {
        [SerializeField]
        GameObject bottomUI;
        [SerializeField]
        Button playButton;
        [SerializeField]
        Button inventoryButton;

        public event Action OnClickPlay;
        public event Action OnClickInventory;

        #region Unity Method
        void Awake() {
            this.playButton.onClick.AddListener(() => {
                this.OnClickPlay?.Invoke();
            });
            this.inventoryButton.onClick.AddListener(() => {
                this.OnClickInventory?.Invoke();
            });
        }
        #endregion

        public Tweener SlideIn() {
            return ((RectTransform)this.bottomUI.transform)
                .DOAnchorPos(Vector2.zero, 0.2f)
                .From(fromValue: Vector2.down * 700f, setImmediately: false);
        }

        public Tweener SlideOut() {
            RectTransform rectTransform = this.bottomUI.GetComponent<RectTransform>();
            return rectTransform
                .DOAnchorPos(Vector2.down * 700f, 0.2f)
                .From(fromValue: Vector2.zero, setImmediately: false)
                .OnComplete(() => {
                    rectTransform.anchoredPosition = new Vector2(0, 200);
                });
        }
    }
}