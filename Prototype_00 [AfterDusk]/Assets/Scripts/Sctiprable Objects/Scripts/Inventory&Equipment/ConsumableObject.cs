using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    [CreateAssetMenu(fileName = "New Consumable Object", menuName = "Inventory System/Items/Consumable Item")]
    public class ConsumableObject : ItemObject
    {
        public ConsumableClass consumableClass;
        public int consumeModifier;

        /*         public override void Use()
                {
                    base.Use();

                    switch (this.consumableClass)
                    {
                        case ConsumableClass.Medkit:
                            Player.instance.PlayerHealth.RestoreHealth(consumeModifier);
                            break;
                        default:
                            break;
                    }
                } */
    }
}
