using System.Linq;
using HarmonyLib;
using SnipersScripts.Behaviors;
using UnityEngine;

namespace SnipersScripts.Patches
{
    [HarmonyPatch]
    internal class EntranceTeleportPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.FinishGeneratingNewLevelClientRpc))]
        private static void CacheEntranceTeleports() // cache main/fire entrance/exit on level load
        {
            foreach (EntranceTeleportLock locker in EntranceTeleportLock.ActiveLockers) { locker.teleports = GameObject.FindObjectsOfType<EntranceTeleport>(includeInactive: false).ToList(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGame))] // empty cache after end of round
        private static void ClearEntranceTeleportsCache()
        {
            foreach (EntranceTeleportLock locker in EntranceTeleportLock.ActiveLockers) { locker.teleports?.Clear(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(EntranceTeleport), nameof(EntranceTeleport.TeleportPlayer))] // unlocks the locked side of a door once someone goes through unlocked side
        private static void UnlockLockedDoor(EntranceTeleport __instance)
        {
            if (!__instance.exitScript.triggerScript.interactable) {
                foreach (EntranceTeleportLock locker in EntranceTeleportLock.ActiveLockers)
                {
                    if (__instance.exitScript.isEntranceToBuilding) { locker.UnlockIndividualOutside(__instance); }
                    else if (!__instance.exitScript.isEntranceToBuilding) { locker.UnlockIndividualInside(__instance); }
                }
            }
        }
    }
}
