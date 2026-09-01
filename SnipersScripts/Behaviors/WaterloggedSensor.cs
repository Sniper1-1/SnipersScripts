using UnityEngine;
using Unity.Netcode;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/WaterloggedSensor")]
    [RequireComponent(typeof(Collider))]
    internal class WaterloggedSensor : NetworkBehaviour
    {
        [Tooltip("If the check should run to invoke the events on start instead of only on switched state.")]
        public bool checkOnStart = true;
        [Tooltip("Invokes when game object under water.")]
        public UnityEngine.Events.UnityEvent onSubmerge;
        [Tooltip("Invokes when game object leaves water.")]
        public UnityEngine.Events.UnityEvent onEmerge;

        private void Start()
        {
            if (checkOnStart) { CheckSensorRpc(); }
        }
        /// <summary>
        /// Checks if the sensor is in or out of water and invokes the appropriate events
        /// </summary>
        [Rpc(SendTo.Everyone, RequireOwnership = false)]
        public void CheckSensorRpc()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
            foreach (Collider collider in colliders)
            {
                if (VallidateCollider(collider))
                {
                    onSubmerge.Invoke();
                    return;
                }
            }
            onEmerge.Invoke();
        }
        private void OnTriggerEnter(Collider collider)
        {
            if (VallidateCollider(collider)) { onSubmerge.Invoke(); }
        }
        private void OnTriggerExit(Collider collider)
        {
            if (VallidateCollider(collider)) { onEmerge.Invoke(); }
        }

        private bool VallidateCollider(Collider collider)
        {
            return (collider.TryGetComponent<QuicksandTrigger>(out var found)) && (found.isWater || found.isInsideWater);
        }
    }
}
