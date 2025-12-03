using System;
using AuroraClimb.Interactions;
using AuroraClimb.Player;
using TMPro;
using UnityEngine;

namespace AuroraClimb.UI
{
    public class CursorUI : MonoBehaviour
    {
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private TextMeshProUGUI interactText;
        
        private void Start()
        {
            playerInteraction.OnNewInteractable += OnNewInteractable;
        }

        private void OnNewInteractable(IInteractable interactable)
        { 
            if (interactable == null)
            {
                interactText.gameObject.SetActive(false);
                return;
            }
            
            interactText.gameObject.SetActive(true);
            bool hasLabel = !string.IsNullOrEmpty(interactable.InteractLabel);
            interactText.text = hasLabel ? $"[E] {interactable.InteractLabel}" : "";
        }
    }
}