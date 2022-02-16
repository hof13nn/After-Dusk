using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterDusk
{
    public class TImeContoler : MonoBehaviour
    {
        // <summary>
        /// Target timeScale.
        /// </summary>
        [SerializeField] [Tooltip("Target timeScale")] private float targetScale = 0.1f;
        /// <summary>
        /// Timescale blending speed.
        /// </summary>
        [SerializeField] [Tooltip("Timescale blending speed")] private float blendSpeed = 2f;

        private bool isActivated = false;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.T))
            {
                isActivated = !isActivated;
            }

            Time.timeScale = Mathf.MoveTowards(Time.timeScale, isActivated ? targetScale : 1, Time.unscaledDeltaTime * blendSpeed);
    }
}
}