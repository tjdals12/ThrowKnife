using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace StuckKnife {
    using PlayData = InGame.PlayData;

    public class Instance : MonoBehaviour
    {
        Rigidbody2D rigid;
        PlayData playData;

        #region Unity Method
        void Awake() {
            this.rigid = this.GetComponent<Rigidbody2D>();

            LayerMask layer = LayerMask.NameToLayer("StuckKnife");
            this.gameObject.layer = layer;
            Collider2D[] colliders = this.GetComponentsInChildren<Collider2D>();
            foreach (var collider in colliders) {
                collider.gameObject.layer = layer;
            }
        }
        void OnDestroy() {
            this.Unsubscribe();
        }
        #endregion

        #region Event Listener
        public void Subscribe(PlayData playData) {
            playData.OnGameClear += this.OnGameClear;
            this.playData = playData;
        }
        void Unsubscribe() {
            if (this.playData == null) return;
            this.playData.OnGameClear -= this.OnGameClear;
        }
        void OnGameClear() {
            StartCoroutine(this.CallAfterSeconds(seconds: 0.05f, this.Fly));
            Destroy(this.gameObject, 1f);
        }
        void OnGameOver() {
            StartCoroutine(this.CallAfterSeconds(seconds: 1, this.FadeOut));
        }
        #endregion

        IEnumerator CallAfterSeconds(float seconds, Action callback) {
            yield return new WaitForSeconds(seconds);
            callback.Invoke();
        }

        void Fly() {
            Collider2D[] colliders = this.GetComponentsInChildren<Collider2D>();
            foreach (var collider in colliders) {
                collider.enabled = false;
            }
            this.transform.SetParent(null);
            this.rigid.bodyType = RigidbodyType2D.Dynamic;
            Vector2 direction = this.transform.position.normalized;
            float power = Random.Range(5f, 10f);
            this.rigid.AddForce(this.transform.rotation * (direction * 10f), ForceMode2D.Impulse);
            float torque = Random.Range(0, 2) == 0 ? 100f : -100f;
            this.rigid.AddTorque(torque);
        }

        void FadeOut() {
            SpriteRenderer[] spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
            List<Color> colors = new();
            Sequence sequence = DOTween.Sequence();
            foreach (var spriteRenderer in spriteRenderers) {
                colors.Add(spriteRenderer.color);
                sequence.Join(spriteRenderer.DOFade(0, 0.3f).From(spriteRenderer.color, setImmediately: false));
            }
            sequence.OnComplete(() => {
                for (int i = 0; i < spriteRenderers.Length; i++) {
                    SpriteRenderer spriteRenderer = spriteRenderers[i];
                    spriteRenderer.color = colors[i];
                    spriteRenderer.gameObject.SetActive(false);
                }
            });
        }
    }
}