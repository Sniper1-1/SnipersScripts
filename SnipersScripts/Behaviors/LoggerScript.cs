using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/LoggerScript")]
    public class  LoggerScript: MonoBehaviour
    {
        private static readonly string MESSAGE_STARTER = "SnipersScripts' LoggerScript says: ";

        [Tooltip("The message to log to the console on Start. Left empty, logs can only be printed by calling the print methods from other events.")]
        public string log=string.Empty;
        [Tooltip("The channel to log message to if log is not empty")]
        public type logType;
        public enum type { Debug, Info, Warning, Error, Fatal }
        [Tooltip("True: logs `Log` on start through the selected `Log Type` channel. False: logs only on manual invocation.")]
        public bool logOnStart = true;

        private void Start()
        {
            if(logOnStart) { PrintCurrentLogOnCurrentType(); }
        }
        /// <summary>
        /// Prints whatever is set in the component
        /// </summary>
        public void PrintCurrentLogOnCurrentType()
        {
            if (!string.IsNullOrEmpty(log))
            {
                if (logType == type.Debug) { PrintDebug(log); }
                if (logType == type.Info) { PrintInfo(log); }
                if (logType == type.Warning) { PrintWarning(log); }
                if (logType == type.Error) { PrintError(log); }
                if (logType == type.Fatal) { PrintFatal(log); }
            }
        }
        public void PrintDebug(string message) { SnipersScripts.Logger.LogDebug($"{MESSAGE_STARTER}\"{message}\""); }
        public void PrintInfo(string message) { SnipersScripts.Logger.LogInfo($"{MESSAGE_STARTER}\"{message}\""); }
        public void PrintWarning(string message) { SnipersScripts.Logger.LogWarning($"{MESSAGE_STARTER}\"{message}\""); }
        public void PrintError(string message) { SnipersScripts.Logger.LogError($"{MESSAGE_STARTER}\"{message}\""); }
        public void PrintFatal(string message) { SnipersScripts.Logger.LogFatal($"{MESSAGE_STARTER}\"{message}\""); }
    }
}
