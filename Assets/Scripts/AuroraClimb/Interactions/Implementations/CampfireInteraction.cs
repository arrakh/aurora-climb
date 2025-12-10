using System;
using UnityEngine;

namespace AuroraClimb.Interactions.Implementations
{
    public class CampfireInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpriteRenderer[] fireSpriteRenderers;
        [SerializeField] private Light fireLight;
        [SerializeField] private Sprite[] fireSprites;
        [SerializeField] private float fireFps = 30;
        [SerializeField] private bool turnedOnAtStart = false;

        private int spriteIndex;
        private float animTime;
        private bool isAnimating = false;

        public bool CanInteract { get; private set; }
        public string InteractLabel => "Light Up";

        private void Start()
        {
            SetFireVisible(turnedOnAtStart);
        }

        private void SetFireVisible(bool on)
        {
            foreach (var rend in fireSpriteRenderers)
                rend.gameObject.SetActive(on);

            isAnimating = on;
            CanInteract = !on;
            fireLight.enabled = on;
        }

        private void Update()
        {
            if (isAnimating) AnimateFireUpdate();
        }

        private void AnimateFireUpdate()
        {
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
            SetFireVisible(true);
        }
    }
}