using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryUI {
    using PlayerData = InGame.PlayerData;
    using KnifeInfoData = InGame.KnifeInfoData;
    using KnifeData = KnifeDatabase.Data;

    public class EquippedKnifeUI : MonoBehaviour
    {
        [Header("Data")]
        [Space(4)]
        [SerializeField]
        PlayerData playerData;
        [SerializeField]
        KnifeInfoData knifeInfoData;

        [Header("UI")]
        [Space(4)]
        [SerializeField]
        GameObject knifePanel;
        [SerializeField]
        GameObject equippedKnifePrefab;
        
        EquippedKnife equippedKnife;

        #region Unity Method
        void Awake() {
            EquippedKnife equippedKnife = knifePanel.GetComponentInChildren<EquippedKnife>();
            if (equippedKnife != null) {
                Destroy(equippedKnife.gameObject);
            }
            this.playerData.OnChangeKnife += this.OnChangeKnife;
            this.knifeInfoData.OnSelectKnife += this.OnSelectKnife;
        }
        #endregion

        #region Event Listener
        void OnChangeKnife(KnifeData prev, KnifeData current) {
            if (prev == null) {
                GameObject clone = Instantiate(this.equippedKnifePrefab, this.knifePanel.transform);
                EquippedKnife equippedKnife = clone.GetComponent<EquippedKnife>();
                equippedKnife.Change(current.image);
                this.equippedKnife = equippedKnife;
            } else {
                GameObject clone = Instantiate(this.equippedKnifePrefab, this.knifePanel.transform);
                EquippedKnife equippedKnife = clone.GetComponent<EquippedKnife>();
                equippedKnife.Change(current.image);
                this.equippedKnife.ScaleOut();
                equippedKnife.ScaleIn();
                this.equippedKnife = equippedKnife;
            }
        }

        void OnSelectKnife(KnifeData knifeData) {
            GameObject clone = Instantiate(this.equippedKnifePrefab, this.knifePanel.transform);
            EquippedKnife equippedKnife = clone.GetComponent<EquippedKnife>();
            equippedKnife.Select(knifeData.image);
            this.equippedKnife.ScaleOut();
            equippedKnife.ScaleIn();
            this.equippedKnife = equippedKnife;
        }
        #endregion
    }
}
