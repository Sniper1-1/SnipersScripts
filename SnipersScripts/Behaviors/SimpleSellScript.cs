using System.Collections.Generic;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace SnipersScripts.Behaviors
{
    [AddComponentMenu("SnipersScripts/SimpleSellScript")]
    internal class SimpleSellScript : NetworkBehaviour
    {
        [Header("Sell Settings")]
        [Tooltip("If true, items sold will use the company buy rate. If false, the override buy rate will be used.")]
        public bool UseCompanyBuyRate = true;
        [Tooltip("The buy rate to use if UseCompanyBuyRate is false. 1 is the full 100% buy rate.")]
        public float OverrideBuyRate = 1f;
        [Tooltip("The maximum number of items that can be placed on the counter.")]
        public int maxItems = 150;

        [Header("Events")]
        [Tooltip("Invoked when an item is placed on the counter.")]
        public UnityEngine.Events.UnityEvent onItemPlaced;
        [Tooltip("Invoked when items are sold.")]
        public UnityEngine.Events.UnityEvent onSell;

        [Header("References & Audios")]
        [Tooltip("The collider used for item placement.")]
        public Collider placementCollider;
        private InteractTrigger placementTrigger;

        private List<GrabbableObject> itemsToSell = new List<GrabbableObject>();

        [Tooltip("The audio source used to play reward sounds.")]
        public AudioSource rewardsMusic;
        [Tooltip("Plays when your profit is > 1/4 of credits.")]
        public AudioClip rewardGood;
        [Tooltip("Plays when your profit is <= 1/4 of credits.")]
        public AudioClip rewardBad;

        private void Awake()
        {
            if (!placementCollider.TryGetComponent<InteractTrigger>(out placementTrigger))
            {
                SnipersScripts.Logger.LogError($"SimpleSellScript on {this.name} failed to find InteractTrigger for {placementCollider.name}");
            }
        }

        /// <summary>
        /// Places an item at a random location on the counter, removing it from the player's inventory.
        /// </summary>
        /// <param name="playerPlacingItem">The player placing the item on the counter</param>
        public void PlaceItem(PlayerControllerB playerPlacingItem)
        {
            if (itemsToSell.Count < maxItems && placementCollider!=null)
            {
                UnityEngine.Vector3 pointInCounterCollider = RoundManager.RandomPointInBounds(placementCollider.bounds);
                pointInCounterCollider.y = placementCollider.bounds.min.y;
                if (Physics.Raycast(new Ray(pointInCounterCollider + UnityEngine.Vector3.up * 3f, UnityEngine.Vector3.down), out var hitInfo, 8f, 1048640, QueryTriggerInteraction.Collide))
                {
                    pointInCounterCollider = hitInfo.point;
                }
                GrabbableObject itemToSell = playerPlacingItem.currentlyHeldObjectServer;
                pointInCounterCollider.y += itemToSell.itemProperties.verticalOffset;
                pointInCounterCollider = placementCollider.gameObject.transform.InverseTransformPoint(pointInCounterCollider);
                AddItemToDeskRpc(itemToSell.gameObject.GetComponent<NetworkObject>());
                playerPlacingItem.DiscardHeldObject(placeObject: true, parentObjectTo: placementCollider.GetComponent<NetworkObject>(), placePosition: pointInCounterCollider, matchRotationOfParent: false);
            }
        }

        /// <summary>
        /// Puts item on the desk and adds it to sell list.
        /// </summary>
        /// <param name="itemNetworkObject">The network object of the item to add to the desk.</param>
        [Rpc(SendTo.Everyone, RequireOwnership = false)]
        private void AddItemToDeskRpc(NetworkObjectReference itemNetworkObject)
        {
            if (itemNetworkObject.TryGet(out NetworkObject objectToAdd))
            {
                if (!itemsToSell.Contains(objectToAdd.GetComponentInChildren<GrabbableObject>()))
                {
                    itemsToSell.Add(objectToAdd.GetComponentInChildren<GrabbableObject>());
                    onItemPlaced.Invoke();
                }
            }
        }

        /// <summary>
        /// Calculates the total scrap value of items to sell, updates group credits and displays the profit, and manages item despawning.
        /// </summary>
        [Rpc(SendTo.Everyone, RequireOwnership = false)]
        public void SellItemsRpc()
        {
            // add up all items to sell
            int total = 0;
            foreach (GrabbableObject item in itemsToSell)
            {
                total += item.scrapValue;
            }
            // either apply the company buy rate or use the override buy rate
            total = (int)((float)total * (UseCompanyBuyRate ? StartOfRound.Instance.companyBuyingRate : OverrideBuyRate));

            Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
            terminal.groupCredits += total;

            StartOfRound.Instance.gameStats.scrapValueCollected += total;
            TimeOfDay.Instance.quotaFulfilled += total;
            TimeOfDay.Instance.UpdateProfitQuotaCurrentTime();
            HUDManager.Instance.DisplayCreditsEarning(total, itemsToSell.ToArray(), terminal.groupCredits);
            if ((float)total < (float)terminal.groupCredits / 4f)
            {
                rewardsMusic?.PlayOneShot(rewardBad);
            }
            else
            {
                rewardsMusic?.PlayOneShot(rewardGood);
            }

            foreach (GrabbableObject item in itemsToSell)
            {
                if(item.TryGetComponent<NetworkObject>(out var itemNetworkObject) && base.IsServer) //only host can call Despawn
                {
                    itemNetworkObject.Despawn();
                }
            }
            itemsToSell.Clear();
            onSell.Invoke();
        }

        private void Update()
        {
            placementTrigger.interactable = GameNetworkManager.Instance.localPlayerController.isHoldingObject; // can only interact with trigger if holding item
            foreach (GrabbableObject item in itemsToSell) // prevents items on counter from being picked back up
            {
                if (item.grabbable)
                {
                    item.grabbable = false;
                }
            }
        }
    }
}
