using System.Collections.Generic;
using GameNetcodeStuff;
using SnipersScripts.Editor;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/ShipController")]
    public class ShipController : MonoBehaviour
    {
        // Registry of every active ship controller currently loaded
        internal static readonly List<ShipController> ActiveControllers = new List<ShipController>();
        private void OnEnable() { ActiveControllers.Add(this); }
        private void OnDisable() { ActiveControllers.Remove(this); }

        [Header("Ship Magnet")]
        public UnityEngine.Events.UnityEvent onMagnetEnable;
        public UnityEngine.Events.UnityEvent onMagnetDisable;
        public UnityEngine.Events.UnityEvent onMagnetToggle;
        /// <summary>
        /// Sets the magnets powered state
        /// </summary>
        /// <param name="powered">Used to turn magnet on or off</param>
        public void SetMagnetPowered(bool powered)
        {
            if (powered != StartOfRound.Instance.magnetOn) { ToggleMagnetPower(); } // only cycle it if its current state isn't what is wanted
        }
        /// <summary>
        /// Inverts current magnet state
        /// </summary>
        public void ToggleMagnetPower()
        {
            ToggleMagnetPower(true);
        }
        private void ToggleMagnetPower(bool powered)
        {
            StartOfRound.Instance.SetMagnetOn(powered);
            StartOfRound.Instance.magnetLever.TriggerAnimation(StartOfRound.Instance.localPlayerController);
        }

        [Header("Ship Flight")]
        public UnityEngine.Events.UnityEvent onShipDescend;
        public UnityEngine.Events.UnityEvent onShipLand;
        public UnityEngine.Events.UnityEvent onShipAscend;
        public UnityEngine.Events.UnityEvent onShipEnterOrbit;
        /// <summary>
        /// Used to make ship take off or land
        /// </summary>
        /// <param name="land"> if true, lands the ship if it can land. If false, takes off if it can.</param>
        public void SetShipLanded(bool land)
        {
            if (land != StartOfRound.Instance.shipHasLanded)
            {
                ToggleShipFlight(); 
            } 
        }
        /// <summary>
        /// Makes the ship land or takeoff (whichever it isn't currently)
        /// </summary>
        public void ToggleShipFlight()
        {
            ToggleShipFlight(!StartOfRound.Instance.shipHasLanded);
        }
        private void ToggleShipFlight(bool land)
        {
            if (FindFirstObjectByType<StartMatchLever>().triggerScript.interactable)
            {
                if (land)
                {
                    FindFirstObjectByType<StartMatchLever>().LeverAnimation();
                    StartOfRound.Instance.StartGame();
                }
                else
                {
                    FindFirstObjectByType<StartMatchLever>().LeverAnimation();
                    StartOfRound.Instance.ShipLeave();
                }
            }
            else
            {
                SnipersScripts.Logger.LogWarning("Tried to move ship when lever can't be pulled. Cancelling action.");
            }
        }

        [Header("Ship Messages")]
        [Tooltip("The alerts like the ones that appear when the ship leaves at midnight.")]
        public DialogueSegment[] shipMessage;
        [Tooltip("Events that trigger when a ship message starts")]
        public UnityEngine.Events.UnityEvent onShipMessageStart;
        [Tooltip("Events that trigger when a ship message ends")]
        public UnityEngine.Events.UnityEvent onShipMessageEnd;
        /// <summary>
        /// Displays message on the component like the one that plays when the ship leaves at midnight.
        /// </summary>
        public void BroadcastShipMessage()
        {
            HUDManager.Instance.ReadDialogue(shipMessage);
        }
        /// <summary>
        /// Displays message in the provided scriptable object like the one that plays when the ship leaves at midnight.
        /// </summary>
        /// <param name="message">The scriptable object to pull the message from.</param>
        public void BroadcastShipMessage(ShipMessageSO message)
        {
            HUDManager.Instance.ReadDialogue(message.shipMessage);
        }

        [Tooltip("Invokes when the ship speaker is muted")]
        public UnityEngine.Events.UnityEvent onSpeakerMute;
        /// <summary>
        /// Plays the provided audio clip on the ship speaker
        /// </summary>
        /// <param name="audioClip">The audio to play over the ship speaker.</param>
        public void PlayAudioOverSpeaker(AudioClip audioClip)
        {
            StartOfRound.Instance.speakerAudioSource.PlayOneShot(audioClip);
        }
        /// <summary>
        /// Stops the ship speaker
        /// </summary>
        public void StopAudioOverSpeaker()
        {
            StartOfRound.Instance.DisableShipSpeaker();
        }

        [Header("Ship Doors")]
        public UnityEngine.Events.UnityEvent onDoorOpen;
        public UnityEngine.Events.UnityEvent onDoorClose;
        public UnityEngine.Events.UnityEvent onDoorToggle;
        /// <summary>
        /// Sets the door open/close state
        /// </summary>
        /// <param name="open"> If true, sets door to be open if not already. If false, closes door if not already</param>
        public void SetShipDoorOpen(bool open)
        {
            if (open == FindFirstObjectByType<HangarShipDoor>().shipDoorsAnimator.GetBool("Closed"))
            {
                ToggleShipDoor();
            }
        }
        /// <summary>
        /// Inverts the current door open/close state
        /// </summary>
        public void ToggleShipDoor()
        {
            ToggleShipDoor(!FindFirstObjectByType<HangarShipDoor>().shipDoorsAnimator.GetBool("Closed"));
        }
        private void ToggleShipDoor(bool powered)
        {
            if (powered) { FindFirstObjectByType<HangarShipDoor>().PlayDoorAnimation(true); }
            else { FindFirstObjectByType<HangarShipDoor>().PlayDoorAnimation(false); }
        }

        [Header("Map Radar")]
        public UnityEngine.Events.UnityEvent onScreenTurnOn;
        public UnityEngine.Events.UnityEvent onScreenTurnOff;
        public UnityEngine.Events.UnityEvent onScreenPoweredToggle;
        public UnityEngine.Events.UnityEvent onScreenSpectatorToggle;
        /// <summary>
        /// Sets screen powered state
        /// </summary>
        /// <param name="powered">If true, turns on screen if not already on. If false, turns screen off if not already off.</param>
        public void SetScreenOn(bool powered)
        {
            if (powered != StartOfRound.Instance.mapScreen.isScreenOn) { ToggleScreenPower(); }
        }
        /// <summary>
        /// Inverts screen powered state
        /// </summary>
        public void ToggleScreenPower()
        {
            ToggleScreenPower(!StartOfRound.Instance.mapScreen.isScreenOn);
        }
        private void ToggleScreenPower(bool power)
        {
            if (power) { StartOfRound.Instance.mapScreen.SwitchScreenOn(true); }
            else { StartOfRound.Instance.mapScreen.SwitchScreenOn(false); }
        }
        /// <summary>
        /// Switches who the monitor is spectating
        /// </summary>
        public void SwitchScreenSpectatorToggle()
        {
            StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(true);
        }
    }
}
