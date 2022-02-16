using System;
using System.Collections.Generic;
using UnityEngine;
using AfterDusk.Player;
using System.Linq;

namespace AfterDusk
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform activeWeapons;
        public Transform ActiveWeapons { get => activeWeapons; }
        [SerializeField] private Transform inactiveWeapons;
        public Transform InactiveWeapons { get => inactiveWeapons; }
        [SerializeField] private WeaponObject[] weaponPool;
        public WeaponObject[] WeaponPool { get => weaponPool; }
        [SerializeField] private GameObject[] activeWeaponsArray;
        public GameObject[] ActiveWeaponsArray { get => activeWeaponsArray; }
        [SerializeField] private List<GameObject> inactiveWeaponsList;
        public List<GameObject> InactiveWeaponsList { get => inactiveWeaponsList; }
        [SerializeField] private GameObject currentWeapon;
        public GameObject CurrentWeapon { get => currentWeapon; set => currentWeapon = value; }
        [SerializeField] private int weaponNumber;
        public int WeaponNumber { get => weaponNumber; set => weaponNumber = value; }
        private bool isUp;
        private bool isDown;
        public static event Action OnActiveWeaponChange;
        public static event Action<bool> OnWeaponFire;
        public static event Action<bool, WeaponFireType> OnWeaponFireGet;
        public static event Action<bool> OnWeaponManualReload;
        public static event Action<bool> OnWeaponAutoReload;

        private void Awake()
        {
            InitializePool();
            InitializeWeapon();
        }

        private void OnEnable()
        {
            InputManager.OnKeyboardWeaponSwitch += KeyboardNumInput;
            InputManager.OnMouseWheelWeaponSwtich += MouseWheelInput;
            InputManager.OnFirePressed += SetWeaponFireState;
            InputManager.OnReloadPressed += TriggerReload;
        }

        private void OnDisable()
        {
            InputManager.OnKeyboardWeaponSwitch -= KeyboardNumInput;
            InputManager.OnMouseWheelWeaponSwtich -= MouseWheelInput;
            InputManager.OnFirePressed -= SetWeaponFireState;
            InputManager.OnReloadPressed -= TriggerReload;
        }

        private void Update()
        {
            SetWeaponActive();
            GetWeaponFireState(GetActiveWeapon().weapon);
            WeaponAmmoCheck(GetActiveWeapon().weapon);
        }

        private void InitializeWeapon()
        {
            if (activeWeapons.childCount == 0) return;

            for (int i = 0; i < activeWeapons.childCount; i++)
            {
                activeWeaponsArray[activeWeaponsArray.Length - 1] = activeWeapons.GetChild(i).gameObject;
                weaponNumber = activeWeaponsArray.Length - 1;
                currentWeapon = activeWeaponsArray[weaponNumber];
            }
        }

        private void InitializePool()
        {
            for (int i = 0; i < weaponPool.Length; i++)
            {
                var weapon = weaponPool[i];

                GameObject instance = Instantiate(weapon.itemPrefab) as GameObject;
                instance.gameObject.GetComponent<Weapon>().ID = weapon.id;
                instance.transform.parent = inactiveWeapons;
                instance.transform.localPosition = weapon.itemPrefab.transform.position;
                instance.transform.localRotation = weapon.itemPrefab.transform.rotation;
                instance.gameObject.name = weapon.itemPrefab.gameObject.name;
                inactiveWeaponsList.Add(instance);
            }
        }

        public void SetWeaponActive()
        {
            for (int i = 0; i < activeWeaponsArray.Length; i++)
            {
                if (activeWeaponsArray[i] != null)
                {
                    if (weaponNumber < 0) return;

                    if (activeWeaponsArray[i] == activeWeaponsArray[weaponNumber])
                    {
                        currentWeapon = activeWeaponsArray[i];
                        activeWeaponsArray[i].SetActive(true);
                        OnActiveWeaponChange?.Invoke();
                    }
                    else
                    {
                        activeWeaponsArray[i].SetActive(false);
                    }
                }
                else
                {
                    if (isDown && activeWeaponsArray[i] == activeWeaponsArray[weaponNumber])
                    {
                        weaponNumber++;
                        OnActiveWeaponChange?.Invoke();
                    }
                    if (isUp && activeWeaponsArray[i] == activeWeaponsArray[weaponNumber])
                    {
                        weaponNumber--;
                        OnActiveWeaponChange?.Invoke();
                    }
                }
            }
        }

        private void KeyboardNumInput(int index)
        {
            if (activeWeaponsArray[index - 1] == null) return;

            switch (index)
            {
                case 1:
                    weaponNumber = 0;
                    return;
                case 2:
                    weaponNumber = 1;
                    return;
                case 3:
                    weaponNumber = 2;
                    return;
            }
        }

        private void MouseWheelInput(float mouseWheel)
        {

            isUp = mouseWheel > 0;
            isDown = mouseWheel < 0;

            if (mouseWheel > 0)
            {
                if (weaponNumber <= 0)
                {
                    weaponNumber = activeWeaponsArray.Length - 1;
                }
                else
                {
                    weaponNumber--;
                }
            }
            if (mouseWheel < 0)
            {
                if (weaponNumber >= activeWeaponsArray.Length - 1)
                {
                    weaponNumber = 0;
                }
                else
                {
                    weaponNumber++;
                }
            }
        }

        public (Weapon weapon, Animator weaponAnimator) GetActiveWeapon()
        {
            if (activeWeaponsArray[weaponNumber] != null)
            {
                var weapon = activeWeaponsArray[weaponNumber].GetComponent<Weapon>();
                var weaponAnimator = activeWeaponsArray[weaponNumber].GetComponent<Animator>();
                return (weapon, weaponAnimator);
            }
            else return (null, null);
        }

        private void SetWeaponFireState(bool isFired)
        {
            var weapon = GetActiveWeapon().weapon;

            if (weapon.ID != -1)
            {
                if (isFired && weapon.IsReadyToFire)
                {
                    OnWeaponFire?.Invoke(true);
                }
                else if (!isFired || !weapon.IsReadyToFire)
                {
                    OnWeaponFire?.Invoke(false);
                }
            }
        }

        private void GetWeaponFireState(Weapon weapon)
        {
            if (weapon.ID != -1)
            {
                if (weapon.IsFire)
                {
                    OnWeaponFireGet?.Invoke(true, weapon.WeaponStats.weaponFireType);
                }
                else if (!weapon.IsFire || !weapon.IsReadyToFire)
                {
                    OnWeaponFireGet?.Invoke(false, weapon.WeaponStats.weaponFireType);
                    OnWeaponFire?.Invoke(false);
                }
            }
        }

        private void WeaponAmmoCheck(Weapon weapon)
        {
            if (weapon.ID != -1)
            {
                if (weapon.WeaponStats.ammoInMagazine == 0 && weapon.WeaponStats.ammoInReserve != 0 && !weapon.IsReloading)
                {
                    OnWeaponAutoReload?.Invoke(true);
                    weapon.IsReadyToFire = false;
                    weapon.IsFire = false;
                    weapon.IsReloading = true;
                }
                else if (weapon.WeaponStats.ammoInMagazine == 0 && weapon.WeaponStats.ammoInReserve == 0)
                {
                    weapon.IsReadyToFire = false;
                    weapon.IsFire = false;
                }
            }
        }

        private void TriggerReload(bool isReload)
        {
            if (isReload)
            {
                SetReloadState(GetActiveWeapon().weapon);
            }
        }

        private void SetReloadState(Weapon weapon)
        {
            if (!weapon.IsReloading && weapon.WeaponStats.ammoInMagazine < weapon.WeaponStats.maxAmmoInMagazine && weapon.WeaponStats.ammoInReserve != 0)
            {
                if (PlayerManager.PlayerController.IsAiming) PlayerManager.PlayerController.IsAiming = false;

                if (weapon.IsFire) weapon.IsFire = false;

                OnWeaponManualReload?.Invoke(true);
                weapon.IsReadyToFire = false;
                weapon.IsReloading = true;
            }
        }

        public void FindWeaponToSet(int slotIndex)
        {
            var activeWeapon = activeWeaponsArray[weaponNumber];
            var array = ActiveWeaponFilter(activeWeaponsArray);

            if (activeWeapon != null) return;

            for (int i = 0; i < array.Length; i++)
            {
                if (array.Length > 1)
                {
                    if (array[i].GetComponent<Weapon>().ID != -1)
                    {
                        weaponNumber = ActiveWeaponFilter(activeWeaponsArray, array[i]);
                        OnActiveWeaponChange?.Invoke();
                        return;
                    }
                }
                else
                {
                    weaponNumber = activeWeaponsArray.Length - 1;
                }
            }
        }

        public GameObject[] ActiveWeaponFilter(GameObject[] array)
        {
            return array.Where(i => i != null).ToArray();
        }

        public int ActiveWeaponFilter(GameObject[] array, GameObject gameObject)
        {
            return System.Array.IndexOf(array, gameObject);
        }
    }
}