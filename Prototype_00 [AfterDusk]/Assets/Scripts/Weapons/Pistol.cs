using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk.Player.Weapons
{
    public class Pistol : SingleFireWeapon
    {
        [SerializeField] private Transform bulletShellStartPos;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private PistolFX pistolFX;

        protected override void Awake()
        {
            base.Awake();
            weaponBrass = pistolFX.bulletShellPrefab;
            weaponFX = pistolFX;
        }

        #region WeaponFXs
        public void MuzzleFX()
        {
            muzzleFlash.Play();
        }

        public void ShellFX()
        {
            var brass = GetNextAvailableBrass();

            if (brass != null)
            {
                brass.transform.parent = bulletShellStartPos.transform;
                brass.transform.position = bulletShellStartPos.position;
                brass.transform.rotation = Quaternion.identity;
                brass.gameObject.SetActive(true);
                brass.GetComponent<Rigidbody>().AddRelativeForce(transform.TransformDirection(new Vector3(-3, Random.Range(2, 4), 0)), ForceMode.Impulse);
                brassesActiveInWorld.Enqueue(brass);
                StartCoroutine(DisableBrass(brass));
            }
        }

        private IEnumerator DisableBrass(Rigidbody brass)
        {
            yield return new WaitForSeconds(10f);
            brassesActiveInWorld.Dequeue();
            brassesInPool.Enqueue(brass);
            brass.gameObject.SetActive(false);
        }

        public void RetrieveAudioFX()
        {
            audioSource.PlayOneShot(pistolFX.retrieveAudio);
        }

        public void ShotAudioFX()
        {
            audioSource.PlayOneShot(pistolFX.shootAudio);
        }

        public void DryFireFX()
        {
            audioSource.PlayOneShot(pistolFX.dryFireAudio);
        }

        public void MagEjectAudioFX()
        {
            audioSource.PlayOneShot(pistolFX.ejectMagAudio);
        }
        public void MagInsertAudioFX()
        {
            audioSource.PlayOneShot(pistolFX.insertMagAudio);
        }

        public void ReloadAudioFX()
        {
            audioSource.PlayOneShot(pistolFX.reloadAudio);
        }

        public void EjectGrenadeRingAudioFX()
        {
            audioSource.PlayOneShot(grenadeController.GetActiveGrenade().grenadeFX.removeSaftyRing);
        }

        public void ThrowGrenadeAudioFX()
        {
            audioSource.PlayOneShot(grenadeController.GetActiveGrenade().grenadeFX.throwGrenadeAudio);
        }

        public void MeleeAttackAudioFX()
        {
            audioSource.PlayOneShot(knife.MeleeAttackAudio);
        }
        #endregion

    }
}
