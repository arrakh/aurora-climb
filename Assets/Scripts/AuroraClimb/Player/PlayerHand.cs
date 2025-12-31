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
        [SerializeField] private float maxStamina = 1f;
        [SerializeField] private float staminaDrainDuration = 5f;
        [SerializeField] private float staminaRecoveryDuration = 8f;
        [SerializeField] private float grabStaminaConsumption = 0.2f;
        [SerializeField] private float grabCooldown = 0.2f;
        [SerializeField] private float maxDistance = 3f;

        [Header("Animation")] 
        [SerializeField] private AnimationCurve shakeIntensityCurve;
        [SerializeField] private float shakeStrength = 0.8f;

        public event Action OnMaxStaminaUpdate;
        
        public float Stamina => currentStamina;
        public float MaxStamina => maxStamina;
        public bool IsGrabbing => isGrabbing;

        public RaycastHit Hit => latestHit;
        
        private float currentStamina = 1f;

        private bool isGrabbing = false;

        private float currentGrabCooldown;

        private RaycastHit latestHit;

        private void Awake()
        {
            currentStamina = maxStamina;
        }

        public void Grab(RaycastHit grabTarget)
        {
            if (currentGrabCooldown > 0f) return;
            latestHit = grabTarget;
            isGrabbing = true;
            currentStamina -= grabStaminaConsumption;
            currentGrabCooldown = grabCooldown;
        }

        public void AddMaxStamina(float amount)
        {
            maxStamina += amount;
            OnMaxStaminaUpdate?.Invoke();
        }

        public void ConsumeStamina(float amount)
        {
            currentStamina -= amount;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        public void Release()
        {
            isGrabbing = false;
        }
        
        private void FixedUpdate()
        {
            return;
            if (!isGrabbing) return;

            var pullDirection = (latestHit.point - pullPoint.position) * pullStrength;
            movement.AddExternalVelocity(pullDirection);
            
            //Debug.Log($"{name} | PULLING VECTOR {pullDirection} TOWARDS {latestHit.collider.name}");
        }

        public Vector3 GetPullDirection()
            => (latestHit.point - pullPoint.position) * pullStrength;

        private void Update()
        {
            currentGrabCooldown -= Time.deltaTime;

            //DistanceCheck();
            
            StaminaUpdate();

            if (isGrabbing) visual.transform.position = latestHit.point;
            else visual.transform.localPosition = Vector3.zero;

            HandVisualUpdate();
        }

        private void DistanceCheck()
        {
            if (!isGrabbing) return;

            var distance = Vector3.Distance(latestHit.point, pullPoint.position);
            if (distance > maxDistance) Release();
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

            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            
            if (currentStamina == 0f) Release();
        }
    }
}