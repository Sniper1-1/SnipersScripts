using System.Collections.Generic;
using GameNetcodeStuff;
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

        [Header("Magnet")]
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

        [Header("ShipFlight")]
        public UnityEngine.Events.UnityEvent onShipDescend;
        public UnityEngine.Events.UnityEvent onShipLand;
        public UnityEngine.Events.UnityEvent onShipAscend;
        public UnityEngine.Events.UnityEvent onShipEnterOrbit;
        public void ShipTakeoff()
        {
            FindFirstObjectByType<StartMatchLever>().LeverAnimation();
            StartOfRound.Instance.ShipLeave();            
        }
        public void ShipLand()
        {
            FindFirstObjectByType<StartMatchLever>().LeverAnimation();
            StartOfRound.Instance.StartGame();            
        }
        public void ShipFlightToggle()
        {
            if (StartOfRound.Instance.shipHasLanded) { ShipTakeoff(); }
            else { ShipLand(); }
        }
    }
}
