using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    public class SmokeGrenade : Grenade
    {
        [SerializeField] private ParticleSystem smoke;
        private float inactivationSmokeDelay = 15f;
        private float inactivationObjectDelay = 5f;

        protected override void GrenadeEffect()
        {
            smoke.Play();

            Collider[] collidersToBlind = Physics.OverlapSphere(transform.position, grenadeStats.explodeRadius);

            foreach (Collider nearbyEnemies in collidersToBlind)
            {
                Enemy enemy = nearbyEnemies.GetComponent<Enemy>();

                if (enemy != null)
                {
                    Debug.Log($"Trying to blind {enemy.gameObject.name}");
                }

            }

            StartCoroutine(SetSmokeInactive(smoke));
        }

        private IEnumerator SetSmokeInactive(ParticleSystem smoke)
        {
            yield return new WaitForSeconds(inactivationSmokeDelay);
            smoke.Stop();
            StartCoroutine(SetObjectInactive());
        }

        private IEnumerator SetObjectInactive()
        {
            yield return new WaitForSeconds(inactivationObjectDelay);
            gameObject.transform.SetParent(parent);
            gameObject.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(smoke.gameObject.transform.position, grenadeStats.explodeRadius);
        }
    }
}
