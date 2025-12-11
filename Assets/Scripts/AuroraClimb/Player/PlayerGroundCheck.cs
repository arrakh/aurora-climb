using System;
using UnityEngine;

namespace AuroraClimb.Player
{
    public class PlayerGroundCheck : MonoBehaviour
    {
        [Header("Ground Check")]
        [SerializeField] private float groundCheckDistance = 0.3f;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField, Range(0f, 89f)] private float maxGroundAngle = 45f;
        [SerializeField] private LayerMask layerMask = ~0;

        [Header("Debug")]
        [SerializeField] private bool debugDraw = false;

        private bool isGrounded;
        public bool IsGrounded => isGrounded;

        public event Action OnGroundTouched;

        private void Update()
        {
            bool wasGrounded = isGrounded;
            isGrounded = CheckGround(out RaycastHit hit);

            if (isGrounded && !wasGrounded)
            {
                OnGroundTouched?.Invoke();
                Debug.Log("Grounded");
            }
            else if (!isGrounded && wasGrounded)
            {
                Debug.Log("NOT Grounded");
            }

            if (debugDraw)
            {
                Vector3 origin = GetCastOrigin();
                Debug.DrawRay(origin, Vector3.down * groundCheckDistance,
                    isGrounded ? Color.green : Color.red);
            }
        }

        private bool CheckGround(out RaycastHit hit)
        {
            Vector3 origin = GetCastOrigin();
            float castDistance = groundCheckDistance;

            if (Physics.SphereCast(origin, groundCheckRadius, Vector3.down,
                                   out hit, castDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                //Debug.Log($"{hit.collider.name}: {angle} <= {maxGroundAngle}");
                return angle <= maxGroundAngle;
            }

            return false;
        }

        private Vector3 GetCastOrigin()
        {
            return transform.position + Vector3.up * 0.3f;
        }
    }
}