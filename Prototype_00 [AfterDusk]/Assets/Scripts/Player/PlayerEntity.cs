using UnityEngine;

namespace AfterDusk.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(PlayerHealth))]
    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AudioClip[] stepAudioClips;
        [SerializeField] private AudioClip jumpAudio;
        [SerializeField] private AudioClip crouchAudio;
        [SerializeField] private float walkVolumeMin = 0.2f;
        [SerializeField] private float walkVolumeMax = 0.6f;
        [SerializeField] private float sprintVolume = 1f;
        [SerializeField] private float crouchVolume = 0.1f;
        private AudioSource audioSource;
        private float walkStep = 0.6f;
        private float sprintStep = 0.4f;
        private float crouchStep = 0.7f;
        private float stepTime;

        private void Awake() => audioSource = GetComponent<AudioSource>();

        private void OnEnable()
        {
            PlayerController.OnPlayerIsWalking += PlayWalkSteps;
            PlayerController.OnPlayerIsSprinting += PlaySprintSteps;
            PlayerController.OnPlayerIsCrouching += PlayCrouchSteps;
            PlayerController.OnPlayerIsJumping += PlayJumpAudio;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerIsWalking -= PlayWalkSteps;
            PlayerController.OnPlayerIsSprinting -= PlaySprintSteps;
            PlayerController.OnPlayerIsCrouching -= PlayCrouchSteps;
            PlayerController.OnPlayerIsJumping -= PlayJumpAudio;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            stepTime = walkStep;
        }

        private void PlayWalkSteps(bool isWalking)
        {
            if (isWalking)
            {
                stepTime += Time.deltaTime;

                if (stepTime >= walkStep)
                {
                    audioSource.volume = Random.Range(walkVolumeMin, walkVolumeMax);
                    audioSource.clip = stepAudioClips[Random.Range(0, stepAudioClips.Length)];
                    audioSource.Play();
                    stepTime = 0;
                }
            }

        }

        private void PlaySprintSteps(bool isSprinting)
        {
            if (isSprinting)
            {
                stepTime += Time.deltaTime;

                if (stepTime >= sprintStep)
                {
                    audioSource.volume = sprintVolume;
                    audioSource.clip = stepAudioClips[Random.Range(0, stepAudioClips.Length)];
                    audioSource.Play();
                    stepTime = 0;
                }
            }
        }

        private void PlayCrouchSteps(bool isCrouching)
        {
            if (isCrouching)
            {
                stepTime += Time.deltaTime;

                if (stepTime >= crouchStep)
                {
                    audioSource.volume = crouchVolume;
                    audioSource.clip = stepAudioClips[Random.Range(0, stepAudioClips.Length)];
                    audioSource.Play();
                    stepTime = 0;
                }
            }
        }

        private void PlayJumpAudio()
        {
            audioSource.PlayOneShot(jumpAudio);
        }
    }
}
