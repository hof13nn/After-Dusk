using UnityEngine;

namespace AfterDusk
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private ItemObject itemObject;
        public ItemObject ItemObject { get { return itemObject; } }
        private Rigidbody pickupRB;

        private void OnEnable()
        {
            pickupRB = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            var ground = other.gameObject.name == "Ground";

            if (ground)
            {
                pickupRB.useGravity = false;
                pickupRB.isKinematic = true;
            }
        }
    }
}
