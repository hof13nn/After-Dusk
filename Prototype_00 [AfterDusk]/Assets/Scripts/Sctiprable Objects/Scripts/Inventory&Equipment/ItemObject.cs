using UnityEngine;

namespace AfterDusk
{
    public class ItemObject : ScriptableObject
    {
        public int id;
        public ItemType type;
        new public string name;
        public int amount;
        public int maxAmount;
        public bool stackable;
        public Sprite itemIcon = null;
        public GameObject itemPrefab = null;
        public GameObject pickupPrefab = null;

        public virtual void Use()
        {
            Debug.Log("Using " + name);
        }
        public virtual void Remove()
        {
            Debug.Log("Removing " + name);
        }
    }
}
