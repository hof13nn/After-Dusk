using UnityEngine;
using AfterDusk.Player;

namespace AfterDusk
{
    public class EquipmentUI : MonoBehaviour
    {
        private Equipment equipment;
        [SerializeField] private EquipmentSlot[] equipmentSlots;
        [SerializeField] private EquipmentSlot[] grenadeSlot;
        [SerializeField] private Transform equipmentPanel;
        [SerializeField] private Transform grenadePanel;

        private void OnEnable()
        {
            Equipment.OnEquipmentChange += UpdateEquipmentUI;
            Equipment.OnGrenadeChange += UpdateGrenadeUI;
        }

        private void OnDisable()
        {
            Equipment.OnEquipmentChange -= UpdateEquipmentUI;
            Equipment.OnGrenadeChange -= UpdateGrenadeUI;
        }

        private void Start()
        {
            equipment = PlayerManager.PlayerEquipment;
            equipmentSlots = equipmentPanel.GetComponentsInChildren<EquipmentSlot>();
            grenadeSlot = grenadePanel.GetComponentsInChildren<EquipmentSlot>();
        }

        private void UpdateEquipmentUI()
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (i < equipment.EquipmentItems.Length)
                {
                    if (equipment.EquipmentItems[i] != null)
                    {
                        equipmentSlots[i].AddItem(equipment.EquipmentItems[i]);
                    }
                    else
                    {
                        equipmentSlots[i].ClearSlot();
                    }
                }
            }
        }

        private void UpdateGrenadeUI()
        {
            for (int i = 0; i < grenadeSlot.Length; i++)
            {
                if (i < equipment.GrenadeItem.Length)
                {
                    if (equipment.GrenadeItem[i] != null)
                    {
                        grenadeSlot[i].AddGrenade(equipment.GrenadeItem[i]);
                    }
                    else
                    {
                        grenadeSlot[i].ClearSlot();
                    }
                }
            }
        }
    }
}