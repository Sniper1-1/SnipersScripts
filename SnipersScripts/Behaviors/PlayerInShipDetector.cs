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

        // used to track the previous state of the player being in the ship to detect if it changes
        private readonly Dictionary<PlayerControllerB, bool> previousShipRoomState = new Dictionary<PlayerControllerB, bool>();

        [Tooltip("If true, events invoke when player enters/exits ship. If false, events only run on a manual call to EvaluateIsInShipManualInvoke.")]
        public bool checkPassively = true;
        [Tooltip("If true, the detector will immediately check the player's position and fire the appropriate events, instead of waiting for the first swtich")]
        public bool checkImmediately = false;

        [Tooltip("The event to trigger when the player enters the ship.")]
        public UnityEngine.Events.UnityEvent<PlayerControllerB> onPlayerEnterShip;
        [Tooltip("The event to trigger when the player exits the ship.")]
        public UnityEngine.Events.UnityEvent<PlayerControllerB> onPlayerExitShip;
        [Tooltip("The event to trigger when the player switches between locations.")]
        public UnityEngine.Events.UnityEvent<PlayerControllerB> onPlayerSwitchLocation;

        private void OnEnable() { ActiveDetectors.Add(this); }

        private void OnDisable() { ActiveDetectors.Remove(this); }

        /// <summary>Called in the update loop of PlayerControllerB</summary>
        internal void UpdatePlayerPosition(PlayerControllerB player)
        {
            if (checkPassively) { EvaluateIsInShip(player, alwaysFireEnterExit: false); }
        }

        /// <summary>wrapper for EvaluateIsInShipManual to allow it to be called from UnityEvents, which don't support return values.</summary>
        public void EvaluateIsInShipManualInvoke(PlayerControllerB player)
        {
            EvaluateIsInShip(player, alwaysFireEnterExit: true);
        }
        /// <summary>Always fires enter/exit for the player's current state, and fires switch if it changed since the last check.</summary>
        public bool EvaluateIsInShipManual(PlayerControllerB player)
        {
            return EvaluateIsInShip(player, alwaysFireEnterExit: true);
        }        

        /// <summary>
        /// Evaluates whether the player is currently in the ship and triggers the appropriate events. onPlayerSwitchLocation: invokes if player transitions. onPlayerExitShip: invokes if player is not in ship. onPlayerEnterShip: invokes if player is in ship.
        /// </summary>
        /// <param name="player">The player being checked</param>
        /// <param name="alwaysFireEnterExit">Used to determine if the enter/exit events should invoke or if they should only invoke on a state change</param>
        /// <returns>True if the player is currently in the ship, false otherwise</returns>
        private bool EvaluateIsInShip(PlayerControllerB player, bool alwaysFireEnterExit)
        {
            bool hasPrevious = previousShipRoomState.TryGetValue(player, out bool previousValue);
            bool changed = hasPrevious && player.isInHangarShipRoom != previousValue;

            previousShipRoomState[player] = player.isInHangarShipRoom;

            if (changed) { onPlayerSwitchLocation?.Invoke(player); }

            bool fireImmediateFirstCheck = !hasPrevious && checkImmediately;

            if (alwaysFireEnterExit || changed || fireImmediateFirstCheck)
            {
                if (player.isInHangarShipRoom) { onPlayerEnterShip?.Invoke(player); }
                if (!player.isInHangarShipRoom) { onPlayerExitShip?.Invoke(player); }
            }

            return player.isInHangarShipRoom;
        }

        /// <summary>
        /// Used to toggle if the detector checks passively or only when manually invoked.
        /// </summary>
        /// <param name="passiveCheck">True: the detector will check passively. False: the detector will only check when manually invoked.</param>
        public void SetPassiveCheck(bool passiveCheck)
        {
            checkPassively = passiveCheck;
        }
    }
}
