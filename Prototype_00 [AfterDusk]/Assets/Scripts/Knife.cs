using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    public class Knife : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private AudioClip meleeAttackAudio;
        public AudioClip MeleeAttackAudio { get => meleeAttackAudio; }
        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy)
            {
                Debug.Log($"Trying to stab: {enemy.gameObject.name} with damage {damage}");
                enemy.TakeDamage(damage);
            }
        }
    }
}
