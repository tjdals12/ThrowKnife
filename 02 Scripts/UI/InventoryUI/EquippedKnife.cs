using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace InventoryUI {
    public class EquippedKnife : MonoBehaviour
    {
        [SerializeField]
        Image image;
        [SerializeField]
        Image mask;

        public void Change(Sprite image) {
            this.mask.gameObject.SetActive(false);
            this.image.sprite = image;
        }

        public void Select(Sprite image) {
            this.mask.gameObject.SetActive(true);
            this.image.sprite = image;
        }

        public Sequence ScaleIn() {
            Sequence sequence = DOTween.Sequence();
            RectTransform[] rectTransforms = this.GetComponentsInChildren<RectTransform>();
            foreach (var rectTransform in rectTransforms) {
                sequence.Join(rectTransform.DOScale(rectTransform.localScale, 0.3f).From(Vector3.zero).SetEase(Ease.OutExpo));
            }
            return sequence;
        }

        public Sequence ScaleOut() {
            Sequence sequence = DOTween.Sequence();
            RectTransform[] recttransforms = this.GetComponentsInChildren<RectTransform>();
            foreach (var rectTransform in recttransforms) {
                sequence.Join(rectTransform.DOScale(Vector3.zero, 0.3f).From(rectTransform.localScale).SetEase(Ease.OutExpo));
            }
            sequence.OnComplete(() => {
                Destroy(this.gameObject);
            });
            return sequence;
        }
    }
}
