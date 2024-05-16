using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KnifeDatabase {
    [Serializable]
    public class Info {
        [HideInInspector]
        public string name;
        [SerializeField]
        int _ID;
        public int ID {
            get { return this._ID; }
            private set { this._ID = value; }
        }
        [SerializeField]
        GameObject _prefab;
        public GameObject prefab {
            get { return this._prefab; }
            private set { this._prefab = value; }
        }
        [SerializeField]
        Sprite _image;
        public Sprite image {
            get { return this._image; }
            private set { this._image = value; }
        }
        [SerializeField]
        int _price = 500;
        public int price {
            get { return this._price; }
            private set { this._price = value; }
        }
    }

    public class Data {
        public int ID { get; private set; }
        public GameObject prefab { get; private set; }
        public Sprite image { get; private set; }
        public int price { get; private set; }
        public Data(Info info) {
            this.ID = info.ID;
            this.prefab = info.prefab;
            this.image = info.image;
            this.price = info.price;
        }
    }

    [CreateAssetMenu(fileName = "Database", menuName = "Database/Knife")]
    public class DatabaseSO : ScriptableObject
    {
        [SerializeField]
        List<Info> infos;

        #region Unity Method
        void OnValidate() {
            for (int i = 0; i < this.infos.Count; i++) {
                Info info = this.infos[i];
                info.name = $"Knife {i}";
            }
        }
        #endregion

        public Data GetByID(int ID) {
            Info info = this.infos.FirstOrDefault((info) => info.ID == ID);
            Data data = new Data(info);
            return data;
        }
        
        public int GetCount() => this.infos.Count();

        public IEnumerable<Data> Iterator() {
            foreach (var info in this.infos) {
                Data data = new Data(info);
                yield return data;
            }
        }
    }
}
