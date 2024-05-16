using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace InGameUI {
    public abstract class IDot: MonoBehaviour {
        public abstract void Enable();
        public abstract void Disable();
    }

    public class StageDots : MonoBehaviour
    {
        [Header("UI")]
        [Space(4)]
        [SerializeField]
        List<IDot> dots;

        public void GameStart() {
            for (int i = 0; i < this.dots.Count; i++) {
                if (i == 0) {
                    this.dots[i].Enable();
                } else {
                    this.dots[i].Disable();
                }
            }
            this.FadeIn();
        }

        public void ChangeStage(int stageIndex) {
            int dotIndex = stageIndex % this.dots.Count;
            bool isBoss = dotIndex == 4;
            if (isBoss) {
                for (int i = 0; i < this.dots.Count; i++) {
                    if (i == dotIndex) {
                        this.dots[i].Enable();
                    } else {
                        this.FadeOutDot(this.dots[i]);
                    }
                }
            } else {
                for (int i = 0; i < this.dots.Count; i++) {
                    if (this.dots[i].gameObject.activeSelf == false) {
                        this.FadeInDot(this.dots[i]);
                    }
                    if (i == dotIndex) {
                        this.dots[i].Enable();
                    } else if (i > dotIndex) {
                        this.dots[i].Disable();
                    }
                }
            }
        }

        Tweener FadeInDot(IDot dot) {
            Image image = dot.GetComponentInChildren<Image>();
            return image.DOFade(image.color.a, 0.3f)
                        .From(0, setImmediately: false)
                        .OnStart(() => {
                            dot.gameObject.SetActive(true);
                        });
        }

        public Sequence FadeIn() {
            Sequence sequence = DOTween.Sequence();
            foreach (var dot in this.dots) {
                sequence.Join(this.FadeInDot(dot));
            }
            return sequence;
        }

        Tweener FadeOutDot(IDot dot) {
            Image image = dot.GetComponentInChildren<Image>();
            Color color = image.color;
            return image.DOFade(0, 0.3f)
                        .From(color, setImmediately: false)
                        .OnComplete(() => {
                            image.color = color;
                            dot.gameObject.SetActive(false);
                        });
        }

        public Sequence FadeOut() {
            Sequence sequence = DOTween.Sequence();
            foreach (var dot in this.dots) {
                sequence.Join(this.FadeOutDot(dot));
            }
            return sequence;
        }
    }
}
