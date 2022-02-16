using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk.Player
{
    public class AutoFireWeapon : Weapon
    {

        private void OnEnable()
        {
            WeaponController.OnWeaponFire += AutoFire;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            WeaponController.OnWeaponFire -= AutoFire;
        }

        private void AutoFire(bool isFired)
        {
            if (!PlayerManager.PlayerInventory.IsInventoryActive)
            {
                if (isFired)
                {
                    if (weaponStats.ammoInMagazine > 0 && !isReloading)
                    {
                        isFire = true;
                    }
                }
                else isFire = false;
            }
        }
    }
}
