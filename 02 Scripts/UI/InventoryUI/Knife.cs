using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryUI {
    public class Knife : MonoBehaviour
    {
        [SerializeField]
        Image border;
        [SerializeField]
        Image blur;
        [SerializeField]
        Image knife;
        [SerializeField]
        Image mask;
        [SerializeField]
        Button button;

        public event Action OnClick;

        #region Unity Method
        void Awake() {
            this.button.onClick.AddListener(() => {
                this.OnClick?.Invoke();
            });
        }
        #endregion

        public void Setup(Sprite knife) {
            this.knife.sprite = knife;
            this.Lock();
        }

        public void Lock() {
            this.border.gameObject.SetActive(false);
            this.blur.gameObject.SetActive(true);
            this.mask.gameObject.SetActive(true);
        }

        public void Unlock() {
            this.blur.gameObject.SetActive(false);
            this.mask.gameObject.SetActive(false);
        }

        public void Select() {
            this.border.gameObject.SetActive(true);
        }

        public void Deselect() {
            this.border.gameObject.SetActive(false);
        }
    }
}
