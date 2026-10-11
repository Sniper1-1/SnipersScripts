using System.Collections.Generic;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/RandomPropEnabler")]
    public class RandomPropEnabler: MonoBehaviour
    {
        [SerializeField]
        [Tooltip("If true, only use the level seed. This means that additional objects with the same possibleObjects list will end up with the same result. If false, additional randomization is used.")]
        private bool onlyUseLevelSeed = false;
        [SerializeField]
        [Min(0)]
        private int minimumObjects = 0;
        [SerializeField]
        [Min(0)]
        private int maximumObjects = 1;
        [SerializeField]
        [Tooltip("The weighted list of objects to choose from.")]
        private List<RandomObjectSelectorData> possibleObjects = new List<RandomObjectSelectorData>();
        [SerializeField]
        [Tooltip("If true, randomized on start. If false, call Randomize() to randomize.")]
        private bool randomizeOnStart = true;

        private System.Random random;

        private void Start()
        {
            // seed is either just the level seed, or it is the combination of the level seed and the position of this object
            int seed = onlyUseLevelSeed ? RoundManager.Instance.playersManager.randomMapSeed : RoundManager.Instance.playersManager.randomMapSeed + (int)(this.transform.position.x * 100) + (int)(this.transform.position.y * 100) + (int)(this.transform.position.z * 100);

            random = new System.Random(seed);

            if (maximumObjects > possibleObjects.Count)
            {
                SnipersScripts.Logger.LogWarning($"RandomPropEnabler on {this.name} has maximumObjects ({maximumObjects}) greater than possibleObjects.Count ({possibleObjects.Count}). Setting maximumObjects to possibleObjects.Count.");
                maximumObjects = possibleObjects.Count;
            }

            if (minimumObjects > maximumObjects)
            {
                SnipersScripts.Logger.LogWarning($"RandomPropEnabler on {this.name} has minimumObjects ({minimumObjects}) greater than maximumObjects ({maximumObjects}). Switching.");
                int temp = minimumObjects;
                minimumObjects = maximumObjects;
                maximumObjects = temp;
            }

            if (randomizeOnStart)
            {
                Randomize();
            }
        }

        public void Randomize()
        {
            int numberOfObjectsToEnable = random.Next(minimumObjects, maximumObjects + 1);
            var tempList = new List<RandomObjectSelectorData>(possibleObjects);

            foreach (RandomObjectSelectorData container in possibleObjects) // objects default to disabled
            {
                container.gameOjbect.SetActive(false);
            }

            for (int i = 0; i < numberOfObjectsToEnable; i++) // copies the list over to a temp one, goes through numberOfObjectsToEnalbe times,
                                                              // picks something, and removes from the list so it can't be picked again.
            {
                if (tempList.Count == 0)
                {
                    break;
                }
                RandomObjectSelectorData randomlySelectedObject = WeightedSelector<RandomObjectSelectorData>.SelectWeightedRandom(tempList, random.Next());
                if (randomlySelectedObject.gameOjbect != null)
                {
                    randomlySelectedObject.gameOjbect.SetActive(true);
                }
                tempList.Remove(randomlySelectedObject);
            }
        }

        [System.Serializable]
        public class RandomObjectSelectorData : IWeighted
        {
            [Tooltip("The materials applied to the mesh renderer if chosen")]
            public GameObject gameOjbect;
            [Min(0.0f)]
            public float weight;

            public float Weight => weight;
        }
    }
}
