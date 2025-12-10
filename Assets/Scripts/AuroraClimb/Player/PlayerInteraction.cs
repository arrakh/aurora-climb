using System;
using AuroraClimb.Interactions;
using UnityEngine;

namespace AuroraClimb.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float interactionDistance = 100f;

        private IInteractable currentInteractable;
        private IHighlightable currentHighlightable;

        public event Action<IInteractable> OnNewInteractable; 

        private void Update()
        {
            DetectInteractableUpdate();
            InputUpdate();
        }

        private void InputUpdate()
        {
            if (currentInteractable == null) return;
            
            if (!Input.GetButtonDown("Interact")) return;

            currentInteractable.OnInteract(gameObject);
        }

        private void DetectInteractableUpdate()
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (!Physics.Raycast(ray, out var hit, interactionDistance))
            {
                ClearCurrentTarget();
                return;
            }

            if (!hit.collider.TryGetComponent(out IInteractable interactable))
            {
                ClearCurrentTarget();
                return;
            }

            if (!interactable.CanInteract)
            {
                ClearCurrentTarget();
                return;
            }

            if (ReferenceEquals(interactable, currentInteractable)) return;
            
            ClearHighlightable();

            currentInteractable = interactable;
            OnNewInteractable?.Invoke(currentInteractable);

            if (hit.collider.TryGetComponent(out IHighlightable highlightable))
            {
                currentHighlightable = highlightable;
                currentHighlightable.OnHighlight(true);
            }
        }

        private void ClearCurrentTarget()
        {
            ClearHighlightable();

            if (currentInteractable == null) return;
            currentInteractable = null;
            OnNewInteractable?.Invoke(null);
        }

        private void ClearHighlightable()
        {
            if (currentHighlightable == null) return;
            currentHighlightable.OnHighlight(false);
            currentHighlightable = null;
        }
    }
}
