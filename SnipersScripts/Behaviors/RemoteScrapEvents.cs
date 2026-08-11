using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/RemoteScrapEvents")]
    public class RemoteScrapEvents: MonoBehaviour
    {
        [Tooltip("Event to run when the remote scrap item is clicked")]
        public UnityEngine.Events.UnityEvent onRemoteClicked;
    }
}
