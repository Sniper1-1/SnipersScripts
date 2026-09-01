using SnipersScripts.Patches;
using Unity.Netcode;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/ItemDropDistanceOverride")]
    public class ItemDropDistanceOverride: NetworkBehaviour
    {
        [Min(0f)]
        [Tooltip("The distance from which items can be dropped.")]
        public float itemDropDistance = ItemDropDistancePatch.VanillaRaycastDistance;
        [Tooltip("If true, the override will be applied on Start. If false, you can call ApplyDropDistanceOverride() manually.")]
        public bool applyOverrideOnStart = true;

        private void Start()
        {
            if (applyOverrideOnStart)
            {
                ApplyDropDistanceOverrideRpc(itemDropDistance);
            }
        }

        /// <summary>
        /// Sets a custom item drop distance.
        /// </summary>
        /// <param name="distance">The distance from which items can be dropped.</param>
        [Rpc(SendTo.Everyone, RequireOwnership = false)]
        public void ApplyDropDistanceOverrideRpc(float distance)
        {
            ItemDropDistancePatch.CustomRaycastDistance = distance;
            SnipersScripts.Logger.LogDebug($"Item drop distance override applied: {distance}");
        }

        // Reset to vanilla distance when this component is destroyed
        private void OnDestroy()
        { 
            ItemDropDistancePatch.CustomRaycastDistance = ItemDropDistancePatch.VanillaRaycastDistance;
            SnipersScripts.Logger.LogDebug($"Item drop distance override removed, reset to {ItemDropDistancePatch.VanillaRaycastDistance}.");
        }
    }
}
