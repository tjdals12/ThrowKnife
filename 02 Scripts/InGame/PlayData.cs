using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame {
    using StageDatabase = StageDatabase.DatabaseSO;
    using StageData = StageDatabase.Data;

    public enum AdType {
        Banner = 0,
        Interstitial,
        Rewarded
    }

    public class PlayData : MonoBehaviour
    {
        [SerializeField]
        StageDatabase stageDatabase;
        [SerializeField]
        PlayerData playerData;

        int currentStageIndex;
        StageData currentStageData;
        int currentScore;

        /// <summary>
        /// 게임 시작 여부
        /// </summary>
        public bool isGameStart {
            get { return this.isSpawnTarget && this.isSpawnKnives; }
        }
        bool isSpawnTarget;
        bool isSpawnKnives;
        
        /// <summary>
        /// 타깃
        /// </summary>
        /// 
        [HideInInspector]
        public GameObject target;

        /// <summary>
        /// 나이프 목록
        /// </summary>
        public Queue<GameObject> throwableKnives;

        public event Action<StageData> OnGameStart;
        public event Action<AdType> OnRequestLoadAd;
        /// <summary>
        /// 게임 시작
        /// </summary>
        public void GameStart() {
            this.playerData.ChangePlayCount(this.playerData.playCount + 1);
            if (this.playerData.playCount == 4) {
                this.OnRequestLoadAd?.Invoke(AdType.Interstitial);
            } else if (this.playerData.playCount == 8) {
                this.OnRequestLoadAd?.Invoke(AdType.Rewarded);
                this.playerData.ResetPlayCount();
            } else {
                this.currentStageIndex = 0;
                this.currentStageData = this.stageDatabase.GetByIndex(this.currentStageIndex);
                this.currentScore = 0;
                this.isSpawnTarget = false;
                this.isSpawnKnives = false;
                this.OnGameStart?.Invoke(this.currentStageData);
            }
        }

        /// <summary>
        /// 타깃 생성 완료
        /// </summary>
        public void CompleteSpawnTarget(GameObject target) {
            this.isSpawnTarget = true;
            this.target = target;
        }

        /// <summary>
        /// 나이프 생성 완료
        /// </summary>
        public void CompleteSpawnKnives(List<GameObject> throwableKnives) {
            this.isSpawnKnives = true;
            this.throwableKnives = new(throwableKnives);
        }

        public Action<GameObject[], GameObject> OnThrowKnife;
        /// <summary>
        /// 나이프 던지기
        /// </summary>
        public void ThrowKnife() {
            GameObject knife = this.throwableKnives.Dequeue();
            this.OnThrowKnife?.Invoke(this.throwableKnives.ToArray(), knife);
        }

        public event Action<int, StageData> OnChangeStage;
        /// <summary>
        /// 다음 스테이지
        /// </summary>
        public void NextStage() {
            if (++this.currentStageIndex % this.stageDatabase.GetCount() == 0) {
                this.currentStageIndex = this.stageDatabase.loopStage - 1;
            }
            this.currentStageData = this.stageDatabase.GetByIndex(this.currentStageIndex % this.stageDatabase.GetCount());
            this.OnChangeStage?.Invoke(this.currentStageIndex, this.currentStageData);
        }

        public event Action OnGameClear;
        /// <summary>
        /// 게임 클리어
        /// </summary>
        public void GameClear() {
            this.isSpawnTarget = false;
            this.isSpawnKnives = false;
            this.OnGameClear?.Invoke();
        }

        public event Action<int, int> OnGameOver;
        /// <summary>
        /// 게임 종료
        /// </summary>
        public void GameOver() {
            this.isSpawnTarget = false;
            this.isSpawnKnives = false;
            this.playerData.ChangeBestStage(this.currentStageIndex + 1);
            this.playerData.ChangeBestScore(this.currentScore);
            this.OnGameOver?.Invoke(this.currentStageIndex, this.currentScore);
        }

        public event Action<int, int> OnChangeScore;
        /// <summary>
        /// 점수 획득
        /// </summary>
        /// <param name="value">얻은 점수</param>
        public void AddScore(int value) {
            int prevScore = this.currentScore;
            this.currentScore += value;
            this.OnChangeScore?.Invoke(prevScore, this.currentScore);
        }

        public event Action<GameObject[]> OnKnifeStuck;
        /// <summary>
        /// 나이프 던지기 성공
        /// </summary>
        public void StickKnife() {
            this.OnKnifeStuck?.Invoke(this.throwableKnives.ToArray());
            this.AddScore(1);
            if (this.throwableKnives.Count == 0) {
                this.GameClear();
            }
        }
    }
}