using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StageDatabase {
    [Serializable]
    public class Info {
        [HideInInspector]
        public string name;
        [HideInInspector]
        public int index;
        [HideInInspector]
        public bool isBoss;
        [SerializeField]
        List<GameObject> _targetsPool;
        public List<GameObject> targetsPool {
            get { return this._targetsPool; }
            private set { this._targetsPool = value; }
        }
        [SerializeField, Range(0f, 10f)]
        float _rotateSpeed;
        public float rotateSpeed {
            get { return this._rotateSpeed; }
            private set { this._rotateSpeed = value; }
        }
        [SerializeField]
        GameObject _itemPrefab;
        public GameObject itemPrefab {
            get { return this._itemPrefab; }
            private set { this._itemPrefab = value; }
        }
        [SerializeField]
        GameObject _stuckKnifePrefab;
        public GameObject stuckKnifePrefab {
            get { return this._stuckKnifePrefab; }
            private set { this._stuckKnifePrefab = value; }
        }
        [SerializeField]
        int _minItemCount;
        public int minItemCount {
            get { return this._minItemCount; }
            private set { this._minItemCount = value; }
        }
        [SerializeField]
        int _maxItemCount;
        public int maxItemCount {
            get { return this._maxItemCount; }
            private set { this._maxItemCount = value; }
        }
        [SerializeField]
        int _minStuckKnifeCount;
        public int minStuckKnifeCount {
            get { return this._minStuckKnifeCount; }
            private set { this._minStuckKnifeCount = value; }
        }
        [SerializeField]
        int _maxStuckKnifeCount;
        public int maxStuckKnifeCount {
            get { return this._maxStuckKnifeCount; }
            private set { this._maxStuckKnifeCount = value; }
        }
        [SerializeField]
        int _throwableKnifeCount;
        public int throwableKnifeCount {
            get { return this._throwableKnifeCount; }
            private set { this._throwableKnifeCount = value; }
        }
    }

    public class Data {
        public int index { get; private set; }
        public bool isBoss { get; private set; }
        public GameObject targetPrefab { get; private set; }
        public float rotateSpeed { get; private set; }
        public GameObject itemPrefab { get; private set; }
        public GameObject stuckKnifePrefab { get; private set; }
        public int itemCount { get; private set; }
        public int stuckKnifeCount { get; private set; }
        public int throwableKnifeCount { get; private set; }
        public Data(Info info) {
            this.index = info.index;
            this.isBoss = info.isBoss;
            this.targetPrefab = info.targetsPool[Random.Range(0, info.targetsPool.Count)];
            this.rotateSpeed = info.rotateSpeed;
            this.itemPrefab = info.itemPrefab; 
            this.stuckKnifePrefab = info.stuckKnifePrefab;
            this.itemCount = Random.Range(info.minItemCount, info.maxItemCount + 1);
            this.stuckKnifeCount = Random.Range(info.minStuckKnifeCount, info.maxStuckKnifeCount + 1);
            this.throwableKnifeCount = info.throwableKnifeCount;
        }
    }

    [CreateAssetMenu(fileName = "Database", menuName = "Database/StageNew")]
    public class DatabaseSO : ScriptableObject
    {
        [SerializeField]
        int _loopStage;
        public int loopStage {
            get { return this._loopStage; }
            private set { this._loopStage = value; }
        }
        [SerializeField]
        List<Info> infos;

        #region Unity Method
        void OnValidate() {
            if (this._loopStage > this.infos.Count) {
                this._loopStage = this.infos.Count;
            }
            for (int i = 0; i < this.infos.Count; i++) {
                Info info = this.infos[i];
                info.name = $"STAGE {i + 1}";
                if ((i + 1) % 5 == 0) {
                    info.name += " (BOSS)";
                }
                info.index = i;
                if ((i + 1) % 5 == 0) {
                    info.isBoss = true;
                }
            }
        }
        #endregion

        public Data GetByIndex(int index) {
            Info info = this.infos[index];
            Data data = new Data(info);
            return data;
        }

        public int GetCount() => this.infos.Count;

        public IEnumerable<Data> Iterator() {
            foreach (var info in this.infos) {
                Data data = new Data(info);
                yield return data;
            }
        }
    }
}
