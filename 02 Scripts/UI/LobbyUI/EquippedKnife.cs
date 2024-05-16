using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace LobbyUI {
    using PlayerData = InGame.PlayerData;
    using PlayData = InGame.PlayData;
    using StageData = StageDatabase.Data;
    using KnifeData = KnifeDatabase.Data;
    using UIData = UI.Data;
    using UIType = UI.UIType;
    using UIAnimationType = UI.UIAnimationType;

    public class EquippedKnife : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        UIData uIData;
        [SerializeField]
        PlayData playData;
        [SerializeField]
        PlayerData playerData;

        GameObject knife;

        #region Unity Method
        void Awake() {
            this.uIData.OnMove += this.OnMove;
            this.playerData.OnChangeKnife += this.OnChangeKnife;
            this.playData.OnGameStart += this.OnGameStart;
        }
        #endregion

        #region Event Listener
        void OnMove(UIType uiType, UIAnimationType uiAnimationType, Action callback) {
            if (uiType == UIType.Lobby) {
                switch (uiAnimationType) {
                    default:
                        this.Open(callback);
                        break;
                }
            } else {
                switch (uiAnimationType) {
                    default:
                        this.Close(null);
                        break;
                }
            }
        }

        void OnChangeKnife(KnifeData prev, KnifeData current) {
            if (this.knife != null) {
                DOTween.Kill("MoveEquippedKnife");
                Destroy(this.knife);
            }
            GameObject clone = Instantiate(current.prefab, this.transform);
            clone.name = "Knife";
            clone.transform.localScale = Vector3.one;
            clone.transform.rotation = Quaternion.Euler(0, 0, 270);
            Sequence sequence = DOTween.Sequence().SetId("MoveEquippedKnife");
            sequence.Join(clone.transform.DOMove(Vector3.zero, 0.2f).From(Vector3.down * 3.5f).SetEase(Ease.OutExpo));
            foreach (var spriteRender in clone.GetComponentsInChildren<SpriteRenderer>()) {
                sequence.Join(spriteRender.DOFade(1, 0.3f).From(0));
            }
            this.knife = clone;
        }

        void OnGameStart(StageData stageData) {
            this.knife.SetActive(false);
        }
        #endregion

        void Open(Action callback) {
            this.knife.SetActive(true);
            callback?.Invoke();
        }

        void Close(Action callback) {
            this.knife.SetActive(false);
            callback?.Invoke();
        }
    }
}