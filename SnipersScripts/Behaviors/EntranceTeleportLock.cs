using System.Collections.Generic;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/EntranceTeleportLock")]
    public class EntranceTeleportLock: MonoBehaviour
    {
        // Registry of every active locker currently loaded
        internal static readonly List<EntranceTeleportLock> ActiveLockers = new List<EntranceTeleportLock>();
        private void OnEnable() { ActiveLockers.Add(this); }
        private void OnDisable() { ActiveLockers.Remove(this); }

        [HideInInspector]
        public List<EntranceTeleport> teleports = new List<EntranceTeleport>();
        public string lockedMessage = "Door locked";

        //all
        public void LockAll()
        {
            LockInside();
            LockOutside();
        }
        public void UnlockAll()
        {
            UnlockInside();
            UnlockOutside();
        }
        //inside
        public void LockInside()
        {
            LockMainInside();
            LockFireInside();
        }
        public void UnlockInside()
        {
            UnlockMainInside();
            UnlockFireInside();
        }
        public void LockMainInside()
        {
            foreach(var teleport in teleports)
            {
                if(teleport.entranceId == 0) { LockIndividualInside(teleport); }
            }
        }
        public void UnlockMainInside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId == 0) { UnlockIndividualInside(teleport); }
            }
        }
        public void LockFireInside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId != 0) { LockIndividualInside(teleport); }
            }
        }
        public void UnlockFireInside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId != 0) { UnlockIndividualInside(teleport); }
            }
        }
        public void LockIndividualInside(EntranceTeleport tp)
        {
            if (tp.isEntranceToBuilding) { Lock(teleport: tp.exitScript, locked: true); }
            else if (!tp.isEntranceToBuilding) { Lock(teleport: tp, locked: true); }
        }
        public void UnlockIndividualInside(EntranceTeleport tp)
        {
            if (tp.isEntranceToBuilding) { Lock(teleport: tp.exitScript, locked: false); }
            else if (!tp.isEntranceToBuilding) { Lock(teleport: tp, locked: false); }
        }
        //outside
        public void LockOutside()
        {
            LockMainOutside();
            LockFireOutside();
        }
        public void UnlockOutside()
        {
            UnlockMainOutside();
            UnlockFireOutside();
        }
        public void LockMainOutside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId == 0) { LockIndividualOutside(teleport); }
            }
        }
        public void UnlockMainOutside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId == 0) { UnlockIndividualOutside(teleport); }
            }
        }
        public void LockFireOutside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId != 0) { LockIndividualOutside(teleport); }
            }
        }
        public void UnlockFireOutside()
        {
            foreach (var teleport in teleports)
            {
                if (teleport.entranceId != 0) { UnlockIndividualOutside(teleport); }
            }
        }
        public void LockIndividualOutside(EntranceTeleport tp)
        {
            if (!tp.isEntranceToBuilding) { Lock(teleport: tp.exitScript, locked: true); }
            else if (tp.isEntranceToBuilding) { Lock(teleport: tp, locked: true); }
        }
        public void UnlockIndividualOutside(EntranceTeleport tp)
        {
            if (!tp.isEntranceToBuilding) { Lock(teleport: tp.exitScript, locked: false); }
            else if (tp.isEntranceToBuilding) { Lock(teleport: tp, locked: false); }
        }

        //-----------------Helpers-------------------------
        public void SetLockedMessage(string message = null)
        {
            lockedMessage = message;
            foreach(var teleport in teleports) { teleport.triggerScript.disabledHoverTip = lockedMessage; }
        }

        private void Lock(EntranceTeleport teleport, bool locked)
        {
            if (teleport != null && teleport.triggerScript != null) 
            {
                SetLockedMessage(lockedMessage);
                teleport.triggerScript.interactable = !locked;
            }
        }
    }
}
