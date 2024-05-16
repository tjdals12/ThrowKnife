using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGameUI {
    public class Knife : MonoBehaviour
    {
        [SerializeField]
        GameObject enable;
        [SerializeField]
        GameObject disable;

        public void Enable() {
            this.enable.SetActive(true);
            this.disable.SetActive(false);
        }

        public void Disable() {
            this.disable.SetActive(true);
            this.enable.SetActive(false);
        }
    }
}
