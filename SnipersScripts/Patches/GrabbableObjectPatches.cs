using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using GameNetcodeStuff;
using HarmonyLib;

namespace SnipersScripts.Patches
{
    [HarmonyPatch]
    public class ItemDropDistancePatch
    {
        public static float CustomRaycastDistance = 80f;
        public static readonly float VanillaRaycastDistance = 80f;

        [HarmonyTargetMethods] // all 3 need to be patched for dropping items
        private static IEnumerable<System.Reflection.MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(GrabbableObject), nameof(GrabbableObject.GetItemFloorPosition));
            yield return AccessTools.Method(typeof(GrabbableObject), nameof(GrabbableObject.GetPhysicsRegionOfDroppedObject));
            yield return AccessTools.Method(typeof(GrabbableObject), nameof(GrabbableObject.GetPhysicsRegionOfDroppedObjectSynced));
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
        {
            var matcher = new CodeMatcher(instructions);
            var originalDropDistance = new CodeMatch(OpCodes.Ldc_R4, VanillaRaycastDistance); // find where vanilla drop distance value is mentioned
            var raycastDistanceField = AccessTools.Field(typeof(ItemDropDistancePatch), nameof(CustomRaycastDistance)); //prepares a field that can be put in place of the vanilla drop distance inline value

            matcher.MatchForward(false, originalDropDistance); //go to first vanilla drop distance

            while (matcher.IsValid)
            {                
                matcher.SetInstructionAndAdvance(new CodeInstruction(OpCodes.Ldsfld, raycastDistanceField)); //replace with new distance field that can be dynamically changed
                SnipersScripts.Logger.LogDebug("Patched raycast distance.");
                matcher.MatchForward(false, originalDropDistance); //find subsequent vanilla drop distances
            }

            return matcher.InstructionEnumeration();
        }
    }
}