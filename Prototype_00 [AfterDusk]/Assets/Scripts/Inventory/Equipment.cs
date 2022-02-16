using System;
using UnityEngine;
using AfterDusk.Player;
using System.Linq;

namespace AfterDusk
{
    public class Equipment : MonoBehaviour
    {
        [SerializeField] private GameObject equipmentUI;
        public GameObject EquipmentUI { get => equipmentUI; set => equipmentUI = value; }
        [SerializeField] private WeaponObject[] equipmentItems;
        public WeaponObject[] EquipmentItems { get => equipmentItems; }
        [SerializeField] private GrenadeObject[] grenadeItem;
        public GrenadeObject[] GrenadeItem { get => grenadeItem; }
        private Inventory inventory;
        private WeaponController weaponController;
        private GrenadeController grenadeController;
        private int grenadeIndex = 0;
        private bool isWeaponSwaped;
        public static event Action OnEquipmentChange;
        public static event Action OnGrenadeChange;

        private void OnEnable()
        {
            Inventory.OnInventoryToggle += EquipmentToggle;
        }

        private void OnDisable()
        {
            Inventory.OnInventoryToggle -= EquipmentToggle;
        }

        private void Start()
        {
            inventory = PlayerManager.PlayerInventory;
            weaponController = PlayerManager.WeaponController;
            grenadeController = PlayerManager.GrenadeController;
            int slotsNumber = System.Enum.GetNames(typeof(WeaponType)).Length;
            equipmentItems = new WeaponObject[slotsNumber];
            grenadeItem = new GrenadeObject[1];
        }

        private void EquipmentToggle(bool IsInventoryActive)
        {
            if (IsInventoryActive)
            {
                equipmentUI.SetActive(true);
            }
            else
            {
                equipmentUI.SetActive(false);
            }

        }

        public void EquipWeapon(WeaponObject item)
        {
            int slotIndex = (int)item.weaponType;

            if (equipmentItems[slotIndex] != null)
            {
                SwapWeapons(item, slotIndex);
                OnEquipmentChange?.Invoke();
            }
            else
            {
                equipmentItems[slotIndex] = item;
                inventory.RemoveItem(item);
                SetWeaponActive(item);
                OnEquipmentChange?.Invoke();
            }
        }
        public void EquipGrenade(GrenadeObject item)
        {
            if (grenadeItem[grenadeIndex] != null)
            {
                SwapGrenades(item, grenadeIndex);
                OnGrenadeChange?.Invoke();
            }
            else
            {
                grenadeItem[grenadeIndex] = item;
                inventory.RemoveItem(item);
                SetGrenadeActive(item);
                OnGrenadeChange?.Invoke();
            }
        }

        public void UnequipWeapon(WeaponObject item)
        {
            int slotIndex = (int)item.weaponType;
            equipmentItems[slotIndex] = null;
            SetWeaponInactive(item);
            OnEquipmentChange?.Invoke();
        }

        public void UnequipGrenade(GrenadeObject item)
        {
            grenadeItem[grenadeIndex] = null;
            SetGrenadeInactive(item);
            OnGrenadeChange?.Invoke();
        }

        private void SwapWeapons(WeaponObject item, int slotIndex)
        {
            var tempItem = equipmentItems[slotIndex];
            UnequipWeapon(tempItem);
            SetWeaponInactive(tempItem);
            EquipWeapon(item);
            SetWeaponActive(item);
            inventory.AddItem(tempItem);
        }

        private void SwapGrenades(GrenadeObject item, int slotIndex)
        {
            var tempItem = grenadeItem[grenadeIndex];
            UnequipGrenade(tempItem);
            SetGrenadeInactive(tempItem);
            EquipGrenade(item);
            SetGrenadeActive(item);
            inventory.AddItem(tempItem);
        }

        private void SetWeaponActive(WeaponObject item)
        {
            int slotIndex = (int)item.weaponType;
            var currentWeapon = weaponController.GetActiveWeapon();

            for (int i = 0; i < weaponController.InactiveWeaponsList.Count; i++)
            {
                var weapon = weaponController.InactiveWeaponsList[i];

                if (weapon.GetComponent<Weapon>().ID == item.id)
                {
                    weaponController.ActiveWeaponsArray[slotIndex] = weapon;
                    weaponController.InactiveWeaponsList.Remove(weapon);
                    weapon.transform.parent = weaponController.ActiveWeapons.transform;

                    if (currentWeapon.weapon.ID == -1)
                    {
                        weaponController.CurrentWeapon = weapon;
                        weaponController.WeaponNumber = slotIndex;
                    }
                }
            }
        }

        private void SetGrenadeActive(GrenadeObject item)
        {
            GameObject[] tempArray = grenadeController.InactiveGrenadesList.ToArray();

            for (int i = 0; i < tempArray.Length; i++)
            {
                var grenade = tempArray[i];

                if (grenade.GetComponent<Grenade>().ID == item.id)
                {
                    grenadeController.ActiveGrenadeList.Add(grenade);
                    grenade.transform.parent = grenadeController.ActiveGrenade.transform;
                    grenadeController.InactiveGrenadesList.Remove(grenade);
                }
            }
        }

        private void SetWeaponInactive(WeaponObject item)
        {
            int slotIndex = (int)item.weaponType;

            for (int i = 0; i < weaponController.ActiveWeaponsArray.Length; i++)
            {
                var weapon = weaponController.ActiveWeaponsArray[i];

                if (weapon != null && weapon.GetComponent<Weapon>().ID == item.id)
                {
                    weaponController.InactiveWeaponsList.Add(weapon);
                    weaponController.ActiveWeaponsArray[i].SetActive(false);
                    weaponController.ActiveWeaponsArray[i].transform.parent = weaponController.InactiveWeapons.transform;
                    weaponController.ActiveWeaponsArray[slotIndex] = null;
                    weaponController.FindWeaponToSet(slotIndex);
                }
            }
        }

        private void SetGrenadeInactive(GrenadeObject item)
        {
            GameObject[] tempArray = grenadeController.ActiveGrenadeList.ToArray();

            for (int i = 0; i < tempArray.Length; i++)
            {
                var grenade = tempArray[i];

                if (grenade.GetComponent<Grenade>().ID == item.id)
                {
                    grenadeController.InactiveGrenadesList.Add(grenade);
                    grenade.transform.parent = grenadeController.InactiveGrenades.transform;
                    grenadeController.ActiveGrenadeList.Remove(grenade);
                }
            }
        }
    }
}