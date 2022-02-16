using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Weapon FX", menuName = "Weapons/Weapon FX/Sniper Rifle FX")]
    public class SniperRifleFX : WeaponFX
    {
        public AudioClip ejectMagAudio;
        public AudioClip insertMagAudio;
        public AudioClip backwardSlideAudio;
        public AudioClip forwardSlideAudio;
    }
}
