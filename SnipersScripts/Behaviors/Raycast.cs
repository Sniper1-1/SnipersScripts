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

        public void FireRays()
        {
            foreach (var ray in raycastOptions) { FireRay(ray); }
        }
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
        public Vector3 direction;
        public float distance;
        public bool globalAxis=false;
        public LayerMask mask;
        public UnityEngine.Events.UnityEvent<Vector3> onRayStart;
        public UnityEngine.Events.UnityEvent<Vector3> onRayHit;
        public UnityEngine.Events.UnityEvent<Vector3> onRayFail;

        // all the below is just so the Unity editor serializes the proper defaults, because it 0s them out when they're in lists
        [NonSerialized] private bool initialized = false;
        public void OnAfterDeserialize()
        {
            if (!initialized)
            {
                initialized = true;
                if (distance == 0f) distance = 100f;
                if (mask.value == 0) mask = 301992193;
            }
        }
        public void OnBeforeSerialize() { }
    }
}
