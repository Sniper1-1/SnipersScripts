using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/EntranceTeleportLock")]
    public class EntranceTeleportLock: NetworkBehaviour
    {
        // Registry of every active locker currently loaded
        internal static readonly List<EntranceTeleportLock> ActiveLockers = new List<EntranceTeleportLock>();
        private void OnEnable() { ActiveLockers.Add(this); }
        private void OnDisable() { ActiveLockers.Remove(this); }

        [HideInInspector]
        public List<EntranceTeleport> teleports = new List<EntranceTeleport>();
        public string lockedMessage = "Door locked";

        // ----------------------------- all -----------------------------
        /// <summary>
        /// Locks all EntranceTeleports
        /// </summary>
        public void LockAll()
        {
            LockInside();
            LockOutside();
        }
        /// <summary>
        /// Unlocks all EntranceTeleports
        /// </summary>
        public void UnlockAll()
        {
            UnlockInside();
            UnlockOutside();
        }
        // ----------------------------- inside -----------------------------
        /// <summary>
        /// Locks inside EntranceTeleports
        /// </summary>
        public void LockInside()
        {
            LockMainInside();
            LockFireInside();
        }
        /// <summary>
        /// Unlocks inside EntranceTeleports
        /// </summary>
        public void UnlockInside()
        {
            UnlockMainInside();
            UnlockFireInside();
        }
        /// <summary>
        /// Locks the main EntranceTeleport inside
        /// </summary>
        public void LockMainInside()
        {
            foreach(var teleport in teleports)
            {
                if(teleport.entranceId == 0) { LockIndividualInside(teleport); }
            }
        }
        /// <summary>
        /// Unlocks the main EntranceTeleport inside
        /// </summary>
        public void UnlockMainInside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId == 0) { UnlockIndividualInside(teleport); }
            }
        }
        /// <summary>
        /// Locks the fire EntranceTeleports inside
        /// </summary>
        public void LockFireInside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId != 0) { LockIndividualInside(teleport); }
            }
        }
        /// <summary>
        /// Unlocks the fire EntranceTeleports inside
        /// </summary>
        public void UnlockFireInside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId != 0) { UnlockIndividualInside(teleport); }
            }
        }
        /// <summary>
        /// Locks a specific EntranceTeleport on the inside
        /// </summary>
        /// <param name="tp">The EntranceTeleport to lock the inside of</param>
        public void LockIndividualInside(EntranceTeleport tp)
        {
            if (tp.isEntranceToBuilding) { Lock(teleport: tp.exitScript, locked: true); }
            else if (!tp.isEntranceToBuilding) { Lock(teleport: tp, locked: true); }
        }
        /// <summary>
        /// Unlocks a specific EntranceTeleport on the inside
        /// </summary>
        /// <param name="tp">The EntranceTeleport to unlock the inside of</param>
        public void UnlockIndividualInside(EntranceTeleport tp)
        {
            if (tp.isEntranceToBuilding) { Lock(teleport: tp.exitScript, locked: false); }
            else if (!tp.isEntranceToBuilding) { Lock(teleport: tp, locked: false); }
        }
        // ----------------------------- outside -----------------------------
        /// <summary>
        /// Locks outside EntranceTeleports
        /// </summary>
        public void LockOutside()
        {
            LockMainOutside();
            LockFireOutside();
        }
        /// <summary>
        /// Unlocks outside EntranceTeleports
        /// </summary>
        public void UnlockOutside()
        {
            UnlockMainOutside();
            UnlockFireOutside();
        }
        /// <summary>
        /// Locks the main EntranceTeleport outside
        /// </summary>
        public void LockMainOutside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId == 0) { LockIndividualOutside(teleport); }
            }
        }
        /// <summary>
        /// Unlocks the main EntranceTeleport outside
        /// </summary>
        public void UnlockMainOutside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId == 0) { UnlockIndividualOutside(teleport); }
            }
        }
        /// <summary>
        /// Locks the fire EntranceTeleports outside
        /// </summary>
        public void LockFireOutside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId != 0) { LockIndividualOutside(teleport); }
            }
        }
        /// <summary>
        /// Unlocks the fire EntranceTeleports outside
        /// </summary>
        public void UnlockFireOutside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId != 0) { UnlockIndividualOutside(teleport); }
            }
        }
        /// <summary>
        /// Locks a specific EntranceTeleport on the outside
        /// </summary>
        /// <param name="tp">The EntranceTeleport to lock the outside of</param>
        public void LockIndividualOutside(EntranceTeleport tp)
        {
            if (!tp.isEntranceToBuilding) { Lock(teleport: tp.exitScript, locked: true); }
            else if (tp.isEntranceToBuilding) { Lock(teleport: tp, locked: true); }
        }
        /// <summary>
        /// Unlocks a specific EntranceTeleport on the outside
        /// </summary>
        /// <param name="tp">The EntranceTeleport to unlock the outside of</param>
        public void UnlockIndividualOutside(EntranceTeleport tp)
        {
            if (!tp.isEntranceToBuilding) { Lock(teleport: tp.exitScript, locked: false); }
            else if (tp.isEntranceToBuilding) { Lock(teleport: tp, locked: false); }
        }

        // ----------------------------- Helpers -----------------------------
        [Rpc(SendTo.Everyone, RequireOwnership = false)]
        public void SetLockedMessageRpc(string message = null) //change what message displays on the door when locked
        {
            lockedMessage = message;
            foreach(var teleport in teleports) { teleport.triggerScript.disabledHoverTip = lockedMessage; }
        }

        private void Lock(EntranceTeleport teleport, bool locked) //set a specific EntranceTeleport to locked/unlocked
        {
            if (teleport != null && teleport.triggerScript != null) 
            {
                LockRpc(id: teleport.entranceId, locked: locked, outside: teleport.isEntranceToBuilding); //why can't Rpcs use anything more than basic datatypes! :(
            }
        }
        [Rpc(SendTo.Everyone, RequireOwnership = false)] //all this because I can't just pass custom classes :(
        private void LockRpc(int id, bool locked, bool outside) //syncs its state to everyone
        {
            SetLockedMessageRpc(lockedMessage);
            foreach(var teleport in teleports)
            {
                if (teleport.entranceId==id && teleport.isEntranceToBuilding==outside) { teleport.triggerScript.interactable = !locked; }
            }            
        }
    }
}
