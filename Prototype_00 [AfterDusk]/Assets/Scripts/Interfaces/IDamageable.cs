using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    public interface IDamageable
    {
        void TakeDamage(int damage, Vector3 hitPos, Vector3 hitNormal);
        void Die();
    }
}
