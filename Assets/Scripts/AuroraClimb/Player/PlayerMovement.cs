using System;
using UnityEngine;

namespace AuroraClimb.Player
{
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float jumpStrength = 10f;
        [SerializeField] private Transform playerBody;
        [SerializeField] private PlayerStateController stateController;
        [SerializeField] private Rigidbody rb;

        private Vector3 lastMovement;
        private Vector3 externalVelocity;
        private Vector3 debugVelocity;
        private bool canMove = true;

        private bool isJumpQueued = false;

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
            rb.isKinematic = !canMove;
            
            if (canMove) InputUpdate();
            else rb.linearVelocity = Vector3.zero;
        }

        private void InputUpdate()
        {
            if (Input.GetButtonDown("Jump")) isJumpQueued = true;
            
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

            //transform.position += lastMovement * (baseSpeed * Time.deltaTime);
            
        }

        public void AddExternalVelocity(Vector3 velocity)
        {
            externalVelocity += velocity;
        }

        private void FixedUpdate()
        {
            var vel = rb.linearVelocity;

            var moveDir = new Vector3(lastMovement.x, 0f, lastMovement.z);
            var desiredHorizontal = moveDir * baseSpeed;

            var horizontal = new Vector3(vel.x, 0f, vel.z);

            var velChange = desiredHorizontal - horizontal;
            var maxChange = acceleration * Time.fixedDeltaTime;
            velChange = Vector3.ClampMagnitude(velChange, maxChange);
            horizontal += velChange;

            horizontal += new Vector3(externalVelocity.x, 0f, externalVelocity.z);
            var vertical = vel.y + externalVelocity.y;

            debugVelocity = externalVelocity;
            externalVelocity = Vector3.zero;

            if (isJumpQueued) vertical = jumpStrength;
            isJumpQueued = false;

            rb.linearVelocity = new Vector3(horizontal.x, vertical, horizontal.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var from = transform.position;
            var to = from + debugVelocity;
            
            Gizmos.DrawLine(from, to);
            Gizmos.DrawCube(to, Vector3.one * 0.3f);
        }
    }
}