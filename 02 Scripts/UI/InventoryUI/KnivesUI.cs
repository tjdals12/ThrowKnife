using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryUI {
    using KnifeInfoData = InGame.KnifeInfoData;
    using KnifeData = KnifeDatabase.Data;
    using PlayerData = InGame.PlayerData;
    using SettingsData = Settings.Data;

    [Serializable]
    public class Scroll {
        [SerializeField]
        Scrollbar scrollbar;

        float scrollbarPosition;
        float[] positions;
        float distance;

        List<Dot> dots;
        int currentDotIndex;

        public void Setup(int lastPage, List<Dot> dots) {
            this.scrollbarPosition = 0;
            this.positions = new float[lastPage];
            this.distance = 1f / (this.positions.Length - 1f);
            for (int i = 0; i < this.positions.Length; i++) {
                this.positions[i] = distance * i;
            }
            this.dots = dots;
        }

        public void StartDrag() {
            this.scrollbarPosition = this.scrollbar.value;
        }

        public void EndDrag() {
            for (int i = 0; i < this.positions.Length; i++) {
                if (this.scrollbarPosition < this.positions[i] + (distance / 2) && this.scrollbarPosition > this.positions[i] - (distance / 2)) {
                    this.scrollbar.value = Mathf.Lerp(this.scrollbar.value, this.positions[i], 0.1f);
                    if (currentDotIndex != i) {
                        this.dots[currentDotIndex].Disable();
                        this.dots[i].Enable();
                        this.currentDotIndex = i;
                    }
                }
            }
        }
    }

    public class KnivesUI : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        PlayerData playerData;
        [SerializeField]
        KnifeInfoData knifeInfoData;
        [SerializeField]
        SettingsData settingsData;

        [Header("UI - Knives")]
        [Space(4)]
        [SerializeField]
        GameObject viewPanel;
        [SerializeField]
        GameObject listPanelPrefab;
        [SerializeField]
        GameObject itemPanelPrefab;
        [SerializeField]
        GameObject itemPrefab;

        [Header("UI - Dot")]
        [Space(4)]
        [SerializeField]
        GameObject dotsPanel;
        [SerializeField]
        GameObject dotPrefab;

        [Header("UI - Scroll")]
        [SerializeField]
        Scroll scroll;

        [Header("Sound")]
        [Space(4)]
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip soundOfChangeKnife;
        [SerializeField]
        AudioClip soundOfSelectKnife;

        bool isLoadedKnfieInfoData = false;
        Dictionary<int, Knife> knives;
        KnifeData currentKnfie;
        
        #region Unity Method
        void Awake() {
            this.settingsData.OnToggleSFX += this.OnToggleSFX;
            this.knifeInfoData.OnLoaded += this.OnLoadedKnifeInfoData;
            this.knifeInfoData.OnSelectKnife += this.OnSelectKnife;
            this.playerData.OnLoaded += this.OnLoadedPlayerData;
            this.playerData.OnChangeKnife += this.OnChangeKnife;
        }

        void Update() {
            if (Input.GetMouseButton(0)) {
                this.scroll.StartDrag();
            } else {
                this.scroll.EndDrag();
            }
        }
        #endregion

        #region Event Listener
        void OnToggleSFX(bool value) {
            this.audioSource.mute = value == false;
        }
        void OnLoadedKnifeInfoData() {
            this.ShowKnives();
        }

        void OnLoadedPlayerData() {
            StartCoroutine(this.ShowPlayerKnives());
        }

        void OnChangeKnife(KnifeData prev, KnifeData current) {
            if (this.currentKnfie != null) {
                this.knives[this.currentKnfie.ID].Deselect();
            }
            if (prev != null) {
                this.knives[prev.ID].Deselect();
            }
            if (current != null) {
                this.knives[current.ID].Unlock();
                this.knives[current.ID].Select();
                this.currentKnfie = current;
            }
        }

        void OnSelectKnife(KnifeData knifeData) {
            this.knives[this.currentKnfie.ID].Deselect();
            this.knives[knifeData.ID].Select();
            this.currentKnfie = knifeData;
        }
        #endregion

        void ShowKnives() {
            for (int i = 0; i < this.viewPanel.transform.childCount; i++) {
                GameObject child = this.viewPanel.transform.GetChild(i).gameObject;
                Destroy(child);
            }
            for (int i = 0; i < this.dotsPanel.transform.childCount; i++) {
                GameObject child = this.dotsPanel.transform.GetChild(i).gameObject;
                Destroy(child);
            }
            List<GameObject> itemPanels = new();
            Dictionary<int, Knife> knives = new();
            foreach (var knifeData in this.knifeInfoData.iterator) {
                GameObject itemPanel = Instantiate(this.itemPanelPrefab);
                itemPanels.Add(itemPanel);

                GameObject item = Instantiate(this.itemPrefab, itemPanel.transform);
                item.transform.localScale = Vector3.one;
                Knife knife = item.GetComponent<Knife>();
                knife.Setup(knifeData.image);
                knife.OnClick += () => {
                    if (this.playerData.knives.ContainsKey(knifeData.ID)) {
                        this.audioSource.PlayOneShot(this.soundOfChangeKnife);
                        this.playerData.ChangeKnife(knifeData.ID);
                    } else {
                        this.audioSource.PlayOneShot(this.soundOfSelectKnife);
                        this.knifeInfoData.SelectKnife(knifeData.ID);
                    }
                };
                knives.Add(knifeData.ID, knife);
            }

            int listPanelCount = Mathf.CeilToInt(itemPanels.Count / 16f);
            List<Dot> dots = new();
            for (int i = 0; i < listPanelCount; i++) {
                GameObject listPanel = Instantiate(this.listPanelPrefab, this.viewPanel.transform);
                for (int j = i * 16; j < (i + 1) * 16; j++) {
                    if (j >= itemPanels.Count) break;
                    GameObject itemPanel = itemPanels[j];
                    itemPanel.transform.SetParent(listPanel.transform);
                    itemPanel.transform.localScale = Vector3.one;
                }

                GameObject dotClone = Instantiate(this.dotPrefab, this.dotsPanel.transform);
                Dot dot = dotClone.GetComponent<Dot>();
                dots.Add(dot);
                if (i == 0) {
                    dot.Enable();
                } else {
                    dot.Disable();
                }
            }
            this.scroll.Setup(listPanelCount, dots);
            this.knives = knives;
            this.isLoadedKnfieInfoData = true;
        }

        IEnumerator ShowPlayerKnives() {
            while (this.isLoadedKnfieInfoData == false) {
                yield return null;
            }
            KnifeData equippedKnife = this.playerData.knife;
            foreach (var key in this.knives.Keys) {
                if (this.playerData.knives.ContainsKey(key)) {
                    Knife knife = this.knives[key];
                    knife.Unlock();
                    if (key == equippedKnife.ID) {
                        knife.Select();
                    } else {
                        knife.Deselect();
                    }
                }
            }
            this.knives[equippedKnife.ID].Select();
        }
    }
}
