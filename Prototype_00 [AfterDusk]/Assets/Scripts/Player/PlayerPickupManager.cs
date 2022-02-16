using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AfterDusk.Player
{
    public class PlayerPickupManager : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float pickupTime;
        [SerializeField] private RectTransform pickupImageRoot;
        [SerializeField] private Image pickupProgressImage;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private GameObject crosshair;
        private PlayerController playerController;
        private Inventory playerInventory;
        private Pickup itemBeingPickedUp;
        private float currentPickupTimeElapsed;
        //private float isPickingUp;

        private void OnEnable()
        {
            InputManager.OnPickupPressed += PickupItem;
        }

        private void OnDisable()
        {
            InputManager.OnPickupPressed -= PickupItem;
        }


        private void Update()
        {
            SelectItemToPickUpFromRay();
        }

        private void PickupItem(bool isPickingUp)
        {
            if (itemBeingPickedUp != null && !PlayerManager.PlayerController.IsAiming)
            {
                if (isPickingUp)
                {
                    IncrementPickUpProgressAndTryComlete(itemBeingPickedUp);
                }
                else
                {
                    currentPickupTimeElapsed = 0f;
                }

                UpdatePickupProgressImage();
            }
            else
            {
                currentPickupTimeElapsed = 0f;
            }
        }

        private void IncrementPickUpProgressAndTryComlete(Pickup pickup)
        {
            currentPickupTimeElapsed += Time.deltaTime;

            if (currentPickupTimeElapsed >= pickupTime)
            {
                MoveItemToInventory(pickup);
            }
        }

        private void SelectItemToPickUpFromRay()
        {
            Ray ray = PlayerManager.PlayerController.MainCamera.ViewportPointToRay(Vector3.one / 2f);
            RaycastHit hitInfo;

            Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red);

            if (Physics.Raycast(ray, out hitInfo, 2f, layerMask))
            {
                var hitItem = hitInfo.collider.GetComponent<Pickup>();

                if (hitItem == null)
                {
                    itemBeingPickedUp = null;
                    pickupImageRoot.gameObject.SetActive(false);
                }
                else if (hitItem != null && hitItem != itemBeingPickedUp)
                {
                    pickupImageRoot.gameObject.SetActive(true);
                    itemBeingPickedUp = hitItem;
                    //itemNameText.text = $"{itemBeingPickedUp.ItemObject.itemPrefab.name}";
                }
            }
            else
            {
                itemBeingPickedUp = null;
                pickupImageRoot.gameObject.SetActive(false);
            }
        }

        private void UpdatePickupProgressImage()
        {
            float pct = currentPickupTimeElapsed / pickupTime;
            pickupProgressImage.fillAmount = pct;
        }

        private void MoveItemToInventory(Pickup pickup)
        {
            PlayerManager.PlayerInventory.GetPickup(pickup.ItemObject);
            itemBeingPickedUp = null;
            pickup.gameObject.SetActive(false);
        }
    }
}
