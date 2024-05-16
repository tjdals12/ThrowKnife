using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace InGameUI {
    public class StageTitle : MonoBehaviour
    {
        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject title;
        [SerializeField]
        TextMeshProUGUI normal;
        [SerializeField]
        TextMeshProUGUI boss;

        public void GameStart() {
            this.normal.text = "STAGE 1";
            this.normal.gameObject.SetActive(true);
            this.boss.gameObject.SetActive(false);
            this.FadeIn();
        }

        public void ChangeStage(int stageIndex) {
            bool isBoss = (stageIndex % 5) == 4;
            if (isBoss) {
                this.boss.gameObject.SetActive(true);
                this.normal.gameObject.SetActive(false);
            } else {
                this.normal.text = $"STAGE {stageIndex + 1}";
                this.normal.gameObject.SetActive(true);
                this.boss.gameObject.SetActive(false);
            }
            this.FadeIn();
        }

        public void GameClear() {
            this.FadeOut();
        }

        public Tweener FadeIn() {
            TextMeshProUGUI text = this.title.GetComponentInChildren<TextMeshProUGUI>();
            return text.DOFade(text.color.a, 0.3f)
                        .From(0, setImmediately: false)
                        .OnStart(() => {
                            title.gameObject.SetActive(true);
                        });
        }

        public Tweener FadeOut() {
            TextMeshProUGUI text = this.title.GetComponentInChildren<TextMeshProUGUI>();
            Color color = text.color;
            return text.DOFade(0, 0.3f)
                        .From(text.color.a, setImmediately: false)
                        .OnComplete(() => {
                            title.gameObject.SetActive(false);
                            text.color = color;
                        });
        }
    }
}
