using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGameEffect {
    public class WoodCrumb : MonoBehaviour
    {
        ParticleSystem[] particleSystems;

        #region Unity Method
        void Awake() {
            this.particleSystems = this.GetComponentsInChildren<ParticleSystem>();
        }
        #endregion

        public void Play() {
            foreach (var particleSystem in this.particleSystems) {
                particleSystem.Play();
            }
            Destroy(this.gameObject, 0.5f);
        }
    }
}
