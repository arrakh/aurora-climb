using System;
using UnityEngine;

namespace AuroraClimb.Player
{
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private Transform playerBody;
        [SerializeField] private PlayerStateController stateController;

        private Vector3 lastMovement;
        private bool canMove = true;

        private void Start()
        {
            stateController.OnStateChanged += OnStateChanged;
        }

        private void OnStateChanged(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.FreeMovement: canMove = true; break;
                case PlayerState.InDialogue: canMove = false; break;
            }
        }

        private void Update()
        {
            if (canMove) MoveUpdate();
        }

        private void MoveUpdate()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 forward = playerBody.forward;
            Vector3 right   = playerBody.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDir = forward * z + right * x;
            if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

            lastMovement = moveDir;

            transform.position += lastMovement * (baseSpeed * Time.deltaTime);
            
        }
    }
}