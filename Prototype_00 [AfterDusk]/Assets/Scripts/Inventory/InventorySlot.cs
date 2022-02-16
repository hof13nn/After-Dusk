using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AfterDusk.Player;

namespace AfterDusk
{
    public class InventorySlot : MonoBehaviour
    {
        private ItemObject item;
        private Inventory inventory;
        private Equipment equipment;
        private GameObject itemPickup;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;
        [SerializeField] private Button itemButton;
        [SerializeField] private GameObject removeButton;

        private void Awake()
        {
            inventory = PlayerManager.PlayerInventory;
            equipment = PlayerManager.PlayerEquipment;
            amount = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        }

        public void AddItem(ItemObject newItem)
        {
            item = newItem;
            itemPickup = newItem.pickupPrefab;
            icon.sprite = newItem.itemIcon;
            icon.enabled = true;
            itemButton.interactable = true;
            removeButton.SetActive(true);
            if (newItem.type == ItemType.Weapon || newItem.type == ItemType.Grenade)
            {
                amount.text = "";
            }
            else
            { amount.text = newItem.amount.ToString(); }
        }

        public void ClearSlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            itemButton.interactable = false;
            removeButton.SetActive(false);
            amount.text = "";
        }

        public void OnRemoveButton()
        {
            switch (item.type)
            {
                case ItemType.Consumable:
                    inventory.RemoveItem(item);
                    break;
                case ItemType.Weapon:
                    //ThrowItem();
                    inventory.OwnedWeapons.Remove(item.id);
                    inventory.RemoveItem(item);
                    break;
                case ItemType.Grenade:
                    //ThrowItem();
                    inventory.OwnedGrenades.Remove(item.id);
                    inventory.RemoveItem(item);
                    break;
                default:
                    inventory.RemoveItem(item);
                    break;
            }
        }

        public void OnUseButton()
        {
            switch (item.type)
            {
                case ItemType.Weapon:
                    equipment.EquipWeapon((WeaponObject)item);
                    break;
                case ItemType.Grenade:
                    equipment.EquipGrenade((GrenadeObject)item);
                    break;
                case ItemType.Consumable:
                    item.Use();
                    inventory.RemoveItem(item);
                    break;
                default:
                    Debug.LogWarning(item.name + " cannot be equiped");
                    break;
            }

        }

        /* private void ThrowItem()
        {
            var instPos = Player.instance.InstPos.position;
            var pickupRB = Instantiate(itemPickup, instPos, Quaternion.identity).GetComponent<Rigidbody>();
            pickupRB.useGravity = true;
            pickupRB.isKinematic = false;
            pickupRB.AddForce(Vector3.forward * 2f, ForceMode.Impulse);
        } */

    }
}
