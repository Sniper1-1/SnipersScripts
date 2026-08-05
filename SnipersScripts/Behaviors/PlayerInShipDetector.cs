using System.Collections.Generic;
using GameNetcodeStuff;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/PlayerInShipDetector")]
    public class PlayerInShipDetector : MonoBehaviour
    {

        // Registry of every active detector currently loaded
        internal static readonly List<PlayerInShipDetector> ActiveDetectors = new List<PlayerInShipDetector>();

        [Tooltip("The event to trigger when the player enters the ship.")]
        public UnityEngine.Events.UnityEvent<PlayerControllerB> onPlayerEnterShip;
        [Tooltip("The event to trigger when the player exits the ship.")]
        public UnityEngine.Events.UnityEvent<PlayerControllerB> onPlayerExitShip;
        [Tooltip("The event to trigger when the player switches between locations.")]
        public UnityEngine.Events.UnityEvent<PlayerControllerB> onPlayerSwitchLocation;

        private void OnEnable() { ActiveDetectors.Add(this); }

        private void OnDisable() { ActiveDetectors.Remove(this); }

        internal void NotifySwitchLocation(PlayerControllerB player, bool nowInShip)
        {
            onPlayerSwitchLocation?.Invoke(player);

            if (nowInShip) { onPlayerEnterShip?.Invoke(player); }
            else { onPlayerExitShip?.Invoke(player); }                
        }
    }
}
