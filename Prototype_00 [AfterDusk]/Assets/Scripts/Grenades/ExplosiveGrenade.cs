using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk.Player
{
    public class ExplosiveGrenade : Grenade
    {
        [SerializeField] private ParticleSystem explosion;
        [SerializeField] private AudioClip explosionAudio;
        private float inactivationDelay = 5f;

        protected override void GrenadeEffect()
        {
            explosion.Play();
            audioSource.PlayOneShot(explosionAudio);
            grenadeMesh.enabled = false;

            Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, grenadeStats.explodeRadius);
            foreach (Collider nearbyObject in collidersToDestroy)
            {
                Damageable dest = nearbyObject.GetComponent<Damageable>();

                if (dest != null)
                {
                    dest.TakeDamage(grenadeStats.grenadeDamage);
                }

            }

            Collider[] collidersToMove = Physics.OverlapSphere(transform.position, grenadeStats.explodeRadius);
            foreach (Collider nearbyObject in collidersToMove)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(grenadeStats.explodeForce, transform.position, grenadeStats.explodeRadius);
                }

            }

            StartCoroutine(SetObjectInactive(explosion));
        }

        private IEnumerator SetObjectInactive(ParticleSystem explosion)
        {
            yield return new WaitForSeconds(inactivationDelay);
            explosion.Stop();
            gameObject.transform.SetParent(parent);
            grenadeMesh.enabled = true;
            gameObject.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, grenadeStats.explodeRadius);
        }
    }
}
