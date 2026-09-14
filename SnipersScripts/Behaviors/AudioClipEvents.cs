using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/AudioClipEvents")]
    public class AudioClipEvents: MonoBehaviour
    {
        [Tooltip("The AudioSource to use. If null, will wait for the duration of the clip silently.")]
        public AudioSource audioSource;
        [Tooltip("Event to run when the AudioClip starts playing")]
        public UnityEngine.Events.UnityEvent onAudioClipStart;
        [Tooltip("Event to run when the AudioClip finishes playing")]
        public UnityEngine.Events.UnityEvent onAudioClipEnd;
        [Tooltip("Event to run when the AudioClip is stopped")]
        public UnityEngine.Events.UnityEvent onAudioClipStop;

        private Coroutine audioCorutine = null;

        /// <summary>
        /// Plays the clip over an audio source (if provided) and starts the timer.
        /// </summary>
        /// <param name="clip">The audio clip to play and wait to end.</param>
        public void PlayAudioClip(AudioClip clip)
        {
            if (audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
            onAudioClipStart.Invoke();
            audioCorutine = StartCoroutine(WaitForAudioClipEnd(clip));
        }
        /// <summary>
        /// Stops the current clip and timer
        /// </summary>
        public void StopAudioClip()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
            if (audioCorutine != null)
            {
                StopCoroutine(audioCorutine);
                audioCorutine = null;
            }
            onAudioClipStop.Invoke();
        }

        private System.Collections.IEnumerator WaitForAudioClipEnd(AudioClip clip = null)
        {
            if (audioSource == null && clip != null)
            {
                yield return new WaitForSeconds(clip.length);
            }
            else
            {
                yield return new WaitUntil(() => !audioSource.isPlaying);
            }
            onAudioClipEnd.Invoke();
        }
    }
}
