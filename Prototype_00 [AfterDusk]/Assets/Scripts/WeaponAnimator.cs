using System;
using UnityEngine;

namespace AfterDusk
{
    [Serializable]
    public class WeaponAnimator
    {
        private int id;
        public int Id { get => id; }
        private Animator animator;
        public Animator Animator { get => animator; }

        public WeaponAnimator(int id, Animator animator)
        {
            this.id = id;
            this.animator = animator;
        }
    }
}
