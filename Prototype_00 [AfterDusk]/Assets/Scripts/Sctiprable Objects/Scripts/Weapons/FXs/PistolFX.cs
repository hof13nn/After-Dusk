using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Weapon FX", menuName = "Weapons/Weapon FX/Pistol FX")]
    public class PistolFX : WeaponFX
    {
        public Rigidbody bulletShellPrefab;
        public AudioClip ejectMagAudio;
        public AudioClip insertMagAudio;
    }
}
