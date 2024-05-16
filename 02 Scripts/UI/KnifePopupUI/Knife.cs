using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace KnifePopupUI {
    [Serializable]
    public class LightEffect {
        [SerializeField]
        Image clockWiseImage;
        [SerializeField]
        Image CounterClockWiseImage;

        public void Play(Vector3 value) {
            this.clockWiseImage.transform.Rotate(value);
            this.CounterClockWiseImage.transform.Rotate(value * -1);
        }
    }

    [Serializable]
    public class GlowEffect {
        [SerializeField]
        Image image;

        public void Play() {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(((RectTransform)this.image.transform).DOScale(0.5f, 1.5f))
                .Append(((RectTransform)this.image.transform).DOScale(1f, 1.5f))
                .SetLoops(-1);
        }
    }

    public class Knife : MonoBehaviour
    {
        [SerializeField]
        LightEffect lightEffect;
        [SerializeField]
        GlowEffect glowEffect;

        void Start()
        {
            this.glowEffect.Play();
        }

        void Update()
        {
            this.lightEffect.Play(Vector3.forward * 100f * Time.deltaTime);
        }
    }
}