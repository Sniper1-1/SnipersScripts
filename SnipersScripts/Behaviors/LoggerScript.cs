using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/LoggerScript")]
    public class  LoggerScript: MonoBehaviour
    {
        private static readonly string MESSAGE_STARTER = "SnipersScripts' LoggerScript says: ";
        public void printDebug(string message) { SnipersScripts.Logger.LogDebug(MESSAGE_STARTER+message); }
        public void printInfo(string message) { SnipersScripts.Logger.LogInfo(MESSAGE_STARTER+message); }
        public void printWarning(string message) { SnipersScripts.Logger.LogWarning(MESSAGE_STARTER+message); }
        public void printError(string message) { SnipersScripts.Logger.LogError(MESSAGE_STARTER+message); }
        public void printFatal(string message) { SnipersScripts.Logger.LogFatal(MESSAGE_STARTER+message); }
    }
}
