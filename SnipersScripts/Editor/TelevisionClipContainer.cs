using UnityEngine;
using UnityEngine.Video;

namespace SnipersScripts.Editor
{
    [CreateAssetMenu(fileName = "TVClip", menuName = "SnipersScripts/TelevisionClipContainerSO")]
    public class TelevisionClipContainerSO : ScriptableObject
    {
        [Tooltip("The clip to play.")]
        public VideoClip clip;
        [Tooltip("The audio to play.")]
        public AudioClip audio;
        [Tooltip("Forces the tv on if it isn't currently.")]
        public bool forceTVOn = true;
        [Tooltip("Turns the TV off after completion. If false, resumes normal channels.")]
        public bool turnTVOff = true;
    }
}
