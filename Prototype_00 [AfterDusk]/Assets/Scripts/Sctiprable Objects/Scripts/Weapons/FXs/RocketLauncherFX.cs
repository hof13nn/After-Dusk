using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Weapon FX", menuName = "Weapons/Weapon FX/Rocket Launcher FX")]
    public class RocketLauncherFX : WeaponFX
    {
        public AudioClip backwardSlideAudio;
        public AudioClip forwardSlideAudio;
    }
}
