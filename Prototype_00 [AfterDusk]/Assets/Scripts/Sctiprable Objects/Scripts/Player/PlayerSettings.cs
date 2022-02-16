using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Player Settings", menuName = "Player/Settings")]
    public class PlayerSettings : ScriptableObject
    {
        #region  Player Movement Settings
        [Header("Player Movement Settings")]
        [SerializeField] private float movementSpeed;
        public float MovementSpeed { get => movementSpeed; }
        [SerializeField] private float maxWalkVelocity;
        public float MaxWalkVelocity { get => maxWalkVelocity; }
        [SerializeField] private float maxSprintVelocity;
        public float MaxSprintVelocity { get => maxSprintVelocity; }
        [SerializeField] private float maxCrouchVeloctity;
        public float MaxCrouchVelocity { get => maxCrouchVeloctity; }
        [SerializeField] private float jumpForce;
        public float JumpForce { get => jumpForce; }
        #endregion
        #region  Player Body Settings
        [Header("Player Body Settings")]
        [SerializeField] private Vector3 standLookHeight = new Vector3(0, 0.65f, 0);
        public Vector3 StandHeight { get => standLookHeight; }
        [SerializeField] private Vector3 crouchLookHeight = Vector3.zero;
        public Vector3 CrouchHeight { get => crouchLookHeight; }
        [SerializeField] private float standColliderHeight = 2f;
        public float StandColliderHeight { get => standColliderHeight; }
        [SerializeField] private float crouchColliderHeight = 1f;
        public float CrouchColliderHeight { get => crouchColliderHeight; }
        #endregion
        #region Player Mouse Look Settings
        [Header("Player Mouse Look Settings")]
        [SerializeField] private float verticalClamp;
        public float VerticalClamp { get => verticalClamp; }
        [SerializeField] [Range(5f, 20f)] private float mouseSensitivityX;
        public float MouseSensitivityX { get => mouseSensitivityX; }
        [SerializeField] [Range(0.5f, 1f)] private float adsMouseSensitivityX;
        public float AdsMouseSensitivityX { get => adsMouseSensitivityX; }
        [SerializeField] [Range(5f, 20f)] private float mouseSensitivityY;
        public float MouseSensitivityY { get => mouseSensitivityY; }
        [SerializeField] [Range(0.5f, 1f)] private float adsMouseSensitivityY;
        public float AdsMouseSensitivityY { get => adsMouseSensitivityY; }
        [SerializeField] private bool isInverted;
        public bool IsInverted { get => isInverted; }
        #endregion
    }
}