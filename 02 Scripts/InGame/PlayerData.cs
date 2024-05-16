using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame {
    using PlayerDatabase = PlayerDatabase.DatabaseSO;
    using KnifeDatabase = KnifeDatabase.DatabaseSO;
    using KnifeData = KnifeDatabase.Data;

    public class PlayerData : MonoBehaviour
    {
        [SerializeField]
        PlayerDatabase playerDatabase;
        [SerializeField]
        KnifeDatabase knifeDatabase;

        /// <summary>
        /// 보유중인 나이프 목록
        /// </summary>
        public Dictionary<int, KnifeData> knives;
        /// <summary>
        /// 사용중인 나이프
        /// </summary>
        public KnifeData knife;
        /// <summary>
        /// 보유 포인트
        /// </summary>
        public int point { get; private set; }
        /// <summary>
        /// 베스트 스테이지
        /// </summary>
        public int bestStage { get; private set; }
        /// <summary>
        /// 베스트 스코어
        /// </summary>
        public int bestScore { get; private set; }
        /// <summary>
        /// 플레이 횟수
        /// </summary>
        public int playCount { get; private set; }

        #region Unity Method
        void Awake() {
            this.playerDatabase.Load();
        }

        void Start() {
            this.knives = new();
            foreach (var knifeID in this.playerDatabase.knives) {
                KnifeData knifeData = this.knifeDatabase.GetByID(knifeID);
                this.knives.Add(knifeData.ID, knifeData);
            }
            this.knife = this.knifeDatabase.GetByID(this.playerDatabase.knifeID);
            this.OnChangeKnife(null, this.knife);
            this.point = this.playerDatabase.point;
            this.OnChangePoint?.Invoke(0, this.point);
            this.bestStage = this.playerDatabase.bestStage;
            this.OnChangeBestStage?.Invoke(0, this.bestStage);
            this.bestScore = this.playerDatabase.bestScore;
            this.OnChangeBestScore?.Invoke(0, this.bestScore);
            this.playCount = this.playerDatabase.playCount;
            this.OnLoaded?.Invoke();
        }
        #endregion

        public event Action OnLoaded;


        public event Action<KnifeData, KnifeData> OnChangeKnife;
        /// <summary>
        /// 사용중인 나이프 변경
        /// </summary>
        /// <param name="knifeID">변경할 나이프 ID</param>
        public void ChangeKnife(int knifeID) {
            if (this.knives.ContainsKey(knifeID) == false) return;
            KnifeData prevKnife = this.knife;
            this.knife = this.knives[knifeID];
            this.playerDatabase.ChangeKnifeID(this.knife.ID);
            this.OnChangeKnife?.Invoke(prevKnife, this.knife);
        }

        /// <summary>
        /// 나이프 구매
        /// </summary>
        /// <param name="knifeID">구입할 나이프 ID</param>
        public void BuyKnife(int knifeID) {
            if (this.knives.ContainsKey(knifeID)) return;
            KnifeData knifeData = this.knifeDatabase.GetByID(knifeID);
            if (this.point >= knifeData.price) {
                this.knives.Add(knifeData.ID, knifeData);
                this.playerDatabase.UpdateKnives(new HashSet<int>(this.knives.Keys));
                this.UsePoint(knifeData.price);
                this.ChangeKnife(knifeData.ID);
            }
        }
        
        public event Action<int, int> OnChangePoint;
        /// <summary>
        /// 포인트 획득
        /// </summary>
        /// <param name="point">획득 포인트</param>
        public void EarnPoint(int point) {
            int prevPoint = this.point;
            this.point += point;
            this.playerDatabase.UpdatePoint(this.point);
            this.OnChangePoint?.Invoke(prevPoint, this.point);
        }
        /// <summary>
        /// 포인트 사용 
        /// </summary>
        /// <param name="point">사용 포인트</param>
        public void UsePoint(int point) {
            int prevPoint = this.point;
            this.point -= point;
            this.playerDatabase.UpdatePoint(this.point);
            this.OnChangePoint?.Invoke(prevPoint, this.point);
        }

        public event Action<int, int> OnChangeBestStage;
        /// <summary>
        /// 베스트 스테이지 갱신
        /// </summary>
        /// <param name="stage"></param>
        public void ChangeBestStage(int stage) {
            if (stage > this.bestStage) {
                int prevBestStage = this.bestStage;
                this.bestStage = stage;
                this.playerDatabase.UpdateBestStage(this.bestStage);
                this.OnChangeBestStage?.Invoke(prevBestStage, this.bestStage);
            }
        }

        public event Action<int, int> OnChangeBestScore;
        /// <summary>
        /// 베스트 스코어 갱신
        /// </summary>
        /// <param name="score"></param>
        public void ChangeBestScore(int score) {
            if (score > this.bestScore) {
                int prevBestScore = this.bestScore;
                this.bestScore = score;
                this.playerDatabase.UpdateBestScore(this.bestScore);
                this.OnChangeBestScore?.Invoke(prevBestScore, this.bestScore);
            }
        }

        /// <summary>
        /// 플레이 횟수 초기화
        /// </summary>
        public void ResetPlayCount() {
            this.playCount = 0;
            this.playerDatabase.UpdatePlayCount(0);
        }

        /// <summary>
        /// 플레이 횟수 증가
        /// </summary>
        public void ChangePlayCount(int playCount) {
            this.playCount = playCount;
            this.playerDatabase.UpdatePlayCount(this.playCount);
        }
    }
}
