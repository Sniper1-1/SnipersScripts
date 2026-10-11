using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/InteractTriggerInteractableToggle")]
    internal class InteractTriggerInteractableToggle: MonoBehaviour
    {
        public InteractTrigger interactTrigger;

        /// <summary>
        /// Sets whether the interact trigger is interactable.
        /// </summary>
        /// <param name="isInteractable">true to make the interact trigger interactable; otherwise, false.</param>
        public void SetInteractable(bool isInteractable)
        {
            if (interactTrigger != null)
            {
                interactTrigger.interactable = isInteractable;
            }
            else
            {
                printError();
            }
        }

        /// <summary>
        /// Sets the hover tip text for the interact trigger when interactable.
        /// </summary>
        /// <param name="hoverTip">The text to display as the hover tip.</param>
        public void SetHoverTip(string hoverTip)
        {
            if (interactTrigger != null)
            {
                interactTrigger.hoverTip = hoverTip;
            }
            else
            {
                printError();
            }
        }

        /// <summary>
        /// Sets the hover tip text for the interact trigger when not interactable.
        /// </summary>
        /// <param name="disabledHoverTip">The text to display as the disabled hover tip.</param>
        public void SetDisabledHoverTip(string disabledHoverTip)
        {
            if (interactTrigger != null)
            {
                interactTrigger.disabledHoverTip = disabledHoverTip;
            }
            else
            {
                printError();
            }                
        }

        private void printError()
        {
            SnipersScripts.Logger.LogWarning($"InteractTriggerInteractableToggle on {this.name} has no InteractTrigger assigned.");
        }
    }
}
