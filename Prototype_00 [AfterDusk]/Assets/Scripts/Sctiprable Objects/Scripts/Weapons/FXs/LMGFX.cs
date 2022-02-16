using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Weapon FX", menuName = "Weapons/Weapon FX/LMG FX")]
    public class LMGFX : WeaponFX
    {
        public Rigidbody bulletShellPrefab;
        public AudioClip openAudio;
        public AudioClip openMagAudio;
        public AudioClip ejectMagAudio;
        public AudioClip insertMagAudio;
        public AudioClip fixBulletsAudio;
        public AudioClip closeAudio;
        public AudioClip slideAudio;
    }
}
