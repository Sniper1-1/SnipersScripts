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
        public void SetMagnetPowered(bool powered)
        {
            if (powered != StartOfRound.Instance.magnetOn) { CycleMagnetPower(); }
        }
        public void CycleMagnetPower()
        {
            CycleMagnetPower(true);
        }
        private void CycleMagnetPower(bool powered)
        {
            StartOfRound.Instance.SetMagnetOn(powered);
            StartOfRound.Instance.magnetLever.TriggerAnimation(StartOfRound.Instance.localPlayerController);
        }
        
    }
}
