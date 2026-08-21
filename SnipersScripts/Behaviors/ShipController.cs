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
            if (powered != StartOfRound.Instance.magnetOn) { CycleMagnetPower(); } // only cycle it if its current state isn't what is wanted
        }
        /// <summary>
        /// Inverts current magnet state
        /// </summary>
        public void CycleMagnetPower()
        {
            CycleMagnetPower(true);
        }
        private void CycleMagnetPower(bool powered)
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
        /// Makes the ship take off
        /// </summary>
        public void ShipTakeoff()
        {
            if (FindFirstObjectByType<StartMatchLever>().triggerScript.interactable)
            {
                FindFirstObjectByType<StartMatchLever>().LeverAnimation();
                StartOfRound.Instance.ShipLeave(); 
            }
            else
            {
                SnipersScripts.Logger.LogWarning("Tried to take off when ship lever can't be pulled. Cancelling action.");
            }         
        }
        /// <summary>
        /// Makes the ship land
        /// </summary>
        public void ShipLand()
        {
            if (FindFirstObjectByType<StartMatchLever>().triggerScript.interactable)
            {
                FindFirstObjectByType<StartMatchLever>().LeverAnimation();
                StartOfRound.Instance.StartGame();
            }
            else
            {
                SnipersScripts.Logger.LogWarning("Tried to land when ship lever can't be pulled. Cancelling action.");
            }
        }
        /// <summary>
        /// Makes the ship land or takeoff (whichever it isn't currently)
        /// </summary>
        public void ShipFlightToggle()
        {
            if (StartOfRound.Instance.shipHasLanded) { ShipTakeoff(); }
            else { ShipLand(); }
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
        public void OpenShipDoor()
        {
            FindFirstObjectByType<HangarShipDoor>().PlayDoorAnimation(false);
        }
        public void CloseShipDoor()
        {
            FindFirstObjectByType<HangarShipDoor>().PlayDoorAnimation(true);
        }
        public void ToggleShipDoor()
        {
            if (FindFirstObjectByType<HangarShipDoor>().shipDoorsAnimator.GetBool("Closed"))
            {
                OpenShipDoor();
            }
            else
            {
                CloseShipDoor();
            }
        }

        [Header("Map Radar")]
        public UnityEngine.Events.UnityEvent onScreenTurnOn;
        public UnityEngine.Events.UnityEvent onScreenTurnOff;
        public UnityEngine.Events.UnityEvent onScreenPoweredToggle;
        public UnityEngine.Events.UnityEvent onScreenSpectatorToggle;
        public void TurnScreenOn()
        {
            StartOfRound.Instance.mapScreen.SwitchScreenOn(true);
        }
        public void TurnScreenOff()
        {
            StartOfRound.Instance.mapScreen.SwitchScreenOn(false);
        }
        public void ToggleScreenPower()
        {
            if (StartOfRound.Instance.mapScreen.isScreenOn)
            {
                TurnScreenOff();
            }
            else
            {
                TurnScreenOn();
            }
        }
        public void SwitchScreenSpectatorToggle()
        {
            StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(true);
        }
    }
}
