using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AuroraClimb.Player
{
    public class PlayerHand : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private GameObject visual;
        [SerializeField] private GameObject shakeRoot;
        [SerializeField] private Transform pullPoint;
        [SerializeField] private MeshRenderer meshRenderer;

        [Header("Design")]
        [SerializeField] private float pullStrength = 5f;
        [SerializeField] private float staminaDrainDuration = 5f;
        [SerializeField] private float staminaRecoveryDuration = 8f;

        [Header("Animation")] 
        [SerializeField] private AnimationCurve shakeIntensityCurve;
        [SerializeField] private float shakeStrength = 0.8f;
        
        private float currentStamina = 1f;

        private bool isGrabbing = false;

        private RaycastHit latestHit;

        public void Grab(RaycastHit grabTarget)
        {
            latestHit = grabTarget;
            isGrabbing = true;
        }

        public void Release()
        {
            isGrabbing = false;
        }
        
        private void FixedUpdate()
        {
            if (!isGrabbing) return;

            var pullDirection = (latestHit.point - pullPoint.position) * pullStrength;
            movement.AddExternalVelocity(pullDirection);
            
            Debug.Log($"{name} | PULLING VECTOR {pullDirection} TOWARDS {latestHit.collider.name}");
        }

        private void Update()
        {
            StaminaUpdate();

            if (isGrabbing) visual.transform.position = latestHit.point;
            else visual.transform.localPosition = Vector3.zero;

            HandVisualUpdate();
        }

        private void HandVisualUpdate()
        {
            var alpha = shakeIntensityCurve.Evaluate(1 - currentStamina);
            shakeRoot.transform.localPosition = Random.insideUnitSphere * (shakeStrength * alpha);
            meshRenderer.material.color = Color.Lerp(Color.white, Color.red, alpha);
        }

        private void StaminaUpdate()
        {
            if (isGrabbing) currentStamina -= Time.deltaTime / staminaDrainDuration;
            else currentStamina += Time.deltaTime / staminaRecoveryDuration;

            currentStamina = Mathf.Clamp01(currentStamina);
            
            if (currentStamina == 0f) Release();
        }
    }
}