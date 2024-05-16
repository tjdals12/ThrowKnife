using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace InventoryUI {
    public class Dot : MonoBehaviour
    {
        [SerializeField]
        Image enable;
        [SerializeField]
        Image disable;

        public void Enable() {
            this.enable
                .DOColor(this.enable.color, 0.1f)
                .From(this.disable.color)
                .OnStart(() => {
                    this.enable.gameObject.SetActive(true);
                    this.disable.gameObject.SetActive(false);
                });
        }

        public void Disable() {
            this.disable
                .DOColor(this.disable.color, 0.5f)
                .From(this.enable.color)
                .OnStart(() => {
                    this.disable.gameObject.SetActive(true);
                    this.enable.gameObject.SetActive(false);
                });
        }
    }
}
