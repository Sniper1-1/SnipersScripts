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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.Update))]
        private static void GetLocalPlayer(PlayerControllerB __instance)
        {
            if (!__instance.IsOwner) { return; }
            TrackPlayerInShip(__instance);
        }

        private static void TrackPlayerInShip(PlayerControllerB player)
        {            
            foreach (var detector in PlayerInShipDetector.ActiveDetectors.ToArray())
            {
                detector.UpdatePlayerPosition(player);
            }
        }
    }
}
