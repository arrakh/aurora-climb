using System;
using AuroraClimb.Interactions;
using AuroraClimb.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AuroraClimb.UI
{
    public class CursorUI : MonoBehaviour
    {
        public enum CursorType
        {
            Normal,
            Climb
        }

        [SerializeField] private Image spriteImage;
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private Sprite normalCursor, climbableCursor;

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

        public void SetCursor(CursorType type)
        {
            var sprite = type switch
            {
                CursorType.Normal => normalCursor,
                CursorType.Climb => climbableCursor,
                _ => normalCursor
            };

            spriteImage.sprite = sprite;
        }
    }
}