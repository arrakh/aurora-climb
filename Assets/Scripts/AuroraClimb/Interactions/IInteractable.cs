using UnityEngine;

namespace AuroraClimb.Interactions
{
    public interface IInteractable
    {
        public bool CanInteract { get; }
        
        public string InteractLabel { get; }
        
        public void OnInteract(GameObject instigator);
    }
}