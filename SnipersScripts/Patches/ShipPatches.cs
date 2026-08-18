using HarmonyLib;
using SnipersScripts.Behaviors;

namespace SnipersScripts.Patches
{
    [HarmonyPatch]
    internal class ShipPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound),nameof(StartOfRound.SetMagnetOn))]
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
    }
}
