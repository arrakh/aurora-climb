using System;
using UnityEngine;

namespace AuroraClimb.Player
{
    public enum PlayerState
    {
        FreeMovement,
        InDialogue,
        Cutscene
    }
    
    public class PlayerStateController : MonoBehaviour
    {
        private PlayerState state;

        public event Action<PlayerState> OnStateChanged;

        private void SetState(PlayerState newState)
        {
            state = newState;
            
            OnStateChanged?.Invoke(state);
        }

        public void SetIsInDialogue(bool value)
        {
            SetState(value ? PlayerState.InDialogue : PlayerState.FreeMovement);
        }
    }
}