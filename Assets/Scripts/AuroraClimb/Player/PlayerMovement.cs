using System;
using UnityEngine;

namespace AuroraClimb.Player
{
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float runSpeed = 6.5f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float jumpStrength = 10f;
        [SerializeField] private float climbJumpStrength = 16f;
        [SerializeField] private float jumpStaminaAmount = 0.33f;
        [SerializeField] private Transform playerBody;
        [SerializeField] private PlayerStateController stateController;
        [SerializeField] private PlayerGroundCheck groundCheck;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerHand leftHand, rightHand;
        [SerializeField] private float climbProbeDistance = 2f;
        [SerializeField] private LayerMask climbMask;
        
        private Vector3 lastMovement;
        private Vector3 externalVelocity;
        private bool canMove = true;
        private bool isRunning = false;

        private bool isJumpQueued = false;

        private void Start()
        {
            stateController.OnStateChanged += OnStateChanged;
            groundCheck.OnGroundTouched += OnGroundTouched;
        }

        private void OnGroundTouched()
        {
            
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
            var isClimbing = IsClimbing();
            if ((groundCheck.IsGrounded || isClimbing) && Input.GetButtonDown("Jump"))
            {
                isJumpQueued = true;
            }

            isRunning = Input.GetButtonDown("Run");
            
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

            var input = new Vector2(lastMovement.x, lastMovement.z);
            var moveDir = new Vector3(input.x, 0f, input.y);

            var castDir = moveDir.normalized;

            rb.useGravity = !IsClimbing();

            if (Physics.Raycast(playerBody.position, castDir, out var _, climbProbeDistance, climbMask)
                || Physics.Raycast(groundCheck.transform.position, castDir, out var _, climbProbeDistance, climbMask))
                moveDir = Vector3.zero;

            var desiredHorizontal = moveDir * (isRunning ? runSpeed : baseSpeed);
            var horizontal = new Vector3(vel.x, 0f, vel.z);

            var velChange = desiredHorizontal - horizontal;
            var maxChange = acceleration * Time.fixedDeltaTime;
            velChange = Vector3.ClampMagnitude(velChange, maxChange);
            horizontal += velChange;

            horizontal += new Vector3(externalVelocity.x, 0f, externalVelocity.z);
            var vertical = vel.y + externalVelocity.y ;

            externalVelocity = Vector3.zero;
            
            if (isJumpQueued)
            {
                var isClimbing = IsClimbing();
                vertical = isClimbing ? climbJumpStrength : jumpStrength;
                if (isClimbing)
                {
                    horizontal += desiredHorizontal;
                    ConsumeJumpStamina();
                }
            }

            isJumpQueued = false;

            rb.linearVelocity = new Vector3(horizontal.x, vertical, horizontal.z);
        }

        private void ConsumeJumpStamina()
        {
            if (leftHand.IsGrabbing && rightHand.IsGrabbing)
            {
                leftHand.ConsumeStamina(jumpStaminaAmount / 2f);
                rightHand.ConsumeStamina(jumpStaminaAmount / 2f);
            }
            else if (leftHand.IsGrabbing) leftHand.ConsumeStamina(jumpStaminaAmount);
            else if (rightHand.IsGrabbing) rightHand.ConsumeStamina(jumpStaminaAmount);

            
            leftHand.Release();
            rightHand.Release();
        }

        private Vector3 GetHandsNormal()
        {
            if (leftHand.IsGrabbing && rightHand.IsGrabbing)
                return (leftHand.Hit.normal + rightHand.Hit.normal).normalized;
            
            if (leftHand.IsGrabbing) return leftHand.Hit.normal.normalized;
            if (rightHand.IsGrabbing) return rightHand.Hit.normal.normalized;
            
            return Vector3.zero;
        }

        private bool IsClimbing()
        {
            if (leftHand.IsGrabbing) return true;
            if (rightHand.IsGrabbing) return true;

            return false;
        }

        private void OnDrawGizmos()
        {
            var pos = transform.position;
            DrawArrow(Color.blue, pos, pos + rb.linearVelocity);
        }

        void DrawArrow(Color color, Vector3 from, Vector3 to)
        {
            if (Vector3.Distance(from, to) < 0.0001f) return;
            
            Gizmos.color = color;
            
            Gizmos.DrawLine(from, to);
            Gizmos.DrawCube(to, Vector3.one * 0.2f);
        }
    }
}