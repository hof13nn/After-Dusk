using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemObject[] ItemObjects;

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < ItemObjects.Length; i++)
            {
                if (ItemObjects[i].id != i)
                {
                    ItemObjects[i].id = i;
                }
            }
        }

        public void OnBeforeSerialize()
        {
        }
    }
}