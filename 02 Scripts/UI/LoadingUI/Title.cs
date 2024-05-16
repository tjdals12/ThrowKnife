using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace LoadingUI {
    public class Title : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI title;

        #region Unity Method
        void Awake() {
            StartCoroutine(Loading());
        }
        #endregion

        IEnumerator Loading() {
            string[] texts = { "Loading.", "Loading..", "Loading..." };
            int index = 0;
            while(true) {
                yield return new WaitForSeconds(0.5f);
                this.title.text = texts[index];
                if (index >= (texts.Length - 1)) {
                    index = 0;
                } else {
                    index++;
                }
            }
        }

        public Tweener FadeIn() {
            return this.title.DOFade(this.title.color.a, 0.3f)
                            .From(0);
        } 

        public Tweener FadeOut() {
            Color color = this.title.color;
            return this.title.DOFade(0, 0.3f)
                            .From(color.a)
                            .OnComplete(() => {
                                this.title.color = color;
                            });
        }
    }
}
