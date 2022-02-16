using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AfterDusk.Player;


namespace AfterDusk
{
    public class Zombie : Enemy
    {
        [SerializeField] private int damage = 10;

        protected override void AttackTarget(int id)
        {
            base.AttackTarget(id);

            if (id == this.id)
            {
                if (enemyAnimator.GetBool(EnemyAnimationTags.Walk))
                {
                    enemyAnimator.SetBool(EnemyAnimationTags.Walk, false);
                }

                enemyAnimator.SetBool(EnemyAnimationTags.Attack, true);
            }
        }

        public void AttackEvent()
        {
            var player = PlayerManager.PlayerHealth;
            if (player == null) return;
            player.TakeDamage(damage);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawWireSphere(transform.position, spotDistance);
        }
    }
}
