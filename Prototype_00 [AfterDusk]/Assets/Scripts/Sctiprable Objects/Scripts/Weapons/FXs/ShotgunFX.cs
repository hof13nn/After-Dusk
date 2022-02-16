using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Weapon FX", menuName = "Weapons/Weapon FX/Shotgun FX")]
    public class ShotgunFX : WeaponFX
    {
        public AudioClip slideAudio;
        public AudioClip unfoldAudio;
    }
}
