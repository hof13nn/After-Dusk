using UnityEngine;
using AfterDusk.Player;
using System;

namespace AfterDusk
{
    public class InventoryUI : MonoBehaviour
    {
        private Inventory inventory;
        private InventorySlot[] weaponSlots;
        private InventorySlot[] grenadeSlots;
        private InventorySlot[] consumableSlots;
        [SerializeField] private Transform weaponPanel;
        [SerializeField] private Transform grenadePanel;
        [SerializeField] private Transform consumablePanel;

        private void OnEnable()
        {
            Inventory.OnWeaponItemChange += UpdateWeaponUI;
            Inventory.OnGrenadeItemChange += UpdateGrenadeUI;
            Inventory.OnConsumableItemChange += UpdateConsumableUI;
        }

        private void OnDisable()
        {
            Inventory.OnWeaponItemChange -= UpdateWeaponUI;
            Inventory.OnGrenadeItemChange -= UpdateGrenadeUI;
            Inventory.OnConsumableItemChange -= UpdateConsumableUI;
        }

        void Start()
        {
            inventory = PlayerManager.PlayerInventory;
            weaponSlots = weaponPanel.GetComponentsInChildren<InventorySlot>();
            grenadeSlots = grenadePanel.GetComponentsInChildren<InventorySlot>();
            consumableSlots = consumablePanel.GetComponentsInChildren<InventorySlot>();
        }

        private void UpdateWeaponUI()
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (i < inventory.WeaponItems.Count)
                {
                    weaponSlots[i].AddItem(inventory.WeaponItems[i]);
                }
                else
                {
                    weaponSlots[i].ClearSlot();
                }
            }
        }

        private void UpdateGrenadeUI()
        {
            for (int i = 0; i < grenadeSlots.Length; i++)
            {
                if (i < inventory.GrenadeItems.Count)
                {
                    grenadeSlots[i].AddItem(inventory.GrenadeItems[i]);
                }
                else
                {
                    grenadeSlots[i].ClearSlot();
                }
            }
        }

        private void UpdateConsumableUI()
        {
            for (int i = 0; i < consumableSlots.Length; i++)
            {
                if (i < inventory.ConsumableItems.Count)
                {
                    consumableSlots[i].AddItem(inventory.ConsumableItems[i]);
                }
                else
                {
                    consumableSlots[i].ClearSlot();
                }
            }
        }
    }
}
