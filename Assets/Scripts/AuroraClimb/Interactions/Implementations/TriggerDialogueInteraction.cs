using System;
using Arr.Ink;
using AuroraClimb.Player;
using UnityEngine;

namespace AuroraClimb.Interactions.Implementations
{
    public class TriggerDialogueInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private InkStoryController storyController;
        [SerializeField] private string triggerPath;

        private bool isInStory = false;

        private PlayerStateController stateController;

        private void Awake()
        {
            storyController.OnStoryDone += OnStoryDone;
        }

        public bool CanInteract => !isInStory;
        public string InteractLabel => "Talk";

        public void OnInteract(GameObject instigator)
        {
            if (isInStory) return;
            
            if (string.IsNullOrEmpty(triggerPath))
                throw new Exception($"Trying to trigger dialogue from {gameObject.name} but path is EMPTY!");

            if (!instigator.TryGetComponent(out stateController)) return;

            isInStory = true;
            
            stateController.SetIsInDialogue(true);

            storyController.StartStory(triggerPath);
        }

        private void OnStoryDone()
        {
            if (!isInStory) return;
            stateController.SetIsInDialogue(false);
            isInStory = false;
        }
    }
}