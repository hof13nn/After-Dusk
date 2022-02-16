using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Grenade FX", menuName = "Grenades/FX/Grenade FX")]
    public class GrenadeFX : ScriptableObject
    {
        public AudioClip removeSaftyRing;
        public AudioClip throwGrenadeAudio;

    }
}
