using UnityEngine;

namespace AfterDusk.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private static PlayerEntity playerEntity;
        public static PlayerEntity PlayerEntity { get => playerEntity; }
        private static PlayerHealth playerHealth;
        public static PlayerHealth PlayerHealth { get => playerHealth; }
        private static PlayerController playerController;
        public static PlayerController PlayerController { get => playerController; }
        private static Inventory playerInventory;
        public static Inventory PlayerInventory { get => playerInventory; }
        private static Equipment playerEquipment;
        public static Equipment PlayerEquipment { get => playerEquipment; }
        private static WeaponController weaponController;
        public static WeaponController WeaponController { get => weaponController; }
        private static GrenadeController grenadeController;
        public static GrenadeController GrenadeController { get => grenadeController; }

        private void OnEnable()
        {
            bool a = playerEntity = GetComponentInParent<PlayerEntity>();
            bool b = playerController = playerEntity.GetComponent<PlayerController>();
            bool c = playerInventory = playerEntity.GetComponentInChildren<Inventory>();
            bool d = playerEquipment = playerEntity.GetComponentInChildren<Equipment>();
            bool e = weaponController = playerEntity.GetComponentInChildren<WeaponController>();
            bool f = grenadeController = playerEntity.GetComponentInChildren<GrenadeController>();
            bool g = playerHealth = playerEntity.GetComponent<PlayerHealth>();
            string test = $"Player exists: {a}@PlayerController exists: {b}@PlayerInventory exists: {c}@PlayerEquipment exists: {d}@WeaponController exists: {e}@GrenadeController exists: {e}@PlayerHealth exists: {g}";
            test = test.Replace("@", "" + System.Environment.NewLine);
            Debug.Log(test);
        }
    }
}
