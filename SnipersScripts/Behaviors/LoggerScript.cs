using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/LoggerScript")]
    public class  LoggerScript: MonoBehaviour
    {
        private static readonly string MESSAGE_STARTER = "SnipersScripts' LoggerScript says: ";

        [Tooltip("The message to log to the console on Awake. Left empty, logs can only be printed by calling the print methods from other events.")]
        public string log=string.Empty;
        [Tooltip("The channel to log message to if log is not empty")]
        public type logType;
        public enum type { Debug, Info, Warning, Error, Fatal }

        private void Awake()
        {
            if (!string.IsNullOrEmpty(log))
            {
                if (logType == type.Debug) { printDebug(log); }
                if (logType == type.Info) { printInfo(log); }
                if (logType == type.Warning) { printWarning(log); }
                if (logType == type.Error) { printError(log); }
                if (logType == type.Fatal) { printFatal(log); }
            }
        }
        public void printDebug(string message) { SnipersScripts.Logger.LogDebug($"{MESSAGE_STARTER}\"{message}\""); }
        public void printInfo(string message) { SnipersScripts.Logger.LogInfo($"{MESSAGE_STARTER}\"{message}\""); }
        public void printWarning(string message) { SnipersScripts.Logger.LogWarning($"{MESSAGE_STARTER}\"{message}\""); }
        public void printError(string message) { SnipersScripts.Logger.LogError($"{MESSAGE_STARTER}\"{message}\""); }
        public void printFatal(string message) { SnipersScripts.Logger.LogFatal($"{MESSAGE_STARTER}\"{message}\""); }
    }
}
