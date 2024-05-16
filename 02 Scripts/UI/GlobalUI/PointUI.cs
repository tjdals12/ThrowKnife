using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GlobalUI {
    public class PointUI : MonoBehaviour
    {
        [Header("UI")]
        [Space(4)]
        [SerializeField]
        TextMeshProUGUI point;

        public void ChangePoint(int point) {
            this.point.text = point.ToString();
        }
    }
}