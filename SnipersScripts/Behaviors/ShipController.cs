using System.Collections.Generic;
using System.Linq;
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

        [Header("Ship Communication")]
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
        /// Plays the provided audio clip on the ship speaker, muting the speaker if clip is null
        /// </summary>
        /// <param name="audioClip">The audio to play over the ship speaker. Mutes ship speaker if null.</param>
        public void SetShipSpeakerAudio(AudioClip audioClip)
        {
            if (audioClip != null && !StartOfRound.Instance.speakerAudioSource.isPlaying) { StartOfRound.Instance.speakerAudioSource.PlayOneShot(audioClip); }
            else if (audioClip == null && StartOfRound.Instance.speakerAudioSource.isPlaying) { StartOfRound.Instance.DisableShipSpeaker(); }
        }

        public UnityEngine.Events.UnityEvent onSignalTransmitStart;
        public UnityEngine.Events.UnityEvent onSignalTransmitEnd;
        private SignalTranslator signalTranslator = null;
        public void TransmitMessage(string message)
        {
            if (signalTranslator == null)
            {
                if (!FindShipComponent<SignalTranslator>())
                {
                    SnipersScripts.Logger.LogWarning("Tried to transmit over nonexisting transmitter. Cancelling.");
                    return;
                }
            }
            HUDManager.Instance.UseSignalTranslatorServerRpc(message);
        }

        public UnityEngine.Events.UnityEvent onHornPull;
        public UnityEngine.Events.UnityEvent whileHornPulled;
        public UnityEngine.Events.UnityEvent onHornRelease;
        private ShipAlarmCord shipHorn = null;
        /// <summary>
        /// Pulls the ship horn if it exists
        /// </summary>
        public void PullHorn()
        {
            if (shipHorn == null)
            {
                if (!FindShipComponent<ShipAlarmCord>())
                {
                    SnipersScripts.Logger.LogWarning("Tried to use non-existing horn. Cancelling.");
                    return;
                }
            }
            shipHorn?.HoldCordDown();
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

        [Header("Teleporters")]
        public UnityEngine.Events.UnityEvent onTeleportStart;
        public UnityEngine.Events.UnityEvent onTeleportEnd;
        public UnityEngine.Events.UnityEvent onInverseStart;
        public UnityEngine.Events.UnityEvent onInverseEnd;
        private ShipTeleporter teleporterNormal = null;
        private ShipTeleporter teleporterInverse = null;
        /// <summary>
        /// Used to activate the normal (if false) or inverse (if true) teleporter
        /// </summary>
        /// <param name="inverse">False: normal teleporter. True: inverse teleporter.</param>
        public void TeleportPlayer(bool inverse)
        {
            if ((!inverse && teleporterNormal == null ) || (inverse && teleporterInverse == null))
            {
                if (!FindShipComponent<ShipTeleporter>(inverseTeleporter: inverse))
                {
                    SnipersScripts.Logger.LogWarning("Tried to use non-existing teleporter. Cancelling.");
                    return;
                }
            }
            if (!inverse && teleporterNormal.buttonTrigger.interactable) { teleporterNormal?.PressTeleportButtonOnLocalClient(); }
            else if (inverse && teleporterInverse.buttonTrigger.interactable) { teleporterInverse?.PressTeleportButtonOnLocalClient(); }
        }

        [Header("Lights")]
        public UnityEngine.Events.UnityEvent onShipLightsTurnOn;
        public UnityEngine.Events.UnityEvent onShipLightsTurnOff;
        public UnityEngine.Events.UnityEvent onShipLightsToggle;
        private AnimatedObjectTrigger lightSwitch = null;
        public void SetShipLightsOn(bool on)
        {
            if (on != StartOfRound.Instance.shipRoomLights.areLightsOn) { ToggleShipLights(); }
        }
        public void ToggleShipLights()
        {
            if (lightSwitch == null)
            {
                if (!FindShipComponent<AnimatedObjectTrigger>(gameObjectName: "LightSwitch"))
                {
                    SnipersScripts.Logger.LogWarning("Somehow the light switch is missing.");
                    return;
                }
            }
            lightSwitch?.TriggerAnimation(StartOfRound.Instance.localPlayerController);
            StartOfRound.Instance.shipRoomLights.ToggleShipLights();
        }

        [Header("Electric Chair")]
        public UnityEngine.Events.UnityEvent onClampLock;
        public UnityEngine.Events.UnityEvent onClampUnlock;
        public UnityEngine.Events.UnityEvent onClampToggle;
        public UnityEngine.Events.UnityEvent onShockStart;
        public UnityEngine.Events.UnityEvent onShockEnd;
        private MoveToExitSpecialAnimation electricChair = null;
        public void SetChairClampped(bool clamp)
        {
            if (FindElectricChair())
            {
                if (clamp != electricChair.exitingDisabled) { ToggleChairClamps(); }
            }
            else
            {
                SnipersScripts.Logger.LogWarning("Tried to interact with nonexisting electric chair. Cancelling.");
            }
        }
        public void ToggleChairClamps()
        {
            if (FindElectricChair()) { ToggleChairClamps(!electricChair.exitingDisabled); }
            else { SnipersScripts.Logger.LogWarning("Tried to interact with nonexisting electric chair. Cancelling."); }
        }
        private void ToggleChairClamps(bool clamp)
        {
            electricChair.SetExitingDisabled(clamp);
            electricChair.animatedObjectTrigger.TriggerAnimation(StartOfRound.Instance.localPlayerController);
        }
        public void ShockElectricChair()
        {
            if (FindElectricChair()) { electricChair.OnShipPowerSurge(); }
            else { SnipersScripts.Logger.LogWarning("Tried to interact with nonexisting electric chair. Cancelling."); }
        }
        private bool FindElectricChair()
        {
            if (electricChair != null) { return true; }
            else { return FindShipComponent<MoveToExitSpecialAnimation>(); }
        }

        [Header("TV")]
        public UnityEngine.Events.UnityEvent onTvTurnOn;
        public UnityEngine.Events.UnityEvent OnTvTurnOff;
        public UnityEngine.Events.UnityEvent onTvToggle;
        public UnityEngine.Events.UnityEvent onTvStationChange;
        private TVScript tv = null;
        public void SetTvOn(bool on)
        {
            if (FindTv())
            {
                if (on != tv.tvOn) { ToggleTv(); }
            }
            else { SnipersScripts.Logger.LogWarning("Tried to interact with nonexisting TV. Cancelling."); }
        }
        public void ToggleTv()
        {
            if (FindTv()) { ToggleTv(!tv.tvOn); }
            else { SnipersScripts.Logger.LogWarning("Tried to interact with nonexisting TV. Cancelling."); }
        }
        private void ToggleTv(bool on)
        {
            tv.SwitchTVLocalClient();
        }
        private bool FindTv()
        {
            if (tv != null) { return true; }
            else { return FindShipComponent<TVScript>(); }
        }

        //-------------------------------------------------------------------------------

        /// <summary>
        /// Generic function used to find various ship upgrade classes
        /// </summary>
        /// <typeparam name="T">Used to locate the specific type of ship component</typeparam>
        /// <param name="inverseTeleporter">True if teleporter is inverse teleporter, false if regular teleporter</param>
        /// <param name="gameObjectName">For the light switch, it does not have a good direct reference and must be found by name</param>
        /// <returns>True if found, false if not</returns>
        private bool FindShipComponent<T>(bool inverseTeleporter = false, string gameObjectName = "LightSwitch")
        {
            if(typeof(T) == typeof(ShipTeleporter))
            {
                List<ShipTeleporter> possibleTeleports = FindObjectsInSampleScene<ShipTeleporter>();
                foreach (ShipTeleporter teleporter in possibleTeleports)
                {
                    if (inverseTeleporter && teleporter.isInverseTeleporter) { teleporterInverse =  teleporter; return true; }
                    else if (!inverseTeleporter && !teleporter.isInverseTeleporter) { teleporterNormal = teleporter; return true; }
                }
            }
            if (typeof(T) == typeof(SignalTranslator)) 
            {
                foreach (SignalTranslator translator in FindObjectsInSampleScene<SignalTranslator>())
                {
                    signalTranslator = translator;
                    if (signalTranslator != null) { return true; } 
                }                
            }
            if (typeof(T) == typeof(ShipAlarmCord))
            {
                foreach (ShipAlarmCord horn in FindObjectsInSampleScene<ShipAlarmCord>())
                {
                    shipHorn = horn;
                    if (shipHorn != null) { return true; }
                }
            }
            if (typeof(T) == typeof(AnimatedObjectTrigger) && gameObjectName == "LightSwitch")
            {
                foreach (AnimatedObjectTrigger trigger in FindObjectsInSampleScene<AnimatedObjectTrigger>())
                {
                    if (trigger.gameObject.name == "LightSwitch")
                    {
                        lightSwitch = trigger;
                        if (lightSwitch != null) { return true; }
                    }
                }
            }
            if (typeof (T) == typeof(MoveToExitSpecialAnimation))
            {
                foreach (MoveToExitSpecialAnimation seat in FindObjectsInSampleScene<MoveToExitSpecialAnimation>())
                {
                    if (seat.electricChair) { electricChair = seat; return true; }
                }
            }
            if (typeof (T) == typeof(TVScript))
            {
                foreach (TVScript tvscript in FindObjectsInSampleScene<TVScript>())
                {
                    tv = tvscript;
                    if (tv != null) { return true; }
                }
            }
            return false;
        }
        /// <summary>
        /// Used to get only ship upgrade components out of SampleSceneRelay in case for whatever reason a moon has them
        /// </summary>
        /// <typeparam name="T">Used to locate the specific type of ship component</typeparam>
        /// <returns>List of matching components in SampleSceneRelay</returns>
        private List<T> FindObjectsInSampleScene<T>() where T : Component // where T : Component ensures T is a component
        {
            return FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .Where(component => component.gameObject.scene.name == "SampleSceneRelay")
            .ToList();
        }
    }
}
