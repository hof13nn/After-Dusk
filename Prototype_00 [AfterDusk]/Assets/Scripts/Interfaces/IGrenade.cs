using UnityEngine;

namespace AfterDusk
{
    public interface IGrenade
    {
        void OnTriggerEnter(Collider target);
        void GrenadeEffect();
    }
}
