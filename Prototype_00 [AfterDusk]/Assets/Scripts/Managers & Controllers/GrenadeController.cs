using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk.Player
{
    public class GrenadeController : MonoBehaviour
    {
        [SerializeField] private Transform activeGrenade;
        public Transform ActiveGrenade { get => activeGrenade; }
        [SerializeField] private Transform inactiveGrenades;
        public Transform InactiveGrenades { get => inactiveGrenades; }
        [SerializeField] private GrenadeObject[] grenadesPool;
        [SerializeField] private List<GameObject> activeGrenadeList;
        public List<GameObject> ActiveGrenadeList { get => activeGrenadeList; set => activeGrenadeList = value; }
        [SerializeField] private List<GameObject> inactiveGrenadesList;
        public List<GameObject> InactiveGrenadesList { get => inactiveGrenadesList; }
        [SerializeField] private Transform grenadeStartPos;
        private bool isGrenadeThrown;
        private bool isReadyToThrowGrenade = true;
        private float grenadeThrowDelay = 1.2f;
        public static event Action OnGrenadeUIUpdate;
        public static event Action<GrenadeClass, bool> OnGrenadeThrow;

        private void Awake()
        {
            InitializePool();
        }

        private void OnEnable()
        {
            InputManager.OnGrenadePressed += ThrowGrenade;
        }

        private void OnDisable()
        {
            InputManager.OnGrenadePressed -= ThrowGrenade;
        }

        private void ThrowGrenade(bool isGrenadeThrown)
        {
            this.isGrenadeThrown = isGrenadeThrown;

            if (isGrenadeThrown && isReadyToThrowGrenade && GetActiveGrenade().grenadeStats.ammoInReserve != 0)
            {
                var grenadeClass = GetActiveGrenade().grenadeStats.grenadeClass;

                switch (grenadeClass)
                {
                    case GrenadeClass.Explosive:
                        OnGrenadeThrow?.Invoke(grenadeClass, isReadyToThrowGrenade);
                        isReadyToThrowGrenade = !isReadyToThrowGrenade;
                        StartCoroutine(ToggleGrenadeThrow());
                        return;
                    case GrenadeClass.Smoke:
                        OnGrenadeThrow?.Invoke(grenadeClass, isReadyToThrowGrenade);
                        isReadyToThrowGrenade = !isReadyToThrowGrenade;
                        StartCoroutine(ToggleGrenadeThrow());
                        return;
                    default:
                        Debug.Log("No action for this grenade");
                        return;
                }
            }
        }

        private void InitializePool()
        {
            foreach (ItemObject grenade in grenadesPool)
            {
                var grenadeObject = grenade.itemPrefab.GetComponent<Grenade>();
                var amount = grenadeObject.GrenadeStats.maxAmmoInReserve;

                for (int i = 0; i < amount; i++)
                {
                    GameObject instance = Instantiate(grenade.itemPrefab) as GameObject;
                    instance.transform.parent = inactiveGrenades;
                    inactiveGrenadesList.Add(instance);
                    instance.transform.localPosition = grenadeStartPos.transform.position;
                    instance.transform.localRotation = grenade.itemPrefab.transform.rotation;
                    instance.gameObject.name = grenade.itemPrefab.gameObject.name;
                    instance.gameObject.GetComponent<Grenade>().ID = grenade.id;
                }
            }
        }

        public (Grenade grenade, GrenadeStats grenadeStats, GrenadeFX grenadeFX) GetActiveGrenade()
        {
            if (PlayerManager.PlayerEquipment.GrenadeItem[0] != null)
            {
                var grenade = PlayerManager.PlayerEquipment.GrenadeItem[0].itemPrefab.GetComponent<Grenade>();
                var grenadeStats = grenade.GrenadeStats;
                var grenadeFX = grenade.GrenadeFX;
                return (grenade, grenadeStats, grenadeFX);
            }

            return (null, null, null);
        }

        private IEnumerator ToggleGrenadeThrow()
        {
            yield return new WaitForSeconds(grenadeThrowDelay);
            OnGrenadeUIUpdate?.Invoke();
            isGrenadeThrown = false;
            isReadyToThrowGrenade = !isReadyToThrowGrenade;
        }
    }
}
