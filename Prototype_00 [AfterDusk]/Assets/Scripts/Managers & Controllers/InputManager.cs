using AfterDusk.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AfterDusk
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInputActions playerInputActions;
        private PlayerMouseLook playerMouseLook;
        private GameInputActions gameInputActions;
        private bool isAiming;
        private bool isPickingUp;

        #region Events
        public static event Action<Vector2> OnMovementPressed;
        public static event Action<Vector2> OnMouseMove;
        public static event Action<bool> OnSprintPressed;
        public static event Action OnJumpPressed;
        public static event Action OnCrouchPressed;
        public static event Action<bool> OnFirePressed;
        public static event Action<bool> OnADSPressed;
        public static event Action<bool> OnReloadPressed;
        public static event Action<bool> OnGrenadePressed;
        public static event Action OnMeleePressed;
        public static event Action OnInventoryPressed;
        public static event Action<int> OnKeyboardWeaponSwitch;
        public static event Action<float> OnMouseWheelWeaponSwtich;
        public static event Action OnRestartScenePressed;
        public static event Action<bool> OnPickupPressed;
        #endregion

        private void OnEnable()
        {
            #region Input Events
            playerInputActions = new PlayerInputActions();
            playerMouseLook = new PlayerMouseLook();
            gameInputActions = new GameInputActions();
            playerInputActions.Player.Enable();
            playerInputActions.Weapon.Enable();
            playerMouseLook.Mouse.Enable();
            gameInputActions.Game.Enable();
            playerInputActions.Player.Jump.performed += context => OnJumpPressed?.Invoke();
            playerInputActions.Player.Crouch.performed += context => OnCrouchPressed?.Invoke();
            playerInputActions.Player.Sprint.started += context => OnSprintPressed?.Invoke(true);
            playerInputActions.Player.Sprint.canceled += context => OnSprintPressed?.Invoke(false);
            playerInputActions.Weapon.AimDownSights.performed += context => isAiming = true;
            playerInputActions.Weapon.AimDownSights.canceled += context => isAiming = false;
            playerInputActions.Player.Inventory.performed += context => OnInventoryPressed?.Invoke();
            playerInputActions.Weapon.Fire.performed += context => OnFirePressed?.Invoke(true);
            playerInputActions.Weapon.Fire.canceled += context => OnFirePressed?.Invoke(false);
            playerInputActions.Weapon.Reload.performed += context => OnReloadPressed?.Invoke(true);
            playerInputActions.Weapon.Reload.canceled += context => OnReloadPressed?.Invoke(false);
            playerInputActions.Weapon.ThrowGrenade.performed += context => OnGrenadePressed?.Invoke(true);
            playerInputActions.Weapon.ThrowGrenade.canceled += context => OnGrenadePressed?.Invoke(false);
            playerInputActions.Weapon.MeleeAttack.performed += context => OnMeleePressed?.Invoke();
            playerInputActions.Player.Pickup.performed += context => isPickingUp = true;
            playerInputActions.Player.Pickup.canceled += context => isPickingUp = false;
            playerInputActions.Player.SwitchWeaponKeyes.performed += context => OnKeyboardWeaponSwitch?.Invoke((int)context.ReadValue<float>());
            gameInputActions.Game.Restart.performed += context => OnRestartScenePressed?.Invoke();
            #endregion
        }

        private void OnDisable()
        {
            #region Input Events
            playerInputActions.Player.Disable();
            playerInputActions.Weapon.Disable();
            playerMouseLook.Mouse.Disable();
            gameInputActions.Game.Disable();
            playerInputActions.Player.Jump.performed -= context => OnJumpPressed?.Invoke();
            playerInputActions.Player.Crouch.performed -= context => OnCrouchPressed?.Invoke();
            playerInputActions.Player.Sprint.performed -= context => OnSprintPressed?.Invoke(true);
            playerInputActions.Player.Sprint.canceled -= context => OnSprintPressed?.Invoke(false);
            playerInputActions.Weapon.AimDownSights.performed -= context => isAiming = true;
            playerInputActions.Weapon.AimDownSights.canceled -= context => isAiming = false;
            playerInputActions.Player.Inventory.performed -= context => OnInventoryPressed?.Invoke();
            playerInputActions.Weapon.Fire.performed -= context => OnFirePressed?.Invoke(true);
            playerInputActions.Weapon.Fire.canceled -= context => OnFirePressed?.Invoke(false);
            playerInputActions.Weapon.Reload.performed -= context => OnReloadPressed?.Invoke(true);
            playerInputActions.Weapon.Reload.canceled -= context => OnReloadPressed?.Invoke(false);
            playerInputActions.Weapon.ThrowGrenade.performed -= context => OnGrenadePressed?.Invoke(true);
            playerInputActions.Weapon.ThrowGrenade.canceled -= context => OnGrenadePressed?.Invoke(false);
            playerInputActions.Weapon.MeleeAttack.performed -= context => OnMeleePressed?.Invoke();
            #endregion
        }

        private void Update()
        {
            MouseLookRotation();
            MouseWheelInput();
            AimDownSights();
            PickupItem();
        }

        private void FixedUpdate()
        {
            PerfomMovement();
        }

        private void PerfomMovement()
        {
            OnMovementPressed?.Invoke(playerInputActions.Player.Movement.ReadValue<Vector2>());
        }

        private void MouseLookRotation()
        {
            OnMouseMove?.Invoke(playerMouseLook.Mouse.Look.ReadValue<Vector2>());
        }

        private void AimDownSights()
        {
            if (isAiming)
            {
                OnADSPressed?.Invoke(true);
            }
            else
            {
                OnADSPressed?.Invoke(false);
            }
        }

        private void PickupItem()
        {
            if (isPickingUp)
            {
                OnPickupPressed?.Invoke(true);
            }
            else
            {
                OnPickupPressed?.Invoke(false);
            }
        }

        private void MouseWheelInput()
        {
            OnMouseWheelWeaponSwtich?.Invoke(playerInputActions.Player.SwitchWeaponWheel.ReadValue<float>());
        }
    }
}
