using HarmonyLib;
using SnipersScripts.Behaviors;

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
    }
}
