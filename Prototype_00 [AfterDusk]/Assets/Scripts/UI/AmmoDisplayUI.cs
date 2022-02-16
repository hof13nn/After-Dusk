using UnityEngine;
using AfterDusk.Player;
using TMPro;
using UnityEngine.UI;

namespace AfterDusk
{
    public class AmmoDisplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoInReserve;
        [SerializeField] private TextMeshProUGUI ammoInMagazine;
        [SerializeField] private TextMeshProUGUI grenadeAmmo;
        [SerializeField] private Image grenadeIcon;
        [SerializeField] private Image weaponIcon;
        private WeaponController weaponController;
        private GrenadeController grenadeController;
        private Equipment equipment;

        private void Awake()
        {
            grenadeAmmo.text = "";
            grenadeIcon.gameObject.SetActive(false);
            grenadeIcon.sprite = null;
        }

        private void OnEnable()
        {
            WeaponController.OnActiveWeaponChange += UpdateAmmoUI;
            Equipment.OnGrenadeChange += UpdateGrenadeAmmoUI;
            GrenadeController.OnGrenadeUIUpdate += UpdateGrenadeAmmoUI;
        }

        private void OnDisable()
        {
            WeaponController.OnActiveWeaponChange -= UpdateAmmoUI;
            Equipment.OnGrenadeChange -= UpdateGrenadeAmmoUI;
            GrenadeController.OnGrenadeUIUpdate -= UpdateGrenadeAmmoUI;
        }

        private void Start()
        {
            weaponController = PlayerManager.WeaponController;
            grenadeController = PlayerManager.GrenadeController;
            equipment = PlayerManager.PlayerEquipment;
        }

        private void UpdateAmmoUI()
        {
            if (weaponController.CurrentWeapon.GetComponent<Weapon>().ID == -1)
            {
                ammoInReserve.text = "";
                ammoInMagazine.text = "";
                weaponIcon.sprite = null;
                weaponIcon.gameObject.SetActive(false);
            }
            else
            {
                ammoInReserve.text = weaponController.GetActiveWeapon().weapon.WeaponStats.ammoInReserve.ToString();
                ammoInMagazine.text = weaponController.GetActiveWeapon().weapon.WeaponStats.ammoInMagazine.ToString();
                weaponIcon.gameObject.SetActive(true);
                weaponIcon.sprite = weaponController.GetActiveWeapon().weapon.WeaponIcon;
            }
        }

        private void UpdateGrenadeAmmoUI()
        {
            if (grenadeController.GetActiveGrenade().grenade != null)
            {
                grenadeAmmo.text = grenadeController.GetActiveGrenade().grenadeStats.ammoInReserve.ToString();
                grenadeIcon.gameObject.SetActive(true);
                grenadeIcon.sprite = grenadeController.GetActiveGrenade().grenade.GrenadeIcon;
            }
            else
            {
                grenadeAmmo.text = "";
                grenadeIcon.gameObject.SetActive(false);
                grenadeIcon.sprite = null;
            }
        }
    }
}
