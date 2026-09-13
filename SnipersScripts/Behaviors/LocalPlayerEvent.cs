using GameNetcodeStuff;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/LocalPlayerEvent")]
    internal class LocalPlayerEvent: MonoBehaviour
    {
        [Tooltip("Runs on the local player.")]
        public UnityEngine.Events.UnityEvent<PlayerControllerB> localPlayerEvent;

        /// <summary>
        /// Invokes the localPlayerEvent on the local player
        /// </summary>
        public void InvokeLocalPlayerEvent()
        {
            localPlayerEvent.Invoke(StartOfRound.Instance.localPlayerController);
        }
    }
}
