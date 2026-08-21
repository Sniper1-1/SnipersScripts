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
        private static void EndShipMessage(ref IEnumerator __result)
        {
            __result = WaitForEnd(__result);
        }
        // used to successfully wait for the end of the coroutine before invoking the events
        private static IEnumerator WaitForEnd(IEnumerator original)
        {
            while (original.MoveNext()) { yield return original.Current; } // waits for all the yields in the original
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onShipMessageEnd.Invoke(); }
        }

        // ship speaker
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.DisableShipSpeakerLocalClient))]
        private static void SpeakerMute()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onSpeakerMute.Invoke(); }
        }

        // ship doors
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HangarShipDoor), nameof(HangarShipDoor.SetDoorClosed))]
        private static void DoorClosed()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) 
            { 
                controller.onDoorClose.Invoke(); 
                controller.onDoorToggle.Invoke();
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HangarShipDoor), nameof(HangarShipDoor.SetDoorOpen))]
        private static void DoorOpened()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) 
            { 
                controller.onDoorOpen.Invoke();
                controller.onDoorToggle.Invoke();
            }
        }

        // radar screen
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ManualCameraRenderer),nameof(ManualCameraRenderer.SwitchRadarTargetForward))]
        private static void SwitchSpectateTarget()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onScreenSpectatorToggle.Invoke(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ManualCameraRenderer),nameof(ManualCameraRenderer.SwitchScreenOn))]
        private static void CheckScreenPowered(bool on)
        {
            if (on)
            {
                foreach (ShipController controller in ShipController.ActiveControllers) { controller.onScreenTurnOn.Invoke(); }
            }
            else
            {
                foreach (ShipController controller in ShipController.ActiveControllers) { controller.onScreenTurnOff.Invoke(); }
            }
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onScreenPoweredToggle.Invoke(); }
        }
    }
}
