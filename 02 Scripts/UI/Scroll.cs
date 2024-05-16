using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    [SerializeField]
    GameObject view;
    [SerializeField]
    GameObject scrollbar;

    float scrollPosition = 0;
    float[] positions;
    float distance;

    void Start()
    {
        this.positions = new float[this.view.transform.childCount];
        this.distance = 1f / (this.positions.Length - 1f);
        for (int i = 0; i < this.positions.Length; i++) {
            this.positions[i] = distance * i;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) {
            this.scrollPosition = this.scrollbar.GetComponent<Scrollbar>().value;
        } else {
            for (int i = 0; i < this.positions.Length; i++) {
                if (this.scrollPosition < positions[i] + (distance * 0.5f) && this.scrollPosition > positions[i] - (distance * 0.5f)) {
                    Scrollbar sb = this.scrollbar.GetComponent<Scrollbar>();
                    sb.value = Mathf.Lerp(sb.value, positions[i], 0.1f);
                }
            }
        }
    }
}
