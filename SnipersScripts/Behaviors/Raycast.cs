using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/Raycast")]
    internal class Raycast: MonoBehaviour
    {
        [Tooltip("Determins if the raycasts should run on Start or only when manually called")]
        public bool fireRaysOnStart = true;
        public List<RaycastOptions> raycastOptions;

        void Start()
        {
            if (fireRaysOnStart) { FireRays(); }
        }

        /// <summary>
        /// Fires every raycast in the Raycast Options list
        /// </summary>
        public void FireRays()
        {
            foreach (var ray in raycastOptions) { FireRay(ray); }
        }
        /// <summary>
        /// Fire a specific raycast based on index in the Raycast Options list
        /// </summary>
        /// <param name="rayIndex">The index of the specific ray to fire</param>
        public void FireRay(int rayIndex) // 
        {
            if (rayIndex >= 0 && rayIndex < raycastOptions.Count)
            {
                FireRay(raycastOptions[rayIndex]);
            }
            else
            {
                SnipersScripts.Logger.LogError($"Index {rayIndex} is not within the bounds of the Raycast Options list on {this.gameObject.name}");
            }
        }

        /// <summary>
        /// Fire a specific raycast
        /// </summary>
        /// <param name="ray">The RaycastOptions to use</param>
        public void FireRay(RaycastOptions ray)
        {
            Vector3 rayStartPosition = this.transform.position;
            Vector3 rayDirection;
            if (ray.globalAxis) { rayDirection = ray.direction; } // direction is global
            else { rayDirection = this.transform.TransformDirection(ray.direction); } // direction is local

            ray.onRayStart?.Invoke(rayStartPosition);
            if(Physics.Raycast(rayStartPosition, rayDirection, out var hitInfo, ray.distance, ray.mask))
            {
                var pos = hitInfo.point;
                ray.onRayHit?.Invoke(pos);                
            }
            else
            {
                ray.onRayFail?.Invoke(rayStartPosition);
            }
        }
    }

    [System.Serializable]
    public class RaycastOptions : ISerializationCallbackReceiver // extend ISerializationCallbackReceiver so the editor can be forced to display proper defaults
    {
        [Tooltip("The direction to fire the raycast")]
        public Vector3 direction;
        [Tooltip("How far the ray should travel before failing")]
        public float distance;
        [Tooltip("If true, ray fires in the global rotation. If false, it fires relative to the attached game object's rotation.")]
        public bool globalAxis=false;
        [Tooltip("What layers the raycast hits successfully.")]
        public LayerMask mask;
        [Tooltip("Fires when the ray starts, with its start position.")]
        public UnityEngine.Events.UnityEvent<Vector3> onRayStart;
        [Tooltip("Fires when the ray hits something in the Mask, with its hit position.")]
        public UnityEngine.Events.UnityEvent<Vector3> onRayHit;
        [Tooltip("Fires when the ray fails to hit something in the Mask, with its start position.")]
        public UnityEngine.Events.UnityEvent<Vector3> onRayFail;

        // all the below is just so the Unity editor serializes the proper defaults, because it 0s them out when they're in lists
        [NonSerialized] private bool initialized = false;
        public void OnAfterDeserialize()
        {
            if (!initialized)
            {
                initialized = true;
                if (distance == 0f) { distance = 100f; }
                if (mask.value == 0) { mask = 301992193; }
            }
        }
        public void OnBeforeSerialize() { }
    }
}
