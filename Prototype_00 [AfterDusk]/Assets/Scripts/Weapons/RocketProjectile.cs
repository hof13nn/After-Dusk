using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk.Player
{
    public class RocketProjectile : MonoBehaviour
    {
        [SerializeField] private float explodeDelay = 3.5f;
        [SerializeField] private float explodeRadius = 5f;
        [SerializeField] private float explodeForce = 500f;
        [SerializeField] private ParticleSystem explosion;
        [SerializeField] private ParticleSystem smoke;
        [SerializeField] private AudioClip projectileAudio;
        [SerializeField] private AudioClip explosionAudio;
        private MeshRenderer projectileMesh;
        private AudioSource audioSource;
        private Rigidbody projectileRb;
        private int damage;
        public int Damage { get => damage; set => damage = value; }
        private float explodeTimer;
        private float inactivationDelay = 6f;
        private Vector3 startPos;
        private Quaternion startRotation;
        private Transform parent;
        private bool isExploded = false;
        private float originalVolume = 1f;
        private float fadedOutVolume = 0f;
        private float t;

        private void Awake()
        {
            parent = gameObject.transform.parent;
            startPos = gameObject.transform.localPosition;
            startRotation = gameObject.transform.localRotation;
            explodeTimer = explodeDelay;
            projectileMesh = GetComponent<MeshRenderer>();
            projectileRb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            explosion.Simulate(0f, true, true);
            smoke.Simulate(0f, true, true);
            smoke.Play();
            audioSource.PlayOneShot(projectileAudio);
            t = 0f;
        }

        private void Update()
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(originalVolume, fadedOutVolume, t / explodeDelay);
        }

        private void FixedUpdate()
        {
            explodeTimer -= Time.deltaTime;

            if (explodeTimer <= 0f && !isExploded)
            {
                ExplodeFX();
                isExploded = true;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            ExplodeFX();
            Debug.Log($"Try to destory: {other.gameObject.name}");
            isExploded = true;
        }

        public void ExplodeFX()
        {
            projectileMesh.enabled = false;
            projectileRb.isKinematic = true;
            smoke.Stop();
            explosion.Play();
            audioSource.PlayOneShot(explosionAudio);

            Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, explodeRadius);
            foreach (Collider nearbyObject in collidersToDestroy)
            {
                Damageable dest = nearbyObject.GetComponent<Damageable>();

                if (dest != null)
                {
                    dest.TakeDamage(damage);
                }

            }

            Collider[] collidersToMove = Physics.OverlapSphere(transform.position, explodeRadius);
            foreach (Collider nearbyObject in collidersToMove)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explodeForce, transform.position, explodeRadius);
                }

            }

            StartCoroutine(SetObjectInactive(explosion));
        }

        private IEnumerator SetObjectInactive(ParticleSystem explosion)
        {
            yield return new WaitForSeconds(inactivationDelay);
            audioSource.volume = originalVolume;
            explosion.Stop();
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = startPos;
            gameObject.transform.localRotation = startRotation;
            explodeTimer = explodeDelay;
            isExploded = false;
            projectileRb.isKinematic = false;
            projectileMesh.enabled = true;
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
