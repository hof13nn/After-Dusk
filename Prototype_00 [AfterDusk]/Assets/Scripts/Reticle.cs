using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk.Player
{
    public class Reticle : MonoBehaviour
    {
        private RectTransform reticle;
        [SerializeField] private Vector2 minSize, maxSize;
        [SerializeField] private float singleFireSpeed, autoFireSpeed;
        private Vector2 currentSize;

        private void Awake()
        {
            reticle = GetComponent<RectTransform>();
            currentSize = minSize;
        }

        private void OnEnable()
        {
            WeaponController.OnWeaponFireGet += ApplyDynamic;
        }

        private void OnDisable()
        {
            WeaponController.OnWeaponFireGet -= ApplyDynamic;
        }

        private void ApplyDynamic(bool isFire, WeaponFireType weaponFireType)
        {
            if (PlayerManager.PlayerInventory.IsInventoryActive) return;

            if (isFire)
            {
                if (weaponFireType == WeaponFireType.SingleFire)
                {
                    currentSize = Vector2.Lerp(currentSize, maxSize, singleFireSpeed * Time.deltaTime);
                }
                else if (weaponFireType == WeaponFireType.AutoFire)
                {
                    currentSize = Vector2.Lerp(currentSize, maxSize, autoFireSpeed * Time.deltaTime);
                }

            }
            else
            {
                if (weaponFireType == WeaponFireType.SingleFire)
                {
                    currentSize = Vector2.Lerp(currentSize, minSize, singleFireSpeed * Time.deltaTime);
                }
                else if (weaponFireType == WeaponFireType.AutoFire)
                {
                    currentSize = Vector2.Lerp(currentSize, minSize, autoFireSpeed * Time.deltaTime);
                }
            }

            reticle.sizeDelta = currentSize;
        }
    }
}
