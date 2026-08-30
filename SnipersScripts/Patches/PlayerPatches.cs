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
            if (StartOfRound.Instance == null || __instance != StartOfRound.Instance.localPlayerController) { return; }
            TrackPlayerInShip(__instance);
        }

        private static void TrackPlayerInShip(PlayerControllerB player) //used to keep track of when a player enters/exits the ship
        {            
            foreach (var detector in PlayerInShipDetector.ActiveDetectors.ToArray())
            {
                detector.UpdatePlayerPosition(player);
            }
        }
    }
}
