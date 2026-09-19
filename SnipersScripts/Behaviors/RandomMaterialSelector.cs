using System.Collections.Generic;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/RandomMaterialSelector")]
    public class RandomMaterialSelector : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("If true, only use the level seed. This means that additional objects with the same possibleMaterials list will end up with the same result. If false, additional randomization is used.")]
        private bool onlyUseLevelSeed = false;
        [SerializeField]
        [Tooltip("If true, all mesh renderers will share the same outcome. If false, each mesh renderer will have its own outcome.")]
        private bool renderersShareOutcome = false;
        [SerializeField]
        [Tooltip("The mesh renderers to randomize the materials of.")]
        private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        [SerializeField]
        [Tooltip("The weighted list of materials to choose from.")]
        private List<RandomMaterialSelectorData> possibleMaterials = new List<RandomMaterialSelectorData>();
        public void Start()
        {
            // seed is either just the level seed, or it is the combination of the level seed and the position of this object
            int seed = onlyUseLevelSeed ? RoundManager.Instance.playersManager.randomMapSeed : RoundManager.Instance.playersManager.randomMapSeed+(int)(this.transform.position.x*100)+ (int)(this.transform.position.y*100)+ (int)(this.transform.position.z*100);

            System.Random random = new System.Random(seed);
            if (renderersShareOutcome)
            {   
                //only randomize once and then apply that to all renderers
                RandomMaterialSelectorData randomlySelectedMaterials = WeightedSelector<RandomMaterialSelectorData>.SelectWeightedRandom(possibleMaterials, random.Next());
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    if (meshRenderer != null && randomlySelectedMaterials.materials.Count > 0)
                    {
                        meshRenderer.sharedMaterials = randomlySelectedMaterials.materials.ToArray();
                    }
                }
            }
            else
            {
                for (int i = 0; i < meshRenderers.Count; i++)
                {
                    if (meshRenderers[i] != null)
                    {
                        //each renderer gets its own randomization
                        RandomMaterialSelectorData randomlySelectedMaterials = WeightedSelector<RandomMaterialSelectorData>.SelectWeightedRandom(possibleMaterials, random.Next());
                        if (randomlySelectedMaterials.materials.Count > 0)
                        {
                            meshRenderers[i].sharedMaterials = randomlySelectedMaterials.materials.ToArray();
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class RandomMaterialSelectorData: IWeighted
    {
        [Tooltip("The materials applied to the mesh renderer if chosen")]
        public List<Material> materials = new List<Material>();
        [Min(0.0f)]
        public float weight;

        public float Weight => weight;
    }
}
