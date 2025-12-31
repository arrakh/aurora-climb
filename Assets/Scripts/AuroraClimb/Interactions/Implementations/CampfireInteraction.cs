using System;
using AuroraClimb.Player;
using AuroraClimb.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AuroraClimb.Interactions.Implementations
{
    public class CampfireInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpriteRenderer[] fireSpriteRenderers;
        [SerializeField] private Light fireLight;
        [SerializeField] private Sprite[] fireSprites;
        [SerializeField] private float fireFps = 30;
        [SerializeField] private float flickerSpeed = 3;
        [SerializeField] private bool turnedOnAtStart = false;

        private int spriteIndex;
        private float animTime;
        private float noiseProgress;
        private float targetIntensity;
        private bool isFireLit = false;

        private PlayerBurnItem burnItem = null;

        public bool CanInteract { get; private set; } = true;
        public string InteractLabel => GetInteractionLabel();

        private string GetInteractionLabel()
        {
            if (!isFireLit) return "Light Up";
            return "Burn Item";
        }

        private void Start()
        {
            targetIntensity = fireLight.intensity;
            noiseProgress = Random.value * 100f;
            SetFireVisible(turnedOnAtStart);
        }

        public void SetFireVisible(bool on)
        {
            foreach (var rend in fireSpriteRenderers)
                rend.gameObject.SetActive(on);

            isFireLit = on;
            fireLight.enabled = on;
        }

        private void Update()
        {
            if (isFireLit) AnimateFireUpdate();
        }

        private void AnimateFireUpdate()
        {
            noiseProgress += Time.deltaTime * flickerSpeed;
            var noiseAlpha = Mathf.PerlinNoise1D(noiseProgress);
            fireLight.intensity = Mathf.Lerp(targetIntensity * 0.3f, targetIntensity, noiseAlpha);
            
            if (fireSprites == null || fireSprites.Length == 0)
                return;

            float timeBetweenFrame = 1f / fireFps;

            animTime += Time.deltaTime;

            while (animTime >= timeBetweenFrame)
            {
                animTime -= timeBetweenFrame;

                spriteIndex++;
                if (spriteIndex >= fireSprites.Length)
                    spriteIndex = 0;
            }

            var sprite = fireSprites[spriteIndex];
            foreach (var rend in fireSpriteRenderers)
                rend.sprite = sprite;
        }

        public void OnInteract(GameObject instigator)
        {
            burnItem = instigator.GetComponent<PlayerBurnItem>();
            
            if (!isFireLit) SetFireVisible(true);
            else if (burnItem != null) burnItem.PromptBurnItem();
        }
    }
}