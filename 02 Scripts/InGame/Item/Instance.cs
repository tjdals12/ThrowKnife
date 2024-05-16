using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace Item {
    using PlayData = InGame.PlayData;
    using Cut = InGameEffect.Cut;
    using StarLight = InGameEffect.StarLight;
    using ItemSlice = InGameEffect.ItemSlice;

    public class Instance : MonoBehaviour
    {
        [Header("Effect")]
        [Space(4)]
        [SerializeField]
        GameObject cutEffect;
        [SerializeField]
        GameObject startLightEffect;
        [SerializeField]
        GameObject sliceEffect;

        Rigidbody2D rigid;

        LayerMask stuckKnifeLayer;
        bool isProcessing = false;
        PlayData playData;

        public event Action OnCollision;

        #region Unity Method
        void Awake() {
            this.rigid = this.GetComponent<Rigidbody2D>();
            this.stuckKnifeLayer = LayerMask.NameToLayer("StuckKnife");

            LayerMask layer = LayerMask.NameToLayer("Item");
            this.gameObject.layer = layer;
            Collider2D[] colliders = this.GetComponentsInChildren<Collider2D>();
            foreach (var collider in colliders) {
                collider.gameObject.layer = layer;
            }
        }

        void OnTriggerStay2D(Collider2D collider) {
            if (this.isProcessing) return;
            if (collider.gameObject.layer.Equals(stuckKnifeLayer)) {
                this.isProcessing = true;
                this.HandleSlice();
                this.HandleStarLight();
                this.HandleCut();
                this.OnCollision?.Invoke();
                Destroy(this.gameObject);
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
            playData.OnGameClear -= this.OnGameClear;
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

        void HandleSlice() {
            GameObject clone = Instantiate(this.sliceEffect, this.transform.position, Quaternion.identity);
            ItemSlice itemSlice = clone.GetComponent<ItemSlice>();
            itemSlice.Play();
        }

        void HandleStarLight() {
            GameObject clone = Instantiate(this.startLightEffect, this.transform.position, Quaternion.identity);
            StarLight starLight = clone.GetComponent<StarLight>();
            starLight.Play();
        }

        void HandleCut() {
            GameObject clone = Instantiate(this.cutEffect, this.transform.position, Quaternion.identity);
            Cut cut = clone.GetComponent<Cut>();
            cut.Play();
        }

        void Fly() {
            Collider2D[] colliders = this.GetComponentsInChildren<Collider2D>();
            foreach (var collider in colliders) {
                collider.enabled = false;
            }
            this.transform.SetParent(null);
            this.rigid.bodyType = RigidbodyType2D.Dynamic;
            Vector2 direction = this.transform.position.normalized * (Random.Range(0, 2) == 0 ? 1 : -1);
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
