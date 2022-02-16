using AfterDusk.Player;
using System;
using UnityEngine;

namespace AfterDusk
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Enemy[] enemies;
        [SerializeField] private Transform enemySpawner;
        private PlayerEntity player;
        private float distanceToPlayer;
        public static event Action<int, Vector3, bool> OnPlayerDetected;
        public static event Action<int, bool> OnPlayerRanAway;
        public static event Action<int> OnPlayerAttack;

        private void Start()
        {
            enemies = enemySpawner.GetComponentsInChildren<Enemy>();
            player = PlayerManager.PlayerEntity;
        }

        private void Update()
        {
            EnemyMovement();
        }

        private void EnemyMovement()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                distanceToPlayer = Vector3.Distance(player.transform.position, enemies[i].transform.position);

                var id = enemies[i].Id;
                var spotDistance = enemies[i].SpotDistance;
                var stopDistance = enemies[i].StopDistance;
                var provokedSpotDistance = enemies[i].SpotDistance * enemies[i].SpotDistanceMultiplier;

                if (!enemies[i].IsProvoked)
                {
                    if (!enemies[i].IsChasing)
                    {
                        if (distanceToPlayer <= spotDistance) OnPlayerDetected?.Invoke(id, player.transform.position, true);
                    }
                    else
                    {
                        if (distanceToPlayer > spotDistance) OnPlayerRanAway?.Invoke(id, false);
                        else if (distanceToPlayer <= stopDistance) OnPlayerAttack?.Invoke(id);
                        else OnPlayerDetected?.Invoke(id, player.transform.position, true);
                    }
                }
                else
                {
                    if (distanceToPlayer <= spotDistance) enemies[i].IsProvoked = false;
                    else if (distanceToPlayer <= provokedSpotDistance) OnPlayerDetected?.Invoke(id, player.transform.position, true);
                }
            }
        }
    }
}