using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk.Player
{
    public class AnimationManager : MonoBehaviour
    {
        private List<WeaponAnimator> weaponAnimators = new List<WeaponAnimator>();

        private void OnEnable()
        {
            InputManager.OnMeleePressed += MeleeAttack;
            WeaponController.OnWeaponManualReload += ManualReload;
            WeaponController.OnWeaponAutoReload += AutoReload;
            WeaponController.OnWeaponFire += Fire;
            GrenadeController.OnGrenadeThrow += ThrowGrenade;
            PlayerController.OnPlayerIsWalking += Walk;
            PlayerController.OnPlayerIsSprinting += Sprint;
            PlayerController.OnPlayerSit += Crouch;
            PlayerController.OnPlayerIsJumping += Jump;
        }

        private void OnDisable()
        {
            InputManager.OnMeleePressed -= MeleeAttack;
            WeaponController.OnWeaponManualReload -= ManualReload;
            WeaponController.OnWeaponAutoReload -= AutoReload;
            WeaponController.OnWeaponFire -= Fire;
            GrenadeController.OnGrenadeThrow -= ThrowGrenade;
        }

        private void Start()
        {
            weaponAnimators.Add(new WeaponAnimator(PlayerManager.WeaponController.GetActiveWeapon().weapon.ID, PlayerManager.WeaponController.GetActiveWeapon().weaponAnimator));

            for (int i = 0; i < PlayerManager.WeaponController.InactiveWeaponsList.Count; i++)
            {
                var weapon = PlayerManager.WeaponController.InactiveWeaponsList[i];
                weaponAnimators.Add(new WeaponAnimator(weapon.GetComponent<Weapon>().ID, weapon.GetComponent<Animator>()));
            }
        }

        private void Fire(bool isFired)
        {
            if (PlayerManager.PlayerInventory.IsInventoryActive) return;

            var weaponFireType = PlayerManager.WeaponController.GetActiveWeapon().weapon.WeaponStats.weaponFireType;
            var isReadyToFire = PlayerManager.WeaponController.GetActiveWeapon().weapon.IsReadyToFire;

            if (isFired && isReadyToFire)
            {
                if (weaponFireType == WeaponFireType.SingleFire)
                {
                    GetActiveAnimator().SetTrigger(PlayerAnimationTags.SingleFire);
                }
                else if (weaponFireType == WeaponFireType.AutoFire)
                {
                    GetActiveAnimator().SetBool(PlayerAnimationTags.AutoFire, true);
                }
            }
            else
            {
                if (weaponFireType == WeaponFireType.AutoFire)
                {
                    GetActiveAnimator().SetBool(PlayerAnimationTags.AutoFire, false);
                }
            }
        }

        private void ManualReload(bool isManualReload)
        {
            var isReloading = PlayerManager.WeaponController.GetActiveWeapon().weapon.IsReloading;

            if (isManualReload && !isReloading)
            {
                GetActiveAnimator().SetTrigger(PlayerAnimationTags.Reload);
            }
        }

        private void AutoReload(bool isAutoReload)
        {
            var weaponStats = PlayerManager.WeaponController.GetActiveWeapon().weapon.WeaponStats;
            var isReloading = PlayerManager.WeaponController.GetActiveWeapon().weapon.IsReloading;

            if (isAutoReload && !isReloading)
            {
                if (weaponStats.weaponClass == WeaponClass.Shotgun) GetActiveAnimator().SetTrigger(PlayerAnimationTags.Reload);
                else GetActiveAnimator().SetTrigger(PlayerAnimationTags.EmptyReload);
            }
        }

        private void MeleeAttack()
        {
            GetActiveAnimator().SetTrigger(PlayerAnimationTags.MeleeAttack);
        }

        private void ThrowGrenade(GrenadeClass grenadeClass, bool isReadyToThrowGrenade)
        {
            if (isReadyToThrowGrenade)
            {
                switch (grenadeClass)
                {
                    case GrenadeClass.Explosive:
                        GetActiveAnimator().SetTrigger(PlayerAnimationTags.ThrowExplosiveGrenade);
                        return;
                    case GrenadeClass.Smoke:
                        GetActiveAnimator().SetTrigger(PlayerAnimationTags.ThrowSmokeGrenade);
                        return;
                }
            }
        }

        private void Walk(bool isWalking)
        {
            if (GetActiveAnimator().GetBool(PlayerAnimationTags.Sprint)) GetActiveAnimator().SetBool(PlayerAnimationTags.Sprint, false);
            GetActiveAnimator().SetBool(PlayerAnimationTags.Walk, isWalking);
        }

        private void Sprint(bool isSprinting)
        {
            if (GetActiveAnimator().GetBool(PlayerAnimationTags.Walk)) GetActiveAnimator().SetBool(PlayerAnimationTags.Walk, false);
            GetActiveAnimator().SetBool(PlayerAnimationTags.Sprint, isSprinting);
        }

        private void Crouch()
        {
            GetActiveAnimator().SetTrigger(PlayerAnimationTags.Crouch);
        }

        private void Jump()
        {
            GetActiveAnimator().SetTrigger(PlayerAnimationTags.Jump);
        }

        private Animator GetActiveAnimator()
        {
            for (int i = 0; i < weaponAnimators.Count; i++)
            {
                if (PlayerManager.WeaponController.GetActiveWeapon().weapon.ID == weaponAnimators[i].Id)
                {
                    return weaponAnimators[i].Animator;
                }
            }
            return null;
        }
    }
}