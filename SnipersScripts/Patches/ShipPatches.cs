using HarmonyLib;
using SnipersScripts.Behaviors;
using UnityEngine;
using System;
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
            __result = WaitForEnd(__result, () => { foreach (ShipController controller in ShipController.ActiveControllers) { controller.onShipMessageEnd.Invoke(); } });
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

        // teleporters
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipTeleporter), nameof(ShipTeleporter.beamUpPlayer))]
        private static void TeleportNormal()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onTeleportStart.Invoke(); }
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipTeleporter), nameof(ShipTeleporter.beamOutPlayer))]
        private static void TeleportInverse()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onInverseStart.Invoke(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipTeleporter), nameof(ShipTeleporter.beamUpPlayer))]
        private static void TeleportEnd(ref IEnumerator __result)
        {
            __result = WaitForEnd(__result, () => { foreach (ShipController controller in ShipController.ActiveControllers) { controller.onTeleportEnd.Invoke(); } });
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipTeleporter), nameof(ShipTeleporter.beamOutPlayer))]
        private static void InverseEnd(ref IEnumerator __result)
        {
            __result = WaitForEnd(__result, () => { foreach (ShipController controller in ShipController.ActiveControllers) { controller.onInverseEnd.Invoke(); } });
        }

        // signal transmitter
        [HarmonyPrefix]
        [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.DisplaySignalTranslatorMessage))]
        private static void TransmitStart()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onSignalTransmitStart.Invoke(); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.DisplaySignalTranslatorMessage))]
        private static void TransmitEnd(ref IEnumerator __result)
        {
            __result = WaitForEnd(__result, () => { foreach (ShipController controller in ShipController.ActiveControllers) { controller.onSignalTransmitEnd.Invoke(); } });
        }

        // ship horn
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipAlarmCord), nameof(ShipAlarmCord.PullCordServerRpc))]
        private static void StartHorn()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onHornPull.Invoke(); }
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipAlarmCord), nameof(ShipAlarmCord.HoldCordDown))]
        private static void HoldCord()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.whileHornPulled.Invoke(); }
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipAlarmCord), nameof(ShipAlarmCord.StopHorn))]
        private static void StopHorn()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onHornRelease.Invoke(); }
        }

        // ship lights
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipLights), nameof(ShipLights.ToggleShipLights))]
        private static void ToggleLights()
        {
            foreach (var controller in ShipController.ActiveControllers)
            {
                controller.onShipLightsToggle.Invoke();
                if (StartOfRound.Instance.shipRoomLights.areLightsOn) { controller.onShipLightsTurnOn.Invoke(); }
                else { controller.onShipLightsTurnOff.Invoke(); }
            }
        }

        // electric chair
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MoveToExitSpecialAnimation), nameof(MoveToExitSpecialAnimation.SetExitingDisabled))]
        private static void ClampToggle(bool disabled)
        {
            foreach (var controller in ShipController.ActiveControllers)
            {
                if (disabled) { controller.onClampLock.Invoke(); }
                if (!disabled) { controller.onClampUnlock.Invoke(); }
                controller.onClampToggle.Invoke();
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MoveToExitSpecialAnimation), nameof(MoveToExitSpecialAnimation.shockChair))]
        private static void OnShockStart()
        {
            foreach(var controller in ShipController.ActiveControllers) { controller.onShockStart.Invoke();}
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MoveToExitSpecialAnimation), nameof(MoveToExitSpecialAnimation.shockChair))]
        private static void OnShockEnd(ref IEnumerator __result)
        {
            __result = WaitForEnd(__result, () => { foreach (ShipController controller in ShipController.ActiveControllers) { controller.onShockEnd.Invoke(); } });
        }

        // tv
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TVScript), nameof(TVScript.TurnTVOnOff))]
        private static void PowerTV(bool on)
        {
            foreach (var controller in ShipController.ActiveControllers)
            {
                if (on) { controller.onTvTurnOn.Invoke(); }
                else { controller.OnTvTurnOff.Invoke(); }
                controller.onTvToggle.Invoke();
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TVScript), nameof(TVScript.TVFinishedClip))]
        private static void ClipFinished()
        {
            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onTvStationChange.Invoke(); }
        }
        //patches for playing custom clips over the TV
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TVScript), nameof(TVScript.TVFinishedClip))]
        private static bool HandleOverrideFinished(TVScript __instance)
        {
            if (ShipController.currentClipOverride == null) return true; // just run the vanilla code if no override

            var finishedOverride = ShipController.currentClipOverride;
            ShipController.currentClipOverride = null;
            ShipController.overrideVisiblyPlaying = false;

            foreach (ShipController controller in ShipController.ActiveControllers) { controller.onTvStationChange.Invoke(); }

            if (finishedOverride.turnTVOff)
            {
                __instance.TurnTVOnOff(false);
                return false; // TV off, stop vanilla going to next clip
            }

            return true; // let vanilla's own channel-advance logic run now that the override is cleared
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TVScript), nameof(TVScript.Update))]
        private static void TrackOverridePlayback(TVScript __instance)
        {
            if (ShipController.currentClipOverride == null) { return; }

            if (__instance.tvOn)
            {
                if (!ShipController.overrideVisiblyPlaying)
                {
                    ShipController.overrideVisiblyPlaying = true;
                    __instance.video.clip = ShipController.currentClipOverride.clip;
                    __instance.video.time = ShipController.overrideElapsedTime;
                    __instance.video.Play();

                    if (ShipController.currentClipOverride.audio != null)
                    {
                        __instance.tvSFX.clip = ShipController.currentClipOverride.audio;
                        __instance.tvSFX.time = ShipController.overrideElapsedTime;
                        __instance.tvSFX.Play();
                    }

                    foreach (ShipController controller in ShipController.ActiveControllers) { controller.onTvStationChange.Invoke(); }
                }
                // completion while visible is handled by TVFinishedClipPrefix above, via the real loopPointReached event
            }
            else
            {
                if (ShipController.overrideVisiblyPlaying)
                {
                    // just turned off — capture position and go quiet
                    ShipController.overrideVisiblyPlaying = false;
                    ShipController.overrideElapsedTime = (float)__instance.video.time;
                    __instance.video.Stop();
                    __instance.tvSFX.Stop();
                }

                ShipController.overrideElapsedTime += Time.deltaTime; // used to track when the clip should be if turned on during its runtime

                if (ShipController.overrideElapsedTime >= ShipController.currentClipOverride.clip.length) { ShipController.currentClipOverride = null; } // the clip has already finished
            }
        }

        // coroutine helping
        // used to successfully wait for the end of the coroutine before invoking the events
        private static IEnumerator WaitForEnd(IEnumerator original, Action callback)
        {
            while (original.MoveNext()) { yield return original.Current; } // waits for all the yields in the original
            callback?.Invoke();
        }
    }
}
