using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace InGameUI {
    public class ScoreUI : MonoBehaviour
    {
        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject scoreUI;
        [SerializeField]
        TextMeshProUGUI score;

        public void GameStart() {
            this.score.text = "0";
            this.FadeIn();
        }

        public void ChangeScore(int score) {
            this.score.text = score.ToString();
        }

        public Sequence FadeIn() {
            Sequence sequence = DOTween.Sequence();
            Image[] images = this.scoreUI.GetComponentsInChildren<Image>();
            foreach (var image in images) {
                sequence.Join(image.DOFade(image.color.a, 0.3f).From(0f, setImmediately: false));
            }
            TextMeshProUGUI[] texts = this.scoreUI.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in texts) {
                sequence.Join(text.DOFade(text.color.a, 0.3f).From(0f, setImmediately: false));
            }
            sequence.OnStart(() => {
                this.scoreUI.SetActive(true);
            });
            return sequence;
        }

        public Sequence FadeOut() {
            Sequence sequence = DOTween.Sequence();
            Image[] images = this.scoreUI.GetComponentsInChildren<Image>();
            List<Color> imageColors = new();
            foreach (var image in images) {
                imageColors.Add(image.color);
                sequence.Join(image.DOFade(0f, 0.3f).From(image.color.a, setImmediately: false));
            }
            TextMeshProUGUI[] texts = this.scoreUI.GetComponentsInChildren<TextMeshProUGUI>();
            List<Color> textColors = new();
            foreach (var text in texts) {
                textColors.Add(text.color);
                sequence.Join(text.DOFade(0f, 0.3f).From(text.color.a, setImmediately: false));
            }
            sequence.OnComplete(() => {
                for (int i = 0; i < images.Length; i++) {
                    images[i].color = imageColors[i];
                }
                for (int i = 0; i < texts.Length; i++) {
                    texts[i].color = textColors[i];
                }
                this.scoreUI.SetActive(false);
            });
            return sequence;
        }
    }
}