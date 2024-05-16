using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGameEffect {
    public class ItemSlice : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem[] particleSystems;

        public void Play() {
            foreach (var particleSystem in this.particleSystems) {
                particleSystem.Play();
            }
            Destroy(this.gameObject, 1);
        }
    }
}
