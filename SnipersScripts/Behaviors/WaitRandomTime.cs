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
        [Tooltip("If true, once a time is chosen between min and max, it will not be re-rolled on subsequent waits.")]
        public bool onlyRandomizeOnce = false;

        [Tooltip("Event to run when the wait starts")]
        public UnityEngine.Events.UnityEvent onWaitStart;
        [Tooltip("Event to run when the wait is complete")]
        public UnityEngine.Events.UnityEvent onWaitComplete;
        [Tooltip("Event to run when the wait is stopped")]
        public UnityEngine.Events.UnityEvent onWaitStop;

        private Coroutine waitCoroutine = null;
        private System.Random random;
        private bool timeIsLocked = false; // used in sync with onlyRandomizedOnce to determine if a new time needs to be gotten
        private float currentWaitingTime = 0f;

        public void Start()
        {
            if (runOnStart) { StartWait(); }
        }

        public void StartWait()
        {
            random ??= new(RoundManager.Instance.playersManager.randomMapSeed + (int)base.transform.position.x + (int)base.transform.position.y + (int)base.transform.position.z); //seeds the random number generator with the same seed as round if random is null (also factoring in the object's position)
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
            yield return new WaitForSeconds(GetWaitTime());
            onWaitComplete?.Invoke();
        }

        /// <summary>
        /// picks a time between min and max to wait each time called unless onlyRandomizeOnce is true, then it will always return the first rolled wait time
        /// </summary>
        /// <returns>the currentWaitingTime</returns>
        private float GetWaitTime()
        {
            if (!timeIsLocked)
            { 
                currentWaitingTime = minWaitTime + (float)(random.NextDouble() * (maxWaitTime - minWaitTime)); // generate a random wait time between minWaitTime and maxWaitTime (NextDouble only gives between 0 and 1 so it's multiplied to scale it properly)
                if (onlyRandomizeOnce) { timeIsLocked = true; }
            }
            return currentWaitingTime;
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
