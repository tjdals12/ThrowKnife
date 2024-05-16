using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace InGameUI {
    public class BossDot : IDot
    {
        [SerializeField]
        GameObject enable;
        [SerializeField]
        GameObject disable;

        Vector2 enablePosition;
        Vector2 disablePosition;
        Sequence sequence;

        #region Unity Method
        void Awake() {
            RectTransform rectTransform = this.GetComponent<RectTransform>();
            this.enablePosition = Vector2.zero;
            this.disablePosition = rectTransform.anchoredPosition;
            this.sequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(this.enable.transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBack))
                .Append(this.enable.transform.DOScale(1, 0.1f))
                .Append(rectTransform.DOAnchorPos(this.enablePosition, 0.5f))
                .Pause();
        }
        #endregion

        public override void Enable() {
            this.enable.SetActive(true);
            this.sequence.Restart();
            this.disable.SetActive(false);
        }

        public override void Disable() {
            RectTransform rectTransform = this.GetComponent<RectTransform>();
            this.disable.SetActive(true);   
            rectTransform.DOAnchorPos(this.disablePosition, 0.5f);
            this.enable.SetActive(false);
        }
    }
}
