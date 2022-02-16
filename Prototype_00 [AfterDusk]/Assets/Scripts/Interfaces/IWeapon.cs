using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    public interface IWeapon
    {
        void ShootRaycast();
        void ReduceAmmo();
        // void ManualReload(bool isManualReload);
        // // void AutoReload();
    }
}
