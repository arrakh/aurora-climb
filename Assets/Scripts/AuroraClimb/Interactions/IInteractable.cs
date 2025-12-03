using UnityEngine;

namespace AuroraClimb.Interactions
{
    public interface IInteractable
    {
        public string InteractLabel { get; }
        
        public void OnInteract(GameObject instigator);
    }
}