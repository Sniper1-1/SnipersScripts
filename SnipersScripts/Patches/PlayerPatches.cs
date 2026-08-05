using System;
using System.Collections.Generic;
using GameNetcodeStuff;
using HarmonyLib;
using SnipersScripts.Behaviors;

namespace SnipersScripts.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class PlayerPatches
    {
        // used to track the previous state of the player being in the ship to detect if it changes
        private static readonly Dictionary<PlayerControllerB, bool> previousShipRoomState = new Dictionary<PlayerControllerB, bool>();

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.Update))]
        private static void GetLocalPlayer(PlayerControllerB __instance)
        {
            if (!__instance.IsOwner) { return; }
            TrackPlayerInShip(__instance);
        }

        private static void TrackPlayerInShip(PlayerControllerB player)
        {
            bool currentValue = player.isInHangarShipRoom;
            if (!previousShipRoomState.TryGetValue(player, out bool previousValue)) // create previous value if it doesn't exist yet
            {
                previousShipRoomState[player] = currentValue;
                return;
            }
            if (currentValue != previousValue) // if the value has changed, notify all active detectors
            {
                foreach (var detector in PlayerInShipDetector.ActiveDetectors.ToArray())
                {
                    detector.NotifySwitchLocation(player, currentValue);
                }
                previousShipRoomState[player] = currentValue; // update the previous value
            }
        }
    }
}
