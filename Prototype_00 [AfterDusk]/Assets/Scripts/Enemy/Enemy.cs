using System;
using UnityEngine.AI;
using UnityEngine;

namespace AfterDusk
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int id;
        public int Id { get => id; }
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] protected float spotDistance;
        public float SpotDistance { get => spotDistance; }
        [SerializeField] private int spotDistanceMultiplier;
        public int SpotDistanceMultiplier { get => spotDistanceMultiplier; }
        [SerializeField] private float stopDistance;
        public float StopDistance { get => stopDistance; }
        [SerializeField] private float rotationSpeed = 2f;
        private bool isProvoked = false;
        public bool IsProvoked { get => isProvoked; set => isProvoked = value; }
        private bool isChasing = false;
        public bool IsChasing { get => isChasing; }
        private NavMeshAgent navMeshAgent;
        protected Animator enemyAnimator;
        private Action<bool, Vector3> ChaseTargetHandler;
        private Action<Vector3> RotateToPlayer;

        private void Awake()
        {
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyAnimator = GetComponent<Animator>();
            stopDistance = navMeshAgent.stoppingDistance;
            currentHealth = maxHealth;
        }

        private void OnEnable()
        {
            EnemyManager.OnPlayerDetected += ChaseTarget;
            EnemyManager.OnPlayerRanAway += StopChasingTarget;
            EnemyManager.OnPlayerAttack += AttackTarget;
        }
        private void Start()
        {
            ChaseTargetHandler = (bool isChasing, Vector3 position) =>
            {
                if (isChasing)
                {
                    this.isChasing = isChasing;
                    enemyAnimator.SetBool(EnemyAnimationTags.Attack, !isChasing);
                    enemyAnimator.SetBool(EnemyAnimationTags.Walk, isChasing);
                    navMeshAgent.SetDestination(position);
                }
                else
                {
                    this.isChasing = isChasing;
                    enemyAnimator.SetBool(EnemyAnimationTags.Walk, isChasing);
                    navMeshAgent.SetDestination(position);
                }
            };
            RotateToPlayer = (Vector3 playerPosition) =>
            {
                var direction = (playerPosition - transform.position).normalized;
                var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                var rotateTowards = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = rotateTowards;
            };
        }

        private void OnDisable()
        {
            EnemyManager.OnPlayerDetected -= ChaseTarget;
            EnemyManager.OnPlayerRanAway -= StopChasingTarget;
            EnemyManager.OnPlayerAttack -= AttackTarget;
        }

        private void ChaseTarget(int id, Vector3 playerPosition, bool isChasing)
        {
            if (id == this.id)
            {
                RotateToPlayer(playerPosition);
                ChaseTargetHandler(isChasing, playerPosition);
            }
        }

        private void StopChasingTarget(int id, bool isChasing)
        {
            if (id == this.id)
            {
                Debug.Log("I stopped chasing the Player");
                ChaseTargetHandler(isChasing, transform.position);
            }
        }

        protected virtual void AttackTarget(int id)
        {
            Debug.Log("I'm attacking player");
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(int damage, Vector3 hitPos, Vector3 hitNormal)
        {
            if (!isProvoked) isProvoked = true;

            var hitEffect = Instantiate(hitEffectPrefab, hitPos, Quaternion.LookRotation(hitNormal));

            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }
    }
}
