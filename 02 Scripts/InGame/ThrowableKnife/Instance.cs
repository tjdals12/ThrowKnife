using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace ThrowableKnife {
    using PlayData = InGame.PlayData;
    using Glow = InGameEffect.Glow;
    using ScreenFlash = InGameEffect.ScreenFlash;
    public class Instance : MonoBehaviour
    {
        [Header("Effect")]
        [Space(4)]
        [SerializeField]
        GameObject glowPrefab;
        [SerializeField]
        GameObject screenFlashPrefab;

        Rigidbody2D rigid;

        LayerMask stuckKnifeLayer;
        LayerMask targetLayer;

        bool isProcessing = false;
        PlayData playData;

        public event Action OnCollision;
        public event Action OnStuck;

        #region Unity Method
        void Awake() {
            this.rigid = this.GetComponent<Rigidbody2D>();

            this.stuckKnifeLayer = LayerMask.NameToLayer("StuckKnife");
            this.targetLayer = LayerMask.NameToLayer("Target");

            LayerMask layer = LayerMask.NameToLayer("ThrowableKnife");
            this.ChangeLayer(layer);
        }

        void OnTriggerEnter2D(Collider2D collider) {
            if (this.isProcessing) return;
            if (collider.gameObject.layer.Equals(this.stuckKnifeLayer)) {
                this.isProcessing = true;
                this.HandleCollision(collider);
                this.OnCollision?.Invoke();
            } else if (collider.gameObject.layer.Equals(this.targetLayer)) {
                this.isProcessing = true;
                this.HandleStuck();
                this.OnStuck?.Invoke();
            }
        }

        void OnDestroy() {
            this.Unsubscribe();
        }
        #endregion

        #region Event Listener
        public void Subscribe(PlayData playData) {
            playData.OnGameClear += this.OnGameClear;
            playData.OnGameOver += this.OnGameOver;
            this.playData = playData;
        }
        void Unsubscribe() {
            if (this.playData == null) return;
            this.playData.OnGameClear -= this.OnGameClear;
            this.playData.OnGameOver -= this.OnGameOver;
        }
        void OnGameClear() {
            StartCoroutine(this.CallAfterSeconds(seconds: 0.05f, this.Fly));
            Destroy(this.gameObject, 1f);
        }

        void OnGameOver(int currentStageIndex, int currentScore) {
            StartCoroutine(this.CallAfterSeconds(seconds: 1, this.FadeOut));
        }
        #endregion

        IEnumerator CallAfterSeconds(float seconds, Action callback) {
            yield return new WaitForSeconds(seconds); 
            callback.Invoke();
        }

        void ChangeLayer(LayerMask layer) {
            this.gameObject.layer = layer;
            Collider2D[] colliders = this.GetComponentsInChildren<Collider2D>();
            foreach (var collider in colliders) {
                collider.gameObject.layer = layer;
            }
        }

        void HandleCollision(Collider2D collider) {
            this.DoBounce(collider);
            this.DoGlow(collider);
            this.DoScreenFlash(collider);
            Destroy(this.gameObject, 1f);
        }

        void DoBounce(Collider2D collider) {
            Vector2 contactNormal = (collider.transform.position - this.transform.position).normalized;
            contactNormal.y = 1;
            Vector2 reflectedForce = Vector2.Reflect(Vector2.up * 20f, contactNormal);
            this.rigid.velocity = reflectedForce;
            this.rigid.angularVelocity = 1000;
        }

        void DoGlow(Collider2D collider) {
            Vector2 contactNormal = (collider.transform.position - this.transform.position).normalized;
            contactNormal.y = 0.25f;
            GameObject clone = Instantiate(this.glowPrefab, contactNormal, Quaternion.identity);
            Glow glow = clone.GetComponent<Glow>();
            glow.Play();
        }

        void DoScreenFlash(Collider2D collider) {
            GameObject clone = Instantiate(this.screenFlashPrefab);
            ScreenFlash screenFlash = clone.GetComponent<ScreenFlash>();
            screenFlash.Play();
        }

        void HandleStuck() {
            this.ChangeLayer(this.stuckKnifeLayer);
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
                Destroy(this.gameObject, 0.1f);
            });
        }
    }
}