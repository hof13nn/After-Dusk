using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Weapon Stats", menuName = "Weapons/Stats/Weapon Stats")]
    public class WeaponStats : ScriptableObject
    {
        public WeaponClass weaponClass;
        public WeaponFireType weaponFireType;
        public float weaponRange;
        public int weaponDamage;
        public int ammoInReserve;
        public int maxAmmoInReserve;
        public int ammoInMagazine;
        public int maxAmmoInMagazine;
        public Vector2 recoilDirection;
        public float adsSpeed;
        public Vector3 adsCamPos;
        public bool hasScope;

        private void OnEnable()
        {
            ammoInReserve = maxAmmoInReserve;
            ammoInMagazine = maxAmmoInMagazine;
        }
    }
}
