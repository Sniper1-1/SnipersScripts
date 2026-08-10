using HarmonyLib;
using SnipersScripts.Behaviors;

namespace SnipersScripts.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRoundPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Start))]
        private static void StartOfRound_Postfix(StartOfRound __instance)
        {
            __instance.StartCoroutine(EnemyRequiresNestOverrideInternal.populateEnemyList());
        }
    }
}
