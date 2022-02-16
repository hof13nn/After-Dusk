using System.Collections;
using UnityEngine;

namespace AfterDusk.Player
{
    public class AssaultRifle : AutoFireWeapon
    {
        [SerializeField] private Transform bulletShellStartPos;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private AutoRifleFX arFX;

        protected override void Awake()
        {
            base.Awake();
            weaponBrass = arFX.bulletShellPrefab;
            weaponFX = arFX;
        }


        #region WeaponFX
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
            audioSource.PlayOneShot(arFX.retrieveAudio);
        }

        public void ShotAudioFX()
        {
            audioSource.PlayOneShot(arFX.shootAudio);
        }

        public void DryFireFX()
        {
            audioSource.PlayOneShot(arFX.dryFireAudio);
        }

        public void MagEjectAudioFX()
        {
            audioSource.PlayOneShot(arFX.ejectMagAudio);
        }
        public void MagInsertAudioFX()
        {
            audioSource.PlayOneShot(arFX.insertMagAudio);
        }

        public void ReloadAudioFX()
        {
            audioSource.PlayOneShot(arFX.reloadAudio);
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
