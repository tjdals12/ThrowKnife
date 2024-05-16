using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace InGameUI {
    using PlayData = InGame.PlayData;

    public class KnivesUI : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        PlayData playData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject knivesUI;
        [SerializeField]
        GameObject knifePrefab;

        List<Knife> knives;

        void Awake() {
            for (int i = 0; i < this.knivesUI.transform.childCount; i++) {
                Destroy(this.knivesUI.transform.GetChild(i).gameObject);
            }
        }

        public void GameStart(int knifeCount) {
            this.SpawnKnives(knifeCount);
            this.FadeIn();
        }

        public void KnifeStuck(int remainKnivesCount) {
            int max = (this.knives.Count - remainKnivesCount) - 1;
            for (int i = 0; i <= max; i++) {
                this.knives[i].Disable();
            }
        }

        public void GameClear() {
            this.FadeOut()
                .OnComplete(() => {
                    for (int i = 0; i < this.knivesUI.transform.childCount; i++) {
                        Destroy(this.knivesUI.transform.GetChild(i).gameObject);
                    }
                });
        }

        public void ChangeStage(int knifeCount) {
            this.SpawnKnives(knifeCount);
            this.FadeIn();
        }

        public void GameOver() {
            for (int i = 0; i < this.knivesUI.transform.childCount; i++) {
                Destroy(this.knivesUI.transform.GetChild(i).gameObject);
            }
        }

        void SpawnKnives(int count) {
            this.knives = new();
            for (int i = 0; i < count; i++) {
                GameObject clone = Instantiate(this.knifePrefab, this.knivesUI.transform);
                Knife knife = clone.GetComponent<Knife>();
                this.knives.Add(knife);
            }
        }

        public Sequence FadeIn() {
            Sequence sequence = DOTween.Sequence();
            Image[] images = this.knivesUI.GetComponentsInChildren<Image>();
            foreach (var image in images) {
                sequence.Join(image.DOFade(image.color.a, 0.5f).From(0f, setImmediately: false));
            }
            TextMeshProUGUI[] texts = this.knivesUI.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in texts) {
                sequence.Join(text.DOFade(text.color.a, 0.5f).From(0f, setImmediately: false));
            }
            return sequence;
        }

        public Sequence FadeOut() {
            Sequence sequence = DOTween.Sequence();
            Image[] images = this.knivesUI.GetComponentsInChildren<Image>();
            foreach (var image in images) {
                sequence.Join(image.DOFade(0f, 0.5f).From(image.color.a, setImmediately: false));
            }
            TextMeshProUGUI[] texts = this.knivesUI.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in texts) {
                sequence.Join(text.DOFade(0f, 0.5f).From(text.color.a, setImmediately: false));
            }
            return sequence;
        }
    }
}
