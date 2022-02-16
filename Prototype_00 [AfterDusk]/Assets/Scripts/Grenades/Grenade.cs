using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    public abstract class Grenade : MonoBehaviour
    {
        [SerializeField] private int id;
        public int ID { get => id; set => id = value; }
        [SerializeField] protected GrenadeStats grenadeStats;
        public GrenadeStats GrenadeStats { get => grenadeStats; }
        [SerializeField] private GrenadeFX grenadeFX;
        public GrenadeFX GrenadeFX { get => grenadeFX; }
        [SerializeField] private Sprite greandeIcon;
        public Sprite GrenadeIcon { get => greandeIcon; }
        protected MeshRenderer grenadeMesh;
        protected AudioSource audioSource;
        private Vector3 startPos;
        private Quaternion startRotation;
        protected Transform parent;
        private float explodeTimer;
        private bool isExploded;
        protected float originalVolume = 1f;
        protected float fadedOutVolume = 0f;
        protected float t;

        private void Awake()
        {
            if (GetComponent<MeshRenderer>() == null) grenadeMesh = GetComponentInChildren<MeshRenderer>();
            else grenadeMesh = GetComponent<MeshRenderer>();

            audioSource = GetComponent<AudioSource>();
            parent = gameObject.transform.parent;
        }

        private void OnEnable()
        {
            explodeTimer = grenadeStats.explodeDelay;
            startPos = gameObject.transform.localPosition;
            startRotation = gameObject.transform.localRotation;
            isExploded = false;
        }

        private void OnDisable()
        {
            gameObject.transform.localPosition = startPos;
            gameObject.transform.localRotation = startRotation;
        }

        private void FixedUpdate()
        {
            explodeTimer -= Time.deltaTime;

            if (explodeTimer <= 0f && !isExploded)
            {
                GrenadeEffect();
                isExploded = true;
            }
        }

        protected void OnTriggerEnter(Collider target)
        {
            if (target.gameObject.GetComponent<Damageable>())
            {
                if (grenadeStats.grenadeClass == GrenadeClass.Explosive)
                {
                    Debug.Log("Try to destory: " + target.gameObject.name);
                    GrenadeEffect();
                    isExploded = true;
                }
            }
        }

        protected virtual void GrenadeEffect()
        {

        }
    }
}
