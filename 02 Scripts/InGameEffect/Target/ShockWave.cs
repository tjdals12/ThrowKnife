using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InGameEffect {
    public class ShockWave : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer glow;
        [SerializeField]
        SpriteRenderer ring;
    
        public void Play() {
            DOTween.Sequence()
                .Join(this.glow.transform.DOScale(2.5f, 0.4f).SetEase(Ease.InOutBack).From(fromValue: 1.5f, setImmediately: false))
                .Join(this.glow.DOFade(0, 0.3f).From(0.5f).SetDelay(0.1f));
            DOTween.Sequence()
                .Join(this.ring.transform.DOScale(5f, 0.4f).SetEase(Ease.InOutBack).From(fromValue: 1.3f, setImmediately: false))
                .Join(this.ring.DOFade(0, 0.3f).From(0.5f).SetDelay(0.1f));
            Destroy(this.gameObject, 1);
        }
    }
}
