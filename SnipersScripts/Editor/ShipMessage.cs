using UnityEngine;

namespace SnipersScripts.Editor
{
    [CreateAssetMenu(fileName = "ShipMessage", menuName = "SnipersScripts/ShipMessage")]
    public class ShipMessageSO : ScriptableObject
    {
        [SerializeField]
        public DialogueSegment[] shipMessage;
    }
}
