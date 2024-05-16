using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InGameUI {
    public class StageUI : MonoBehaviour
    {
        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject stageUI;
        [SerializeField]
        StageDots stageDots;
        [SerializeField]
        StageTitle stageTitle;

        public void GameStart() {
            this.stageDots.GameStart();
            this.stageTitle.GameStart();
        }

        public void ChangeStage(int stageIndex) {
            this.stageDots.ChangeStage(stageIndex);
            this.stageTitle.ChangeStage(stageIndex);
        }

        public void GameClear() {
            this.stageTitle.GameClear();
        }

        public Sequence FadeIn() {
            return DOTween.Sequence()
                    .Join(this.stageTitle.FadeIn())
                    .Join(this.stageDots.FadeIn());
        }

        public Sequence FadeOut() {
            return DOTween.Sequence()
                    .Join(this.stageTitle.FadeOut())
                    .Join(this.stageDots.FadeOut());
        }
    }
}