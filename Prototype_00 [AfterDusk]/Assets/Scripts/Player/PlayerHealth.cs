using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
        }
    }
}
