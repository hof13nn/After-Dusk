using UnityEngine;

namespace AfterDusk.Player
{
    public class SniperRifle : SingleFireWeapon
    {
        [SerializeField] private GameObject scopeObject;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private SniperRifleFX sniperRifleFX;

        private void Start()
        {
            scope = this.scopeObject;
        }

        #region WeaponFX
        public void MuzzleFX()
        {
            muzzleFlash.Play();
        }

        public void RetrieveAudioFX()
        {
            audioSource.PlayOneShot(sniperRifleFX.retrieveAudio);
        }

        public void ShotAudioFX()
        {
            audioSource.PlayOneShot(sniperRifleFX.shootAudio);
        }

        public void DryFireFX()
        {
            audioSource.PlayOneShot(sniperRifleFX.dryFireAudio);
        }

        public void MagEjectAudioFX()
        {
            audioSource.PlayOneShot(sniperRifleFX.ejectMagAudio);
        }
        public void MagInsertAudioFX()
        {
            audioSource.PlayOneShot(sniperRifleFX.insertMagAudio);
        }

        public void BackwardSlideAudioFX()
        {
            audioSource.PlayOneShot(sniperRifleFX.backwardSlideAudio);
        }

        public void ForwardSlideAudioFX()
        {
            audioSource.PlayOneShot(sniperRifleFX.forwardSlideAudio);
        }

        public void ReloadAudioFX()
        {
            audioSource.PlayOneShot(sniperRifleFX.reloadAudio);
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