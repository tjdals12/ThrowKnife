using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace Target {
    using PlayData = InGame.PlayData;
    using PlayerData = InGame.PlayerData;
    using CircleFlash = InGameEffect.CircleFlash;
    using WoodCrumb = InGameEffect.WoodCrumb;
    using ShockWave = InGameEffect.ShockWave;
    using TargetSlice = InGameEffect.TargetSlice;

    public class Instance : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        List<RotatePatternTemplateType> rotatePatternsPool;

        [Header("Effect")]
        [Space(4)]
        [SerializeField]
        GameObject flashEffect;
        [SerializeField]
        float scaleOfFlashEffect;
        [SerializeField]
        GameObject woodCrumbEffect;
        [SerializeField]
        GameObject shockWaveEffect;
        [SerializeField]
        GameObject sliceEffect;

        [Header("Sound")]
        [Space(4)]
        [SerializeField]
        AudioClip soundOfBroke;
        [SerializeField]
        AudioClip soundOfStuckKnife;
        [SerializeField]
        AudioClip soundOfCollisionItem;

        [Header("Point")]
        [Space(4)]
        [SerializeField]
        float _knifeStuckPoint;
        public float knifeStuckPoint {
            get { return this._knifeStuckPoint; }
            private set { this._knifeStuckPoint = value; }
        }
        [SerializeField]
        float _itemSpawnPoint;
        public float itemSpawnPoint {
            get { return this._itemSpawnPoint; }
            private set { this._itemSpawnPoint = value; }
        }
        [SerializeField]
        float _knifeCollisionPoint;
        public float knifeCollisionPoint {
            get { return this._knifeCollisionPoint; }
            private set { this._knifeCollisionPoint = value; }
        }

        [Header("Item")]
        [Space(4)]
        [SerializeField]
        Item.Instance[] items;

        LayerMask throwableKnifeLayer;
        Sequence bounce;
        PlayData playData;
        AudioSource audioSource;

        #region Unity Method
        void Awake() {
            this.throwableKnifeLayer = LayerMask.NameToLayer("ThrowableKnife");

            Vector3 originPosition = this.transform.position;
            Vector3 movePosition = this.transform.position + (Vector3.up * 0.15f);
            this.bounce = DOTween
                .Sequence()
                .SetAutoKill(false)
                .Append(this.transform.DOMove(movePosition, 0.03f).From(fromValue: originPosition, setImmediately: false))
                .Append(this.transform.DOMove(originPosition, 0.02f))
                .Pause();

            LayerMask layer = LayerMask.NameToLayer("Target");
            this.gameObject.layer = layer;
            for (int i = 0; i < this.transform.childCount; i++) {
                this.transform.GetChild(i).gameObject.layer = layer;
            }
        }

        void OnDestroy() {
            this.bounce.Kill();
            this.Unsubscribe();
        }
        #endregion

        public void Setup(AudioSource audioSource) {
            this.audioSource = audioSource;
        }

        public RotatePatternTemplate GetRotatePatternTemplate() {
            RotatePatternTemplateType type = this.rotatePatternsPool[Random.Range(0, this.rotatePatternsPool.Count)];
            switch (type) {
                case RotatePatternTemplateType.LeftKeep:
                    return new RotatePatternTemplate(
                        SpeedPattern.Keep,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Keep, Direction.Left, 5f)
                        }
                    );
                case RotatePatternTemplateType.RightKeep:
                    return new RotatePatternTemplate(
                        SpeedPattern.Keep,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Keep, Direction.Right, 5f)
                        }
                    );
                case RotatePatternTemplateType.LeftSlowFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Keep,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Slow, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Left, 10f),
                        }
                    );
                case RotatePatternTemplateType.RightSlowFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Keep,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Slow, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Right, 10f),
                        }
                    );
                case RotatePatternTemplateType.LeftKeepFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Keep,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Keep, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Left, 5f),
                        }
                    );
                case RotatePatternTemplateType.RightKeepFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Keep,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Keep, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Right, 5f),
                        }
                    );
                case RotatePatternTemplateType.LeftStopKeep:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Keep, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f)
                        }
                    );
                case RotatePatternTemplateType.RightStopKeep:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Keep, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                        }
                    );
                case RotatePatternTemplateType.LeftStopKeepAndRightStopKeep:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Keep, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Keep, Direction.Right, 10f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 10f),
                        }
                    );
                case RotatePatternTemplateType.RightStopKeepAndLeftStopKeep:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Keep, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Keep, Direction.Left, 10f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 10f),
                        }
                    );
                case RotatePatternTemplateType.LeftStopFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Fast, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Slow, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f),
                        }
                    );
                case RotatePatternTemplateType.RightStopFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Fast, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Slow, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                        }
                    );
                case RotatePatternTemplateType.LeftStopFastAndRightStopFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Fast, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f)
                        }
                    );
                case RotatePatternTemplateType.RightStopFastAndLeftStopFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Fast, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f)
                        }
                    );
                case RotatePatternTemplateType.LeftStopSlowStopFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Slow, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f),
                        }
                    );
                case RotatePatternTemplateType.RightStopSlowStopFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Slow, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                        }
                    );
                case RotatePatternTemplateType.LeftStopSlowStopFastRightStopSlowStopFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Slow, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Left, 8f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Slow, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Right, 8f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                        }
                    );
                case RotatePatternTemplateType.RightStopSlowStopFastLeftStopSlowStopFast:
                    return new RotatePatternTemplate(
                        SpeedPattern.Stop,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Slow, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Right, 8f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Right, 5f),
                            new RotatePattern(SpeedPattern.Slow, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f),
                            new RotatePattern(SpeedPattern.Fast, Direction.Left, 8f),
                            new RotatePattern(SpeedPattern.Stop, Direction.Left, 5f),
                        }
                    );
                default:
                    return new RotatePatternTemplate(
                        SpeedPattern.Keep,
                        new List<RotatePattern>() {
                            new RotatePattern(SpeedPattern.Keep, Direction.Left, 3f)
                        }
                    );
            }
        }

        #region Event Listener
        public void Subscribe(PlayerData playerData, PlayData playData) {
            foreach (var item in this.items) {
                item.Subscribe(playData);
                item.OnCollision += () => {
                    this.audioSource.PlayOneShot(this.soundOfCollisionItem);
                    playerData.EarnPoint(1);
                };
            }
            playData.OnKnifeStuck += this.OnKnifeStuck;
            playData.OnGameClear += this.OnGameClear;
            playData.OnGameOver += this.OnGameOver;
            this.playData = playData;
        }

        void Unsubscribe() {
            if (this.playData == null) return;
            playData.OnKnifeStuck -= this.OnKnifeStuck;
            playData.OnGameClear -= this.OnGameClear;
            playData.OnGameOver -= this.OnGameOver;
        }

        void OnKnifeStuck(GameObject[] knives) {
            this.bounce.Restart();
            this.HandleFlash();
            this.HandleWoodCrumb();
            this.audioSource.PlayOneShot(this.soundOfStuckKnife);
        }

        void OnGameClear() {
            this.HandleShockWave();
            this.HandleSlice();
            this.audioSource.PlayOneShot(this.soundOfBroke);
            Destroy(this.gameObject, 0.1f);
        }

        void OnGameOver(int currentStageIndex, int currentScore) {
            StartCoroutine(this.CallAfterSeconds(seconds: 1, this.FadeOut));
            Destroy(this.gameObject, 1.5f);
        }
        #endregion

        IEnumerator CallAfterSeconds(float seconds, Action callback) {
            yield return new WaitForSeconds(seconds);
            callback.Invoke();
        }

        void HandleFlash() {
            GameObject clone = Instantiate(this.flashEffect, this.transform);
            clone.transform.localScale = Vector3.one * this.scaleOfFlashEffect;
            CircleFlash flash = clone.GetComponent<CircleFlash>();
            flash.Play();
        }

        void HandleWoodCrumb() {
            Vector3 position = Utils.GetPositionFromAngle(270, this.itemSpawnPoint) + this.transform.position;
            GameObject clone = Instantiate(this.woodCrumbEffect, position, Quaternion.identity);
            WoodCrumb woodCrumb = clone.GetComponent<WoodCrumb>();
            woodCrumb.Play();
        }

        void HandleShockWave() {
            GameObject clone = Instantiate(this.shockWaveEffect, this.transform.position, Quaternion.identity);
            ShockWave shockWave = clone.GetComponent<ShockWave>();
            shockWave.Play();
        }

        void HandleSlice() {
            GameObject clone = Instantiate(this.sliceEffect, this.transform.position, Quaternion.identity);
            TargetSlice targetSlice = clone.GetComponent<TargetSlice>();
            targetSlice.Play();
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
