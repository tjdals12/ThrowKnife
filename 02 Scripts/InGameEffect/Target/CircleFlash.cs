using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InGameEffect {
    public class CircleFlash : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer spriteRenderer;

        public void Play() {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(this.spriteRenderer.DOFade(0.5f, 0.05f).From(0, setImmediately: false))
                .Append(this.spriteRenderer.DOFade(0, 0.05f))
                .OnComplete(() => {
                    Destroy(this.gameObject, 0.5f);
                });
        }
    }
}