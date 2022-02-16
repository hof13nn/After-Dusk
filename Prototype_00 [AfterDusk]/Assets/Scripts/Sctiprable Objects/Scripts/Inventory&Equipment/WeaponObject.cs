using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
    public class WeaponObject : ItemObject
    {
        public WeaponType weaponType;
    }
}
