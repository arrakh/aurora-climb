using System;
using UnityEngine;

namespace AuroraClimb.Player
{
    public class PlayerGroundCheck : MonoBehaviour
    {
        private bool isGrounded;

        public bool IsGrounded => isGrounded;

        public event Action OnGroundTouched;

        private void OnTriggerEnter(Collider other)
        {
            isGrounded = true;
            OnGroundTouched?.Invoke();
            Debug.Log("Grounded");
        }

        private void OnTriggerExit(Collider other)
        {
            isGrounded = false;
            Debug.Log("NOT Grounded");
        }
    }
}