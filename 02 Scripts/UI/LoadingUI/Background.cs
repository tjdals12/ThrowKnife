using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace LoadingUI {
    public class Background : MonoBehaviour
    {
        [SerializeField]
        Image image;

        public Tweener FadeIn() {
            return this.image
                        .DOFade(this.image.color.a, 0.3f)
                        .From(0);
        }

        public Tweener FadeOut() {
            Color color = this.image.color;
            return this.image
                        .DOFade(0, 0.3f)
                        .From(color.a)
                        .OnComplete(() => {
                            this.image.color = color;
                        });
        }
    }
}
