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
        private static void CacheEntranceTeleports()
        {
            foreach (EntranceTeleportLock locker in EntranceTeleportLock.ActiveLockers) { locker.teleports = GameObject.FindObjectsOfType<EntranceTeleport>(includeInactive: false).ToList(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGame))]
        private static void ClearEntranceTeleportsCache()
        {
            foreach (EntranceTeleportLock locker in EntranceTeleportLock.ActiveLockers) { locker.teleports?.Clear(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(EntranceTeleport), nameof(EntranceTeleport.TeleportPlayer))]
        private static void UnlockLockedDoor(EntranceTeleport __instance)
        {
            if (!__instance.exitScript.triggerScript.interactable) { __instance.exitScript.triggerScript.interactable = true; }
        }
    }
}
