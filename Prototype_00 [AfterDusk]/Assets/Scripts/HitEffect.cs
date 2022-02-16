using System;
using UnityEngine;

namespace AfterDusk
{
    [Serializable]
    public class HitEffect
    {
        private GameObject hitEffectPrefab;
        public GameObject HitEffectPrefab { get => hitEffectPrefab; }
        private ParticleSystem hitEffectPS;
        public ParticleSystem HitEffectPS { get => hitEffectPS; }

        public HitEffect(GameObject hitEffectPrefab, ParticleSystem hitEffectPS)
        {
            this.hitEffectPrefab = hitEffectPrefab;
            this.hitEffectPS = hitEffectPS;
        }
    }
}
