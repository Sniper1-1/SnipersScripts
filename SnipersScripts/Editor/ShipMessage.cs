using UnityEngine;

namespace SnipersScripts.Editor
{
    [CreateAssetMenu(fileName = "ShipMessage", menuName = "SnipersScripts/ShipMessageSO")]
    public class ShipMessageSO : ScriptableObject
    {
        [SerializeField]
        [Tooltip("The alerts like the ones that appear when the ship leaves at midnight.")]
        public DialogueSegment[] shipMessage;
    }
}
