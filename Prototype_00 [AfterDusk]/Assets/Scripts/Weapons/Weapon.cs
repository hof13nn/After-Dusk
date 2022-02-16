using System.Collections.Generic;
using UnityEngine;


namespace AfterDusk.Player
{
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private int id;
        public int ID { get => id; set => id = value; }
        [SerializeField] protected WeaponStats weaponStats;
        public WeaponStats WeaponStats { get => weaponStats; }
        [SerializeField] private Sprite weaponIcon;
        public Sprite WeaponIcon { get => weaponIcon; }
        [SerializeField] protected Knife knife;
        protected float isReadyToFireDelay;
        protected AudioSource audioSource;
        protected Animator weaponAnimator;
        protected GrenadeController grenadeController;
        private AnimationClip[] animationClips;
        protected Rigidbody weaponBrass;
        protected WeaponFX weaponFX = null;
        protected Queue<Rigidbody> brassesInPool;
        protected Queue<Rigidbody> brassesActiveInWorld;
        protected bool isFire = false;
        public bool IsFire { get => isFire; set => isFire = value; }
        protected bool isReadyToFire = true;
        public bool IsReadyToFire { get => isReadyToFire; set => isReadyToFire = value; }
        protected bool isReloading = false;
        public bool IsReloading { get => isReloading; set => isReloading = value; }
        private bool isManualReload = false;
        public bool IsManualReload { get => isManualReload; set => isManualReload = value; }
        protected GameObject scope;
        public GameObject Scope { get => scope; set => scope = value; }
        protected Transform projectileHolder;
        protected float projectileSpeedForce;

        protected virtual void Awake()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            weaponAnimator = gameObject.GetComponent<Animator>();
            animationClips = weaponAnimator.runtimeAnimatorController.animationClips;
        }

        protected virtual void OnDisable()
        {
            if (isReloading) isReloading = false;
            if (!IsReadyToFire) IsReadyToFire = true;
        }

        private void Start()
        {
            grenadeController = PlayerManager.GrenadeController;
            GetReadyToFireDelay(animationClips);
            InitializeBrasses();

        }

        public void ShootRaycast()
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, weaponStats.weaponRange))
            {
                Debug.Log("I shot: " + hitInfo.collider.gameObject.name);

                if (hitInfo.collider.GetComponent<Damageable>() != null)
                {
                    hitInfo.collider.GetComponent<Damageable>().TakeDamage(weaponStats.weaponDamage, hitInfo.point, hitInfo.normal);
                }
                else if (hitInfo.collider.GetComponent<Enemy>() != null)
                {
                    hitInfo.collider.GetComponent<Enemy>().TakeDamage(weaponStats.weaponDamage, hitInfo.point, hitInfo.normal);
                }
            }
        }

        public void ShootProjectile()
        {
            if (projectileHolder == null) return;

            var projectileObject = projectileHolder.GetChild(0).gameObject;
            var projectileRb = projectileObject.GetComponent<Rigidbody>();
            projectileObject.GetComponent<RocketProjectile>().Damage = weaponStats.weaponDamage;

            if (!projectileObject.activeSelf)
            {
                projectileObject.SetActive(true);
                projectileObject.transform.SetParent(null);
                projectileRb.AddRelativeForce(Vector3.forward * projectileSpeedForce, ForceMode.Impulse);
            }
        }

        public void ThrowGrenade()
        {
            for (int i = 0; i < PlayerManager.GrenadeController.ActiveGrenadeList.Count; i++)
            {
                var grenadeObject = PlayerManager.GrenadeController.ActiveGrenadeList[i];
                var grenadeStats = grenadeObject.GetComponent<Grenade>().GrenadeStats;
                var grenadeRb = grenadeObject.GetComponent<Rigidbody>();

                if (!grenadeObject.activeSelf)
                {
                    grenadeObject.SetActive(true);
                    grenadeObject.transform.SetParent(null);
                    grenadeStats.ammoInReserve--; ;
                    grenadeRb.AddRelativeForce(Vector3.forward * grenadeStats.grenadeLaunchForce, ForceMode.Impulse);
                    return;
                }
            }
        }

        public void AddRecoil()
        {
            var randomRecoil = new Vector2(Random.Range(0, weaponStats.recoilDirection.x), Random.Range(0, weaponStats.recoilDirection.y));
            PlayerManager.PlayerController.AddRecoil(randomRecoil);
        }

        public void ReduceAmmo()
        {
            if (weaponStats.ammoInMagazine > 0)
            {
                weaponStats.ammoInMagazine--;
            }
        }

        public void LoadBullet()
        {
            if (weaponStats.ammoInReserve != 0)
            {
                weaponStats.ammoInReserve--;
                weaponStats.ammoInMagazine++;
            }
            if (weaponStats.ammoInMagazine == weaponStats.maxAmmoInMagazine || weaponStats.ammoInReserve == 0)
            {
                isReloading = false;
                isManualReload = false;
                IsReadyToFire = true;
                if (weaponStats.weaponClass == WeaponClass.Shotgun) weaponAnimator.SetTrigger(PlayerAnimationTags.FullMag);
            }
        }

        public void ReloadMag()
        {
            var neededAmount = weaponStats.maxAmmoInMagazine - weaponStats.ammoInMagazine;
            var amount = weaponStats.ammoInReserve - neededAmount;

            if (amount < 0)
            {
                amount = weaponStats.ammoInReserve;
            }
            else
            {
                amount = neededAmount;
            }

            weaponStats.ammoInReserve -= amount;
            weaponStats.ammoInMagazine += amount;
            isReloading = false;
            isManualReload = false;
            IsReadyToFire = true;
        }

        private void GetReadyToFireDelay(AnimationClip[] clips)
        {
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name.Contains(StringVariables.SingleFire))
                {
                    isReadyToFireDelay = clips[i].length;
                    return;
                }
            }
        }

        private void InitializeBrasses()
        {
            if (weaponFX == null || !weaponFX.hasBrass) return;

            brassesInPool = new Queue<Rigidbody>();
            brassesActiveInWorld = new Queue<Rigidbody>();

            for (int i = 0; i < weaponStats.maxAmmoInReserve; i++)
            {
                InstantiateBrass();
            }
        }

        private void InstantiateBrass()
        {
            var instance = Instantiate(weaponBrass, transform);
            brassesInPool.Enqueue(instance);
            instance.gameObject.SetActive(false);
        }

        protected Rigidbody GetNextAvailableBrass()
        {
            if (brassesInPool.Count > 0) return brassesInPool.Dequeue();
            var oldestActiveBrass = brassesActiveInWorld.Dequeue();
            return oldestActiveBrass;
        }
    }
}