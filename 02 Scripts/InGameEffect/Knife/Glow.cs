using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InGameEffect {
    public class Glow : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer spriteRenderer;

        public void Play() {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(this.transform.DOScale(Vector3.one , 0.05f).From(fromValue: Vector3.zero, setImmediately: false))
                .Join(this.spriteRenderer.DOFade(0.5f, 0.05f).From(1, setImmediately: false))
                .Append(this.transform.DOScale(Vector3.one * 2.5f, 0.05f))
                .Join(this.spriteRenderer.DOFade(0, 0.05f))
                .OnComplete(() => {
                    Destroy(this.gameObject, 0.5f);
                });
        }
    }
}
