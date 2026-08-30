using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/WaterloggedSensor")]
    [RequireComponent(typeof(Collider))]
    internal class WaterloggedSensor : MonoBehaviour
    {
        [Tooltip("Invokes when game object under water.")]
        public UnityEngine.Events.UnityEvent onSubmerge;
        [Tooltip("Invokes when game object leaves water.")]
        public UnityEngine.Events.UnityEvent onEmerge;

        private void Start()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
            foreach (Collider collider in colliders) 
            { 
                if((collider.TryGetComponent<QuicksandTrigger>(out var found)) && (found.isWater || found.isInsideWater))
                {
                    onSubmerge.Invoke();
                    return;
                }
            }
            onEmerge.Invoke();
        }
        private void OnTriggerEnter(Collider collider)
        {
            if ((collider.TryGetComponent<QuicksandTrigger>(out var found)) && (found.isWater || found.isInsideWater)) { onSubmerge.Invoke(); }
        }
        private void OnTriggerExit(Collider collider)
        {
            if ((collider.TryGetComponent<QuicksandTrigger>(out var found)) && (found.isWater || found.isInsideWater)) { onEmerge.Invoke(); }
        }
    }
}
