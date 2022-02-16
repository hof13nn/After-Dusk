using System;
using UnityEngine;

namespace AfterDusk.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera fpCamera;
        [SerializeField] private Camera mainCamera;
        public Camera MainCamera { get => mainCamera; }
        [SerializeField] private PlayerSettings settings;
        [SerializeField] private GameObject crosshair;
        [SerializeField] private GameObject body;
        private PlayerEntity player;
        private WeaponController weaponController;
        private Inventory inventory;
        private Rigidbody playerRb;
        private CapsuleCollider playerCollider;
        private Transform playerLookPosition;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float maxVelocity;
        private float groundOffset = 0.2f;
        private Vector2 lookAngles;
        private Vector2 recoilDirection = new Vector2(0, 0);
        private bool isGrounded;
        private bool isAiming;
        public bool IsAiming { get => isAiming; set => isAiming = value; }
        private bool isWalking;
        private bool isSprinting;
        private bool isCrouching;
        private bool isJumping;
        private bool isIdle;
        public static Action<bool> OnPlayerIsWalking;
        public static Action<bool> OnPlayerIsSprinting;
        public static Action<bool> OnPlayerIsCrouching;
        public static Action OnPlayerIsJumping;
        public static Action OnPlayerSit;

        private void Awake()
        {
            playerLookPosition = transform.GetChild(0);
            movementSpeed = settings.MovementSpeed;
            maxVelocity = settings.MaxWalkVelocity;
            player = GetComponent<PlayerEntity>();
            weaponController = GetComponentInChildren<WeaponController>();
            inventory = GetComponentInChildren<Inventory>();
            playerRb = GetComponent<Rigidbody>();
            playerCollider = GetComponent<CapsuleCollider>();
        }

        private void OnEnable()
        {
            InputManager.OnMovementPressed += PerformMovement;
            InputManager.OnSprintPressed += PerformSprint;
            InputManager.OnJumpPressed += PerformJump;
            InputManager.OnCrouchPressed += PerformCrouch;
            InputManager.OnMouseMove += PerformMouseMove;
            InputManager.OnADSPressed += PerformAimDownSights;
        }

        private void OnDisable()
        {
            InputManager.OnMovementPressed -= PerformMovement;
            InputManager.OnSprintPressed -= PerformSprint;
            InputManager.OnJumpPressed -= PerformJump;
            InputManager.OnCrouchPressed -= PerformCrouch;
            InputManager.OnMouseMove -= PerformMouseMove;
            InputManager.OnADSPressed -= PerformAimDownSights;
        }

        private void FixedUpdate()
        {
            GetPlayerState();
            isGrounded = Physics.Raycast(playerCollider.bounds.center, Vector3.down, playerCollider.bounds.extents.y + groundOffset);
        }

        private void PerformMovement(Vector2 inputVector)
        {
            if (playerRb.velocity.magnitude >= maxVelocity) return;

            var movementVector = new Vector3(inputVector.x, 0, inputVector.y).normalized;
            movementVector = mainCamera.transform.TransformDirection(movementVector);
            playerRb.AddForce(movementSpeed * movementVector);
        }

        private void PerformMouseMove(Vector2 currentMouseLook)
        {
            if (!inventory.IsInventoryActive)
            {
                if (!isAiming)
                {
                    lookAngles.x += recoilDirection.x + (currentMouseLook.y * (Time.deltaTime * settings.MouseSensitivityX)) * (settings.IsInverted ? 1f : -1f);
                    lookAngles.y += recoilDirection.y + currentMouseLook.x * (Time.deltaTime * settings.MouseSensitivityY);
                    recoilDirection = new Vector2(0, 0);
                }
                else
                {
                    lookAngles.x += recoilDirection.x + (currentMouseLook.y * (Time.deltaTime * settings.AdsMouseSensitivityX)) * (settings.IsInverted ? 1f : -1f);
                    lookAngles.y += recoilDirection.y + (currentMouseLook.x * (Time.deltaTime * settings.AdsMouseSensitivityY));
                    recoilDirection = new Vector2(0, 0);
                }

                lookAngles.x = Mathf.Clamp(lookAngles.x, -settings.VerticalClamp, settings.VerticalClamp);
                playerLookPosition.transform.localEulerAngles = new Vector3(lookAngles.x, lookAngles.y, 0f);
                body.transform.localEulerAngles = new Vector3(-90f, lookAngles.y, 0f);
            }
        }

        private void PerformSprint(bool isSprinting)
        {
            this.isSprinting = isSprinting;

            if (isCrouching) return;

            if (isGrounded && isSprinting)
            {
                maxVelocity = settings.MaxSprintVelocity;
            }
            else
            {
                maxVelocity = settings.MaxWalkVelocity;
            }
        }

        private void PerformJump()
        {
            if (isGrounded)
            {
                if (!isCrouching)
                {
                    OnPlayerIsJumping?.Invoke();
                    playerRb.AddRelativeForce(Vector3.up * settings.JumpForce, ForceMode.Impulse);
                }
                else
                {
                    isCrouching = false;
                    playerLookPosition.localPosition = settings.StandHeight;
                    playerCollider.height = settings.StandColliderHeight;
                    playerRb.AddRelativeForce(Vector3.up * settings.JumpForce, ForceMode.Impulse);
                    maxVelocity = settings.MaxWalkVelocity;
                }
            }
        }

        private void PerformCrouch()
        {
            if (isGrounded)
            {
                if (!isCrouching)
                {
                    isCrouching = true;
                    OnPlayerSit?.Invoke();
                    playerLookPosition.localPosition = settings.CrouchHeight;
                    playerCollider.height = settings.CrouchColliderHeight;
                    maxVelocity = settings.MaxCrouchVelocity;
                }
                else
                {
                    isCrouching = false;
                    OnPlayerSit?.Invoke();
                    playerLookPosition.localPosition = settings.StandHeight;
                    playerCollider.height = settings.StandColliderHeight;
                    maxVelocity = settings.MaxWalkVelocity;
                }
            }
        }

        private void PerformAimDownSights(bool isAiming)
        {
            this.isAiming = isAiming;

            if (weaponController.GetActiveWeapon().weapon == null) return;

            var currentWeapon = weaponController.GetActiveWeapon().weapon;
            var weaponStats = currentWeapon.WeaponStats;

            if (isAiming)
            {
                crosshair.gameObject.SetActive(false);
                fpCamera.transform.localPosition = Vector3.Lerp(fpCamera.transform.localPosition, weaponStats.adsCamPos, Time.deltaTime * weaponStats.adsSpeed);

                if (weaponStats.hasScope && Vector3.Dot(fpCamera.transform.localPosition, weaponStats.adsCamPos) > 0.9999f)
                {
                    if (currentWeapon.Scope == null) return;
                    currentWeapon.Scope.gameObject.SetActive(true);
                }
            }
            else
            {
                if (weaponStats.hasScope)
                {
                    if (currentWeapon.Scope == null) return;
                    currentWeapon.Scope.gameObject.SetActive(false);
                }
                crosshair.gameObject.SetActive(true);
                fpCamera.transform.localPosition = Vector3.Lerp(fpCamera.transform.localPosition, Vector3.zero, Time.deltaTime * weaponStats.adsSpeed);
            }
        }

        private void GetPlayerState()
        {
            if (playerRb.velocity.magnitude > 1f)
            {
                if (maxVelocity == settings.MaxWalkVelocity)
                {
                    OnPlayerIsWalking?.Invoke(true);
                }
                else if (maxVelocity == settings.MaxSprintVelocity)
                {
                    OnPlayerIsSprinting?.Invoke(true);
                }
                else if (maxVelocity == settings.MaxCrouchVelocity)
                {
                    OnPlayerIsCrouching?.Invoke(true);
                }
            }
            else
            {
                OnPlayerIsWalking?.Invoke(false);
                OnPlayerIsSprinting?.Invoke(false);
            }
        }

        public void AddRecoil(Vector2 recoilDirection)
        {
            this.recoilDirection = new Vector2(UnityEngine.Random.Range(recoilDirection.x, -recoilDirection.x), recoilDirection.y);
        }
    }
}
