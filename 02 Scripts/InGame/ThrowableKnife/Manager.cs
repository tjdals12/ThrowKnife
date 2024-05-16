using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ThrowableKnife {
    using PlayData = InGame.PlayData;
    using PlayerData = InGame.PlayerData;
    using StageData = StageDatabase.Data;
    using SettingsData = Settings.Data;
    using ThrowableKnifeInstance = Instance;
    using TargetRotator = Target.Rotator;
    using TargetInstance = Target.Instance;

    public class Manager : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        PlayData playData;
        [SerializeField]
        PlayerData playerData;
        [SerializeField]
        SettingsData settingsData;

        [Header("Object")]
        [Space(4)]
        [SerializeField]
        GameObject knives;
        [SerializeField]
        TargetRotator targetRotator;

        [Header("Sound")]
        [Space(4)]
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip soundOfThrowKnife;
        [SerializeField]
        AudioClip soundOfCollisionKnife;

        #region Unity Method
        void Awake() {
            this.settingsData.OnToggleSFX += this.OnToggleSFX;
            this.playData.OnGameStart += this.OnGameStart;
            this.playData.OnThrowKnife += this.OnThrowKnife;
            this.playData.OnChangeStage += this.OnChangeStage;
            this.playData.OnGameOver += this.OnGameOver;
        }

        void Update() {
            if (this.playData.isGameStart == false) return;
            if (Input.GetMouseButtonDown(0)) {
                this.playData.ThrowKnife();
            }
        }
        #endregion

        #region Event Listener
        void OnToggleSFX(bool value) {
            this.audioSource.mute = value == false;
        }
        void OnGameStart(StageData stageData) {
            List<GameObject> list = this.SpawnKnives(stageData);
            this.MoveDownFirstKnife(list);
        }

        void OnThrowKnife(GameObject[] remainKnives, GameObject throwedKnife) {
            this.audioSource.PlayOneShot(this.soundOfThrowKnife);
            this.MoveThrowedKnife(throwedKnife);
            this.MoveRemainKnives(remainKnives);
        }

        void OnChangeStage(int currentStageIndex, StageData stageData) {
            List<GameObject> list = this.SpawnKnives(stageData);
            this.MoveUpFirstKnife(list);
        }

        void OnGameOver(int currentStageIndex, int currentScore) {
            DOTween.Kill("MoveRemainKnives");
        }
        #endregion

        List<GameObject> SpawnKnives(StageData stageData) {
            List<GameObject> list = new();
            GameObject knifePrefab = this.playerData.knife.prefab;
            for (int i = 0; i < stageData.throwableKnifeCount; i++) {
                GameObject clone = Instantiate(knifePrefab, this.knives.transform);
                clone.name = $"@ThrowableKnife_{i}";
                Vector3 position = clone.transform.position + ((Vector3.down * 3f) * i);
                clone.transform.position = position;
                clone.transform.rotation = Quaternion.Euler(0, 0, 270);

                ThrowableKnifeInstance instance = clone.GetComponent<ThrowableKnifeInstance>();
                instance.OnCollision += () => {
                    this.audioSource.PlayOneShot(this.soundOfCollisionKnife);
                    this.playData.GameOver();
                };
                instance.OnStuck += () => {
                    instance.transform.SetParent(this.playData.target.transform);
                    this.playData.StickKnife();
                };
                instance.Subscribe(this.playData);

                list.Add(clone);
            }
            return list;
        }

        void MoveDownFirstKnife(List<GameObject> list) {
            GameObject firstKnife = list[0];
            firstKnife.transform
                .DOMove(firstKnife.transform.position, 0.3f)
                .From(fromValue: Vector3.zero, setImmediately: false)
                .OnComplete(() => {
                    this.playData.CompleteSpawnKnives(list);
                });
        }

        void MoveUpFirstKnife(List<GameObject> list) {
            GameObject firstKnife = list[0];
            Sequence sequence = DOTween.Sequence();
            sequence.Join(
                firstKnife.transform
                    .DOMove(firstKnife.transform.position, 0.3f)
                    .From(fromValue: firstKnife.transform.position + (Vector3.down * 3f), setImmediately: false)
            );
            foreach (var spriteRenderer in firstKnife.GetComponentsInChildren<SpriteRenderer>()) {
                sequence.Join(spriteRenderer.DOFade(1, 0.3f).From(0));
            }
            sequence.OnComplete(() => {
                this.playData.CompleteSpawnKnives(list);
            });
        }

        void MoveThrowedKnife(GameObject throwedKnife) {
            TargetInstance targetInstance = this.playData.target.GetComponent<TargetInstance>();
            Vector3 middlePosition = Utils.GetPositionFromAngle(270, targetInstance.knifeCollisionPoint) + this.targetRotator.transform.position;
            Vector3 endPosition = Utils.GetPositionFromAngle(270, targetInstance.knifeStuckPoint) + this.targetRotator.transform.position;

            DOTween.Complete("MoveRemainKnives");
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(throwedKnife.transform.DOMove(middlePosition, 0.03f).SetEase(Ease.InCirc))
                .Append(throwedKnife.transform.DOMove(endPosition, 0).SetDelay(0.03f))
                .OnComplete(() => {
                    throwedKnife.transform.position = endPosition;
                });
        }

        void MoveRemainKnives(GameObject[] remainKnives) {
            DOTween.Complete("MoveRemainKnives");
            Sequence sequence = DOTween.Sequence().SetId("MoveRemainKnives");
            for (int i = 0; i < remainKnives.Length; i++) {
                GameObject remainKnife = remainKnives[i];
                sequence.Join(remainKnife.transform.DOMove(remainKnife.transform.position + Vector3.up * 3f, 0.3f));
                SpriteRenderer[] spriteRenderers = remainKnife.GetComponentsInChildren<SpriteRenderer>();
                foreach (var spriteRenderer in spriteRenderers) {
                    sequence.Join(spriteRenderer.DOFade(1, 0.3f).From(0));
                }
            }
        }
    }
}
