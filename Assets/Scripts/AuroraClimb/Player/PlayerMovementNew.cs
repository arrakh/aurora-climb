using System;
using KinematicCharacterController;
using UnityEngine;

namespace AuroraClimb.Player
{
    public class PlayerMovementNew : MonoBehaviour, ICharacterController
    {
        [SerializeField] private KinematicCharacterMotor motor;
        [SerializeField] private float baseSpeed = 4f;
        [SerializeField] private float runSpeed = 6.5f;
        [SerializeField] private float jumpStrength = 8f;
        [SerializeField] private float climbJumpStrength = 10f;
        [SerializeField] private float jumpStaminaAmount = 0.33f;
        [SerializeField] private Transform playerBody;
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private PlayerStateController stateController;
        [SerializeField] private PlayerHand leftHand, rightHand;
        [SerializeField] private float airControlFactor = 0.4f;
        [SerializeField] private float stableMovementSharpness = 15f;
        [SerializeField] private float airAccelerationSpeed = 30f;
        [SerializeField] private float maxAirMoveSpeed = 6f;
        [SerializeField] private float drag = 0.1f;


        [SerializeField] private float climbTargetDistance = 2f;
        [SerializeField] private LayerMask climbMask;

        private Vector3 lastMovement;
        private Vector3 debug;
        private Vector3 debugA;
        private Vector3 debugB;
        private bool canMove = true;
        private bool isRunning;
        private bool isJumpQueued;

        private void Start()
        {
            stateController.OnStateChanged += OnStateChanged;
            motor.CharacterController = this;
        }

        private void OnStateChanged(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.FreeMovement:
                    canMove = true;
                    break;
                case PlayerState.InDialogue or PlayerState.Cutscene:
                    canMove = false;
                    break;
            }
        }

        private void Update()
        {
            if (canMove) InputUpdate();
        }

        private void InputUpdate()
        {
            var isClimbing = IsClimbing();
            Debug.Log(
                $"Is Stable? {motor.GroundingStatus.IsStableOnGround}, Is on Any Ground? {motor.GroundingStatus.FoundAnyGround}");
            if ((motor.GroundingStatus.IsStableOnGround || isClimbing) && Input.GetButtonDown("Jump"))
                isJumpQueued = true;

            isRunning = Input.GetKey(KeyCode.LeftShift);

            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");

            var forward = playerCamera.Camera.transform.forward;
            var right = playerCamera.Camera.transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            var moveDir = forward * z + right * x;
            if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

            lastMovement = moveDir;
        }

        private bool IsClimbing()
        {
            if (leftHand.IsGrabbing) return true;
            if (rightHand.IsGrabbing) return true;

            return false;
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            var forward = playerCamera.transform.forward;
            forward.y = 0f;
            playerBody.forward = Vector3.forward;

            currentRotation = Quaternion.LookRotation(forward);
        }
        
        public void AddStaminaDuration(float amount)
        {
            leftHand.AddStaminaDuration(amount);
            rightHand.AddStaminaDuration(amount);
        }

