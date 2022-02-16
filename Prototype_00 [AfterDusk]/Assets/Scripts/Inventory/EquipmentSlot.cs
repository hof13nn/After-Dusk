using UnityEngine;
using UnityEngine.UI;
using AfterDusk.Player;

namespace AfterDusk
{
    public class EquipmentSlot : MonoBehaviour
    {
        private ItemObject item;
        private Inventory inventory;
        private Equipment equipment;
        [SerializeField] private Image icon;
        [SerializeField] private GameObject removeButton;

        private void Awake()
        {
            inventory = PlayerManager.PlayerInventory;
            equipment = PlayerManager.PlayerEquipment;
        }

        public void AddItem(WeaponObject newItem)
        {
            item = newItem;
            icon.sprite = newItem.itemIcon;
            icon.enabled = true;
            removeButton.SetActive(true);
        }

        public void AddGrenade(GrenadeObject newItem)
        {
            item = newItem;
            icon.sprite = newItem.itemIcon;
            icon.enabled = true;
            removeButton.SetActive(true);
        }

        public void ClearSlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            removeButton.SetActive(false);
        }

        public void OnRemoveButton()
        {
            inventory.AddItem(item);

            switch (item.type)
            {
                case ItemType.Weapon:
                    equipment.UnequipWeapon((WeaponObject)item);
                    break;
                case ItemType.Grenade:
                    equipment.UnequipGrenade((GrenadeObject)item);
                    break;
            }
        }
    }
}
