using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace Target {
    using PlayData = InGame.PlayData;
    using PlayerData = InGame.PlayerData;
    using StageData = StageDatabase.Data;
    using SettingsData = Settings.Data;
    using TargetRotator = Target.Rotator;
    using TargetInstance = Target.Instance;
    using ItemInstance = Item.Instance;
    using StuckKnifeInstance = StuckKnife.Instance;

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

        [Header("Target")]
        [Space(4)]
        [SerializeField]
        TargetRotator rotator;

        [Header("Sound")]
        [Space(4)]
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip soundOfAppear;
        [SerializeField]
        AudioClip soundOfCollisionItem;

        [SerializeField]
        int totalStuckObjectCount = 20;

        #region Unity Method
        void Awake() {
            this.settingsData.OnToggleSFX += this.OnToggleSFX;
            this.playData.OnGameStart += this.OnGameStart;
            this.playData.OnGameClear += this.OnGameClear;
            this.playData.OnChangeStage += this.OnChangeStage;
            this.playData.OnGameOver += this.OnGameOver;
        }
        #endregion
    
        #region Event Listener
        void OnToggleSFX(bool value) {
            this.audioSource.mute = value == false;
        }
        void OnGameStart(StageData stageData) {
            TargetInstance targetInstance = this.SpawnTarget(stageData);
            this.audioSource.PlayOneShot(this.soundOfAppear);
            this.SpawnObjects(stageData, targetInstance);
            this.Rotate(stageData, targetInstance);
        }

        void OnGameClear() {
            this.rotator.Reset();
            StartCoroutine(this.CallAfterSeconds(seconds: 1f, this.playData.NextStage));
        }

        void OnChangeStage(int currentStageIndex, StageData stageData) {
            TargetInstance targetInstance = this.SpawnTarget(stageData);
            this.audioSource.PlayOneShot(this.soundOfAppear);
            if (stageData.isBoss == false) {
                this.SpawnObjects(stageData, targetInstance);
            }
            this.Rotate(stageData, targetInstance);
        }

        void OnGameOver(int currentStageIndex, int currentScore) {
            this.rotator.Reset();
        }
        #endregion

        IEnumerator CallAfterSeconds(float seconds, Action callback) {
            yield return new WaitForSeconds(seconds);
            callback.Invoke();
        }

        TargetInstance SpawnTarget(StageData stageData) {
            GameObject clone = Instantiate(stageData.targetPrefab, this.rotator.transform);
            clone.name = "@Target";
            TargetInstance instance = clone.GetComponent<TargetInstance>();
            instance.Setup(this.audioSource);
            instance.Subscribe(this.playerData, this.playData);
            return instance;
        }

        void SpawnObjects(StageData stageData, TargetInstance targetInstance) {
            List<float> angles = new();
            for (int i = 0; i < this.totalStuckObjectCount; i++) {
                float angle = (360 / totalStuckObjectCount) * i;
                angles.Add(angle);
            }
            for (int i = 0; i < stageData.itemCount; i++) {
                int index = Random.Range(0, angles.Count);
                float angle = angles[index];
                angles.RemoveAt(index);

                GameObject clone = Instantiate(stageData.itemPrefab, targetInstance.transform);
                clone.name = $"@Item_{i + 1}";
                clone.transform.position = Utils.GetPositionFromAngle(angle, targetInstance.itemSpawnPoint) + this.rotator.transform.position;
                clone.transform.rotation = Quaternion.Euler(0, 0, angle);

                ItemInstance instance = clone.GetComponent<ItemInstance>();
                instance.Subscribe(this.playData);
                instance.OnCollision += () => {
                    this.audioSource.PlayOneShot(this.soundOfCollisionItem);
                    this.playerData.EarnPoint(1);
                };
            }
            for (int i = 0; i < stageData.stuckKnifeCount; i++) {
                int index = Random.Range(0, angles.Count);
                float angle = angles[index];
                angles.RemoveAt(index);

                GameObject clone = Instantiate(stageData.stuckKnifePrefab, targetInstance.transform);
                clone.name = $"@StuckKnife_{i + 1}";
                clone.transform.position = Utils.GetPositionFromAngle(angle, targetInstance.knifeStuckPoint) + this.rotator.transform.position;
                clone.transform.rotation = Quaternion.Euler(0, 0, angle);

                StuckKnifeInstance instance = clone.GetComponent<StuckKnifeInstance>();
                instance.Subscribe(this.playData);
            }
        }

        void Rotate(StageData stageData, TargetInstance targetInstance) {
            RotatePatternTemplate template = targetInstance.GetRotatePatternTemplate();
            Vector3 originScale = this.rotator.transform.localScale;
            this.rotator.transform
                .DOScale(originScale, 0.3f)
                .From(fromValue: Vector3.zero)
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    this.rotator.Setup(stageData.rotateSpeed, template);
                    this.playData.CompleteSpawnTarget(targetInstance.gameObject);
                });
        }
    }
}