using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    public abstract class Damageable : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHealth;
        private float currentHealth;
        [SerializeField] private GameObject[] hitEffectPool;
        [SerializeField] private int maxDecals = 10;
        private List<HitEffect> hitEffects = new List<HitEffect>();

        private void Awake()
        {
            InitializeDecals();
        }

        private void OnEnable()
        {
            currentHealth = maxHealth;
        }

        private void InitializeDecals()
        {
            for (int i = 0; i < maxDecals; i++)
            {
                InstantiateDecals();
            }
        }

        // private void Shuffle(List<HitEffect> hitEffects)
        // {
        //     var count = hitEffects.Count;
        //     var last = count - 1;
        //     for (int i = 0; i < last; i++)
        //     {
        //         var random = UnityEngine.Random.Range(i, count);
        //         var tmp = hitEffects[i];
        //         hitEffects[i] = hitEffects[random];
        //         hitEffects[random] = tmp;
        //     }
        // }

        private void InstantiateDecals()
        {
            foreach (var prefab in hitEffectPool)
            {
                var instance = Instantiate(prefab, transform) as GameObject;
                var hitEffect = new HitEffect(instance, instance.GetComponentInChildren<ParticleSystem>());
                hitEffects.Add(hitEffect);
            }
        }

        public void TakeDamage(int damage, Vector3 hitPos, Vector3 hitNormal)
        {
            EnableHitEffect(hitPos, hitNormal);

            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }

        private void EnableHitEffect(Vector3 hitPos, Vector3 hitNormal)
        {
            int index = UnityEngine.Random.Range(0, hitEffects.Count);
            var hitEffectPrefab = hitEffects[index].HitEffectPrefab;
            var hitEffectPS = hitEffects[index].HitEffectPS;
            if (hitEffectPrefab.gameObject.activeSelf) index = UnityEngine.Random.Range(0, hitEffects.Count);
            hitEffectPrefab.transform.position = hitPos;
            hitEffectPrefab.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffectPrefab.SetActive(true);
            hitEffectPS.Play();
            StartCoroutine(DisableHitEffct(hitEffectPrefab, hitEffectPS));
        }

        private IEnumerator DisableHitEffct(GameObject hitEffectPrefab, ParticleSystem hitEffectPS)
        {
            yield return new WaitForSeconds(5f);
            hitEffectPS.Simulate(0f, true, true);
            hitEffectPrefab.gameObject.SetActive(false);
        }
    }
}
