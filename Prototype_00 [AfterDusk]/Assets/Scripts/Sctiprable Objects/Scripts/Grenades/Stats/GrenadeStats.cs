using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Grenade Stats", menuName = "Grenades/Stats/Grenade Stats")]
    public class GrenadeStats : ScriptableObject
    {
        public GrenadeClass grenadeClass;
        public int grenadeDamage;
        public float explodeDelay;
        public float explodeRadius;
        public float explodeForce;
        public int ammoInReserve;
        public int maxAmmoInReserve;
        public float grenadeLaunchForce;
        private void OnEnable()
        {
            ammoInReserve = maxAmmoInReserve;
        }
    }
}
