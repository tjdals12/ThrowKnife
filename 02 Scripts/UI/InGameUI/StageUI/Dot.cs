using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace InGameUI {
    public class Dot : IDot
    {
        [SerializeField]
        Image enable;
        [SerializeField]
        Image disable;

        Sequence sequence;

        #region Unity Method
        void Awake() {
            this.sequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(this.enable.transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBack))
                .Append(this.enable.transform.DOScale(1, 0.1f))
                .Pause();
        }
        #endregion

        public override void Enable() {
            this.enable.gameObject.SetActive(true);
            this.sequence.Restart();
            this.disable.gameObject.SetActive(false);
        }

        public override void Disable() {
            this.disable.gameObject.SetActive(true);
            this.enable.gameObject.SetActive(false);
        }
    }
}