        /*public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            var isClimbing = IsClimbing();
            var isGrounded = motor.GroundingStatus.IsStableOnGround;

            Vector2 input = new Vector2(lastMovement.x, lastMovement.z);

            var vertical = currentVelocity.y + (isClimbing ? 0f : Physics.gravity.y * deltaTime); 

            if (isJumpQueued)
            {
                vertical = isClimbing ? climbJumpStrength : jumpStrength;
                if (isClimbing) ConsumeJumpStamina();
                motor.ForceUnground();
            }
            
            if (isClimbing)
            {
                motor.ForceUnground();
                
                if (!TryGetAverageGrab(out var point, out var normal))
                    throw new Exception("COULD NOT GET AVERAGE GRAB");

                var target = point + (normal * climbTargetDistance);

                debugA = point;
                debugB = target;
                
                var move = target - transform.position;
                debug = move;
                var jumpMove = input.normalized * baseSpeed;
                currentVelocity = isJumpQueued ? new Vector3(jumpMove.x, vertical, jumpMove.y) : move;
            }
            else
            {
                Vector3 moveDir = new Vector3(input.x, 0f, input.y);
                if (moveDir.sqrMagnitude > 1f)
                    moveDir.Normalize();

                moveDir *= (isRunning ? runSpeed : baseSpeed);
                moveDir = PreventSlopeMovement(isGrounded, moveDir);
                moveDir = isGrounded ? moveDir : moveDir * airControlFactor;

                currentVelocity = new Vector3(moveDir.x, vertical, moveDir.z);
            }
            
            isJumpQueued = false;
        }*/

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!canMove)
            {
                currentVelocity = Vector3.zero;
                return;
            }
            
            var isClimbing = IsClimbing();
            var isGrounded = motor.GroundingStatus.IsStableOnGround;

            var input = new Vector3(lastMovement.x, 0f, lastMovement.z);
            if (input.sqrMagnitude > 1f) input.Normalize();

            if (isClimbing)
            {
                motor.ForceUnground();

                if (!TryGetAverageGrab(out var point, out var normal))
                    throw new Exception("COULD NOT GET AVERAGE GRAB");

                var target = point + normal * climbTargetDistance;

                debugA = point;
                debugB = target;

                var move = target - transform.position;
                debug = move;

                if (isJumpQueued)
                {
                    ConsumeJumpStamina();

                    var jumpMove = input * baseSpeed;
                    currentVelocity = new Vector3(jumpMove.x, climbJumpStrength, jumpMove.z);

                    motor.ForceUnground();
                }
                else
                {
                    currentVelocity = move;
                }

                isJumpQueued = false;
                return;
            }

            // Ground movement (stable)
            if (isGrounded)
            {
                var speed = currentVelocity.magnitude;
                var groundNormal = motor.GroundingStatus.GroundNormal;

                currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, groundNormal) * speed;

                var desiredSpeed = isRunning ? runSpeed : baseSpeed;

                var inputRight = Vector3.Cross(input, motor.CharacterUp);
                var reorientedInput = Vector3.Cross(groundNormal, inputRight).normalized * input.magnitude;

                var targetVelocity = reorientedInput * desiredSpeed;

                currentVelocity = Vector3.Lerp(
                    currentVelocity,
                    targetVelocity,
                    1f - Mathf.Exp(-stableMovementSharpness * deltaTime)
                );
            }
            // Air movement (accelerate + clamp + anti-wall-climb)
            else
            {
                if (input.sqrMagnitude > 0f)
                {
                    var addedVelocity = input * airAccelerationSpeed * airControlFactor * deltaTime;

                    var velocityOnPlane = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);

                    if (velocityOnPlane.magnitude < maxAirMoveSpeed)
                    {
                        var newTotal = Vector3.ClampMagnitude(velocityOnPlane + addedVelocity, maxAirMoveSpeed);
                        addedVelocity = newTotal - velocityOnPlane;
                    }
                    else if (Vector3.Dot(velocityOnPlane, addedVelocity) > 0f)
                    {
                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, velocityOnPlane.normalized);
                    }

                    if (motor.GroundingStatus.FoundAnyGround)
                    {
                        var obstructionNormal = motor.GroundingStatus.GroundNormal;

                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                        {
                            var perpObstructionNormal =
                                Vector3.Cross(Vector3.Cross(motor.CharacterUp, obstructionNormal), motor.CharacterUp)
                                    .normalized;

                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpObstructionNormal);
                        }
                    }

                    currentVelocity += addedVelocity;
                }

                currentVelocity += Physics.gravity * deltaTime;
                currentVelocity *= 1f / (1f + drag * deltaTime);
            }

            // Jump (impulse) — MUST be after movement smoothing
            if (isJumpQueued && isGrounded)
            {
                var jumpDir = motor.CharacterUp;

                motor.ForceUnground();

                currentVelocity += jumpDir * jumpStrength - Vector3.Project(currentVelocity, motor.CharacterUp);
                // currentVelocity += input * jumpForwardSpeed; // optional
            }

            isJumpQueued = false;
        }


        private Vector3 PreventSlopeMovement(bool isGrounded, Vector3 moveDir)
        {
            if (isGrounded || !motor.GroundingStatus.FoundAnyGround) return moveDir;

            var perpendicularObstructionNormal = Vector3.Cross(
                Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal),
                motor.CharacterUp
            ).normalized;

            return Vector3.ProjectOnPlane(moveDir, perpendicularObstructionNormal);
        }


        private bool TryGetAverageGrab(out Vector3 grabPoint, out Vector3 grabNormal)
        {
            var count = 0;
            grabPoint = Vector3.zero;
            grabNormal = Vector3.zero;

            if (leftHand.IsGrabbing)
            {
                grabPoint += leftHand.Hit.point;
                grabNormal += leftHand.Hit.normal;
                count++;
            }

            if (rightHand.IsGrabbing)
            {
                grabPoint += rightHand.Hit.point;
                grabNormal += rightHand.Hit.normal;
                count++;
            }

            if (count == 0)
                return false;

            grabPoint /= count;
            grabNormal = grabNormal.normalized;
            return true;
        }

        private void ConsumeJumpStamina()
        {
            if (leftHand.IsGrabbing && rightHand.IsGrabbing)
            {
                leftHand.ConsumeStamina(jumpStaminaAmount / 2f);
                rightHand.ConsumeStamina(jumpStaminaAmount / 2f);
            }
            else if (leftHand.IsGrabbing)
            {
                leftHand.ConsumeStamina(jumpStaminaAmount);
            }
            else if (rightHand.IsGrabbing)
            {
                rightHand.ConsumeStamina(jumpStaminaAmount);
            }


            leftHand.Release();
            rightHand.Release();
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var pos = transform.position;
            DrawArrow(Color.blue, pos, pos + motor.Velocity);
            DrawArrow(Color.red, pos, pos + debug);
            DrawArrow(Color.yellow, debugA, debugB);
        }

        private void DrawArrow(Color color, Vector3 from, Vector3 to)
        {
            if (Vector3.Distance(from, to) < 0.0001f) return;

            Gizmos.color = color;

            Gizmos.DrawLine(from, to);
            Gizmos.DrawCube(to, Vector3.one * 0.2f);
        }
#endif
    }
}