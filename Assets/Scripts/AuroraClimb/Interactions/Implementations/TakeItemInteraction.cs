using System;
using AuroraClimb.Items;
using AuroraClimb.Player;
using DG.Tweening;
using UnityEngine;

namespace AuroraClimb.Interactions.Implementations
{
    public class TakeItemInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemDefinition item;
        [SerializeField] private int itemAmount = 1;

        [Header("ScaleAnim")] 
        [SerializeField] private float animDuration = 0.33f;
        [SerializeField] private AnimationCurve scaleCurve;

        public bool CanInteract { get; private set; } = true;

        public string InteractLabel => $"Take {item.DisplayName}";
        
        public void OnInteract(GameObject instigator)
        {
            if (!instigator.TryGetComponent(out PlayerInventory inventory))
                throw new Exception($"INTERACTOR {instigator.name} DOES NOT HAVE INVENTORY");
            
            inventory.AddItem(new ItemData(item.ID, itemAmount));
            CanInteract = false;

            transform.DOScale(Vector3.zero, animDuration).SetEase(scaleCurve);
            
            Invoke(nameof(DestroySelf), animDuration);
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}