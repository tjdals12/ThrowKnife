using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame {
    using KnifeDatabase = KnifeDatabase.DatabaseSO;
    using KnifeData = KnifeDatabase.Data;

    public class KnifeInfoData : MonoBehaviour
    {
        [SerializeField]
        KnifeDatabase knifeDatabase;

        /// <summary>
        /// 전체 나이프 목록
        /// </summary>
        public IEnumerable<KnifeData> iterator {
            get { return this.knifeDatabase.Iterator(); }
        }

        void Start() {
            this.OnLoaded?.Invoke();
        }

        public event Action OnLoaded;
        public event Action<KnifeData> OnSelectKnife;
        /// <summary>
        /// 나이프 선택
        /// </summary>
        /// <param name="knifeID"></param>
        public void SelectKnife(int knifeID) {
            KnifeData knifeData = this.knifeDatabase.GetByID(knifeID);
            this.OnSelectKnife?.Invoke(knifeData);
        }
    }
}
