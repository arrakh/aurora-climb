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
        [SerializeField] private float stableMovementSharpness = 15f;
        [SerializeField] private float airAccelerationSpeed = 25f;
        [SerializeField] private float maxAirMoveSpeed = 6f;
        [SerializeField] private float airDrag;
        
        
        
        //MOVE TO CLIMBING
        [SerializeField] private LayerMask climbMask;
        [SerializeField] private float wallStopDistance = 0.5f;
        [SerializeField] private float wallZeroDistance = 0.2f;
        [SerializeField] private float climbVelocitySmoothTime = 0.08f;
        [SerializeField] private float climbMaxSpeed = 20f;
        [SerializeField] private float climbDeadzone = 0.1f;
        private Vector3 climbVelocity;
        private Vector3 climbVelocityDeriv;
        
        private Vector3 lastMovement;
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

            isRunning = Input.GetButtonDown("Run");

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

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            var moveInput = lastMovement; // camera-relative, magnitude 0..1
            var isClimbing = IsClimbing();
            var isStable = motor.GroundingStatus.IsStableOnGround;

            // ---------- GROUND MOVEMENT (stable slope, not climbing) ----------
            if (isStable && !isClimbing)
            {
                var currentVelocityMagnitude = currentVelocity.magnitude;
                var effectiveGroundNormal = motor.GroundingStatus.GroundNormal;

                // Reorient current velocity on the slope (from KCC example)
                currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal)
                                  * currentVelocityMagnitude;

                // Reorient input on the slope
                var inputRight = Vector3.Cross(moveInput, motor.CharacterUp);
                var reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * moveInput.magnitude;

                var maxSpeed = isRunning ? runSpeed : baseSpeed;
                var targetMovementVelocity = reorientedInput * maxSpeed;

                // Smoothly move toward target velocity
                currentVelocity = Vector3.Lerp(
                    currentVelocity,
                    targetMovementVelocity,
                    1f - Mathf.Exp(-stableMovementSharpness * deltaTime)
                );
            }
            // ---------- AIR / TOO-STEEP SLOPE / CLIMBING ----------
            else
            {
                // Regular air movement (but don't apply “air input” while actively climbing, you’re using hand pull for that)
                if (moveInput.sqrMagnitude > 0f && !isClimbing)
                {
                    var addedVelocity = moveInput * airAccelerationSpeed * deltaTime;

                    var currentVelocityOnInputsPlane =
                        Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);

                    // Limit air velocity from inputs
                    if (currentVelocityOnInputsPlane.magnitude < maxAirMoveSpeed)
                    {
                        var newTotal =
                            Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, maxAirMoveSpeed);
                        addedVelocity = newTotal - currentVelocityOnInputsPlane;
                    }
                    else
                    {
                        // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                        if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                            addedVelocity = Vector3.ProjectOnPlane(
                                addedVelocity,
                                currentVelocityOnInputsPlane.normalized
                            );
                    }

                    // Prevent air-climbing sloped walls
                    if (motor.GroundingStatus.FoundAnyGround)
                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                        {
                            var perpendicularObstructionNormal = Vector3.Cross(
                                Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal),
                                motor.CharacterUp
                            ).normalized;

                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpendicularObstructionNormal);
                        }

                    currentVelocity += addedVelocity;
                }

                // Gravity (always when not on stable ground)
                currentVelocity += Physics.gravity * deltaTime;

                // Air drag
                currentVelocity *= 1f / (1f + airDrag * deltaTime);
            }

            // ---------- JUMP ----------
            if (isJumpQueued)
            {
                // Climb jump
                if (isClimbing)
                {
                    ConsumeJumpStamina();
                    motor.ForceUnground();

                    // Jump up relative to character up; you can tweak to be more wall-normal based if needed
                    currentVelocity += motor.CharacterUp * climbJumpStrength;
                }
                // Ground / sliding jump (like example)
                else if (motor.GroundingStatus.FoundAnyGround)
                {
                    var jumpDirection = motor.CharacterUp;
                    if (!motor.GroundingStatus.IsStableOnGround)
                        // If sliding, jump along the surface normal
                        jumpDirection = motor.GroundingStatus.GroundNormal;

                    motor.ForceUnground();

                    // Replace vertical component with jump
                    currentVelocity += jumpDirection * jumpStrength
                                       - Vector3.Project(currentVelocity, motor.CharacterUp);
                }

                isJumpQueued = false;
            }

            // ---------- CLIMB PULL ----------

            if (isClimbing)
            {
                currentVelocity += GetHandClimbVelocity(deltaTime);
            }
            else
            {
                // Reset smoothing when you’re not climbing
                climbVelocity = Vector3.zero;
                climbVelocityDeriv = Vector3.zero;
            }
        }

        private Vector3 GetHandClimbVelocity(float deltaTime)
        {
            // Raw desired pull from hands
            Vector3 desiredPull = Vector3.zero;

            if (leftHand.IsGrabbing)
                desiredPull += leftHand.GetPullDirection();

            if (rightHand.IsGrabbing)
                desiredPull += rightHand.GetPullDirection();

            if (desiredPull.sqrMagnitude < 0.0001f) return Vector3.zero;

            float errorMag = desiredPull.magnitude;
            if (errorMag < climbDeadzone)
            {
                desiredPull = Vector3.zero;
            }

            // 2. Raycast towards desired pull to see wall distance
            Vector3 origin = transform.position;          // or a chest point if you have one
            Vector3 dir    = desiredPull.normalized;

            float maxRayDist = wallStopDistance + 0.5f;   // small margin

            if (Physics.Raycast(origin, dir, out RaycastHit hit, maxRayDist, climbMask,
                    QueryTriggerInteraction.Ignore))
            {
                float dist = hit.distance;

                // Ensure parameters are sane
                float minD = Mathf.Min(wallZeroDistance, wallStopDistance);
                float maxD = Mathf.Max(wallZeroDistance, wallStopDistance);

                // factor = 1 when far (>= maxD), factor = 0 when very close (<= minD)
                float factor = Mathf.InverseLerp(minD, maxD, dist);
                factor = Mathf.Clamp01(factor);

                // Scale pull based on distance
                desiredPull *= factor;

                // 3. Remove the "into wall" component: only slide along the surface
                desiredPull = Vector3.ProjectOnPlane(desiredPull, hit.normal);

                // If we are really close, factor is ~0 → velocity becomes basically 0
            }

            // 4. Dampen climb velocity toward the desired vector (stops springing)
            climbVelocity = Vector3.SmoothDamp(
                climbVelocity,
                desiredPull,
                ref climbVelocityDeriv,
                climbVelocitySmoothTime,
                climbMaxSpeed,
                deltaTime
            );

            return climbVelocity;
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