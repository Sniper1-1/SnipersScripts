using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/AudioClipEvents")]
    public class AudioClipEvents: MonoBehaviour
    {
        [Tooltip("The AudioSource to use")]
        public AudioSource audioSource;
        [Tooltip("Event to run when the AudioClip starts playing")]
        public UnityEngine.Events.UnityEvent onAudioClipStart;
        [Tooltip("Event to run when the AudioClip finishes playing")]
        public UnityEngine.Events.UnityEvent onAudioClipEnd;
        [Tooltip("Event to run when the AudioClip is stopped")]
        public UnityEngine.Events.UnityEvent onAudioClipStop;

        private Coroutine audioCorutine = null;

        /// <summary>
        /// Plays the clip over an audio source and starts the timer.
        /// </summary>
        /// <param name="clip">The audio clip to play and wait to end.</param>
        public void PlayAudioClip(AudioClip clip)
        {
            if (audioSource == null)
            {
                SnipersScripts.Logger.LogWarning("AudioSource is not assigned.");
                return;
            }
            audioSource.clip = clip;
            audioSource.Play();
            onAudioClipStart.Invoke();
            audioCorutine = StartCoroutine(WaitForAudioClipEnd());
        }
        /// <summary>
        /// Stops the current clip and timer
        /// </summary>
        public void StopAudioClip()
        {
            if (audioSource == null)
            {
                SnipersScripts.Logger.LogWarning("AudioSource is not assigned.");
                return;
            }
            if (audioCorutine != null)
            {
                StopCoroutine(audioCorutine);
                audioCorutine = null;
            }
            audioSource.Stop();
            onAudioClipStop.Invoke();
        }

        private System.Collections.IEnumerator WaitForAudioClipEnd()
        {
            if (audioSource == null)
            {
                yield break;
            }
            yield return new WaitUntil(() => !audioSource.isPlaying);
            onAudioClipEnd.Invoke();
        }
    }
}
