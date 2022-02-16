using System;
using System.Collections.Generic;
using UnityEngine;
using AfterDusk.Player;

namespace AfterDusk
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryUI;
        public GameObject InventoryUI { get => inventoryUI; set => inventoryUI = value; }
        [SerializeField] private List<ItemObject> consumableItems = new List<ItemObject>();
        public List<ItemObject> ConsumableItems { get => consumableItems; }
        [SerializeField] private List<ItemObject> weaponItems = new List<ItemObject>();
        public List<ItemObject> WeaponItems { get => weaponItems; }
        [SerializeField] private List<ItemObject> grenadeItems = new List<ItemObject>();
        public List<ItemObject> GrenadeItems { get => grenadeItems; }
        public bool IsInventoryActive { get; private set; }
        private Dictionary<int, WeaponStats> ownedWeapons = new Dictionary<int, WeaponStats>();
        public Dictionary<int, WeaponStats> OwnedWeapons { get => ownedWeapons; }
        private Dictionary<int, GrenadeStats> ownedGrenades = new Dictionary<int, GrenadeStats>();
        public Dictionary<int, GrenadeStats> OwnedGrenades { get => ownedGrenades; }
        [SerializeField] private int consumableSlots = 4;
        [SerializeField] private int weaponSlots = 6;
        [SerializeField] private int grenadeSlots = 3;
        public static event Action<bool> OnInventoryToggle;
        public static event Action OnConsumableItemChange;
        public static event Action OnWeaponItemChange;
        public static event Action OnGrenadeItemChange;

        private void Awake()
        {

        }

        private void OnEnable()
        {
            InputManager.OnInventoryPressed += InventoryToggle;
        }

        private void OnDisable()
        {
            InputManager.OnInventoryPressed -= InventoryToggle;
        }

        private void InventoryToggle()
        {
            if (!IsInventoryActive)
            {
                IsInventoryActive = true;
                OnInventoryToggle?.Invoke(true);
                inventoryUI.SetActive(IsInventoryActive);
                Cursor.lockState = CursorLockMode.None;

            }
            else
            {
                IsInventoryActive = false;
                OnInventoryToggle?.Invoke(false);
                inventoryUI.SetActive(IsInventoryActive);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void AddItem(ItemObject item)
        {
            if (consumableItems.Count < consumableSlots && item.type == ItemType.Consumable)
            {
                AddConsumableItem(item);
            }
            else if (weaponItems.Count < weaponSlots && item.type == ItemType.Weapon)
            {
                AddWeaponItem(item);
            }
            else if (grenadeItems.Count < grenadeSlots && item.type == ItemType.Grenade)
            {
                AddGrenadeItem(item);
            }
        }

        public void RemoveItem(ItemObject item)
        {
            switch (item.type)
            {
                case ItemType.Weapon:
                    RemoveWeaponItem(item);
                    break;
                case ItemType.Consumable:
                    RemoveConsumableItem(item);
                    break;
                case ItemType.Grenade:
                    RemoveGrenadeItem(item);
                    break;
            }
        }

        private void AddWeaponItem(ItemObject item)
        {
            weaponItems.Add(item);
            OnWeaponItemChange?.Invoke();
        }

        private void AddGrenadeItem(ItemObject item)
        {
            grenadeItems.Add(item);
            OnGrenadeItemChange?.Invoke();
        }

        private void AddConsumableItem(ItemObject item)
        {
            for (int i = 0; i < consumableItems.Count; i++)
            {
                if (consumableItems[i].stackable && consumableItems[i].id == item.id)
                {
                    if (item.amount >= item.maxAmount)
                    {
                        return;
                    }
                    else
                    {
                        consumableItems[i].amount++;
                        OnConsumableItemChange?.Invoke();
                        return;
                    }
                }
            }

            consumableItems.Add(item);
            OnConsumableItemChange?.Invoke();
        }

        private void RemoveWeaponItem(ItemObject item)
        {
            weaponItems.Remove(item);
            OnWeaponItemChange?.Invoke();
        }

        private void RemoveGrenadeItem(ItemObject item)
        {
            grenadeItems.Remove(item);
            OnGrenadeItemChange?.Invoke();
        }

        private void RemoveConsumableItem(ItemObject item)
        {
            if (item.amount > 1)
            {
                item.amount--;
                OnConsumableItemChange?.Invoke();
            }
            else
            {
                consumableItems.Remove(item);
                OnConsumableItemChange?.Invoke();
            }
        }

        public void GetPickup(ItemObject item)
        {
            if (ownedWeapons.ContainsKey(item.id))
            {
                foreach (var weapon in ownedWeapons)
                {
                    if (weapon.Key == item.id)
                    {
                        item.itemPrefab.GetComponent<Weapon>().WeaponStats.ammoInReserve = weapon.Value.maxAmmoInReserve;
                    }
                }
            }
            else if (ownedGrenades.ContainsKey(item.id))
            {
                foreach (var grenade in ownedGrenades)
                {
                    if (grenade.Key == item.id)
                    {
                        item.itemPrefab.GetComponent<Grenade>().GrenadeStats.ammoInReserve = grenade.Value.maxAmmoInReserve;
                    }
                }
            }
            else
            {
                AddItem(item);

                switch (item.type)
                {
                    case ItemType.Weapon:
                        ownedWeapons.Add(item.id, item.itemPrefab.GetComponent<Weapon>().WeaponStats);
                        break;
                    case ItemType.Grenade:
                        ownedGrenades.Add(item.id, item.itemPrefab.GetComponent<Grenade>().GrenadeStats);
                        break;
                }
            }
        }

        private void OnApplicationQuit()
        {
            for (int i = 0; i < consumableItems.Count; i++)
            {
                consumableItems[i].amount = 1;
            }
        }
    }
}