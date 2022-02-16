using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk.Player
{
    public class Shotgun : SingleFireWeapon
    {
        [SerializeField] private ParticleSystem[] muzzleFlashObjects;
        [SerializeField] private ShotgunFX shotgunFX;

        #region WeaponFX
        public void MuzzleFX()
        {
            foreach (var muzzleFlash in muzzleFlashObjects)
            {
                muzzleFlash.Play();
            }
        }

        public void RetrieveAudioFX()
        {
            audioSource.PlayOneShot(shotgunFX.retrieveAudio);
        }

        public void ShotAudioFX()
        {
            audioSource.PlayOneShot(shotgunFX.shootAudio);
        }

        public void DryFireFX()
        {
            audioSource.PlayOneShot(shotgunFX.dryFireAudio);
        }

        public void SlideAudioFX()
        {
            audioSource.PlayOneShot(shotgunFX.slideAudio);
        }

        public void UnfoldAuidioFX()
        {
            audioSource.PlayOneShot(shotgunFX.unfoldAudio);
        }

        public void ReloadAudioFX()
        {
            audioSource.PlayOneShot(shotgunFX.reloadAudio);
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