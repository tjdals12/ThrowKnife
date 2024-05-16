using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameOverUI {
    public class Mask : MonoBehaviour
    {
        [SerializeField]
        Image image;

        public Tweener FadeIn() {
            Color color = this.image.color;
            return this.image
                        .DOFade(color.a, 0.3f)
                        .From(0)
                        .OnStart(() => {
                            this.image.gameObject.SetActive(true);
                        });
        }

        public Tweener FadeOut() {
            Color color = this.image.color;
            return this.image
                        .DOFade(0, 0.3f)
                        .From(color.a)
                        .OnComplete(() => {
                            this.image.gameObject.SetActive(false);
                            this.image.color = color;
                        });
        }
    }
}
