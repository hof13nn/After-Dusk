using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk.Player
{
    public class RocketLauncher : SingleFireWeapon
    {
        [SerializeField] private GameObject scopePrefab;
        [SerializeField] private Transform projectileHolderObject;
        [SerializeField] private float projectileLaunchForce;
        [SerializeField] private RocketLauncherFX rocketLauncherFX;
        [SerializeField] private ParticleSystem[] smokes;

        private void Start()
        {
            scope = scopePrefab;
            projectileHolder = projectileHolderObject;
            projectileSpeedForce = projectileLaunchForce;
        }

        #region WeaponFX
        public void MuzzleFX()
        {
            foreach (var smoke in smokes)
            {
                smoke.Play();
            }
        }

        public void RetrieveAudioFX()
        {
            audioSource.PlayOneShot(rocketLauncherFX.retrieveAudio);
        }

        public void ShotAudioFX()
        {
            audioSource.PlayOneShot(rocketLauncherFX.shootAudio);
        }

        public void DryFireFX()
        {
            audioSource.PlayOneShot(rocketLauncherFX.dryFireAudio);
        }

        public void BackwardSlideAudioFX()
        {
            audioSource.PlayOneShot(rocketLauncherFX.backwardSlideAudio);
        }

        public void ForwardSlideAudioFX()
        {
            audioSource.PlayOneShot(rocketLauncherFX.forwardSlideAudio);
        }

        #endregion
    }
}
