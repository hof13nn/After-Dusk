using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Weapon FX", menuName = "Weapons/Weapon FX/Auto Rifle FX")]
    public class AutoRifleFX : WeaponFX
    {
        public Rigidbody bulletShellPrefab;
        public AudioClip ejectMagAudio;
        public AudioClip insertMagAudio;
    }
}
