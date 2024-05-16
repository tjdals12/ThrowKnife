using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InGameEffect {
    public class Cut : MonoBehaviour
    {
        SpriteRenderer[] spriteRenderers;

        #region Unity Method
        void Awake() {
            this.spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            this.transform.localScale = Vector3.zero;
        }
        #endregion

        public void Play() {
            Sequence sequence = DOTween.Sequence();
            sequence
                .OnComplete(() => {
                    Destroy(this.gameObject, 0.5f);
                })
                .Append(this.transform.DOScale(1.8f, 0.1f).From(fromValue: 0));
            foreach (var spriteRenderer in this.spriteRenderers) {
                sequence.Join(spriteRenderer.DOFade(0.6f, 0.1f).From(1));
            }
            sequence.Append(this.transform.DOScale(0, 0.1f));
        }
    }
}