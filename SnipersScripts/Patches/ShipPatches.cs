using HarmonyLib;
using SnipersScripts.Behaviors;
using System.Collections;

namespace SnipersScripts.Patches
{
    [HarmonyPatch]
    internal class ShipPatches
    {
        // magnet
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.SetMagnetOn))]
        private static void MagnetTogglePrefix(bool on)
        {
            if (on)
            {
                foreach (ShipController controller in ShipController.ActiveControllers) { controller.onMagnetEnable.Invoke(); }
            }
            else
            {
                foreach (ShipController controller in ShipController.ActiveControllers) { controller.onMagnetDisable.Invoke(); }
            }
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onMagnetToggle.Invoke(); }
        }

        // ship flight
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGame))]
        private static void ShipEnterOrbit()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onShipEnterOrbit.Invoke(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        private static void ShipAscend()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onShipAscend.Invoke(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.OnShipLandedMiscEvents))]
        private static void ShipLanded()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onShipLand.Invoke(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.FinishGeneratingNewLevelClientRpc))]
        private static void ShipDescend()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onShipDescend.Invoke(); }
        }

        // ship messages
        [HarmonyPrefix]
        [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.ReadOutDialogue))]
        private static void StartShipMessage()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onShipMessageStart.Invoke(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.ReadOutDialogue))]
        private static void WrapDialogueCoroutine(ref IEnumerator __result)
        {
            __result = WrapWithEndEvent(__result);
        }
        // used to successfully wait for the end of the coroutine before invoking the events
        private static IEnumerator WrapWithEndEvent(IEnumerator original)
        {
            while (original.MoveNext()) { yield return original.Current; }
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onShipMessageEnd.Invoke(); }
        }

        // ship speaker
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.DisableShipSpeakerLocalClient))]
        private static void SpeakerMute()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onSpeakerMute.Invoke(); }
        }
    }
}
