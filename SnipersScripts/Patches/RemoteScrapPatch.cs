using HarmonyLib;
using SnipersScripts.Behaviors;
using UnityEngine;

namespace SnipersScripts.Patches
{
    [HarmonyPatch(typeof(RemoteProp))]
    internal class RemoteScrapPatch
    {
        // invokes all the RemoteScrapEvents when the remote scrap item is clicked
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RemoteProp), nameof(RemoteProp.ItemActivate))]
        private static void ItemActivate_Postfix(RemoteProp __instance)
        {
            var remoteScrapEvents = GameObject.FindObjectsOfType<RemoteScrapEvents>();
            foreach (var remoteScrapEvent in remoteScrapEvents)
            {
                remoteScrapEvent.onRemoteClicked?.Invoke();
            }
        }
    }
}
