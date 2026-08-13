using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/WaitRandomTime")]
    internal class WaitRandomTime: MonoBehaviour
    {
        [Min(0f)]
        public float minWaitTime = 1f;
        [Min(0f)]
        public float maxWaitTime = 5f;

        [Tooltip("True, timer starts automatically. False, timer must be started manually.")]
        public bool runOnStart = false;

        [Tooltip("Event to run when the wait starts")]
        public UnityEngine.Events.UnityEvent onWaitStart;
        [Tooltip("Event to run when the wait is complete")]
        public UnityEngine.Events.UnityEvent onWaitComplete;
        [Tooltip("Event to run when the wait is stopped")]
        public UnityEngine.Events.UnityEvent onWaitStop;

        private Coroutine waitCoroutine = null;
        private System.Random random;

        public void Start()
        {
            random ??= new(RoundManager.Instance.playersManager.randomMapSeed); //seeds the random number generator with the same seed as round if random is null
            if (runOnStart)
            {
                StartWait();
            }
        }

        public void StartWait()
        {
            if (minWaitTime > maxWaitTime)
            {
                SnipersScripts.Logger.LogWarning($"WaitRandomTime: minWaitTime ({minWaitTime}) is greater than maxWaitTime ({maxWaitTime}). Switching values.");
                float temp = minWaitTime;
                minWaitTime = maxWaitTime;
                maxWaitTime = temp;
            }

            waitCoroutine = StartCoroutine(Wait());
        }

        private System.Collections.IEnumerator Wait()
        {
            onWaitStart?.Invoke();
            float waitTime = minWaitTime + (float)(random.NextDouble() * (maxWaitTime - minWaitTime)); // generate a random wait time between minWaitTime and maxWaitTime (NextDouble only gives between 0 and 1 so it's multiplied to scale it properly)
            yield return new WaitForSeconds(waitTime);
            onWaitComplete?.Invoke();
        }

        public void StopWait()
        {
            if (waitCoroutine != null)
            {
                onWaitStop?.Invoke();
                StopCoroutine(waitCoroutine);
                waitCoroutine = null;
            }
        }
    }
}
