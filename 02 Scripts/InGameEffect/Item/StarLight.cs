using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InGameEffect {
    public class StarLight : MonoBehaviour
    {
        #region Unity Method
        void Awake() {
            this.transform.localScale = Vector3.zero;
            this.Play();
        }
        #endregion

        public void Play() {
            this.transform.DORotate(Vector3.forward * 180, 0.3f).From(Vector3.zero);
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(this.transform.DOScale(1.5f, 0.1f).From(fromValue: 0))
                .Append(this.transform.DOScale(0, 0.1f))
                .OnComplete(() => {
                    Destroy(this.gameObject, 0.5f);
                });
        }
    }
}
