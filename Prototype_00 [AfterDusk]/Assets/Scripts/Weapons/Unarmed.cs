using UnityEngine;

namespace AfterDusk.Player
{
    public class Unarmed : Weapon
    {
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
    }
}
