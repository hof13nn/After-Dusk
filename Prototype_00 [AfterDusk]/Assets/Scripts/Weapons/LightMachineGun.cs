using System.Collections;
using UnityEngine;

namespace AfterDusk.Player
{
    public class LightMachineGun : AutoFireWeapon
    {
        [SerializeField] private Transform bulletShellStartPos;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private LMGFX lmgFX;

        protected override void Awake()
        {
            base.Awake();
            weaponBrass = lmgFX.bulletShellPrefab;
            weaponFX = lmgFX;
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
            Debug.Log($"Brasses active in world: {brassesActiveInWorld.Count} \nBrasses in Pool: {brassesInPool.Count}");
        }

        public void RetrieveAudioFX()
        {
            audioSource.PlayOneShot(lmgFX.retrieveAudio);
        }

        public void ShotAudioFX()
        {
            audioSource.PlayOneShot(lmgFX.shootAudio);
        }

        public void DryFireFX()
        {
            audioSource.PlayOneShot(lmgFX.dryFireAudio);
        }

        public void OpenWeaponAudioFX()
        {
            audioSource.PlayOneShot(lmgFX.openAudio);
        }

        public void OpenMagAudioFX()
        {
            audioSource.PlayOneShot(lmgFX.openMagAudio);
        }

        public void MagEjectAudioFX()
        {
            audioSource.PlayOneShot(lmgFX.ejectMagAudio);
        }

        public void MagInsertAudioFX()
        {
            audioSource.PlayOneShot(lmgFX.insertMagAudio);
        }

        public void FixBulletsAudioFX()
        {
            audioSource.PlayOneShot(lmgFX.fixBulletsAudio);
        }
        public void CloseWeaponAudioFx()
        {
            audioSource.PlayOneShot(lmgFX.closeAudio);
        }

        public void SlideAudioFX()
        {
            audioSource.PlayOneShot(lmgFX.slideAudio);
        }

        public void ReloadAudioFX()
        {
            audioSource.PlayOneShot(lmgFX.reloadAudio);
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
