using System.Linq;
using HarmonyLib;
using SnipersScripts;
using Unity.Netcode;
using UnityEngine;

//register network prefab to require all players to have mod installed
[HarmonyPatch(typeof(NetworkManager))]
internal static class NetworkPrefabPatch1
{
    private static readonly string MOD_GUID = MyPluginInfo.PLUGIN_GUID;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(NetworkManager.SetSingleton))]
    private static void RegisterPrefab()
    {
        var prefab = new GameObject(MOD_GUID + " Prefab");
        prefab.hideFlags |= HideFlags.HideAndDontSave;
        Object.DontDestroyOnLoad(prefab);
        var networkObject = prefab.AddComponent<NetworkObject>();
        networkObject.GlobalObjectIdHash = GetHash(MOD_GUID);

        NetworkManager.Singleton.PrefabHandler.AddNetworkPrefab(prefab);
        return;

        static uint GetHash(string value)
        {
            return value?.Aggregate(17u, (current, c) => unchecked((current * 31) ^ c)) ?? 0u;
        }
    }
}