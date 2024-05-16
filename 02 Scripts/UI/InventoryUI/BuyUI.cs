using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace InventoryUI {
    using PlayerData = InGame.PlayerData;
    using KnifeInfoData = InGame.KnifeInfoData;
    using KnifeData = KnifeDatabase.Data;

    public class BuyUI : MonoBehaviour
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
        Button button;
        [SerializeField]
        TextMeshProUGUI price;

        KnifeData selectedKnife;

        #region Unity Method
        void Awake() {
            this.playerData.OnChangeKnife += this.OnChangeKnife;
            this.knifeInfoData.OnSelectKnife += this.OnSelectKnife;
            this.button.onClick.AddListener(() => {
                if (selectedKnife != null) {
                    this.playerData.BuyKnife(selectedKnife.ID);
                }
            });
        }
        #endregion

        #region Event Listener
        void OnChangeKnife(KnifeData prev, KnifeData current) {
            this.button.gameObject.SetActive(false);
        }

        void OnSelectKnife(KnifeData knifeData) {
            this.selectedKnife = knifeData;
            this.price.text = knifeData.price.ToString();
            if (this.button.gameObject.activeSelf == false) {
                this.button.transform
                    .DOScale(Vector3.one, 0.1f)
                    .From(Vector3.zero)
                    .OnStart(() => {
                        this.button.gameObject.SetActive(true);
                    });
            }
        }

        #endregion
    }
}
