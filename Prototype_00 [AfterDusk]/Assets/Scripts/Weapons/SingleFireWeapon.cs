using System.Collections;
using UnityEngine;

namespace AfterDusk.Player
{
    public class SingleFireWeapon : Weapon
    {
        private void OnEnable()
        {
            WeaponController.OnWeaponFire += SingleFire;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            WeaponController.OnWeaponFire -= SingleFire;
        }

        private void SingleFire(bool isFired)
        {
            if (!PlayerManager.PlayerInventory.IsInventoryActive)
            {
                if (isFired)
                {
                    if (isReadyToFire && !isReloading && weaponStats.ammoInMagazine > 0)
                    {
                        isFire = true;
                        isReadyToFire = false;
                        StartCoroutine(ToggleIsReadyToFire());
                    }
                }
                else isFire = false;
            }
        }

        private IEnumerator ToggleIsReadyToFire()
        {
            yield return new WaitForSeconds(isReadyToFireDelay + 0.3f);
            isFire = false;
            isReadyToFire = true;
        }
    }
}