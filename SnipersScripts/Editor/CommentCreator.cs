using UnityEngine;

namespace SnipersScripts.Editor
{
    [CreateAssetMenu(fileName = "Comment", menuName = "SnipersScripts/Comment")]
    public class CommentSO : ScriptableObject
    {
        [SerializeField]
        [TextArea(1, 20)]
        private string commentText = string.Empty;
    }
    [AddComponentMenu("SnipersScripts/Comment")]
    public class  CommentComponent : MonoBehaviour
    {
        [SerializeField]
        [TextArea(1, 5)]
        private string commentText = string.Empty;
    }
}