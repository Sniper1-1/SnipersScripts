using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/InteractTriggerInteractableToggle")]
    internal class InteractTriggerInteractableToggle: MonoBehaviour
    {
        public InteractTrigger interactTrigger;

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
